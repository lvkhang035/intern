using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIcontroller : MonoBehaviour{


    internal enum driveType
    {
        frontWheelDrive,
        rearWheelDrive,
        allWheelDrive
    }
    [SerializeField] private driveType drive;


    //other classes ->

    private carEffects CarEffects;

    [Header("DEBUG")]
    public bool test;

    [Header("Variables")]
    public float handBrakeFrictionMultiplier = 2f;
    public float maxRPM, minRPM;
    public float[] gears;
    public float[] gearChangeSpeed;
    public int gearNum = 0;
    public AnimationCurve enginePower;


    [HideInInspector] public bool playPauseSmoke = false;
    [HideInInspector] public float KPH;
    [HideInInspector] public float engineRPM;
    [HideInInspector] public bool reverse = false;
    [HideInInspector] public float nitrusValue;
    [HideInInspector] public bool nitrusFlag = false;
    public float totalPower;
    private float driftFactor;
    private float wheelsRPM;
    private GameObject wheelMeshes, wheelColliders;
    private WheelCollider[] wheels = new WheelCollider[4];
    private GameObject[] wheelMesh = new GameObject[4];
    private GameObject centerOfMass;
    private Rigidbody rigidbody;

    //car Shop Values
    public int carPrice;
    public string carName;
    public float smoothTime = 0.08f;

    [HideInInspector]public float horizontal, vertical;
    private WheelFrictionCurve forwardFriction, sidewaysFriction;
    private float radius = 6, brakPower = 0, DownForceValue = 100f, lastValue;
    private bool flag = false;

    private void Awake()
    {

        if (SceneManager.GetActiveScene().name == "awakeScene") return;

        getObjects();
        StartCoroutine(timedLoop());
    }

    private void FixedUpdate()
    {

        lastValue = engineRPM;


        addDownForce();
        animateWheels();
        steerVehicle();
        calculateEnginePower();
        AIDrive();
    }
    // AI 

    public trackWaypoints waypoints;
    public Transform currentWaypoint;
    public List<Transform> nodes = new List<Transform>();
    [Range(0, 10)] public int distanceOffset = 5;
    [Range(0, 5)] public float sterrForce = 1;
    [Range(0, 1)] public float acceleration = 0.5f;





    private void AIDrive()
    {
        if (gameObject.tag == "AI")
        {
            calculateDistanceOfWaypoints();
            AISteer();

            vertical = acceleration;
            //if (Input.GetKey(KeyCode.LeftShift)) boosting = true; else boosting = false;
            // handbrake = (Input.GetAxis("Jump") != 0) ? true : false;
        }
    }


    private void calculateDistanceOfWaypoints()
    {
        Vector3 position = gameObject.transform.position;
        float distance = Mathf.Infinity;

        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 difference = nodes[i].transform.position - position;
            float currentDistance = difference.magnitude;
            if (currentDistance < distance)
            {
                if ((i + distanceOffset) >= nodes.Count)
                {
                    currentWaypoint = nodes[1];
                    distance = currentDistance;
                }
                else
                {
                    currentWaypoint = nodes[i + distanceOffset];
                    distance = currentDistance;
                }
            }


        }
    }


    private void AISteer()
    {

        Vector3 relative = transform.InverseTransformPoint(currentWaypoint.transform.position);
        relative /= relative.magnitude;

        horizontal = (relative.x / relative.magnitude) * sterrForce;

    }


    private void calculateEnginePower()
    {

        if (vertical != 0)
        {
            rigidbody.drag = 0.005f;
        }
        if (vertical == 0)
        {
            rigidbody.drag = 0.1f;
        }
        totalPower = 3.6f * enginePower.Evaluate(engineRPM) * (vertical);




        float velocity = 0.0f;
        if (engineRPM >= maxRPM || flag)
        {
            engineRPM = Mathf.SmoothDamp(engineRPM, maxRPM - 500, ref velocity, 0.05f);

            flag = (engineRPM >= maxRPM - 450) ? true : false;
            test = (lastValue > engineRPM) ? true : false;
        }
        else
        {
            engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + (Mathf.Abs(wheelsRPM) * 3.6f * (gears[gearNum])), ref velocity, smoothTime);
            test = false;
        }
        if (engineRPM >= maxRPM + 1000) engineRPM = maxRPM + 1000; // clamp at max
        moveVehicle();
        shifter();
    }


    private bool checkGears()
    {
        if (KPH >= gearChangeSpeed[gearNum]) return true;
        else return false;
    }

    private void shifter()
    {

        if (!isGrounded()) return;
        //automatic
        if (engineRPM > maxRPM && gearNum < gears.Length - 1 && !reverse && checkGears())
        {
            gearNum++;
            
            return;
        }
        if (engineRPM < minRPM && gearNum > 0)
        {
            gearNum--;
            
        }

    }

    private bool isGrounded()
    {
        if (wheels[0].isGrounded && wheels[1].isGrounded && wheels[2].isGrounded && wheels[3].isGrounded)
            return true;
        else
            return false;
    }

    private void moveVehicle()
    {

        brakeVehicle();

        if (drive == driveType.allWheelDrive)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = totalPower / 4;
                wheels[i].brakeTorque = brakPower;
            }
        }
        else if (drive == driveType.rearWheelDrive)
        {
            wheels[2].motorTorque = totalPower / 2;
            wheels[3].motorTorque = totalPower / 2;

            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].brakeTorque = brakPower;
            }
        }
        else
        {
            wheels[0].motorTorque = totalPower / 2;
            wheels[1].motorTorque = totalPower / 2;

            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].brakeTorque = brakPower;
            }
        }

        KPH = rigidbody.velocity.magnitude * 3.6f;


    }

    private void brakeVehicle()
    {

        if (vertical < 0)
        {
            brakPower = (KPH >= 10) ? 500 : 0;
        }
        else if (vertical == 0 && (KPH <= 10 || KPH >= -10))
        {
            brakPower = 10;
        }
        else
        {
            brakPower = 0;
        }


    }

    private void steerVehicle()
    {


        //acerman steering formula
        //steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontalInput;

        if (horizontal > 0)
        {
            //rear tracks size is set to 1.5f       wheel base has been set to 2.55f
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
        }
        else if (horizontal < 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
            //transform.Rotate(Vector3.up * steerHelping);

        }
        else
        {
            wheels[0].steerAngle = 0;
            wheels[1].steerAngle = 0;
        }

    }

    private void animateWheels()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < 4; i++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelMesh[i].transform.position = wheelPosition;
            wheelMesh[i].transform.rotation = wheelRotation;
        }
    }

    private void getObjects()
    {

        CarEffects = GetComponent<carEffects>();
        rigidbody = GetComponent<Rigidbody>();
        wheelColliders = GameObject.Find("wheelColliders");
        wheelMeshes = GameObject.Find("wheelMeshes");
        wheels[0] = wheelColliders.transform.Find("0").gameObject.GetComponent<WheelCollider>();
        wheels[1] = wheelColliders.transform.Find("1").gameObject.GetComponent<WheelCollider>();
        wheels[2] = wheelColliders.transform.Find("2").gameObject.GetComponent<WheelCollider>();
        wheels[3] = wheelColliders.transform.Find("3").gameObject.GetComponent<WheelCollider>();

        wheelMesh[0] = wheelMeshes.transform.Find("0").gameObject;
        wheelMesh[1] = wheelMeshes.transform.Find("1").gameObject;
        wheelMesh[2] = wheelMeshes.transform.Find("2").gameObject;
        wheelMesh[3] = wheelMeshes.transform.Find("3").gameObject;
        //waypoints
        waypoints = GameObject.FindGameObjectWithTag("path").GetComponent<trackWaypoints>();
        currentWaypoint = gameObject.transform;
        nodes = waypoints.nodes;

        centerOfMass = GameObject.Find("mass");
        rigidbody.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void addDownForce()
    {

        rigidbody.AddForce(-transform.up * DownForceValue * rigidbody.velocity.magnitude);

    }

   

    private IEnumerator timedLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(.7f);
            radius = 6 + KPH / 20;

        }
    }
    /*
    public void activateNitrus()
    {
        if (!IM.boosting && nitrusValue <= 10)
        {
            nitrusValue += Time.deltaTime / 2;
        }
        else
        {
            nitrusValue -= (nitrusValue <= 0) ? 0 : Time.deltaTime;
        }

        if (IM.boosting)
        {
            if (nitrusValue > 0)
            {
                CarEffects.startNitrusEmitter();
                rigidbody.AddForce(transform.forward * 5000);
            }
            else CarEffects.stopNitrusEmitter();
        }
        else CarEffects.stopNitrusEmitter();

    }
    */
}

