using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HealItem", menuName = "Inventory/Item")]
public class HealItem : Item
{
    [SerializeField] private ValueManager _valueManager;
    public int PlayerHP =>_valueManager._playerHP;
   
    public override void UseItem()
    {
        base.UseItem();
        if (PlayerHP < 3)
        {
           _valueManager.Heal();
        }
    }
}
