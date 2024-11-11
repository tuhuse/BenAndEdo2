using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightBattery", menuName = "Inventory/LightBattery")]
public class LightBattery : Item
{
    private LightItem _lightItem;
    private const int MAX_BatteryLife = 60;
    private void Awake()
    {
        _lightItem = GameObject.FindGameObjectWithTag("Player").GetComponent<LightItem>();
    }
    public override void UseItem()
    {
        if (_lightItem._batteryLife <MAX_BatteryLife)
        {
            _lightItem._batteryLife = MAX_BatteryLife;
        }
    }
}
