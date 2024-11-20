﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    // インベントリ内のアイテムを格納する配列
    [SerializeField] private Item[] _items;
    [SerializeField] private CashBox _cashBox;
    // インベントリUIの参照
    [SerializeField] private InventoryUI _inventoryUI;
    // アイテムの最小数（スタック処理で使用）
    private const int MIN_ITEM = 1;
    // インベントリの最大サイズ
    [SerializeField] private int _inventorySize = 5;
    // 現在選択しているインベントリの番号
    [SerializeField] private int _selectInventoryNumber = 0;
    // アイテムの個別カウント
    private int _healCnt;
    private int _weaponCnt;
    private int _lightBatteryCnt;
    private bool _isUse = true; // アイテムを使用可能かどうか


    // シングルトンのインスタンス
    public static InventoryManager Instance { get; private set; }
    /// <summary>
    /// プロパティ
    /// </summary>
    public int KeyCount { get; private set; } // キーアイテムのカウント

    private void Awake()
    {
        // シングルトンパターンの設定
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // インスタンスが重複している場合、破棄する
            return;
        }
    }

    void Start()
    {
        // インベントリ配列を初期化
        _items = new Item[_inventorySize];
    }

    private void Update()
    {
        // インベントリ選択とアイテム使用の入力処理
        SelectInventory();
        UseClick();
    }

    /// <summary>
    /// アイテムを使う
    /// </summary>
    private void UseClick()
    {
      
        if (Input.GetMouseButtonDown(1) && _isUse)
        {
            UseItem(_selectInventoryNumber);
        }
    }
    /// <summary>
    /// クールタイム
    /// </summary>
    private IEnumerator UseCoolTime()
    {
        // アイテム使用のクールタイム処理
        int waitTime = 3;
        _isUse = false;
        yield return new WaitForSeconds(waitTime);
        _isUse = true;
    }
    /// <summary>
    /// インベントリ選択番号の更新
    /// </summary>
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
    /// <summary>
    /// アイテムを追加する処理
    /// </summary>
    /// <param name="newItem"></param>
    /// <returns></returns>
    public bool AddItem(Item newItem)
    {
        // インベントリ内の既存アイテムと一致するか確認
        for (int itemNumber = 0; itemNumber < _items.Length; itemNumber++)
        {
            if (_items[itemNumber] != null && newItem.MyItemName == _items[itemNumber].MyItemName && newItem.IsStackable)
            {
                // スタック可能ならスタックを増加
                AddStack(newItem, itemNumber);
                Debug.Log(newItem.MyItemName + " のスタック数を増加しました。");
                return true;
            }
        }

        // 空きスロットを探して新しいアイテムを追加
        for (int itemNumber = 0; itemNumber < _items.Length; itemNumber++)
        {
            if (_items[itemNumber] == null)
            {
                _items[itemNumber] = newItem;
                _inventoryUI.UpdateSlotImage(itemNumber, newItem.MyIcon);
                AddStack(newItem, itemNumber); // スタック数も初期化
                Debug.Log(newItem.MyItemName + " をインベントリに追加しました。");
                return true;
            }
        }

        Debug.Log("インベントリが満タンです");
        return false;
    }
    /// <summary>
    /// アイテムをインベントリから消す処理
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public bool RemoveItem(int itemIndex)
    {
        // 指定スロットからアイテムを削除
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
    /// <summary>
    /// アイテムを使用時の効果とインベントリの処理
    /// </summary>
    /// <param name="itemIndex"></param>
    public void UseItem(int itemIndex)
    {

        string keyPiece = "KeyPiece";
        string key = "Key";
        string light = "Light";
        
        if (itemIndex < 0 || itemIndex >= _items.Length || _items[itemIndex] == null)
        {
            Debug.Log("使用できるアイテムがありません");
            return;
        }

        Item selectItem = _items[itemIndex];

       
        if (_items[itemIndex].MyItemName != keyPiece && _items[itemIndex].MyItemName != light&&_items[itemIndex].MyItemName != key)
        {
            Debug.Log(_items[itemIndex].MyItemName + " を使用しました。");
            DecreaseStack(_items[itemIndex], itemIndex);
            selectItem.ItemEffect();
            StartCoroutine(UseCoolTime());
        }
        else if (_items[itemIndex].MyItemName == light)
        {
            // 特定アイテム（Light）の使用
            selectItem.ItemEffect();
        }
        else if (_items[itemIndex].MyItemName == keyPiece)
        {
            selectItem.ItemEffect();
            DecreaseStack(_items[itemIndex], itemIndex);
            // 特定アイテム（KeyItem）の使用
        }
        else if (_items[itemIndex].MyItemName == key)
        {
            if (_cashBox.OpenDoor)
            {
                RemoveItem(itemIndex);
            }
            selectItem.ItemEffect();
        }
    }
    /// <summary>
    /// ストックできるアイテムの時
    /// </summary>
    /// <param name="item"></param>
    /// <param name="itemIndex"></param>
    private void AddStack(Item item, int itemIndex)
    {
        // アイテムスタック数を増加
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
                if (item.MaxStack > KeyCount)
                {
                    KeyCount++;
                    _inventoryUI.UpdateSlotText(itemIndex);
                }
                break;
        }
    }
    /// <summary>
    /// ストックできるアイテムを使った時
    /// </summary>
    /// <param name="item"></param>
    /// <param name="deleteItem"></param>
    private void DecreaseStack(Item item, int deleteItem)
    {
        // アイテムスタック数を減少、スタックが0ならアイテム削除
        switch (item.MyItemID)
        {
            case 1:
                if (_weaponCnt <= item.MaxStack && _weaponCnt > MIN_ITEM)
                {
                    _weaponCnt--;
                }
                else if (_weaponCnt == MIN_ITEM)
                {
                    _weaponCnt--;
                    RemoveItem(deleteItem);
                }
                break;
            case 2:
                if (_lightBatteryCnt <= item.MaxStack && _lightBatteryCnt > MIN_ITEM)
                {
                    _lightBatteryCnt--;
                }
                else if (_lightBatteryCnt == MIN_ITEM)
                {
                    _lightBatteryCnt--;
                    RemoveItem(deleteItem);
                }
                break;
            case 3:
                if (_healCnt <= item.MaxStack && _healCnt > MIN_ITEM)
                {
                    _healCnt--;
                }
                else if (_healCnt == MIN_ITEM)
                {
                    _healCnt--;
                    RemoveItem(deleteItem);
                }
                break;
            case 4:
                if (KeyCount == item.MaxStack)
                {
                    RemoveItem(deleteItem);
                    KeyCount -= 3;
                }
                break;
        }
        _inventoryUI.DeleteSlotText(deleteItem);
    }
}
