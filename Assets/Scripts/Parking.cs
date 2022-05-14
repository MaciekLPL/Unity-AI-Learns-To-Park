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
        if(position.x < 0)
        this.transform.eulerAngles = new Vector3(0f,180f,0f);

    }
}
