using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{

    // �U���A�C�e���֘A
    [SerializeField] 
    private BoxCollider _weaponCollider = default; // ����̃R���C�_�[

    // ���A�C�e���֘A
    [SerializeField]
    private Item _completedKey = default;
    [SerializeField]
    private CashBox _cashBox = default;
    private const float WAIT_TIME = 0.5f;
    private const int MAX_KEY_COUNT = 3;
  
    // �����d���֘A
    private const int MAX_BATTERY_LIFE = 100; // �o�b�e���[�����̍ő�l
    [SerializeField]
    private Light[] _flashLight = default;
    [SerializeField] 
    private float _batteryLife = MAX_BATTERY_LIFE; // ���݂̃o�b�e���[�����i�b�j
    private int _lightInventorySlotNumber = default;
    [SerializeField] private InventoryUI _inventoryUI = default;
    [SerializeField] private CameraView _cameraView = default;

    // �V���O���g���p�^�[���p�̃C���X�^���X
    public static ItemManager Instance { get; private set; }
    public bool LightOn { get; private set; } // �����d�����I�����ǂ���

    public Light[] FlashLight { get => _flashLight; set=>_flashLight=value; }
    private void Awake()
    {
        // �V���O���g���p�^�[���̎���
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // �C���X�^���X���d�����Ă���ꍇ�͔j��
            return;
        }
    }
    void Update()
    {
        // �����d�����I���̏ꍇ�A�o�b�e���[��������J�n
        if (LightOn)
        {
            UpdateLight();
            
        }
    }

    #region �����d������
    /// <summary>
    /// �����d���̃I���I�t�؂�ւ�
    /// </summary>
    public void LightActive()
    {
        if (!LightOn)
        {
            LightOn = true;
            if (_batteryLife > 0)
            {
                _cameraView.LightSwitch(); // �o�b�e���[������΃��C�g���I��
            }
        }
        else
        {
            _cameraView.LightOff();// ���C�g���I�t
            LightOn = false;
        }
    }
    /// <summary>
    /// �����d���̃o�b�e���[�����
    /// </summary>
    private void UpdateLight()
    {
        if (_batteryLife > 0)
        {

            _batteryLife -= Time.deltaTime; // �o�b�e���[������������

            // �o�b�e���[�c�ʂ��C���x���g���ɍX�V
            _inventoryUI.UpdateBatteryText(_lightInventorySlotNumber, _batteryLife, MAX_BATTERY_LIFE);

            if (_batteryLife <= 0)
            {
                _cameraView.LightOff(); // �o�b�e���[���؂ꂽ�烉�C�g���I�t
                LightOn = false;
            }
        }
    }
    /// <summary>
    /// �����d�����ǂ��̃C���x���g���[�ŃQ�b�g������
    /// </summary>
    /// <param name="slotIndex"></param>
    public void GetLightInventoryNumber(int slotIndex)
    {
        _lightInventorySlotNumber = slotIndex;
        // �����o�b�e���[�c�ʂ��C���x���g���ɔ��f
        _inventoryUI.UpdateBatteryText(_lightInventorySlotNumber, _batteryLife, MAX_BATTERY_LIFE);
    }
    /// <summary>
    /// �d�r���g�p�����Ƃ��o�b�e���[��
    /// </summary>
    public void GetBattery()
    {

        if (_batteryLife < MAX_BATTERY_LIFE)
        {
            _batteryLife = MAX_BATTERY_LIFE;
        }
    }
    #endregion

    #region �U���A�C�e������
    /// <summary>
    /// �U������
    /// </summary>
    /// <returns></returns>
    private IEnumerator WeaponAttack()
    {
        // ����U���̏����i��莞�ԃR���C�_�[��L�����j
        _weaponCollider.enabled = true; // ����̃R���C�_�[��L����
        yield return new WaitForSeconds(1f); // 1�b��ɖ�����
        _weaponCollider.enabled = false;
    }
    /// <summary>
    /// �U�����}
    /// </summary>
    public void WeaponStart()
    {
        // ����U���̊J�n
        StartCoroutine(WeaponAttack());
    }
    #endregion
    #region ���֘A�̏���
    /// <summary>
    /// ���������J�M���C���x���g���Ɋi�[���鏈��
    /// </summary>
    public void KeyInstantiate()
    {
        StartCoroutine(AddKey());
    }
    private IEnumerator AddKey()
    {
        yield return new WaitForSeconds(WAIT_TIME);
        InventoryManager.Instance.AddItem(_completedKey);
    }
    /// <summary>
    /// ���̂����炪����Ό�����鏈��
    /// </summary>
    public void MakeKey()
    {
        if (InventoryManager.Instance.KeyCount < MAX_KEY_COUNT)
        {
            Debug.Log("�f�ނ�����܂���");
        }
        else
        {
            KeyInstantiate();
        }

    }
    /// <summary>
    /// ���ɂƂ̋������߂������献�ŋ��ɂ������鏈��
    /// </summary>
    public void OpenKey()
    {
        _cashBox.OpenCashBox();
    }
    #endregion
}
