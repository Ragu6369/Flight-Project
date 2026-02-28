using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FlightEntryManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;                             
    public Transform playerRespawnPoint;    
    public GameObject uiPrompt;             
    public Camera flightCamera;
    public Animator playerAnimator;
    public MonoBehaviour flightController;      
    public MonoBehaviour BladespinnerScript;
    public MonoBehaviour BalloonDropScript;


    [Header("Sphere Settings")]
    public float detectionRadius = 5f;       
    public LayerMask playerLayer;            

    private bool inFlight = false;
    private bool playerNearby = false;

    private void Start()
    {
        if (uiPrompt != null) uiPrompt.SetActive(false);

        
        if (flightCamera != null)
        {
            flightCamera.gameObject.SetActive(false);
        }
        if (flightController != null)
        {
            flightController.enabled = false;
        }
        if (BladespinnerScript != null)
        {
            BladespinnerScript.enabled = false;
        }
        if (BalloonDropScript != null)
        {
            BalloonDropScript.enabled = false;
        }
    }

    private void Update()
    {
        // Detect player within sphere
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        playerNearby = hits.Length > 0;

        if (playerNearby && !inFlight)
        {
            if (uiPrompt != null) uiPrompt.SetActive(true);
        }
        else if (!inFlight)
        {
            if (uiPrompt != null) uiPrompt.SetActive(false);
        }

        // Toggle control on E
        if (playerNearby && (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Fire3")))
        {
            ToggleControl();
        }
        else if (inFlight && (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Fire3")))
        {
            ToggleControl();
        }
    }

    private void ToggleControl()
    {
        inFlight = !inFlight;

        if (inFlight)
        {
            // Disable player entirely
            player.gameObject.SetActive(false);

           
            if (flightCamera != null)
            {
                flightCamera.gameObject.SetActive(true);
            }
            if(flightController != null)
            {
                flightController.enabled = true;
            }
            if (BladespinnerScript != null)
            {
                 BladespinnerScript.enabled = true;
            }
            if (BalloonDropScript != null)
            {
                BalloonDropScript.enabled = true;
            }
            if (uiPrompt != null)
            {
                uiPrompt.SetActive(false);
            }

        }
        else
        {
            // Respawn player at given point
            player.position = playerRespawnPoint.position;
            player.rotation = playerRespawnPoint.rotation;
            player.gameObject.SetActive(true);
            if(playerAnimator != null)
            {
                playerAnimator.SetBool("IsJumping",true);
                playerAnimator.SetBool("IsFalling ", true);

            }


            if (flightCamera != null)
            {
                flightCamera.gameObject.SetActive(false);
            }

            if (flightController != null)
            {
                flightController.enabled = false;
            }

            if (BladespinnerScript != null)
            {
                BladespinnerScript.enabled = false;
            }
            if (BalloonDropScript != null)
            {
                BalloonDropScript.enabled = false;
            }

            if (uiPrompt != null) uiPrompt.SetActive(true);
        }
    }

   

    private void OnDrawGizmosSelected()
    {
        // Visualize sphere in editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
