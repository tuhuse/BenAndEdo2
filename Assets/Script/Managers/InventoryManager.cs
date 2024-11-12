using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }  // �V���O���g���C���X�^���X
    [SerializeField] private Inventory _playerInventory;  // �v���C���[�̃C���x���g��
    //[SerializeField] private InventoryUI inventoryUI;  // InventoryUI �̎Q��

    private void Awake()
    {
        // �V���O���g���p�^�[���̎���
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // �V�[�����܂����ł��j������Ȃ��悤�ɂ���
        }
        else
        {
            Destroy(gameObject);  // �C���X�^���X���d�����Ă���ꍇ�A�j������
            return;
        }

        // �C���x���g�����ݒ肳��Ă��Ȃ��ꍇ�̌x��
        if (_playerInventory == null)
        {
            Debug.LogWarning("Player Inventory ���ݒ肳��Ă��܂���B");
        }
    }

    // �A�C�e�����C���x���g���ɒǉ����郁�\�b�h
    public void AddItemToInventory(Item item)
    {
        // Null �`�F�b�N���s���A�C���x���g�������݂���ꍇ�̂ݒǉ��������s��
        if (_playerInventory != null && _playerInventory.AddItem(item))
        {
            Debug.Log(item.MyItemName + " ���C���x���g���ɒǉ����܂����B");

            //inventoryUI ���ݒ肳��Ă���ꍇ�̂� UI ���X�V
            //if (inventoryUI != null)
            //{
            //    inventoryUI.UpdateInventoryUI(_playerInventory.GetItems());
            //}
        }
        else
        {
            Debug.LogWarning("�C���x���g�����ݒ肳��Ă��Ȃ����A�A�C�e���̒ǉ��Ɏ��s���܂����B");
        }
    }
}
