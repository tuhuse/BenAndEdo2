using UnityEngine;

public class LightItem : MonoBehaviour
{
    private const int MAX_BatteryLife = 60;
    [SerializeField] private Light _flashlight;
    [SerializeField]private float _batteryLife = 60f;  // バッテリー寿命（秒）
    public bool _lightOn { get; private set; }

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
        print("yorp");
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
    public void GetBattery()
    {
        if (_batteryLife < MAX_BatteryLife)
        {
            _batteryLife = MAX_BatteryLife;
        }
    }
}
