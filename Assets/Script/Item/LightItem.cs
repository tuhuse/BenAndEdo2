using UnityEngine;

public class LightItem : MonoBehaviour
{
    private Light flashlight;
    public float batteryLife = 60f;  // バッテリー寿命（秒）

    private bool isFlashlightOn = false;

    void Start()
    {
        flashlight = GetComponentInChildren<Light>();
        flashlight.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && batteryLife > 0)
        {
            isFlashlightOn = !isFlashlightOn;
            flashlight.enabled = isFlashlightOn;
        }

        if (isFlashlightOn && batteryLife > 0)
        {
            batteryLife -= Time.deltaTime;  // バッテリー残量を減らす

            if (batteryLife <= 0)
            {
                flashlight.enabled = false;  // バッテリーがなくなったらオフ
                isFlashlightOn = false;
            }
        }
    }
}
