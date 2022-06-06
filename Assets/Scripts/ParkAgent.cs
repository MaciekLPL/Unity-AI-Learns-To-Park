using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using TMPro;
public class ParkAgent : Agent {
    // Start is called before the first frame update
    [SerializeField] private Transform targetTransform;
    [SerializeField] private SimpleCarController carController;
    [SerializeField] private EnvironmentManager environmentManager;
    [SerializeField] private Transform plansza;
    [SerializeField] private GameObject colorTile;
    private float _horizontalInput = 0f;
    private float _verticalInput = 0f;
    private float distance = 0.0f;
    private bool touchParkingBefore = false;
    [SerializeField] private bool debugMe = false;


    public Manager manager;



    public override void OnEpisodeBegin() {
        manager.iteration++;



        touchParkingBefore =false;
        carController.reset();
        transform.position = plansza.position + new Vector3(Random.Range(-11, -1), 0, Random.Range(-11, -1));

        transform.eulerAngles = new Vector3(0f, Random.Range(0, 360), 0f);
        environmentManager.respawn();
        distance = Vector3.Distance(transform.position, targetTransform.position);

    }
    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        sensor.AddObservation(Vector3.Distance(transform.localPosition, targetTransform.localPosition));
        sensor.AddObservation((targetTransform.localPosition - transform.localPosition).normalized);
        sensor.AddObservation(transform.forward);
        sensor.AddObservation(carController.getVelocity());
    }
    public override void OnActionReceived(ActionBuffers actions) {

        switch (actions.DiscreteActions[0]) {
            case 0: _horizontalInput = 0f; break;
            case 1: _horizontalInput = 1f; break;
            case 2: _horizontalInput = -1f; break;
        }
        switch (actions.DiscreteActions[1]) {
            case 0: _verticalInput = 0f; break;
            case 1: _verticalInput = 1f; break;
            case 2: _verticalInput = -1f; break;
        }

        carController.setInput(_horizontalInput, _verticalInput);
        
        if (transform.eulerAngles.z >= 45f && transform.eulerAngles.z <= 315f) {

            AddReward(-1f);
                    colorTile.GetComponent<Renderer>().material.color = Color.red;
            EndEpisode();
        }

        distance = Vector3.Distance(transform.position, targetTransform.position);
        // if (tmp < distance)
        //     AddReward(1f / MaxStep);
        // else if(tmp < 0.5f) {}
        // else
        //     AddReward(-1f / MaxStep);


        float direction = Mathf.Abs(Vector3.Dot(transform.forward, targetTransform.forward));
        float angle = 90f - 90f*direction; 

        
        // if(debugMe)
        //     Debug.Log($"{Mathf.Abs(carController.getVelocity())}v : {tmp}d");

        float maxAngle = 3f;
        float maxDistance = 0.1f;
        float maxSpeed = 0.05f;
        float speed = Mathf.Abs(carController.getVelocity());
        if ((speed < maxSpeed) && (distance < maxDistance) && (angle < maxAngle)) 
        
        {
            float angleReward =  1f- (angle/maxAngle)*0.5f;
            float distanceReward = 1f;
            float parked = 1f;
            float reward =  parked + distanceReward + angleReward;
            
            //Debug.Log("Sukces");
            manager.success++;
            AddReward(reward);
                    colorTile.GetComponent<Renderer>().material.color = Color.green;
            EndEpisode();

        }
        if(StepCount>=250 &&(colorTile.GetComponent<Renderer>().material.color != Color.yellow ||touchParkingBefore==false))
        colorTile.GetComponent<Renderer>().material.color = Color.gray;
    }

    public override void Heuristic(in ActionBuffers actionsOut) {

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = Mathf.CeilToInt(Input.GetAxis("Horizontal"));
        discreteActions[1] = Mathf.CeilToInt(Input.GetAxis("Vertical"));

    }

    private void OnTriggerEnter(Collider collider) {
        
        if (collider.gameObject.TryGetComponent<Parking>(out Parking parking)) {
            if(touchParkingBefore==false)
            {
                //Debug.Log("Parking");
                touchParkingBefore=true;
                //AddReward(1f);
            }
            else
            {
                colorTile.GetComponent<Renderer>().material.color = Color.yellow;
            }
            
        } else {
           // Debug.Log("Kolizja");
            colorTile.GetComponent<Renderer>().material.color = Color.red;
            AddReward(-1f);
            manager.collision++;
            EndEpisode();
        }
    }
}
