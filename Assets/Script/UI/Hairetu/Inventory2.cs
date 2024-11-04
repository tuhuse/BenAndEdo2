using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory2 : MonoBehaviour
{
    [SerializeField] private GetItem[] _items;
    [SerializeField] private int _incentorySize=5;  
    // Start is called before the first frame update
    void Start()
    {
        _items = new GetItem[_incentorySize];
    }

  public bool AddItem(GetItem newItem)
    {
        for(int itemnumber = 0; itemnumber < _items.Length; itemnumber++)
        {
            if (_items[itemnumber] == null)
            {
                _items[itemnumber] = newItem;
                Debug.Log(newItem._itemName + " をインベントリに追加しました。");
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
                Debug.Log(_items[itemnumber]._itemName + " をインベントリからさくじょしました。");
                return true;
            }
        }
        Debug.Log("アイテムが見つかりませんでした");
        return false;
    }
    // インベントリの内容を表示するメソッド
    public void DisplayInventory()
    {
        Debug.Log("インベントリの内容:");
        for (int itemnumber = 0; itemnumber < _items.Length; itemnumber++)
        {
            if (_items[itemnumber] != null)
            {
                Debug.Log("スロット " + itemnumber + ": " + _items[itemnumber]._itemName);
            }
            else
            {
                Debug.Log("スロット " + itemnumber + ": 空");
            }
        }
    }
}
