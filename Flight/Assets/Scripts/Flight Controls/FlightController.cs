using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class FlightController : MonoBehaviour
{
    [Header(" Movement Values ")]
    [SerializeField] private float throttle; //Lever value to pull the Flight Up or Down
    [Tooltip(" z axis rotation / rolling right or right side ")]
    private float roll;
    [Tooltip(" y axis rotation / turning in right or left  ")]
    private float pitch;
    [Tooltip(" x axis rotation / Rotate Up or Down ")]
    [SerializeField] private float yaw;
    [SerializeField] private float throttleUp;
    [SerializeField] private float throttleDown;

    public Rigidbody rb;
    public float responseModifier
    {
        get 
        { 
            return rb.mass / 10f * responsiveness; // increment the response based on the mass of the Flight, so heavier planes are less responsive
        }
    }

    [Tooltip("The speed at which the thrust of the Flight Increases")]
    public float throttleIncrement = 0.1f; 
    [Tooltip("The Maximum Value of Thrust when 100% throttle ")]
    public float maxThrust = 400f;
    [Tooltip("The responsiveness of the Flight to the Roll and Pitch Inputs")]
    public float responsiveness = 2f;
    public float lift = 135f; // The lift force applied to the Flight, which helps it stay in the air

    [SerializeField] private TextMeshProUGUI Display;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    public void Update()
    {
        Handleinput();
        UpdateGUI();
    }

    // For Physics based Things
    public void FixedUpdate()
    {
        MoveFlight();
    }



    // ================ Input Receiving Function  ================ // 
    public void Handleinput()
    {
        // Rotation Controll
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");
        throttleUp = Input.GetAxis("ThrottleUp");
        throttleDown = Input.GetAxis("ThrottleDown");

        // Throttle Increment and decrement
        if( throttleUp > 0.1f) // here throttleUp can't be checked if it's null or not , so we use 0.1f if it's value is more than that means pressed 
        {
            throttle += throttleIncrement;
        }
        else if( throttleDown > 0.1f)
        {
            throttle -= throttleIncrement;
        }
        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }


    // ================ Movement Function (Flight Rigibody)  ================ // 


    public void MoveFlight()
    {
        rb.AddForce(transform.forward * maxThrust * throttle); // To Move forward 
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch  * responseModifier);
        rb.AddTorque(transform.forward * roll * responseModifier);

        rb.AddForce(Vector3.up * lift * rb.velocity.magnitude); // To apply lift force based on the speed of the Flight, so faster planes get more lift
    }

    private void UpdateGUI()
    {
        Display.text = " Throttle:" + throttle.ToString("F0")+"% \n "; // Display the throttle
        Display.text +="Altitude:" + transform.position.y.ToString("F0") +"m \n"; // Diaplay the altitude of the Flight
        Display.text += " Speed:" + (rb.velocity.magnitude * 3.6f) + "km/h \n"; // 1 km = 1000m and 1 hr = 60 min x 60 sec = 3600 sec ,
        // so 1m/s is 3600m/hr and 3600/1000 = 3.6 km/hr
    }


}
