using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredObject : MonoBehaviour
{
    private int coroutineCount;
    public bool powered = false;
    public bool invert = false;
    public int doorIndex = -1;
    public int threshHoldOpens;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Power(float duration)
    {

        powered = true;
        if (duration >= 0)
        {
            StartCoroutine(Hold(duration));
        }

    }
    IEnumerator Hold(float duration)
    {
        coroutineCount++;
        yield return new WaitForSeconds(duration);
        coroutineCount--;

        if(coroutineCount == 0)
        {
            powered = false;
            //Disable power
        }
    }

    public bool GetPowered()
    {
        if (invert)
        {
            return !powered;
        }
        else
        {
            return powered;
        }
    }
}
