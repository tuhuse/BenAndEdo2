using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    [SerializeField] private Item[] _items;
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
        if (Input.GetMouseButtonDown(1))
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
        Item selectItem = _items[itemIndex];

        if (_items[itemIndex].MyItemName != "KeyItem"&& _items[itemIndex].MyItemName != "Light")
        {
            Debug.Log(_items[itemIndex].MyItemName + " を使用しました。");
            selectItem.UseItem(); 
            RemoveItem(itemIndex); // インベントリから削除
           
        }
        else if (_items[itemIndex].MyItemName=="Light")
        {
            selectItem.UseItem();
            print("ehehe");
        }
       
    }

}
