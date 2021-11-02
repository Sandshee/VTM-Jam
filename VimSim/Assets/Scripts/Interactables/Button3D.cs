using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button3D : Interactible
{
    public PoweredObject destination;
    public float duration = 2f;
    public bool locked = false;
    public bool endGame = false;

    public Material onPress;
    private Material[] defaultMats;
    private Material[] onPressMats;

    public UnityEvent onButtonPress;
    private bool running;
    MeshRenderer mr;
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        defaultMats = mr.materials;
        onPressMats = mr.materials;
        onPressMats[1] = onPress;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        if (!running)
        {
            mr.materials = onPressMats;
            StartCoroutine(returnColour());
            onButtonPress.Invoke();
            running = true;
        }
        Debug.Log("Boop!");
        if (!locked)
        {
            destination.Power(duration);
            //FindObjectOfType<PlayerController>().Uninteract();
            //Play unlocked animation.
        } else
        {
            //Play locked animation.
        }
    }

    IEnumerator returnColour()
    {
        yield return new WaitForSeconds(3);

        mr.materials = defaultMats;
        running = false;
    }
}
