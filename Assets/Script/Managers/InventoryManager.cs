using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }  // シングルトンインスタンス
    [SerializeField] private Inventory2 playerInventory;  // プレイヤーのインベントリ

    private void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // アイテムをインベントリに追加するメソッド
    public void AddItemToInventory(GetItem item)
    {
        if (playerInventory.AddItem(item))
        {
            Debug.Log(item._itemName + " をインベントリに追加しました。");
        }
    }

}
