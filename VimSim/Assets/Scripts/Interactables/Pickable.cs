using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : Interactible
{

    private Inventory inv;
    public InventoryItem item;
    // Start is called before the first frame update
    void Start()
    {
        inv = FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        //Add  it to the inventory.
        inv.AddItem(item);
        //Probably also play a pop SFX or something, idk.
        GameObject.Destroy(gameObject);
    }

    //This shouldn't be doable, but nevermind.
    public override void UnInteract()
    {
        base.UnInteract();
    }
}
