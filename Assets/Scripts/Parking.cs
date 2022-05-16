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

        Debug.DrawRay(transform.position, transform.forward* 10f, Color.green);
        //Debug.DrawRay(transform.position, transform.right* 10f, Color.green);
        //Debug.DrawRay(transform.position, -transform.right* 10f, Color.green);
        //Debug.DrawRay(transform.position, -transform.forward* 10f, Color.green);
    }

    public void respawn(Vector3 position)
    {
        this.transform.position = position;
        float rotY = (position.x < 0f) ? -90f : 90f;
        this.transform.eulerAngles = new Vector3(0f,rotY,0f);
    }
}
