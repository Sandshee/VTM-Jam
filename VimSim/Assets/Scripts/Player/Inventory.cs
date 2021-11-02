using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public List<ItemUI> itemUIs = new List<ItemUI>();
    public int inventorySize = 9;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
        UpdateUI();
    }

    private void UpdateUI()
    {
        for(int i = 0; i < inventorySize; i++)
        {
            if (items.Count > i)
            {
                itemUIs[i].AssignItem(items[i]);
            } else
            {
                itemUIs[i].ClearItem();
            }
        }
    }
}
