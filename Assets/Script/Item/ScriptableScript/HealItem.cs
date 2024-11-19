using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewHealItem", menuName = "Inventory/HealItem")]
public class HealItem : Item
{
   
    public override void ItemEffect()
    {
        ValueManager.Instance.Heal();
    }
}
