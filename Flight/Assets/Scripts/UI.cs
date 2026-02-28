using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUIManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startUI;   // Assign your start panel or text here

    private bool uiActive = true;

    // Static flag persists across scene reloads
    private static bool hasShownStartUI = false;

    private void Start()
    {
        if (!hasShownStartUI && startUI != null)
        {
            
            startUI.SetActive(true); // Show UI only on first start
            uiActive = true;
            Time.timeScale = 0f;

        }
        else if (startUI != null)
        {
            
            startUI.SetActive(false); // Hide UI on restarts
            uiActive = false;
            
        }
    }

    private void Update()
    {
        if (uiActive && ( Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space)))
        {
            if (startUI != null)
            {
                startUI.SetActive(false);
            }

            HideUI();
            Time.timeScale = 1.0f;
        }
    }

    private void HideUI()
    {
        
        uiActive = false;
        hasShownStartUI = true; // Mark as shown so it won't appear again
       
    }
}
