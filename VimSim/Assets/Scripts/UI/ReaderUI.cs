using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReaderUI : MonoBehaviour
{
    private Readable currentlyReading;
    public TextMeshProUGUI textMesh;
    public Image background;
    private bool isReading = false;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (!currentlyReading)
        {
            //That's fine, just don't render it.
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReadMe(Readable readable)
    {
        currentlyReading = readable;
        textMesh.SetText(readable.readableText);
        isReading = true;
        Debug.Log("YAY!");
        anim.SetBool("Visible", true);
    }

    public void PutDown()
    {
        anim.SetBool("Visible", false);
    }
}
