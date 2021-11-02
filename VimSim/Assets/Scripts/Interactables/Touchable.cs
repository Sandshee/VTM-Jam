using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchable : Interactible
{
    public PadUI pad;
    public PadInfo info;
    public PoweredObject power;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        pad.Display(info, power);
    }

    public override void UnInteract()
    {
        pad.Hide();
    }
}
