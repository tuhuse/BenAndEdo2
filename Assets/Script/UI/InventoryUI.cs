using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel; // Inventory�̃p�l��
    [SerializeField] private GameObject itemPrefab; // �A�C�e���̃v���n�u
    private Inventory inventory;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    public void UpdateInventoryUI()
    {
        // ������UI�A�C�e�����N���A
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // �C���x���g�����̊e�A�C�e����UI�ɕ\��
        foreach (InventoryBase item in inventory.items)
        {
            GameObject newItemUI = Instantiate(itemPrefab, inventoryPanel.transform);
            newItemUI.GetComponent<Image>().sprite = item.icon;
            newItemUI.GetComponentInChildren<Text>().text = item.quantity.ToString();
        }
    }
}
