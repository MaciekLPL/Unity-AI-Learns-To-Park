using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class ParkAgent : Agent {
    // Start is called before the first frame update
    [SerializeField] private Transform targetTransform;
    [SerializeField] private SimpleCarController carController;
    [SerializeField] private EnvironmentManager environmentManager;
    private float distance = 0.0f;
    private float angle = 0.0f;
    private float successRatio = 0f;
    private int iteration = 0;
    private float episodeReward = 0f;
    private int accidents = 0; 
    private bool firstTouch = false;
    private float velocity = 0.0f;

    void FixedUpdate()
    {   
        float prevDistance = distance;
        updateCarStat();
        float x = 1f / (1f + distance);
        float reward = (distance > prevDistance) ? (-x) : (x);
        AddNewReward(reward);

        //Fail
        if (transform.eulerAngles.z >= 45f && transform.eulerAngles.z <= 315f) 
        {
                Debug.Log("Pijany kierowca" + transform.eulerAngles.z);
                AddNewReward(-10000f);
                EndEpisode();
        }
        
        if((distance < 0.1f) && (Mathf.Abs(velocity) < 0.01f) && (angle < 5f))
        {
            Debug.Log("Perfect Run");
            AddNewReward(10f*(float)(MaxStep-StepCount));
            EndEpisode();
        }
    }

    private void updateCarStat()
    {
        distance = Vector3.Distance(transform.position, targetTransform.position);
        angle = transform.eulerAngles.y - targetTransform.eulerAngles.y;
            if(angle > 180f) 
            angle = 360f - angle;
        velocity = carController.getVelocity();
    }

    private void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.TryGetComponent<Parking>(out Parking parking)) 
        {
            Debug.Log("W strefie parkowania");
            if(firstTouch==false)
            {
                firstTouch = true;
                successRatio++;
                AddNewReward(1000f);
            }
        } 
        else if (other.gameObject.TryGetComponent<Wall>(out Wall wall)) 
        {
            AddNewReward(-2000f);
            accidents++;
            EndEpisode();
        }
        else 
        {
            Debug.Log("Kolizja");
            accidents++;
            AddNewReward(-500f);
            EndEpisode();
        }
    }

    private void OnTriggerStay(Collider other)
    { 
        if (other.gameObject.TryGetComponent<Parking>(out Parking parking)) 
        {
            float reward = 1f;      //OK
            if(distance < 0.1f )
            {
                if(angle > 5f)
                    reward = 3f ;    //Good
                else
                    reward = 5f;     //Perfect
            }
            reward /= (1f + Mathf.Abs(velocity));
            AddNewReward(reward);
        }
    }

    public override void CollectObservations(VectorSensor sensor) //[15]
    {
        //[7] Position Observation
            sensor.AddObservation(distance); //[1] Distance from car to parking
            sensor.AddObservation(targetTransform.position); //[3] Parking position
            sensor.AddObservation(transform.position);//[3] Car position 

        //[7] Direction Observation
            sensor.AddObservation(angle); //[1] Angle
            sensor.AddObservation(targetTransform.forward); //[3]
            sensor.AddObservation(transform.forward);   //[3]

        //[1] Velocity;
            sensor.AddObservation(velocity);

        //Debug.Log($"Distance {distance} - Angle: {angle} - MotorTorque: {FLMotorTorque} / {FRMotorTorque} \n Is in park spot: {inParkingZone}");
    }



    //----------------------------------------------------
    
    public override void OnEpisodeBegin() 
    {
        iteration++;
        firstTouch=false;  
        carController.reset();
        float percentage = (successRatio/ (float)iteration) * 100f;
        Debug.Log($"Success ratio: {successRatio}/{iteration} [{percentage}%] \n Reward: {episodeReward} - Accidents: {accidents}");
        episodeReward = 0.0f;
        
        //Car
        transform.position = new Vector3(Random.Range(-10, -5), 0, Random.Range(-10, -5));
        transform.eulerAngles = new Vector3(0f, Random.Range(30, 60), 0f);
        environmentManager.respawn();
        updateCarStat();
    }

    private void AddNewReward(float reward)
    {
        AddReward(reward); 
        episodeReward += reward;
    }

    public override void OnActionReceived(ActionBuffers actions) 
    {
        float steering = actions.ContinuousActions[0]; 
        float accel = actions.ContinuousActions[1]; 
        carController.setInput(steering, accel);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");  
    }




}


