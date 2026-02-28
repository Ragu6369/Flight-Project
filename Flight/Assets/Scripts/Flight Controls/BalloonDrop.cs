using UnityEngine;

public class BalloonDrop : MonoBehaviour
{
    [Header("Drop Settings")]
    public GameObject balloonPrefab;     
    public Transform dropPoint;          
    public float swayAmount = 0.5f;      
    public float swaySpeed = 2f;        
    public float fallSpeed = 2f;
    public float destTime = 5f;

    public float minheight = 20f;
    public LayerMask groundlayer;
    public float groundCheckDistance = 20f;
    private bool canDropBomd = false;
    private void Update()
    {
        canDropBomd = IsGrounded();

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButton("Fire1"))// Example: press Space to drop
        {
            DropBalloon();
        }
    }

    

    private void DropBalloon()
    {
        if (!canDropBomd)
        {
            GameObject balloon = Instantiate(balloonPrefab, dropPoint.position, Quaternion.identity);


            BalloonMovement movement = balloon.AddComponent<BalloonMovement>();
            movement.swayAmount = swayAmount;
            movement.swaySpeed = swaySpeed;
            movement.fallSpeed = fallSpeed;

            Destroy(balloon, destTime); // destroy after 5 sec
        }
        
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 100f);
    }

}

