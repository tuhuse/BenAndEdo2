using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryBase> items = new List<InventoryBase>();

    public void AddItem(InventoryBase item)
    {
        items.Add(item);
        Debug.Log(item.itemName + " added to inventory.");
    }

    public void UseItem(int index)
    {
        if (index < items.Count)
        {
            items[index].Use();
            // アイテムの数を減らすなどの処理もここで実施
        }
        else
        {
            Debug.Log("Invalid item index!");
        }
    }

    public void DisplayInventory()
    {
        foreach (var item in items)
        {
            Debug.Log("Item: " + item.itemName + ", Quantity: " + item.quantity);
        }
    }
}
