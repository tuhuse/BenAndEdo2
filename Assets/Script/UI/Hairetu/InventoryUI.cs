//using System.Collections.Generic;
//using UnityEngine;

//public class InventoryUI : MonoBehaviour
//{
//    [SerializeField] private GameObject slotPrefab;  // InventorySlot�̃v���n�u
//    [SerializeField] private Transform slotParent;  // Grid Layout Group�̂���p�l��

//    private List<InventorySlot> slots = new List<InventorySlot>();

//    private void Start()
//    {
//        // �K�v�ɉ����ď����X���b�g���쐬
//        for (int i = 0; i < 5; i++)  // 10�̃X���b�g���Ƃ��Đ���
//        {
//            GameObject slotObj = Instantiate(slotPrefab, slotParent);
//            InventorySlot slot = slotObj.GetComponent<InventorySlot>();
//            slots.Add(slot);
//            slot.ClearSlot();
//        }
//    }

//    public void UpdateInventoryUI(Item[] items)
//    {
//        for (int slotNumber = 0; slotNumber < slots.Count; slotNumber++)
//        {
//            if (slotNumber < items.Length && items[slotNumber] != null)
//            {
//                slots[slotNumber].SetItem(items[slotNumber]);
//            }
//            else
//            {
//                slots[slotNumber].ClearSlot();
//            }
//        }
//    }
//}
