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
    private LightItem _lightItem;
    // Start is called before the first frame update
    void Start()
    {
        _items = new Item[_incentorySize];
        _lightItem = GameObject.FindGameObjectWithTag("Player").GetComponent<LightItem>();
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

        if (_items[itemIndex].MyItemName != "KeyItem"&& _items[itemIndex].MyItemName != "LightItem")
        {
            RemoveItem(itemIndex); // インベントリから削除
            selectItem.UseItem();  // Execute the item's "Use" functionality
            Debug.Log(_items[itemIndex].MyItemName + " を使用しました。");
        }
        else if (_items[itemIndex].MyItemName=="LightItem")
        {
            if (!_lightItem._LightOn)
            {
                _lightItem._LightOn = true;
            }
            else
            {
                _lightItem._LightOn = false;
            }
        }
      
       

       
    }

}
