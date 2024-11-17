using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Image[] _inventorySlots;
    [SerializeField] private Text[] _indexText;
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
        _indexText[slotIndex].enabled = false;
    }
    public void UpdateSlotText(int slotIndex)
    {
        if (!_indexText[slotIndex].enabled)
        {
            _indexText[slotIndex].enabled = true;
        }
        
        if (_indexText != null && slotIndex < _indexText.Length)
        {           
            int currentCount = int.Parse(_indexText[slotIndex].text); // 現在のカウントを取得
            currentCount += 1; // カウントを加算
            _indexText[slotIndex].text = currentCount.ToString(); // 新しいのを表示する( ^)o(^ )ｂ
        }
        else
        {
            Debug.LogWarning("スロットインデックスが無効です: " + slotIndex);
        }
    }
    public void DeleteSlotText(int slotindex)
    {
        int cuurentCount = int.Parse(_indexText[slotindex].text);//同じく
        if (cuurentCount != 1)
        {
            cuurentCount -= 1;
            _indexText[slotindex].text = cuurentCount.ToString();
        }
        else
        {
            _indexText[slotindex].enabled = false;
        }
       
    }

}
