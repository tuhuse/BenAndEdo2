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
            int currentCount = int.Parse(_indexText[slotIndex].text); // ���݂̃J�E���g���擾
            currentCount += 1; // �J�E���g�����Z
            _indexText[slotIndex].text = currentCount.ToString(); // �V�����̂�\������( ^)o(^ )��
        }
        else
        {
            Debug.LogWarning("�X���b�g�C���f�b�N�X�������ł�: " + slotIndex);
        }
    }
    public void DeleteSlotText(int slotindex)
    {
        int cuurentCount = int.Parse(_indexText[slotindex].text);//������
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
