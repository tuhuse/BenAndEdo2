using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }  // シングルトンインスタンス
    [SerializeField] private Inventory _playerInventory;  // プレイヤーのインベントリ
    //[SerializeField] private InventoryUI inventoryUI;  // InventoryUI の参照

    private void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // シーンをまたいでも破棄されないようにする
        }
        else
        {
            Destroy(gameObject);  // インスタンスが重複している場合、破棄する
            return;
        }

        // インベントリが設定されていない場合の警告
        if (_playerInventory == null)
        {
            Debug.LogWarning("Player Inventory が設定されていません。");
        }
    }

    // アイテムをインベントリに追加するメソッド
    public void AddItemToInventory(Item item)
    {
        // Null チェックを行い、インベントリが存在する場合のみ追加処理を行う
        if (_playerInventory != null && _playerInventory.AddItem(item))
        {
            Debug.Log(item.MyItemName + " をインベントリに追加しました。");

            //inventoryUI が設定されている場合のみ UI を更新
            //if (inventoryUI != null)
            //{
            //    inventoryUI.UpdateInventoryUI(_playerInventory.GetItems());
            //}
        }
        else
        {
            Debug.LogWarning("インベントリが設定されていないか、アイテムの追加に失敗しました。");
        }
    }
}
