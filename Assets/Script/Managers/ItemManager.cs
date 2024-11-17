using UnityEngine;
using System.Collections;
public class ItemManager : MonoBehaviour
{
    //�����d��
    public static ItemManager Instance { get; private set; }

    [SerializeField] private Inventory _inventory;
    private const int MAX_Battery_Life = 60;
    [SerializeField] private Light _flashlight;
    [SerializeField] private float _batteryLife = 60f;  // �o�b�e���[�����i�b�j
    public bool LightOn { get; private set; }

    //�U���A�C�e��
    [SerializeField] private BoxCollider _weapon;

    //���I�ȓz
    [SerializeField]
    private GameObject _perfectKey;
    [SerializeField]
    private GameObject _unionKey;
    private void Awake()
    {
        // �V���O���g���p�^�[���̎���
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);  // �C���X�^���X���d�����Ă���ꍇ�A�j������
            return;
        }

    }
    void Start()
    {
        _flashlight.enabled = false;
    }

    void Update()
    {

        if (LightOn)
        {
            LightStart();

        }


    }
    #region �����d������
    public void LightActive()
    {
        if (!LightOn)
        {
            LightOn = true;
            if (_batteryLife > 0)
            {
                _flashlight.enabled = true;
            }
        }
        else
        {
            _flashlight.enabled = false;
            LightOn = false;
        }
    }
    private void LightStart()
    {

        if (_batteryLife > 0)
        {

            if (!_flashlight.enabled)
            {
                _flashlight.enabled = true;

            }
            _batteryLife -= Time.deltaTime;  // �o�b�e���[�c�ʂ����炷

            if (_batteryLife <= 0)
            {
                _flashlight.enabled = false;  // �o�b�e���[���Ȃ��Ȃ�����I�t
                LightOn = false;


            }
        }
    }
    public void GetBattery()
    {
        if (_batteryLife < MAX_Battery_Life)
        {
            _batteryLife = MAX_Battery_Life;
        }
    }
    #endregion
    #region �U���A�C�e������
    private IEnumerator WeaponAttack()
    {
        _weapon.enabled = true;
        yield return new WaitForSeconds(1f);
        _weapon.enabled = false;
    }
    public void WeaponStart()
    {
        StartCoroutine(WeaponAttack());
    }
    #endregion
    public void KeyCountPlus()
    {
        if (_inventory.KeyCnt == 3)
        {
            _perfectKey = Instantiate(_unionKey, _perfectKey.transform.position, Quaternion.identity);
        }
    }
}
