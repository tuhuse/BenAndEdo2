using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GetItemUI : MonoBehaviour
{
    [SerializeField] private Text _getItemText = default;
    [SerializeField] private GameObject _gButtonImage;
    // Start is called before the first frame update
    void Start()
    {
   
    }

    public void UpdateTextUI(Item item)
    {
        _gButtonImage.SetActive(true);
        _getItemText.enabled = true;
        _getItemText.text = item.MyItemJapaneseName+"ÇèEÇ§";
    }
    public void TextOff()
    {
        _getItemText.enabled = false;
        _gButtonImage.SetActive(false);
    }
}
