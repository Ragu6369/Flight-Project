using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    

    [Tooltip("Array containing all Camera Positions")]
    [SerializeField] private Transform[] campos;
    [Tooltip(" the speed at which camer afollows the Flight")]
    [SerializeField] private float speed;

    [SerializeField] private int index = 0;
    [SerializeField] private float TimeDelay;
    [SerializeField] private int previndex = 0;
    private Vector3 targetpos;
    private bool isSwitching = false;
    private bool inputreceived = false;
    private bool setBack;
    private Quaternion rotation;

    public Transform player;

    [Header("Orbit Values")]
    public Transform flight;
    public Vector2 sensitivity = new Vector2(2f, 2f);
    public float minPitch = -20f;
    public float maxPitch = 60f;
    public float defaultFOV = 60f;
    public float changeFOV = 30f;

    private float yaw;
    private float pitch;

    private Camera maincam;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        maincam = Camera.main;
        setBack = true;
        
    }
    private void Update()
    {
        // KeyBoard number inputs from 1 to 4 to switch between camera views

        if (Input.GetKeyDown(KeyCode.Alpha1)) index = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            index = 1;
            inputreceived = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            index = 2;
            inputreceived = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            index = 3;
            inputreceived = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            index = 4;
            inputreceived = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            index = 5;
            inputreceived = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            index = 6;
            inputreceived = true;
        }


        if (Input.GetButtonDown("LB")) // reduce index 
        {
            if(index > 0)
            {
                index--;
                inputreceived = true;
            }
        }
        else if (Input.GetButtonDown("RB")) // increase index
        {
            if(index < campos.Length - 1)
            {
                index++;
                inputreceived = true;
            }
        }

        // delay cam switching
        if (index != previndex && !isSwitching)
        {
            StartCoroutine(DelaySwitch());
        }
        

        // setting the current camera position  to targetpos
        targetpos = campos[index].position;

        // field of view for cam 4 
        if(maincam != null)
        {
            if(index == 3)
            {
                maincam.fieldOfView = changeFOV;
            }
            else 
            {
                maincam.fieldOfView = defaultFOV;
            }
        }
    }

    private void LateUpdate()
    {
        // Smoothly move the camera towards the target position
       if(previndex != index && index > 2)
       {
            camSwitch();
       }
       else if (index <= 2)
       {
            OrbitAroundFlight();
       }

       if(previndex != index)
       {
            // when camera switches look back at player for cam 0,1,2
            setBack = true;
       }
       if(previndex == index)
       {
            // here at first index and previndex are 0
            // so if we don't move mouse and switch cam then look at back at player 
            if(inputreceived == false)
            {
                setBack = true;
            }
            else
            {
                setBack = false;
            }
       }
    }

    private void camSwitch()
    {
        // Move smoothly to target position
        transform.position = Vector3.MoveTowards(transform.position, targetpos, Time.fixedDeltaTime * speed);
        transform.rotation = Quaternion.Lerp(transform.rotation, campos[index].rotation, speed * Time.fixedDeltaTime);
       
    }
    private IEnumerator DelaySwitch()
    {
        isSwitching = true;
        yield return new WaitForSeconds(TimeDelay); // Wait one second
        previndex = index;
        isSwitching = false;
    }

    private void OrbitAroundFlight()
    {
        //  Mouse input / joystick input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity.x;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity.y;

        // if no input detected ,it look back at player
        if(Mathf.Abs(mouseX) < 0.01f && Mathf.Abs(mouseY) < 0.01f)
        {
            if(setBack)
            {
                rotation = Quaternion.LookRotation(flight.forward, Vector3.up);
            }

        }
        else
        {
            // updates yaw & pitch only when there is input
            inputreceived = true;
            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            rotation = Quaternion.Euler(pitch, yaw, 0).normalized;

        }

        // using current campos as offset for orbitting around the flight
        Vector3 offset = campos[index].localPosition;

        // positioning the camera to orbit around the flight
        transform.position = flight.position + rotation * offset;
        transform.rotation = rotation; 
    }
}

