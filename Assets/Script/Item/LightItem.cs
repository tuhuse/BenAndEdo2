using UnityEngine;
[CreateAssetMenu(fileName = "LightItem", menuName = "Inventory/LightItem")]
public class LightItem : MonoBehaviour
{
    private Light _flashlight;
    public float _batteryLife = 60f;  // �o�b�e���[�����i�b�j
    public bool _LightOn = false;
    private bool _isFlashlightOn = false;

    void Start()
    {
        _flashlight = GetComponentInChildren<Light>();
        _flashlight.enabled = false;
    }

    void Update()
    {
        if (_LightOn)
        {
            if (_batteryLife > 0)
            {
                _isFlashlightOn = !_isFlashlightOn;
                _flashlight.enabled = _isFlashlightOn;
            }

            if (_isFlashlightOn && _batteryLife > 0)
            {
                _batteryLife -= Time.deltaTime;  // �o�b�e���[�c�ʂ����炷

                if (_batteryLife <= 0)
                {
                    _flashlight.enabled = false;  // �o�b�e���[���Ȃ��Ȃ�����I�t
                    _isFlashlightOn = false;
                }
            }
        }
    }
       


}
