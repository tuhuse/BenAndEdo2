using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public abstract class Item : ScriptableObject
{


    [SerializeField] private string _itemName;
    [SerializeField] private int _itemID;
    [SerializeField] private Sprite _icon;
    [SerializeField] private bool _isStackable;
    [SerializeField, TextArea] private string _description;
    [SerializeField] private int _maxStack = 5;
    [SerializeField] private ItemType _itemType;
    public enum ItemType
    {
        HealItem,
        WeaponItem,
        Light,
        LightBattery,
        KeyPiece,
        Key
    }

   /// <summary>
   /// ƒAƒCƒeƒ€‚ÌŽí—Þ•ª‚¯
   /// </summary>
    public ItemType MyItemType => _itemType;
    public string MyItemName => _itemName;
    public Sprite MyIcon => _icon;
    public bool IsStackable => _isStackable;
    public string Description => _description;
    public int MaxStack => _maxStack;



    public abstract void ItemEffect();
}
