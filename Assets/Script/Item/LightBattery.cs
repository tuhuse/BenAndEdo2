using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightBattery", menuName = "Inventory/LightBattery")]
public class LightBattery : Item
{
    private LightItem _lightItem;
  
    private  void Start()
    {
            _lightItem =GameObject.FindGameObjectWithTag("Player").GetComponent<LightItem>();
        
    }
    public override void UseItem()
    {
        _lightItem.GetBattery();
    }
}
