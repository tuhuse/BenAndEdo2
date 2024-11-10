using UnityEngine;

public class LightItem : MonoBehaviour
{
    private Light flashlight;
    public float batteryLife = 60f;  // �o�b�e���[�����i�b�j

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
            batteryLife -= Time.deltaTime;  // �o�b�e���[�c�ʂ����炷

            if (batteryLife <= 0)
            {
                flashlight.enabled = false;  // �o�b�e���[���Ȃ��Ȃ�����I�t
                isFlashlightOn = false;
            }
        }
    }
}
