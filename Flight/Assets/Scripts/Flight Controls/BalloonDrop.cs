using UnityEngine;

public class BalloonDrop : MonoBehaviour
{
    [Header("Drop Settings")]
    public GameObject balloonPrefab;     
    public Transform dropPoint;          
    public float destTime = 5f;

    public float minheight = 20f;
    public LayerMask groundlayer;
    
    public float groundCheckDistance = 20f;

    [SerializeField] private bool canDropBomd = false;

    private void Update()
    {
        canDropBomd = IsGrounded();

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("Fire1"))// Example: press Space to drop
        {
            DropBalloon();
        }
    }

    

    private void DropBalloon()
    {
        if (!canDropBomd)
        {
            GameObject balloon = Instantiate(balloonPrefab, dropPoint.position, Quaternion.identity);
            
            Destroy(balloon, destTime); // destroy after 5 sec
        }
        
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance,groundlayer);
    }

  
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance); 
    }

}

