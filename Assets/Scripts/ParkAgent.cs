using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class ParkAgent : Agent
{
    // Start is called before the first frame update
     [SerializeField] private Transform targetTransform;
     [SerializeField] private SimpleCarController carController;
     [SerializeField] private Parking parkingSpot;
     private float _horizontalInput = 0f; 
     private float _verticalInput = 0f;
     private float distance = 0.0f;

     public override void OnEpisodeBegin()
     {    AddReward(-1f);
          transform.position = new Vector3(-5.1f, 0f, -7.32f);
          transform.eulerAngles = new Vector3(0f,90f,0f);
          parkingSpot.respawn();
          distance = Mathf.Sqrt(Mathf.Pow(transform.position.x - targetTransform.position.x,2)+Mathf.Pow(transform.position.y - targetTransform.position.y,2)+Mathf.Pow(transform.position.z - targetTransform.position.z,2));
     }
     public override void CollectObservations(VectorSensor sensor)
     {
          //sensor.AddObservation(transform.position);
          //sensor.AddObservation(targetTransform.position);
          sensor.AddObservation(Vector3.Distance(transform.position, targetTransform.position));
          sensor.AddObservation((targetTransform.position - transform.position).normalized);
          sensor.AddObservation(transform.forward);
     }
     public override void OnActionReceived(ActionBuffers actions)
     {     
   
          switch(actions.DiscreteActions[0]){
               case 0: _horizontalInput = 0f; break;
               case 1: _horizontalInput = 1f; break; 
               case 2: _horizontalInput = -1f; break; 
          }
          switch(actions.DiscreteActions[1]){
               case 0: _verticalInput = 0f; break; 
               case 1: _verticalInput = 1f; break; 
               case 2: _verticalInput = -1f; break; 
          }

          carController.setInput(_horizontalInput, _verticalInput);
          if(transform.eulerAngles.z >= 45f && transform.eulerAngles.z <= 315f)
          {
               Debug.Log("Wywalil sie:" +transform.eulerAngles.z);
               AddReward(-1f);
               EndEpisode();
          }
          float tmp = Mathf.Sqrt(Mathf.Pow(transform.position.x - targetTransform.position.x,2)+Mathf.Pow(transform.position.y - targetTransform.position.y,2)+Mathf.Pow(transform.position.z - targetTransform.position.z,2));
          if(tmp<distance)
               AddReward(0.01f);
          distance = tmp;

   }

     private void OnTriggerEnter(Collider collider)
     {
          if(collider.gameObject.TryGetComponent<Wall>(out Wall wall)){
               AddReward(-1f);
               Debug.Log("Przypierdolil dzwona");
               EndEpisode();
          }
          else if(collider.gameObject.TryGetComponent<Parking>(out Parking parking)){
               Debug.Log("GG");
               SetReward(1f);
               EndEpisode();

          }
          else
          {
               AddReward(-0.1f);
               Debug.Log("Uderzyl w cos");
          }
     }


}
