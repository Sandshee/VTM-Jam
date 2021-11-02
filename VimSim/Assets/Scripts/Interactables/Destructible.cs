using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    private PlayerController player;
    public List<GameObject> pieces = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Damage()
    {
        foreach(GameObject g in pieces)
        {
            GameObject.Instantiate(g, transform.position, transform.rotation);
        }
        GameObject.Destroy(this.gameObject);
    }
}
