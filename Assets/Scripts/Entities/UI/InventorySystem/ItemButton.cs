using UnityEngine;

namespace Assets.Scripts.Entities.UI.InventorySystem
{
    public class ItemButton : MonoBehaviour
    {
        public ItemSO item;
        private InventoryUIController inventoryUIController;
        [SerializeField] private GameObject borderGameObject;

        private void Start()
        {
            inventoryUIController = FindObjectOfType<InventoryUIController>();
        }

        public void ClickedButton()
        {
            inventoryUIController.OnItemSelect(item, borderGameObject);
        }
    }
}