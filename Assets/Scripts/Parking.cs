using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parking : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void respawn(Vector3 position)
    {
        this.transform.position = position;
        if (transform.localPosition.x < 0)
            transform.localEulerAngles = new Vector3(0f, -90f, 0f);
        else
            transform.localEulerAngles = new Vector3(0f, +90f, 0f);
    }
}
