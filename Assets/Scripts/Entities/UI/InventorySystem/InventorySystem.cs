using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.UI.InventorySystem
{
    public class InventorySystem : MonoBehaviour
    {
        public ItemSO selectedItem; // This could be the item you selected/clicked on
        public List<ItemStack> currentInventory;
        public InventoryUIController inventoryUIController;

        private void Awake()
        {
            PopulateInitialItems();
        }

        private void PopulateInitialItems()
        {
            if (currentInventory != null)
            {
                // Copy current state into a new list
                List<ItemStack> initialItems = new List<ItemStack>(currentInventory);
                currentInventory.Clear(); // Clear the current inventory as we are about to repopulate it

                foreach (ItemStack item in initialItems)
                {
                    AddItem(item.Item, item.Quantity); // Use the initial quantity of each item
                }
                // Update the UI if necessary
            }
        }

        public void AddItem(ItemSO item, int quantity)
        {
            int remainingQuantity = quantity;

            // First, try to add to existing stacks
            foreach (ItemStack stack in currentInventory)
            {
                if (stack.Item == item)
                {
                    int canAdd = item.MaxStackAmount - stack.Quantity;
                    int toAdd = Mathf.Min(canAdd, remainingQuantity);
                    stack.Quantity += toAdd;
                    remainingQuantity -= toAdd;

                    if (remainingQuantity <= 0)
                    {
                        break;
                    }
                }
            }

            // If there is still remaining quantity, add new stacks
            while (remainingQuantity > 0)
            {
                int toAdd = Mathf.Min(remainingQuantity, item.MaxStackAmount);
                currentInventory.Add(new ItemStack(item, toAdd));
                remainingQuantity -= toAdd;
            }

            // Update the UI
            inventoryUIController.UpdateInventoryUI(currentInventory);
        }
    }
}