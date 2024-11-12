using UnityEngine;

public class LightItem : MonoBehaviour
{
    [SerializeField] private Light _flashlight;
    public float _batteryLife = 60f;  // バッテリー寿命（秒）
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
            _batteryLife -= Time.deltaTime;  // バッテリー残量を減らす

            if (_batteryLife <= 0)
            {
                _flashlight.enabled = false;  // バッテリーがなくなったらオフ
                _lightOn = false;

            }
        }
    }

}
