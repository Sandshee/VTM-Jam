using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private bool displaying = false;
    private Animator anim;
    public InventoryItem currentItem;
    public List<ItemUI> items = new List<ItemUI>();
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Display()
    {
        anim.SetBool("Display", true);
        displaying = true;
    }

    public void Hide()
    {
        anim.SetBool("Display", false);
        displaying = false;
    }

    public bool GetDisplaying()
    {
        return displaying;
    }

    public void Select(ItemUI item)
    {
        if (item.item)
        {
            currentItem = item.item;
            foreach (ItemUI i in items)
            {
                i.Deselect();
            }
            item.Select();
        }
    }

    public InventoryItem GetItem()
    {
        return currentItem;
    }
}
