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
                Debug.Log(newItem.MyItemName + " ���C���x���g���ɒǉ����܂����B");
                return true;
            }
        }
        Debug.Log("���^���ł�");
        return false;
    }public bool RemoveItem(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= _items.Length || _items[itemIndex] == null)
        {
            Debug.Log("�A�C�e����������܂���ł���");
            return false;
        }

        Debug.Log(_items[itemIndex].MyItemName + " ���C���x���g������폜���܂����B");
        _inventoryUI.DeleteSlotImage(itemIndex, _items[itemIndex].MyIcon);
        _items[itemIndex] = null;
        
        return true;
    }
    public void Use(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= _items.Length || _items[itemIndex] == null)
        {
            Debug.Log("�g�p�ł���A�C�e��������܂���");
            return;
        }

        // Assuming the Item class has a Use method
        _items[itemIndex].UseItem();  // Execute the item's "Use" functionality
        Debug.Log(_items[itemIndex].MyItemName + " ���g�p���܂����B");

        RemoveItem(itemIndex); // Remove the item after using it
    }
    public Item[] GetItems()
    {
        return _items;
    }

    // �C���x���g���̓��e��\�����郁�\�b�h
    public void DisplayInventory()
    {
        Debug.Log("�C���x���g���̓��e:");
        for (int itemnumber = 0; itemnumber < _items.Length; itemnumber++)
        {
            if (_items[itemnumber] != null)
            {
                Debug.Log("�X���b�g " + itemnumber + ": " + _items[itemnumber].MyItemName);
            }
            else
            {
                Debug.Log("�X���b�g " + itemnumber + ": ��");
            }
        }
    }
}
