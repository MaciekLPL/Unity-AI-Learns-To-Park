using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carController : MonoBehaviour
{
    
    GameObject WheelLR;
    GameObject WheelLF;
    GameObject WheelRR;
    GameObject WheelRF;
    Rigidbody _rigidbody;
    void Start()
    {
        WheelLR = transform.GetChild(0).gameObject;
        WheelLF = transform.GetChild(1).gameObject;
        WheelRR = transform.GetChild(2).gameObject;
        WheelRF = transform.GetChild(3).gameObject;
        _rigidbody = GetComponent<Rigidbody>();
        Debug.Log(_rigidbody.mass);
    }

    
    void Update()
    {

        Vector3 direction = Vector3.zero;
        direction += WheelLR.transform.right * Input.GetAxis("Vertical");
        //direction += transform.forward * Input.GetAxis("Vertical");
        Vector3 velocity = 5.0f * direction;
        
        Debug.DrawRay(WheelLR.transform.position, WheelLR.transform.right*5.0f, Color.red);
        //WheelLR.transform.right * Input.GetAxis("Horizontal");
        _rigidbody.velocity=velocity;

    }
}
