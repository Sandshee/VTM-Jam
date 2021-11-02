using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AutomaticTrigger : MonoBehaviour
{
    BoxCollider col;
    public PoweredObject door;
    int objects;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(objects < 0)
        {
            Debug.LogError("Negative objects");
        }

        if(objects > 0)
        {
            door.Power(2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objects++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objects--;
        }
    }
}
