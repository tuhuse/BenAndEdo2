using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    [SerializeField] private Item[] _items;
    [SerializeField] private int _inventorySize = 5;
    [SerializeField] private int _selectInventoryNumber = 0;
    [SerializeField] private InventoryUI _inventoryUI;
    private const int Min_Item = 1;
    private int _healCnt;
    private int _weaponCnt;
    private int _lightBatteryCnt;

    private bool _isUse = true;
    public int KeyCnt { get; private set; }

    private void Awake()
    {
      
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);  // �C���X�^���X���d�����Ă���ꍇ�A�j������
            return;
        }

    }
    void Start()
    {
        _items = new Item[_inventorySize];

    }
    private void Update()
    {
        SelectInventory();
        UseClick();

    }
    private void UseClick()
    {
       
            if (Input.GetMouseButtonDown(1)&& _isUse)
            {
                UseItem(_selectInventoryNumber);
            
                
            }
        
    }
    private IEnumerator UseCoolTime()
    {
        int waitTime = 3;
        _isUse = false;
        yield return new WaitForSeconds(waitTime);
        _isUse = true;
    }
    private void SelectInventory()
    {
        for (int addnumber = 0; addnumber < _inventorySize; addnumber++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + addnumber))
            {
                _selectInventoryNumber = addnumber;
                break;
            }
        }
    }

    public bool AddItem(Item newItem)
    {
        // �C���x���g�����̊����A�C�e���ƈ�v���邩�m�F
        for (int itemNumber = 0; itemNumber < _items.Length; itemNumber++)
        {
            if (_items[itemNumber] != null && newItem.MyItemName == _items[itemNumber].MyItemName && newItem.IsStackable)
            {
                // �X�^�b�N�\�Ȃ�X�^�b�N�𑝉�
                AddStack(newItem,itemNumber);
                Debug.Log(newItem.MyItemName + " �̃X�^�b�N���𑝉����܂����B");
                return true;
            }
        }

        // �󂫃X���b�g��T���ĐV�����A�C�e����ǉ�
        for (int itemNumber = 0; itemNumber < _items.Length; itemNumber++)
        {
            if (_items[itemNumber] == null)
            {
                _items[itemNumber] = newItem;
                _inventoryUI.UpdateSlotImage(itemNumber, newItem.MyIcon);
                AddStack(newItem, itemNumber); // �X�^�b�N����������
                Debug.Log(newItem.MyItemName + " ���C���x���g���ɒǉ����܂����B");
                return true;
            }
        }

        Debug.Log("�C���x���g�������^���ł�");
        return false;
    }

    public bool RemoveItem(int itemIndex)
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
    public void UseItem(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= _items.Length || _items[itemIndex] == null)
        {
            Debug.Log("�g�p�ł���A�C�e��������܂���");
            return;
        }
        Item selectItem = _items[itemIndex];
        if (_items[itemIndex].MyItemName != "KeyItem" && _items[itemIndex].MyItemName != "Light")
        {
            Debug.Log(_items[itemIndex].MyItemName + " ���g�p���܂����B");
            DecreaseStack(_items[itemIndex],itemIndex);
            selectItem.UseItem();
            StartCoroutine(UseCoolTime());
        }
        else if (_items[itemIndex].MyItemName == "Light")
        {
            selectItem.UseItem();
        }
        else if (_items[itemIndex].MyItemName == "KeyItem")
        {

        }

    }
    private void AddStack(Item item, int itemIndex)
    {
        switch (item.MyItemID)
        {
            case 1:
                if (item.MaxStack > _weaponCnt)
                {
                   
                    _weaponCnt++;
                    _inventoryUI.UpdateSlotText(itemIndex);
                }
                break;
            case 2:
                if (item.MaxStack > _lightBatteryCnt)
                {
                    _lightBatteryCnt++;
                    _inventoryUI.UpdateSlotText(itemIndex);
                }
               
                break;
            case 3:

                if (item.MaxStack > _healCnt)
                {
                    _healCnt++;
                    _inventoryUI.UpdateSlotText(itemIndex);
                }
                break;
            case 4:
                if (item.MaxStack > KeyCnt)
                {
                    KeyCnt++;
                    _inventoryUI.UpdateSlotText(itemIndex);
                }
                break;
        }
        

    }
    private void DecreaseStack(Item item,int deleteItem)
    {
        switch (item.MyItemID)
        {
            case 1:
                if (_weaponCnt <= item.MaxStack && _weaponCnt>Min_Item)
                {
                    _weaponCnt--;

                } else if (_weaponCnt == Min_Item)
                {
                    _weaponCnt--;
                    RemoveItem(deleteItem);
                }
                break;
            case 2:
                if (_lightBatteryCnt <= item.MaxStack && _lightBatteryCnt > Min_Item)
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
                if (_healCnt <= item.MaxStack && _healCnt > Min_Item)
                {
                    _healCnt--;
                }
                else if (_healCnt == Min_Item)
                {
                    _healCnt--;
                    RemoveItem(deleteItem);
                }
                break;
            case 4:
                if (KeyCnt == item.MaxStack)
                {
                    RemoveItem(deleteItem);
                    UseItem(deleteItem);
                }
                break;
               
        }
        _inventoryUI.DeleteSlotText(deleteItem);
    }

}
