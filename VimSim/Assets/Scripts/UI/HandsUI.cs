using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandsUI : MonoBehaviour
{
    public Sprite lookUI;
    public Sprite grabUI;
    public Sprite touchUI;
    public Sprite defaultUI;

    public Image icon;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetIcon(Sprite imageUI)
    {
        icon.sprite = imageUI;
    }

    public void SetLook()
    {
        SetIcon(lookUI);
    }

    public void SetGrab()
    {
        SetIcon(grabUI);
    }

    public void SetTouch()
    {
        SetIcon(touchUI);
    }

    public void ResetIcon()
    {
        SetIcon(defaultUI);
    }
}
