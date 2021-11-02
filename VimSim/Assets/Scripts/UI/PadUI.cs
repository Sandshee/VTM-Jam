using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadUI : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Display(PadInfo info, PoweredObject power)
    {
        anim.SetBool("Display", true);
    }

    public virtual void Hide()
    {
        anim.SetBool("Display", false);
    }
}
