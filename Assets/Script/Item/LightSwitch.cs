using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LightSwitch", menuName = "Inventory/LightSwitch")]
public class LightSwitch : Item
{
    private LightItem _lightItem;
    private void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _lightItem = player.GetComponent<LightItem>();
        }
       
    }

    public override void UseItem()
    {
        _lightItem.LightActive();
    }
}
