using UnityEngine;
using static UnityEditor.Progress;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private Item _itemData;  // ���̃I�u�W�F�N�g�������Ă���A�C�e���f�[�^
    [SerializeField] private float _pickupRange = 2f;  // �v���C���[���߂Â���͈�
    private ItemInventory _inventoryManager;
    private Transform _playerTransform;  // �v���C���[�̈ʒu

    
    private void Start()
    {
        _inventoryManager = GameObject.FindGameObjectWithTag("Inventory").GetComponent<ItemInventory>();
        _playerTransform = GameObject.FindWithTag("Player").transform;        
    }

    private void Update()
    {
        // �v���C���[�Ƃ̋������m�F
        if (Vector3.Distance(transform.position, _playerTransform.position) <= _pickupRange)
        {
            // G�L�[�������ꂽ�ꍇ�A�A�C�e�����C���x���g���ɒǉ�
            if (Input.GetKeyDown(KeyCode.G))
            {

                _inventoryManager.AddItem(_itemData);
               
                Destroy(gameObject);  // �A�C�e�����V�[������폜
            }
        }
    }
}
