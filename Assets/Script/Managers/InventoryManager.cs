using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }  // �V���O���g���C���X�^���X
    [SerializeField] private Inventory2 playerInventory;  // �v���C���[�̃C���x���g��

    private void Awake()
    {
        // �V���O���g���p�^�[���̎���
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �A�C�e�����C���x���g���ɒǉ����郁�\�b�h
    public void AddItemToInventory(GetItem item)
    {
        if (playerInventory.AddItem(item))
        {
            Debug.Log(item._itemName + " ���C���x���g���ɒǉ����܂����B");
        }
    }

}
