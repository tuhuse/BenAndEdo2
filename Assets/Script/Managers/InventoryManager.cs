using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }  // シングルトンインスタンス
    [SerializeField] private Inventory playerInventory;  // プレイヤーのインベントリ
    //[SerializeField] private InventoryUI inventoryUI;  // InventoryUI の参照
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
    public void AddItemToInventory(Item item)
    {
        if (playerInventory.AddItem(item))
        {
            Debug.Log(item.MyItemName + " をインベントリに追加しました。");
            //inventoryUI.UpdateInventoryUI(playerInventory.GetItems());  // UIを更新
        }
    }

}
