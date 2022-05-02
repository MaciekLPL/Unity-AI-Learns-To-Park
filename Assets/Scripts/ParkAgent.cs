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
     private float _horizontalInput = 0f; 
     private float _verticalInput = 0f;

     public override void OnEpisodeBegin()
     {
          transform.position = new Vector3(-53f,0f,-80f);
          transform.eulerAngles = new Vector3(0f,90f,0f);
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
               AddReward(1f);
               EndEpisode();

          }
     }


}
