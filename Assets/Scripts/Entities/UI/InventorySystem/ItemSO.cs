using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public enum ItemType
    {
        Consumable,
        Equippable,
        KeyItem,
        Tool,
        // ... add more types as needed
    }
    public int MaxStackAmount { get { return maxStackAmount; } }

    [Header("Item Details")]
    public string itemName = "New Item";        // The name of the item
    public Sprite icon = null;                  // The item's icon
    public ItemType itemType;                   // The type of the item
    public bool isStackable = false;            // Can this item stack?
    [SerializeField] private int maxStackAmount = 1;              // Max amount in one stack if stackable
    //public int quantity = 0;                    // Actual amount in a stack

    [TextArea]
    public string description = "Item Description"; // Description of the item

    [Header("Stats Modifiers (if applicable)")]
    public int attackModifier = 0;               // Attack change when this item is equipped
    public int defenseModifier = 0;              // Defense change when this item is equipped
                                                 // ... add other stat modifiers as needed

    // Implement functionality for using the item
    public virtual void Use()
    {
        // Default use effect
        // Override this method in derived classes for specific use effects
        Debug.Log($"Using {itemName}");
    }

    // Check if the item can be used in a specific context
    public virtual bool CanUse()
    {
        // Default check
        // Override this in derived classes based on specific conditions
        return true;
    }

    // If the item affects player stats, apply the effects here
    public virtual void Equip()
    {
        // Default equip effect
        // Override this method in derived classes for specific equip effects
        Debug.Log($"Equipping {itemName}");
    }

    // Remove effects from the player when item is unequipped
    public virtual void Unequip()
    {
        // Default unequip effect
        // Override this method in derived classes for specific unequip effects
        Debug.Log($"Unequipping {itemName}");
    }
}