using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padlock : Destructible
{
    public PoweredObject door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Damage()
    {
        door.Power(-1);
        base.Damage();
    }
}
