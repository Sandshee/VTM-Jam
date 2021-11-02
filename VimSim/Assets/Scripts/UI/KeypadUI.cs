using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeypadUI : PadUI
{
    public string code = "0451";
    public string attempt = "";
    public TextMeshProUGUI textMesh;
    private PoweredObject power;
    private bool locked = false;
    public bool cardReader = false;
    private InventoryItem requiredItem;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cardReader)
        {
            textMesh.text = attempt;
        }
        else
        {
            textMesh.text = attempt.PadRight(4, '_');
        }
    }

    public void AddInt(int i)
    {
        //char c = i.ToString().ToCharArray()[0];
        //You can't enter a code longer
        if(attempt.Length >= code.Length)
        {
            return;
        }

        attempt += i;
    }

    public void Clear()
    {
        attempt = "";
    }

    public void Enter()
    {
        if (cardReader)
        {
            if (requiredItem == FindObjectOfType<InventoryUI>().GetItem())
            {
                //Tell the thing.
                power.Power(-1);
                //Lock this, hide it, and unlock the player.
                FindObjectOfType<PlayerController>().Uninteract();
                attempt = "GRANTED";
                this.Hide();
            } else
            {
                attempt = "DENIED";
            }
;
        }
        else
        {
            if (attempt.Equals(code))
            {
                //Tell the thing.
                power.Power(-1);
                //Lock this, hide it, and unlock the player.
                FindObjectOfType<PlayerController>().Uninteract();
                attempt = "";
                this.Hide();
            }
            else
            {
                Clear();
                //Bzzt
            }
        }
    }

    public override void Display(PadInfo info, PoweredObject power)
    {
        if (!locked)
        {
            code = info.code;
            this.power = power;
            requiredItem = info.item;
            if (cardReader)
            {
                FindObjectOfType<PlayerController>().Freeze();
                FindObjectOfType<InventoryUI>().Display();
            }
            base.Display(info, power);
        }
    }

    public override void Hide()
    {
        if (cardReader)
        {
            //FindObjectOfType<PlayerController>().Freeze();
            FindObjectOfType<InventoryUI>().Hide();
        }
        base.Hide();
    }
}
