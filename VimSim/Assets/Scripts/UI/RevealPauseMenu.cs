using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealPauseMenu : MonoBehaviour
{
    private PlayerController player;
    public Animator anim;
    private bool canMenu;
    private bool revealed;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Oof that's a lot of nested ifs.
        if(Input.GetAxisRaw("Cancel") > 0 && !player.GetFrozen())
        {
            //Reveal the menu.
            if (canMenu)
            {
                canMenu = false;
                if (player.GetInteracting() && revealed){
                    Hide();
                } else if (!player.GetInteracting() && !revealed)
                {
                    Reveal();
                }
            }
        }

        if(Input.GetAxisRaw("Cancel") == 0)
        {
            canMenu = true;
        }
    }

    public void Reveal()
    {
        player.Freeze();
        anim.SetBool("Display", true);
    }

    public void Hide()
    {
        player.UnFreeze();
        anim.SetBool("Display", false);
    }
}
