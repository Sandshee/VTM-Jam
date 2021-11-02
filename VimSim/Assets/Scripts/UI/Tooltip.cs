using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private Camera uiCamera;

    private InventoryItem item;
    public TextMeshProUGUI tooltipText;
    public RectTransform backgroundRectTransform;

    private void Awake()
    {
        HideTooltip();
    }

    private void Update()
    {
        //Vector2 localPoint;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        //transform.position = Input.mousePosition;
    }

    public void ShowTooltip(InventoryItem item)
    {

        if(gameObject.activeSelf && this.item == item)
        {
            return; //No point.
        }
        this.item = item;
        //gameObject.SetActive(true);

        tooltipText.text = item.description;
        //float textPaddingSize = 4f;
        //Vector2 backGroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2f, tooltipText.preferredHeight + textPaddingSize * 2f);
        //backgroundRectTransform.sizeDelta = backGroundSize;
    }

    public void HideTooltip(InventoryItem item)
    {
        if (this.item == item)
        {
            HideTooltip();
        }
    }

    private void HideTooltip()
    {
        //gameObject.SetActive(false);
        tooltipText.text = "";
    }
}
