//using System.Collections.Generic;
//using UnityEngine;

//public class InventoryUI : MonoBehaviour
//{
//    [SerializeField] private GameObject slotPrefab;  // InventorySlotのプレハブ
//    [SerializeField] private Transform slotParent;  // Grid Layout Groupのあるパネル

//    private List<InventorySlot> slots = new List<InventorySlot>();

//    private void Start()
//    {
//        // 必要に応じて初期スロットを作成
//        for (int i = 0; i < 5; i++)  // 10個のスロットを例として生成
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
