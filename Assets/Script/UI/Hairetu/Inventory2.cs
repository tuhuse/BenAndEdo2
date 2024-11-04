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
                Debug.Log(newItem._itemName + " ���C���x���g���ɒǉ����܂����B");
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
                Debug.Log(_items[itemnumber]._itemName + " ���C���x���g�����炳�����債�܂����B");
                return true;
            }
        }
        Debug.Log("�A�C�e����������܂���ł���");
        return false;
    }
    // �C���x���g���̓��e��\�����郁�\�b�h
    public void DisplayInventory()
    {
        Debug.Log("�C���x���g���̓��e:");
        for (int itemnumber = 0; itemnumber < _items.Length; itemnumber++)
        {
            if (_items[itemnumber] != null)
            {
                Debug.Log("�X���b�g " + itemnumber + ": " + _items[itemnumber]._itemName);
            }
            else
            {
                Debug.Log("�X���b�g " + itemnumber + ": ��");
            }
        }
    }
}
