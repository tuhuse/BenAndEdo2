using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance { get; private set; } 
    private const int MAX_BatteryLife = 60;
    [SerializeField] private Light _flashlight;
    [SerializeField]private float _batteryLife = 60f;  // �o�b�e���[�����i�b�j
    public bool _lightOn { get; private set; }

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
       
        if (_lightOn)
        {
            LightStart();
           
        }
  
       
    }
    public void LightActive()
    {
        if (!_lightOn)
        {
            _lightOn =true;
            if (_batteryLife > 0)
            {
                _flashlight.enabled = true;
            }
        }
        else
        {
            _flashlight.enabled = false;
            _lightOn = false;
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
                _lightOn = false;


            }
        }
    }
    public void GetBattery()
    {
        if (_batteryLife < MAX_BatteryLife)
        {
            _batteryLife = MAX_BatteryLife;
        }
    }
}
