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
        private GameObject currentSelectedBorder; // Keeps track of the currently selected item's border

        public void UpdateInventoryUI(List<ItemStack> items)
        {
            ClearInventoryUI();

            foreach (ItemStack item in items)
            {
                CreateInventoryButton(item);
            }
        }

        private void ClearInventoryUI()
        {
            foreach (Transform child in inventoryPanel)
            {
                Destroy(child.gameObject);
            }
        }

        private void CreateInventoryButton(ItemStack itemStack)
        {
            GameObject inventoryButton = Instantiate(itemButtonPrefab, inventoryPanel);
            inventoryButton.GetComponent<Image>().sprite = itemStack.Item.icon;     // Set the sprite
            inventoryButton.GetComponent<ItemButton>().item = itemStack.Item;       // Assign the item to button's script

            // Set the quantity text
            TextMeshProUGUI quantityText = inventoryButton.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();
            if (itemStack.Quantity > 1)
            {
                quantityText.text = itemStack.Quantity.ToString();
            }
            else
            {
                quantityText.text = ""; // Clear the text if there's only one item
            }
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

        private void ClearDescription()
        {
            itemIcon.sprite = null;
            itemNameText.text = "No Item Selected";
            itemDescriptionText.text = "";
        }

        // This function is called when a new item is selected
        public void OnItemSelect(ItemSO selectedItem, GameObject newSelectedBorder)
        {
            DisplayItem(selectedItem);
            // Disable the current selected border
            if (currentSelectedBorder != null)
            {
                currentSelectedBorder.SetActive(false);
            }

            // Enable the new selected border and update the reference
            newSelectedBorder.SetActive(true);
            currentSelectedBorder = newSelectedBorder;
        }
    }
}