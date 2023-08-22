using UnityEngine;

namespace Assets.Scripts.Entities.UI.InventorySystem
{
    public class ItemButton : MonoBehaviour
    {
        public ItemSO item;

        private InventorySystem inventorySystem;

        private void Start()
        {
            inventorySystem = FindObjectOfType<InventorySystem>();
        }

        public void ClickedButton()
        {
            inventorySystem.OnItemSelect(item);
        }
    }
}