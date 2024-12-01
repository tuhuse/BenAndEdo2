using UnityEngine;

/// <summary>
/// ���ꂼ��̃A�C�e���������Ă���f�[�^
/// </summary>
public class WorldItem : MonoBehaviour
{
    [SerializeField] private Item _itemData = default;
    [SerializeField] private float _pickupRange = 2f;  // �v���C���[���߂Â���͈�
    private ItemInventory _inventoryManager = default;
    private Transform _playerTransform = default;  // �v���C���[�̈ʒu
    private GetItemUI _itemUI = default;

    private bool _isPlayerNear = false;  // �v���C���[���߂��ɂ��邩���Ǘ�

    private void Start()
    {
        _inventoryManager = GameObject.FindGameObjectWithTag("Inventory").GetComponent<ItemInventory>();
        _itemUI = FindFirstObjectByType<GetItemUI>();
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        // �v���C���[�Ƃ̋������m�F
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        if (distanceToPlayer <= _pickupRange)
        {
            if (!_isPlayerNear)
            {
                // �v���C���[���߂Â����u�Ԃ�UI���X�V
                _isPlayerNear = true;
                _itemUI.UpdateTextUI(_itemData);
            }

            // G�L�[�������ꂽ�ꍇ�A�A�C�e�����C���x���g���ɒǉ�
            if (Input.GetKeyDown(KeyCode.G))
            {
                AudioManager.Instance.ItemGetSE();
                _inventoryManager.AddItem(_itemData);
                _itemUI.TextOff();
                Destroy(gameObject);  // �A�C�e�����V�[������폜
            }
        }
        else
        {
            if (_isPlayerNear)
            {
                // �v���C���[�����ꂽ�u�Ԃ�UI���\��
                _isPlayerNear = false;
                _itemUI.TextOff();
            }
        }
    }
}
