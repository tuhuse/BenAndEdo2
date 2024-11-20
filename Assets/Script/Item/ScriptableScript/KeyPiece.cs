
using UnityEngine;

[CreateAssetMenu(fileName = "KeyPiece",menuName = "Inventory/KeyPiece")]
public class KeyPiece : Item
{
    public override void ItemEffect()
    {
        ItemManager.Instance.MakeKey();
    }
}
