using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{

    float yPos = 0.5f;
    float[] xPos = {-5.05f, 2.245f};
    float[] zPos = { 10.1f, 8.13f, 6.14f, 4.14f, 2.14f, 0.15f, -1.88f, -3.89f, -5.87f, -7.9f, -9.87f};
    [SerializeField] private GameObject prefab_car_m1;
    [SerializeField] private GameObject prefab_car_m2;
    [SerializeField] private GameObject prefab_car_m3;
    [SerializeField] private GameObject prefab_car_m4;
    [SerializeField] private GameObject prefab_car_m5;
    [SerializeField] private Parking parkingSpot;
    private GameObject[] actors = new GameObject[16];
    void Start()
    {
        //respawn();
    }
    
    void Update()
    {
    
    }


    public void respawn()
    {
        for(uint i = 0; i<16; i++)
        {
            if(actors[i])
                Destroy(actors[i]);
        }


       // 5% szans na parking 19% na szamochód
        for(uint i = 0; i<5; i++)
        {
            int dice = Random.Range(1, 100);
            int type = Random.Range(1, 10);
            float angle = (type<6) ? 90.0f : -90.0f;
            if(dice < 6) /**/;
            else if(dice < 25)
                actors[i]= Instantiate(prefab_car_m1, new Vector3(xPos[0],0f,zPos[i]), Quaternion.Euler(0, angle, 0), this.transform);
            else if(dice < 44)
                actors[i]= Instantiate(prefab_car_m2, new Vector3(xPos[0],0f,zPos[i]), Quaternion.Euler(0, angle, 0), this.transform);
            else if(dice < 63)
                actors[i]= Instantiate(prefab_car_m3, new Vector3(xPos[0],0f,zPos[i]), Quaternion.Euler(0, angle, 0), this.transform);
            else if(dice < 82)
                actors[i]= Instantiate(prefab_car_m4, new Vector3(xPos[0],0f,zPos[i]), Quaternion.Euler(0, angle, 0), this.transform);
            else 
                actors[i]= Instantiate(prefab_car_m5, new Vector3(xPos[0],0f,zPos[i]), Quaternion.Euler(0, angle, 0), this.transform);
        }

        for(uint i = 0; i<11; i++)
        {
            int dice = Random.Range(1, 100);
            int type = Random.Range(1, 10);
            float angle = (type<6) ? 90.0f : -90.0f;
            if(dice < 6) /**/;
            else if(dice < 25)
                actors[5+i]= Instantiate(prefab_car_m1, new Vector3(xPos[1],0f,zPos[i]), Quaternion.Euler(0, angle, 0), this.transform);
            else if(dice < 44)
                actors[5+i]= Instantiate(prefab_car_m2, new Vector3(xPos[1],0f,zPos[i]), Quaternion.Euler(0, angle, 0), this.transform);
            else if(dice < 63)
                actors[5+i]= Instantiate(prefab_car_m3, new Vector3(xPos[1],0f,zPos[i]), Quaternion.Euler(0, angle, 0), this.transform);   
            else if(dice < 82)
                actors[5+i]= Instantiate(prefab_car_m4, new Vector3(xPos[1],0f,zPos[i]), Quaternion.Euler(0, angle, 0), this.transform);
            else 
                actors[5+i]= Instantiate(prefab_car_m5, new Vector3(xPos[1],0f,zPos[i]), Quaternion.Euler(0, angle, 0), this.transform);  
        }

        int n = Random.Range(0, 15);
        if(actors[n])
                Destroy(actors[n]);
         if(n<5)
            parkingSpot.respawn(new Vector3(xPos[0], yPos, zPos[n]));
         else       
            parkingSpot.respawn(new Vector3(xPos[1], yPos, zPos[n-5]));
    }
}
