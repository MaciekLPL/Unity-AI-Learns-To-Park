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
    [SerializeField] private Transform plansza;
    private float _horizontalInput = 0f;
    private float _verticalInput = 0f;
    private float distance = 0.0f;

    public override void OnEpisodeBegin() {
        carController.reset();
        transform.position = plansza.position + new Vector3(Random.Range(-10, -5), 0, Random.Range(-10, -5));
        transform.eulerAngles = new Vector3(0f, Random.Range(30, 60), 0f);
        environmentManager.respawn();
        distance = Vector3.Distance(transform.position, targetTransform.position);
    }
    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        sensor.AddObservation(Vector3.Distance(transform.localPosition, targetTransform.localPosition));
        sensor.AddObservation((targetTransform.localPosition - transform.localPosition).normalized);
        sensor.AddObservation(transform.forward);
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
            Debug.Log("Wywalil sie:" + transform.eulerAngles.z);
            AddReward(-1f);
            EndEpisode();
        }

        float tmp = Vector3.Distance(transform.position, targetTransform.position);
        if (tmp < distance)
            AddReward(1f / MaxStep);
        else
            AddReward(-1f / MaxStep);
        distance = tmp;

    }

    private void OnTriggerEnter(Collider collider) {
        
        if (collider.gameObject.TryGetComponent<Parking>(out Parking parking)) {
            Debug.Log("Parking");
            AddReward(1f);
            EndEpisode();
        } else {
            Debug.Log("Kolizja");
            AddReward(-1f);
            EndEpisode();
        }
    }
}
