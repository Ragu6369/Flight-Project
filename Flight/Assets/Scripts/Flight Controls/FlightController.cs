using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        Handleinput();
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
    }


   

}
