using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightBattery", menuName = "Inventory/LightBattery")]
public class LightBattery : Item
{

    public override void UseItem()
    {        
            ItemManager.Instance.GetBattery();
        
    }
}