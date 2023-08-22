using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.UI.InventorySystem
{
    public class InventorySystem : MonoBehaviour
    {
        public InventoryUIController inventoryUIController;
        public ItemSO selectedItem; // This could be the item you selected/clicked on
        public ItemListSO initialItems;  // Drag the ScriptableObject here
        public List<ItemSO> inventoryItems = new List<ItemSO>();

        private void Awake()
        {
            PopulateInitialItems();
        }

        private void PopulateInitialItems()
        {
            if (initialItems != null && initialItems.items.Count > 0)
            {
                inventoryItems = new List<ItemSO>(initialItems.items); // Copy initial items to inventory
                inventoryUIController.UpdateInventoryUI(inventoryItems); // Update the UI
            }
        }
        public void OnItemSelect(ItemSO selectedItem)
        {
            if (selectedItem != null)
            {
                // Assuming you have a reference to your InventoryUIController
                inventoryUIController.DisplayItem(selectedItem);
            }
        }

    }
}