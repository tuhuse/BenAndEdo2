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
    // Start is called before the first frame update
    void Start()
    {
        _items = new Item[_incentorySize];
    }
    private void Update()
    {
        if (Use())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _items[0].UseItem();
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
                //_itemIcon[itemnumber] = newItem.MyIcon;
                Debug.Log(newItem.MyItemName + " ���C���x���g���ɒǉ����܂����B");
                return true;
            }
        }
        Debug.Log("���^���ł�");
        return false;
    }public bool RemoveItem(int itemID)
    {
        for(int itemnumber = 0; itemnumber < _items.Length; itemnumber++)
        {
            if (_items[itemnumber] != null)
            {
                _items[itemnumber] = null;
                _itemIcon[itemnumber] = null;
                Debug.Log(_items[itemnumber].MyItemName + " ���C���x���g�����炳�����債�܂����B");
                return true;
            }
        }
        Debug.Log("�A�C�e����������܂���ł���");
        return false;
    }
    public bool Use()
    {
        if (_items != null)
        {
            return true;
        }
        else
        {
            return false;
        }
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
