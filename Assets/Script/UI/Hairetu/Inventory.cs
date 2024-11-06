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
                Debug.Log(newItem.MyItemName + " をインベントリに追加しました。");
                return true;
            }
        }
        Debug.Log("満タンです");
        return false;
    }public bool RemoveItem(int itemID)
    {
        for(int itemnumber = 0; itemnumber < _items.Length; itemnumber++)
        {
            if (_items[itemnumber] != null)
            {
                _items[itemnumber] = null;
                _itemIcon[itemnumber] = null;
                Debug.Log(_items[itemnumber].MyItemName + " をインベントリからさくじょしました。");
                return true;
            }
        }
        Debug.Log("アイテムが見つかりませんでした");
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
