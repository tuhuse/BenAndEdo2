using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponItem", menuName = "Inventory/WeaponItem")]
public class WeaponItem : Item
{

    public override void ItemEffect()
    {
        AudioManager.Instance.AttackSE();
        ItemManager.Instance.WeaponStart();
    }
   
}
