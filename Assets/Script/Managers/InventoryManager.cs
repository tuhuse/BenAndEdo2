using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }  // �V���O���g���C���X�^���X
    [SerializeField] private Inventory playerInventory;  // �v���C���[�̃C���x���g��
    //[SerializeField] private InventoryUI inventoryUI;  // InventoryUI �̎Q��
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
    public void AddItemToInventory(Item item)
    {
        if (playerInventory.AddItem(item))
        {
            Debug.Log(item.MyItemName + " ���C���x���g���ɒǉ����܂����B");
            //inventoryUI.UpdateInventoryUI(playerInventory.GetItems());  // UI���X�V
        }
    }

}
