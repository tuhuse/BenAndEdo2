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
    //[SerializeField] private InventorySlot _inventorySlot;

    public string MyItemName => _itemName;
    public int MyItemID => _itemID;
    public Sprite MyIcon => _icon;
    public bool IsStackable => _isStackable;
    public string Description => _description;
    public int MaxStack => _maxStack;



    public abstract void UseItem();
}
