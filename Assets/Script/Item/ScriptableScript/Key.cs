using UnityEngine;
[CreateAssetMenu(fileName ="Key",menuName ="Inventory/Key")]
public class Key : Item
{
    
    
    public override void ItemEffect()
    {
        ItemManager.Instance.OpenKey();
    }
}
