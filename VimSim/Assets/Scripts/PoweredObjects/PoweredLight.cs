using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredLight : MonoBehaviour
{
    private MeshRenderer mr;
    public Material unpoweredMat;
    public Material poweredMat;
    public PoweredObject powerSource;
    private Material[] materialsArr;
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        materialsArr = mr.materials;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (powerSource.GetPowered())
        {
            //mr.material = poweredMat;
            Debug.Log("POWER!");
            materialsArr[1] = poweredMat;
        } else
        {
            //mr.material = unpoweredMat;
            materialsArr[1] = unpoweredMat;
        }

        mr.materials = materialsArr;
    }
}
