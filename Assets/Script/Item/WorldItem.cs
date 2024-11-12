using UnityEngine;
using static UnityEditor.Progress;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private Item _itemData;  // ���̃I�u�W�F�N�g�������Ă���A�C�e���f�[�^
    [SerializeField] private float _pickupRange = 2f;  // �v���C���[���߂Â���͈�

    private Transform playerTransform;  // �v���C���[�̈ʒu

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        // �v���C���[�Ƃ̋������m�F
        if (Vector3.Distance(transform.position, playerTransform.position) <= _pickupRange)
        {
            // G�L�[�������ꂽ�ꍇ�A�A�C�e�����C���x���g���ɒǉ�
            if (Input.GetKeyDown(KeyCode.G))
            {
                InventoryManager.Instance.AddItemToInventory(_itemData);
               
                Destroy(gameObject);  // �A�C�e�����V�[������폜
            }
        }
    }
}
