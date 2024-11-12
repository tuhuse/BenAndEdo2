using UnityEngine;
using static UnityEditor.Progress;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private Item _itemData;  // このオブジェクトが持っているアイテムデータ
    [SerializeField] private float _pickupRange = 2f;  // プレイヤーが近づける範囲

    private Transform playerTransform;  // プレイヤーの位置

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        // プレイヤーとの距離を確認
        if (Vector3.Distance(transform.position, playerTransform.position) <= _pickupRange)
        {
            // Gキーが押された場合、アイテムをインベントリに追加
            if (Input.GetKeyDown(KeyCode.G))
            {
                InventoryManager.Instance.AddItemToInventory(_itemData);
               
                Destroy(gameObject);  // アイテムをシーンから削除
            }
        }
    }
}
