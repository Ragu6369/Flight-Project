using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeDestroy : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 3f;
    public LayerMask balloonLayer;

    private void Update()
    {
        // Check for balloons within sphere
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, balloonLayer);

        foreach (Collider hit in hits)
        {
            BalloonMovement balloon = hit.GetComponent<BalloonMovement>();
            if (balloon != null)
            {
                // Increment score in BalloonMovement
                BalloonMovement.score++;

                // Destroy this cube (the object this script is attached to)
                Destroy(gameObject);

                // Check win condition
                if (BalloonMovement.score >= BalloonMovement.scoreLimit && balloon.winUI != null)
                {
                    balloon.winUI.SetActive(true);
                }
            }
        }

        // If win UI is active, restart scene on any key press
        if (BalloonMovement.winUIInstance != null && BalloonMovement.winUIInstance.activeSelf && ( Input.GetKeyDown(KeyCode.Space) ||Input.GetButtonDown("Fire1"))) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
