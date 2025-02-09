using UnityEngine;

/// <summary>
/// それぞれのアイテムが持っているデータ
/// </summary>
public class WorldItem : MonoBehaviour
{
    [SerializeField] private Item _itemData = default;
    [SerializeField] private float _pickupRange = 2f;  // プレイヤーが近づける範囲
    [SerializeField] private Material _newMaterial;
     [SerializeField] private Material _genMaterial;
  [SerializeField] private MeshRenderer _genMesh;
    private ItemInventory _inventoryManager = default;
    private Transform _playerTransform = default;  // プレイヤーの位置
    private GetItemUI _itemUI = default;

    private bool _isPlayerNear = false;  // プレイヤーが近くにいるかを管理
    private bool _isChang = false;
    private void Start()
    {
        _inventoryManager = GameObject.FindGameObjectWithTag("Inventory").GetComponent<ItemInventory>();
        _itemUI = FindFirstObjectByType<GetItemUI>();
        _playerTransform = GameObject.FindWithTag("Player").transform;
       
       
    }

    private void Update()
    {
        // プレイヤーとの距離を確認
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        if (distanceToPlayer <= _pickupRange)
        {
            if (!_isPlayerNear)
            {
                // プレイヤーが近づいた瞬間にUIを更新
                _isPlayerNear = true;
                _itemUI.UpdateTextUI(_itemData);
            }

            // Gキーが押された場合、アイテムをインベントリに追加
            if (Input.GetKeyDown(KeyCode.G))
            {
                AudioManager.Instance.ItemGetSE();
                _inventoryManager.AddItem(_itemData);
                _itemUI.TextOff();
                Destroy(gameObject);  // アイテムをシーンから削除
            }
        }
        else
        {
            if (_isPlayerNear)
            {
                // プレイヤーが離れた瞬間にUIを非表示
                _isPlayerNear = false;
                _itemUI.TextOff();
            }
        }
        if (_itemData.MyItemType != Item.ItemType.Light)
        {
            if (_isChang)
            {
                _genMesh.material = _newMaterial;
            }
            else
            {
                _genMesh.material = _genMaterial;
            }
        }
       
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("LightCollider"))
        {
            _isChang = true;
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LightCollider"))
        {
            _isChang = false;
           
        }
    }
}
