using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel; // Inventoryのパネル
    [SerializeField] private GameObject itemPrefab; // アイテムのプレハブ
    private Inventory inventory;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    public void UpdateInventoryUI()
    {
        // 既存のUIアイテムをクリア
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // インベントリ内の各アイテムをUIに表示
        foreach (InventoryBase item in inventory.items)
        {
            GameObject newItemUI = Instantiate(itemPrefab, inventoryPanel.transform);
            newItemUI.GetComponent<Image>().sprite = item.icon;
            newItemUI.GetComponentInChildren<Text>().text = item.quantity.ToString();
        }
    }
}
