using UnityEngine;

public class LightItem : MonoBehaviour
{
    [SerializeField] private Light _flashlight;
    public float _batteryLife = 60f;  // �o�b�e���[�����i�b�j
    public bool _lightOn = false;


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
    public void LightActive(bool juge)
    {
        if (juge)
        {
            _lightOn = true;

        }
        else
        {
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

}
