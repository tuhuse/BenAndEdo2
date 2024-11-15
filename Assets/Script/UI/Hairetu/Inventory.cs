using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    [SerializeField] private Item[] _items;
    [SerializeField] private int _incentorySize = 5;
    [SerializeField] private int _selectInventorynumber = 0;
    [SerializeField] private InventoryUI _inventoryUI;
    private const int Min_Item = 1;
    private const int Max_Item = 3;
    private int _healCnt;
    private int _weaponCnt;
    private int _lightBatteryCnt;
    public int KeyCnt { get; private set; }
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
        for (int addnumber = 0; addnumber < _incentorySize; addnumber++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + addnumber))
            {
                _selectInventorynumber = addnumber;
                break;
            }
        }
    }

    public bool AddItem(Item newItem)
    {
        for (int itemnumber = 0; itemnumber < _items.Length; itemnumber++)
        {
            if (_items[itemnumber] == null)
            {
                _items[itemnumber] = newItem;
                _inventoryUI.UpdateSlotImage(itemnumber, newItem.MyIcon);
                //_itemIcon[itemnumber] = newItem.MyIcon;
                Debug.Log(newItem.MyItemName + " をインベントリに追加しました。");
                return true;
            }
            else if (newItem.MyItemName == _items[itemnumber].MyItemName&&newItem.IsStackable)
            {
                AddStack(newItem);
            }
        }
        Debug.Log("満タンです");
        return false;
    }
    public bool RemoveItem(int itemIndex)
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
        if (_items[itemIndex].MyItemName != "KeyItem" && _items[itemIndex].MyItemName != "Light")
        {
            Debug.Log(_items[itemIndex].MyItemName + " を使用しました。");
            DecreaseStack(_items[itemIndex],itemIndex);
            selectItem.UseItem();
            
        }
        else if (_items[itemIndex].MyItemName == "Light")
        {
            selectItem.UseItem();
        }
        else if (_items[itemIndex].MyItemName == "KeyItem")
        {

        }

    }
    private void AddStack(Item item)
    {
        switch (item.MyItemID)
        {
            case 1:
                if (item.MaxStack > _weaponCnt)
                {
                    _weaponCnt++;
                }
                break;
            case 2:
                if (item.MaxStack > _lightBatteryCnt)
                {
                    _lightBatteryCnt++;
                }
               
                break;
            case 3:

                if (item.MaxStack > _healCnt)
                {
                    _healCnt++;
                }
                break;
            case 4:
                if (item.MaxStack > KeyCnt)
                {
                    KeyCnt++;
                }else if (item.MaxStack == KeyCnt)
                {

                }
                break;
        }


    }
    private void DecreaseStack(Item item,int deleteItem)
    {
        switch (item.MyItemID)
        {
            case 1:
                if (_weaponCnt > 2&&_weaponCnt==Max_Item)
                {
                    _weaponCnt--;
                    
                }else if (_weaponCnt == Min_Item)
                {
                    _weaponCnt--;
                    RemoveItem(deleteItem);
                }
                break;
            case 2:
                if (_lightBatteryCnt > 2 &&_lightBatteryCnt == Max_Item)
                {
                    _lightBatteryCnt--;
                }
                else if (_lightBatteryCnt == Min_Item)
                {
                    _lightBatteryCnt--;
                    RemoveItem(deleteItem);
                }
                break;
            case 3:
                if (_healCnt > 2 && _healCnt == Max_Item)
                {
                    _healCnt--;
                }
                else if (_healCnt == Min_Item)
                {
                    _healCnt--;
                    RemoveItem(deleteItem);
                }
                break;
         
        }
       
    }
    private void KeyUse()
    {
        if (ItemManager.Instance.KeyCount <= 3)
        {

        }
    }
}
