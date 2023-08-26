using UnityEngine;

[System.Serializable]
public class ItemStack
{
    public ItemSO Item;
    public int Quantity;

    public ItemStack(ItemSO item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }
}

