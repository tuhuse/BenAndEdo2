using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class GetItem : ScriptableObject
{
    [SerializeField] private string _itemName;
    [SerializeField] private int _itemID;
    [SerializeField] private Sprite _icon; // �A�C�e���̃A�C�R���i�C�Ӂj

    // �v���p�e�B�ŊO������A�N�Z�X
    public string MyItemName
    {
        get { return _itemName; }
    }
    public int MyItemID
    {
        get { return _itemID; }
    }
    public Sprite MyIcon
    {
        get { return _icon; }
    }
    public virtual void UseItem()
    {
        
    }
}
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class HealItem : GetItem
{
    private int _healAmount = 1;
    public int HealAmount
    {
        get { return _healAmount;}
    }

}
