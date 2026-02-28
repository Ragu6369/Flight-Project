using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeSpin : MonoBehaviour
{
    private float throttleUp;
    private float throttleDown;
    [SerializeField] private float rotateSpeed = 0f;
    [SerializeField] private float startSpeed = 5f;
    [SerializeField] private float speedincrement = 1f;
    private Rigidbody rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        throttleUp = Input.GetAxis("ThrottleUp"); // defined by me in InputMangaer 
        throttleDown = Input.GetAxis("ThrottleDown"); // defined by me in InputMangaer 

        if (throttleUp > 0.1f)// here throttleUp can't be checked if it's null or not , so we use 0.1f if it's value is more than that means pressed 
        {
            if(rotateSpeed <= 0f)
            {
                rotateSpeed = startSpeed;
            }

            rotateSpeed += speedincrement;

        }
        else if (throttleDown > 0.1f)
        {
            rotateSpeed -= (speedincrement * 4 );
        }
        rotateSpeed = Mathf.Clamp(rotateSpeed, 0f, 300f);
    }

    private void FixedUpdate()
    {
        if (rotateSpeed > 0f)
        {
            // transform.rotation = Quaternion.Euler(rotateSpeed * Time.fixedDeltaTime * 2f, 0, 0);
            rb.AddTorque(transform.right * rotateSpeed * 1.5f);
        }
       
    }
}
