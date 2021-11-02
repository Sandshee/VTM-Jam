using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Readable : Interactible
{
    [TextArea(15,20)]
    public string readableText;
    private ReaderUI playerReader;
    // Start is called before the first frame update
    void Start()
    {
        playerReader = FindObjectOfType<ReaderUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        playerReader.ReadMe(this);
    }

    public override void UnInteract()
    {
        playerReader.PutDown();
    }
}
