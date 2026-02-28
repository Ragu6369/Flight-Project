using UnityEngine;
using UnityEngine.SceneManagement;

public class BalloonMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float swayAmount = 0.5f;
    public float swaySpeed = 2f;
    public float fallSpeed = 2f;

    [Header("Game Settings")]
    public static int score = 0;              // Shared score across balloons
    public static int scoreLimit = 3 ;         // Win condition
    public GameObject winUI;                  // Assign in prefab or manager
    public static GameObject winUIInstance;   // Static reference

    private float swayOffset;

    private void Start()
    {
        swayOffset = Random.Range(0f, 2f * Mathf.PI);

        // Store static reference to winUI
        if (winUI != null) winUIInstance = winUI;
    }

    private void Update()
    {
        // Sway left-right while falling
        float sway = Mathf.Sin(Time.time * swaySpeed + swayOffset) * swayAmount;
        transform.position += new Vector3(sway, -fallSpeed * Time.deltaTime, 0);
    }
}
