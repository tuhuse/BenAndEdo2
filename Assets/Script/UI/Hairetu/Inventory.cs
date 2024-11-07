using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    [SerializeField] private Item[] _items;
    private Sprite[] _itemIcon; 
    [SerializeField] private int _incentorySize=5;
   [SerializeField] private int _selectInventorynumber=0;
    [SerializeField] private InventoryUI _inventoryUI;
    // Start is called before the first frame update
    void Start()
    {
        _items = new Item[_incentorySize];
    }
    private void Update()
    {
        SelectInventory();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Use(_selectInventorynumber);
        }
    }
    private void SelectInventory()
    {
        for(int addnumber = 0; addnumber < _incentorySize; addnumber++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1  + addnumber))
            {
                _selectInventorynumber = addnumber;
                break;
            }
        }
    }
    
    public bool AddItem(Item newItem)
    {
        for(int itemnumber = 0; itemnumber < _items.Length; itemnumber++)
        {
            if (_items[itemnumber] == null)
            {
                _items[itemnumber] = newItem;
               _inventoryUI.UpdateSlotImage(itemnumber, newItem.MyIcon);
                //_itemIcon[itemnumber] = newItem.MyIcon;
                Debug.Log(newItem.MyItemName + " をインベントリに追加しました。");
                return true;
            }
        }
        Debug.Log("満タンです");
        return false;
    }public bool RemoveItem(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= _items.Length || _items[itemIndex] == null)
        {
            Debug.Log("アイテムが見つかりませんでした");
            return false;
        }

        Debug.Log(_items[itemIndex].MyItemName + " をインベントリから削除しました。");
        _inventoryUI.DeleteSlotImage(itemIndex, _items[itemIndex].MyIcon);
        _items[itemIndex] = null;
        
        return true;
    }
    public void Use(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= _items.Length || _items[itemIndex] == null)
        {
            Debug.Log("使用できるアイテムがありません");
            return;
        }

        // Assuming the Item class has a Use method
        _items[itemIndex].UseItem();  // Execute the item's "Use" functionality
        Debug.Log(_items[itemIndex].MyItemName + " を使用しました。");

        RemoveItem(itemIndex); // Remove the item after using it
    }
    public Item[] GetItems()
    {
        return _items;
    }

    // インベントリの内容を表示するメソッド
    public void DisplayInventory()
    {
        Debug.Log("インベントリの内容:");
        for (int itemnumber = 0; itemnumber < _items.Length; itemnumber++)
        {
            if (_items[itemnumber] != null)
            {
                Debug.Log("スロット " + itemnumber + ": " + _items[itemnumber].MyItemName);
            }
            else
            {
                Debug.Log("スロット " + itemnumber + ": 空");
            }
        }
    }
}
