using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �C���x���g����UI���Ǘ����Ă���
/// </summary>
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Image[] _inventorySlots;
    [SerializeField] private Image[] _inventoryBoxUI;
    [SerializeField] private Image[] _inventoryBoxCoolDownUI;
    [SerializeField] private Text[] _indexText;
    [SerializeField] private List<Text> _lightBatteryText;
    private const int UNSELECT_INVENTORY_COLOR = 80;
    private const int SELECT_INVENTORY_COLOR = 255;
    private const int MAX_COOLDOWN = 3;
    private const int MAX_FILLAMOUNT = 1;
    private int _selectSlot;
    private float _coolDown=0;
    private bool _isUseItem = false;
    private void Update()
    {
        UpdateCoolDown();
    }
    /// <summary>
    /// �C���x���g���ɃA�C�e���̃X�v���C�g��ۑ�����
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <param name="itemIcon"></param>
    public void UpdateSlotImage(int slotIndex, Sprite itemIcon,Item.ItemType light)
    {
        if (slotIndex < _inventorySlots.Length)
        {
            _inventorySlots[slotIndex].sprite = itemIcon;
            _inventorySlots[slotIndex].enabled = true;
            _inventorySlots[slotIndex].enabled = itemIcon != null;
        }if (light == Item.ItemType.Light)
        {
            _lightBatteryText[slotIndex].enabled = true;
        }
    }
    /// <summary>
    /// �C���x���g���ɓ����Ă���A�C�e���̃X�v���C�g������
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <param name="itemIcon"></param>
    public void DeleteSlotImage(int slotIndex, Sprite itemIcon)
    {
        _inventorySlots[slotIndex].enabled = itemIcon = null;
        _indexText[slotIndex].enabled = false;
    }
    /// <summary>
    /// �����J�E���g����UI�ł킩��悤�ɂ���
    /// </summary>
    /// <param name="slotIndex"></param>
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
    /// <summary>
    /// �g�p������������炵��UI�ɕ\������
    /// </summary>
    /// <param name="slotIndex"></param>
    public void DeleteSlotText(int slotIndex,int argument)
    {
        int cuurentCount = int.Parse(_indexText[slotIndex].text);//������
        if (cuurentCount != 0)
        {
            cuurentCount -= argument;
            _indexText[slotIndex].text = cuurentCount.ToString();
        }
        else
        {
            _indexText[slotIndex].enabled = false;
        }

    }
    /// <summary>
    /// �����d���̃o�b�e���[�󋵂�\��
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <param name="batteryLife"></param>
    /// <param name="maxBatteryLife"></param>
    public void UpdateBatteryText(int slotIndex, float batteryLife, float maxBatteryLife)
    {
        // �e�L�X�g�I�u�W�F�N�g���j������Ă��邩�`�F�b�N
        if (_lightBatteryText == null || _lightBatteryText[slotIndex] == null)
        {
            Debug.LogWarning("Battery text reference is missing for slot " + slotIndex);
            return; // ���S�ɃX�L�b�v
        }

        // �o�b�e���[�c�ʂ��v�Z���ĕ\��
        int batteryPercentage = Mathf.CeilToInt((batteryLife / maxBatteryLife) * 100);
        _lightBatteryText[slotIndex].text = batteryPercentage.ToString() + "%";
    }


    /// <summary>
    /// �I�����Ă���C���x���g���̐F�̓����x��ς��Ă���
    /// </summary>
    /// <param name="selectIndex"></param>
    public void SelectInventoryUI(int selectIndex)
    {

        for (int selectNumber = 0; selectNumber < _indexText.Length; selectNumber++)
        {

            Color32 textColor = _indexText[selectNumber].color;
            Color32 batteryTextColor = _lightBatteryText[selectNumber].color;
            Color32 itemImageColor = _inventorySlots[selectNumber].color;
            Color32 inventoryBoxColor = _inventoryBoxUI[selectNumber].color;
            if (selectNumber == selectIndex)
            {
                textColor.a =SELECT_INVENTORY_COLOR; // ���S�s����
                itemImageColor.a = SELECT_INVENTORY_COLOR;
                inventoryBoxColor.a = SELECT_INVENTORY_COLOR;
                batteryTextColor.a = SELECT_INVENTORY_COLOR;
            }
            else
            {
                textColor.a =UNSELECT_INVENTORY_COLOR; // �������i��j
                itemImageColor.a = UNSELECT_INVENTORY_COLOR;
                inventoryBoxColor.a = UNSELECT_INVENTORY_COLOR;
                batteryTextColor.a = UNSELECT_INVENTORY_COLOR;
            }

            _indexText[selectNumber].color = textColor;
            _lightBatteryText[selectNumber].color=batteryTextColor;
            _inventorySlots[selectNumber].color = itemImageColor;
            _inventoryBoxUI[selectNumber].color = inventoryBoxColor;
        }

    }
    public void CoolDownUI(int selectSlot) 
    {
        _selectSlot = selectSlot;
        _isUseItem = true;
    }
    private void UpdateCoolDown()
    {
        if (_isUseItem)
        {
            // �N�[���^�C�����i�s��
            if (_coolDown < MAX_COOLDOWN)
            {
                _coolDown += Time.deltaTime; // �o�ߎ��Ԃ����Z
                float fillRatio = 1 - (_coolDown / MAX_COOLDOWN); // �c�莞�Ԃɉ����Ċ������v�Z
                _inventoryBoxCoolDownUI[_selectSlot].enabled = true;
                _inventoryBoxCoolDownUI[_selectSlot].fillAmount = Mathf.Clamp(fillRatio, 0, MAX_FILLAMOUNT); // fillAmount���X�V
            }
            else
            {
                // �N�[���^�C�����I��
                _isUseItem = false;
                _inventoryBoxCoolDownUI[_selectSlot].enabled = false;
                _inventoryBoxCoolDownUI[_selectSlot].fillAmount = MAX_FILLAMOUNT; // ���S���Z�b�g
                _coolDown = 0f; // �J�E���^�[�����Z�b�g
            }
        }
    }



}
