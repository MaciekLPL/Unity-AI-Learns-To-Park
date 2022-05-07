using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parking : MonoBehaviour
{
    // Start is called before the first frame update
    float yPos = 0.5f;
    float[] xPos = {-9.78f, -5.05f, 2.245f, 6.97f};
    float[] zPos = { 10.1f, 8.13f, 6.14f, 4.14f, 2.14f, 0.15f, -1.88f, -3.89f, -5.87f, -7.9f, 9.87f};
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void respawn()
    {
        Vector3 position;
        int parkingSpot = Random.Range(0, 31);
        if(parkingSpot < 5)
            position = new Vector3(xPos[0], yPos, zPos[parkingSpot]);
        else if(parkingSpot < 10)
            position = new Vector3(xPos[1], yPos, zPos[parkingSpot-5]);
        else if(parkingSpot < 21)
            position = new Vector3(xPos[2], yPos, zPos[parkingSpot-10]);
        else
            position = new Vector3(xPos[3], yPos, zPos[parkingSpot-21]);
        
        this.transform.position = position;
    }
}
