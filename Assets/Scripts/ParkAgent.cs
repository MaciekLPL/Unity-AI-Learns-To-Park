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
    private float iteration = 0f;
    private float episodeReward = 0f;
    private bool inParkingZone = false;
    //
    public override void OnEpisodeBegin() {
        iteration++;
        inParkingZone = false;    
        float percentage = (successRatio/ iteration) *100f;
        Debug.Log($"Success ratio: {successRatio}/{iteration} [{percentage}%] \n Reward: {episodeReward}");

        episodeReward = 0.0f;

        transform.position = new Vector3(Random.Range(-10, -5), 0, Random.Range(-10, -5));
        transform.eulerAngles = new Vector3(0f, Random.Range(30, 60), 0f);
        environmentManager.respawn();
        distance = Vector3.Distance(transform.position, targetTransform.position);
    }
    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(targetTransform.position);
        sensor.AddObservation(Vector3.Distance(transform.position, targetTransform.position));
        sensor.AddObservation((targetTransform.position - transform.position).normalized);
        sensor.AddObservation(transform.forward);
    }
    public override void OnActionReceived(ActionBuffers actions) {
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


    private void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.TryGetComponent<Parking>(out Parking parking)) {
            Debug.Log("W strefie parkowania");

      
            if(!inParkingZone)
            {
                AddNewReward(500f);
                AddNewReward((float)(MaxStep - StepCount));
                inParkingZone = true;
                successRatio++;
            }

            //EndEpisode();
        } 
        else if (other.gameObject.TryGetComponent<Wall>(out Wall wall)) 
        {
            AddNewReward(-1000f);
            EndEpisode();
        }
        
        else 
        {
            Debug.Log("Kolizja");
            //AddNewReward(-100f);
            EndEpisode();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<Parking>(out Parking parking)) 
            AddNewReward(15f / (1f + angle));
    }
   
    private void AddNewReward(float reward)
    {
        AddReward(reward); 
        episodeReward += reward;
    }

    void FixedUpdate()
    {     
        calculateReward();
    }
        private void calculateReward()
        {
            angle = transform.eulerAngles.y - targetTransform.eulerAngles.y;
            if(angle > 180f)
            angle = 360f - angle;

            distance = Vector3.Distance(transform.position, targetTransform.position);
            AddNewReward(1f / (1f+distance));

            if(distance < 0.05f && angle < 5f)
            {
                Debug.Log("Perfect run");
                AddNewReward(10000f);
                EndEpisode();
                return;
            } 
            else if (transform.eulerAngles.z >= 45f && transform.eulerAngles.z <= 315f) {
                Debug.Log("Pijany kierowca" + transform.eulerAngles.z);
                AddNewReward(-10000f);
                EndEpisode();
            }

        }
}
