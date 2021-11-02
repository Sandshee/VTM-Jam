using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
{
    public InventoryItem item;
    public Image icon;
    private Image background;
    public Color defaultCol;
    public Color selectedCol;
    private bool wasJustOver;
    public Tooltip tooltip;
    private bool selected;

    private Color clear = Color.clear;
    private Color visible = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
        if (item)
        {
            icon.sprite = item.icon;
            icon.color = visible;
            Debug.Log("But how?");
        }
        else
        {
            icon.color = clear;
        }
        //I should only have one, its quick, dirty, but oh well.
    }

    // Update is called once per frame
    void Update()
    {
        /*
         if (EventSystem.current.IsPointerOverGameObject())
         {
             if (!wasJustOver)
             {
                 tooltip.ShowTooltip(item);
                 wasJustOver = true;
                 Debug.Log("SHOW ME THE WAY!");
             }
         } else
         {
             if (wasJustOver)
             {
                 tooltip.HideTooltip(item);
                 wasJustOver = false;
             }
         }
         */

        if (selected)
        {
            background.color = selectedCol;

        } else
        {
            background.color = defaultCol;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //No point if I've no item to show.
        if (!item)
        {
            return;
        }

        if (!wasJustOver)
        {
            tooltip.ShowTooltip(item);
            wasJustOver = true;
        }
        Debug.Log("I was called");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //No point if I've no item to show.
        if (!item)
        {
            return;
        }

        if (wasJustOver)
        {
            tooltip.HideTooltip(item);
            wasJustOver = false;
        }
    }

    public void Select()
    {
        selected = true;
    }

    public void Deselect()
    {
        selected = false;
    }

    public void AssignItem(InventoryItem item)
    {
        this.item = item;
        icon.sprite = item.icon;
        icon.color = visible;
    }

    public void ClearItem()
    {
        this.item = null;
        icon.sprite = null;
        icon.color = clear;
    }
}
