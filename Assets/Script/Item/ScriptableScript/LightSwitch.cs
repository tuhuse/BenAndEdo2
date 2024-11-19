using UnityEngine;
[CreateAssetMenu(fileName = "LightSwitch", menuName = "Inventory/LightSwitch")]
public class LightSwitch : Item
{
 
    public override void ItemEffect()
    {
       ItemManager.Instance.LightActive();
    }
}
