using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    public bool freezePlayer = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Activate()
    {
        Debug.Log("For use when interacting with an object.");
    }

    public virtual void Interact()
    {
        Debug.Log("For use when interacting with an object.");
    }

    public virtual void UnInteract()
    {
        Debug.Log("For use when uninteracting with an object.");
    }

    public bool DoesFreezePlayer()
    {
        return freezePlayer;
    }
}
