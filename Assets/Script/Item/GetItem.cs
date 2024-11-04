using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GetItem : MonoBehaviour
{
    public int _itemID;
    public string _itemName;
    public GetItem(string itemname, int itemID)
    {
        _itemID = itemID;
        _itemName = itemname;
    }

}
