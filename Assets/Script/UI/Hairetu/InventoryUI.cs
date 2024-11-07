using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Image[] _inventorySlots;

    public void UpdateSlotImage(int slotIndex, Sprite itemIcon)
    {
        if (slotIndex < _inventorySlots.Length)
        {
            _inventorySlots[slotIndex].sprite = itemIcon;
            _inventorySlots[slotIndex].enabled = true ;
            _inventorySlots[slotIndex].enabled = itemIcon != null; 
        }
    }
    public void DeleteSlotImage(int slotIndex,Sprite itemIcon)
    {
        _inventorySlots[slotIndex].enabled = itemIcon = null;
    }
}
