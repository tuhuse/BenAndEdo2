using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Heal,
        Weapon,
        KeyItem
    }

    [SerializeField] private string _itemName;
    [SerializeField] private int _itemID;
    [SerializeField] private Sprite _icon;
    [SerializeField] private ItemType _itemType;
    [SerializeField] private bool _isStackable;
    [SerializeField, TextArea] private string _description;
    [SerializeField] private int _maxStack = 5;
    //[SerializeField] private InventorySlot _inventorySlot;

    public string MyItemName => _itemName;
    public int MyItemID => _itemID;
    public Sprite MyIcon => _icon;
    public ItemType MyItemType => _itemType;
    public bool IsStackable => _isStackable;
    public string Description => _description;
    public int MaxStack => _maxStack;

   

    public virtual void UseItem()
    {
        
        //_inventorySlot.ClearSlot();
    }
}
