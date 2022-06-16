using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManager : MonoBehaviour {

    [HideInInspector] public float vertical;
    [HideInInspector] public float horizontal;
    [HideInInspector] public bool handbrake;
    [HideInInspector] public bool boosting;

    // Các thành phần AI

    private trackWaypoints waypoints;
    private Transform currentWaypoint;
    private List<Transform> nodes = new List<Transform> ();
    private int distanceOffset = 5;
    private float sterrForce = 1;
    [Header("AI acceleration value")]
    [Range(0,1)]public float acceleration = 0.5f;
    public int currentNode;


    private void Start() {
        waypoints = GameObject.FindGameObjectWithTag("path").GetComponent<trackWaypoints>();
        currentWaypoint = gameObject.transform;
        nodes = waypoints.nodes;
        
        //print(gameObject.name + "offset distance " + distanceOffset + "steer force = " + sterrForce + "acc " + acceleration);
    }

    private void FixedUpdate () {

        if (gameObject.tag == "AI") AIDrive ();
        else if (gameObject.tag == "Player"){
            calculateDistanceOfWaypoints();
            keyboard ();
        } 

    }

    private void keyboard () {
        vertical = Input.GetAxis ("Vertical");
        horizontal = Input.GetAxis ("Horizontal");
        handbrake = (Input.GetAxis ("Jump") != 0) ? true : false;
        if (Input.GetKey (KeyCode.LeftShift)) boosting = true;
        else boosting = false;

    }

    private void AIDrive () {
        calculateDistanceOfWaypoints ();
        AISteer ();
        vertical = acceleration;

    }

    private void calculateDistanceOfWaypoints () {
        Vector3 position = gameObject.transform.position;
        float distance = Mathf.Infinity;

        for (int i = 0; i < nodes.Count; i++) {
            Vector3 difference = nodes[i].transform.position - position;
            float currentDistance = difference.magnitude;
            if (currentDistance < distance) {
                if ((i + distanceOffset) >= nodes.Count) {
                    currentWaypoint = nodes[1];
                    distance = currentDistance;
                } else {
                    currentWaypoint = nodes[i + distanceOffset];
                    distance = currentDistance;
                }
                currentNode = i;
            }
            
        }
        
    }

    private void AISteer () {

        Vector3 relative = transform.InverseTransformPoint (currentWaypoint.transform.position);
        relative /= relative.magnitude;

        horizontal = (relative.x / relative.magnitude) * sterrForce;

    }

}