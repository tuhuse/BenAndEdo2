using UnityEngine;

public class InventoryBase:MonoBehaviour
{
    public string itemName;
    public Sprite icon;
    public int quantity;

    public InventoryBase(string name, Sprite iconSprite, int number)
    {
        itemName = name;
        icon = iconSprite;
        quantity = number;
    }

    public virtual void Use()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
        // ��{�I�Ȏg�p���@�B�h���N���X�ŃI�[�o�[���C�h��
        Debug.Log(itemName);
    }
}
public class HealingItem : InventoryBase
{
    public int healAmount;
    [SerializeField]private ValueManager _valueManager;
    private const int MAXHP=3;
    public HealingItem(string name, Sprite iconSprite, int qty, int heal)
        : base(name, iconSprite, qty)
    {
        healAmount = heal;
        if (_valueManager._playerHP < MAXHP)
        {
            _valueManager._playerHP++;
        }
    }

    public override void Use()
    {
        base.Use();
        Debug.Log(itemName + " used to heal for " + healAmount + " points!");
        // �����Ńv���C���[���񕜂��鏈����ǉ�
    }
}

