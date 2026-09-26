using System;
using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace DismantleEquipment
{
    [BepInPlugin(PluginId, "Dismantle Equipment", "1.0.1")]
    public sealed class Plugin : BaseUnityPlugin
    {
        private const string PluginId = "com.valheimmods.dismantleequipment";
        private Harmony _harmony;

        private void Awake()
        {
            _harmony = new Harmony(PluginId);
            _harmony.PatchAll(typeof(Plugin).Assembly);
            Logger.LogInfo("Dismantle Equipment loaded: Shift + right-click a crafted item.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }

        [HarmonyPatch(typeof(InventoryGui), "OnRightClickItem")]
        private static class InventoryRightClickPatch
        {
            private static bool Prefix(InventoryGrid grid, ItemDrop.ItemData item)
            {
                if (!ZInput.GetKey(KeyCode.LeftShift, true) &&
                    !ZInput.GetKey(KeyCode.RightShift, true))
                {
                    return true;
                }

                Player player = Player.m_localPlayer;
                if (player == null || grid == null || grid.GetInventory() != player.GetInventory())
                {
                    return true;
                }

                // A Shift + right-click is always ours, even when no recipe exists.
                // Otherwise it would accidentally equip or use an item.
                if (item != null && !UnifiedPopup.IsVisible())
                {
                    Dismantle(player, item);
                }

                return false;
            }
        }

        private static void Dismantle(Player player, ItemDrop.ItemData item)
        {
            if (player.IsDead())
            {
                return;
            }

            if (!TryGetRefund(item, out int itemCount, out List<Refund> refunds, out string reason))
            {
                player.Message(MessageHud.MessageType.Center, reason);
                return;
            }

            Inventory inventory = player.GetInventory();
            if (!inventory.GetAllItems().Contains(item))
            {
                return;
            }

            string itemName = Localize(item.m_shared.m_name);
            player.UnequipItem(item, true);
            if (!inventory.RemoveItem(item, itemCount))
            {
                player.Message(MessageHud.MessageType.Center, "The item could not be removed.");
                return;
            }

            Vector3 position = player.transform.position + Vector3.up * 0.5f;
            foreach (Refund refund in refunds)
            {
                int remaining = refund.Amount;
                int maxStack = Math.Max(1, refund.Item.m_itemData.m_shared.m_maxStackSize);
                while (remaining > 0)
                {
                    int count = Math.Min(remaining, maxStack);
                    ItemDrop.ItemData dropped = refund.Item.m_itemData.Clone();
                    dropped.m_dropPrefab = refund.Item.gameObject;
                    ItemDrop.DropItem(dropped, count, position, Quaternion.identity);
                    remaining -= count;
                }
            }

            player.Message(MessageHud.MessageType.Center,
                "Dismantled " + itemCount + " × " + itemName + ".");
        }

        private static bool TryGetRefund(
            ItemDrop.ItemData item, out int itemCount, out List<Refund> refunds, out string reason)
        {
            itemCount = 0;
            refunds = new List<Refund>();
            reason = "This item has no crafting recipe and cannot be dismantled.";

            Recipe recipe = ObjectDB.instance?.GetRecipe(item);
            if (recipe == null || !recipe.m_enabled || recipe.m_noCraftOnlyUpgrade ||
                recipe.m_amount <= 0 || recipe.m_resources == null || recipe.m_resources.Length == 0)
            {
                return false;
            }

            // The game does not save which alternative ingredient was used.
            if (recipe.m_requireOnlyOneIngredient)
            {
                reason = "This recipe has alternative ingredients, so its original material is unknown.";
                return false;
            }

            itemCount = recipe.m_amount;
            if (item.m_stack < itemCount)
            {
                reason = "A complete crafting batch is needed to dismantle this item.";
                return false;
            }

            foreach (Piece.Requirement requirement in recipe.m_resources)
            {
                if (requirement == null)
                {
                    reason = "The recipe has an invalid material requirement.";
                    return false;
                }

                int amount = 0;
                for (int quality = 1; quality <= item.m_quality; quality++)
                {
                    // Valheim only consumes upgrader resources at an upgrader station.
                    // Match the station required for this recipe and quality level.
                    CraftingStation station = recipe.GetRequiredStation(quality);
                    bool usesUpgrader = station != null && station.m_upgrader;
                    if (requirement.m_upgraderResource != usesUpgrader)
                    {
                        continue;
                    }

                    amount = checked(amount + requirement.GetAmount(quality));
                }

                if (amount > 0)
                {
                    if (requirement.m_resItem == null || requirement.m_resItem.m_itemData == null)
                    {
                        reason = "The recipe has an unavailable material.";
                        return false;
                    }

                    refunds.Add(new Refund(requirement.m_resItem, amount));
                }
            }

            if (refunds.Count == 0)
            {
                reason = "This recipe does not use recoverable materials.";
                return false;
            }

            return true;
        }

        private static string Localize(string name)
        {
            return Localization.instance != null ? Localization.instance.Localize(name) : name;
        }

        private sealed class Refund
        {
            internal readonly ItemDrop Item;
            internal readonly int Amount;

            internal Refund(ItemDrop item, int amount)
            {
                Item = item;
                Amount = amount;
            }
        }
    }
}
