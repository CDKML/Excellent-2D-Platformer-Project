using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item List", menuName = "Inventory/ItemList")]
public class ItemListSO : ScriptableObject
{
    public List<ItemStack> items = new List<ItemStack>();
}
