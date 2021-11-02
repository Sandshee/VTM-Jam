using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetinaScanner : MonoBehaviour
{

    public CameraLook playerCamera;
    private int peopleInside;
    public PoweredObject door;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (peopleInside > 0)
        {
            if(Vector3.Angle(playerCamera.transform.forward, transform.forward) > 90)
            {
                Debug.DrawRay(transform.position, transform.forward, Color.red);
                door.Power(0.1f);
            } else
            {
                Debug.DrawRay(transform.position, transform.forward, Color.green);
            }
        }
        else if (peopleInside == 0)
        {
        } else
        {
            Debug.LogError("Negative people inside");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            peopleInside++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            peopleInside--;
        }
    }
}
