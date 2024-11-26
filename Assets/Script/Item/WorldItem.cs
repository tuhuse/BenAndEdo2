using UnityEngine;
using static UnityEditor.Progress;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private Item _itemData;  // このオブジェクトが持っているアイテムデータ
    [SerializeField] private float _pickupRange = 2f;  // プレイヤーが近づける範囲
    private ItemInventory _inventoryManager;
    private Transform _playerTransform;  // プレイヤーの位置

    
    private void Start()
    {
        _inventoryManager = GameObject.FindGameObjectWithTag("Inventory").GetComponent<ItemInventory>();
        _playerTransform = GameObject.FindWithTag("Player").transform;        
    }

    private void Update()
    {
        // プレイヤーとの距離を確認
        if (Vector3.Distance(transform.position, _playerTransform.position) <= _pickupRange)
        {
            // Gキーが押された場合、アイテムをインベントリに追加
            if (Input.GetKeyDown(KeyCode.G))
            {

                _inventoryManager.AddItem(_itemData);
               
                Destroy(gameObject);  // アイテムをシーンから削除
            }
        }
    }
}
