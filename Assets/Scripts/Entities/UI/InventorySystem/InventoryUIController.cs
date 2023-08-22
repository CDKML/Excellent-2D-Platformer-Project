using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Entities.UI.InventorySystem
{
    public class InventoryUIController : MonoBehaviour
    {
        // References to your UI elements
        public Image itemIcon;
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI itemDescriptionText;
        public GameObject itemButtonPrefab;  // A prefab representing an item button in the UI
        public Transform inventoryPanel;     // A parent object where all the item buttons will be instantiated

        public void UpdateInventoryUI(List<ItemSO> items)
        {
            ClearInventoryUI();

            foreach (ItemSO item in items)
            {
                CreateInventoryButton(item);
            }

            DisplayFirstItem(items);
        }

        private void ClearInventoryUI()
        {
            foreach (Transform child in inventoryPanel)
            {
                Destroy(child.gameObject);
            }
        }

        private void CreateInventoryButton(ItemSO item)
        {
            GameObject newButton = Instantiate(itemButtonPrefab, inventoryPanel);
            newButton.GetComponent<Image>().sprite = item.icon;     // Set the sprite
            newButton.GetComponent<ItemButton>().item = item;       // Assign the item to button's script
        }

        private void DisplayFirstItem(List<ItemSO> items)
        {
            ItemSO firstItem = items.FirstOrDefault();

            if (firstItem != null)
            {
                DisplayItem(firstItem);
            }
            else
            {
                ClearDescription();
            }
        }

        private void ClearDescription()
        {
            itemIcon.sprite = null;
            itemNameText.text = "No Item Selected";
            itemDescriptionText.text = "";
        }


        // Method to update the UI
        public void DisplayItem(ItemSO item)
        {
            if (item != null)
            {
                itemIcon.sprite = item.icon;
                itemNameText.text = item.itemName;
                itemDescriptionText.text = item.description;
            }
            else
            {
                // Clear the UI or display a default message
                ClearDescription();
            }
        }
    }

}