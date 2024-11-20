using UnityEngine;
[CreateAssetMenu(fileName ="Key",menuName ="Inventory/Key")]
public class Key : Item
{
    private CashBox _cashBox;
    private void Awake()
    {
        _cashBox = GameObject.FindGameObjectWithTag("CashBox").GetComponent<CashBox>();
    }
    public override void ItemEffect()
    {       
        _cashBox.OpenCashBox();
    }
}
