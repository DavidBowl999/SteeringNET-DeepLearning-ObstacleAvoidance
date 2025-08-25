using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 20f;
    public float turnSpeed = 200f;
    public float sensorLength = 2f;
    public float avoidTurnMultiplier = 1f;

    private float targetTurn = 0f;
    private float currentTurn = 0f;

    void Update()
    {
        float move = Input.GetAxis("Vertical");
        float manualTurn = -Input.GetAxis("Horizontal");

        // Raycast origin
        Vector3 rayOrigin = transform.position + transform.up * 0.6f;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, transform.up, sensorLength);
        Debug.DrawRay(rayOrigin, transform.up * sensorLength, Color.red);

        // Obstacle avoidance logic
        if (hit.collider != null && hit.collider.CompareTag("Obstacle"))
        {
            Debug.Log("Obstacle detected! Adjusting path...");

            // Dynamically decide to steer left or right
            if (hit.point.x < transform.position.x)
            {
                targetTurn = avoidTurnMultiplier;  // steer right
            }
            else
            {
                targetTurn = -avoidTurnMultiplier; // steer left
            }
        }
        else
        {
            // No obstacle ? return to manual steering
            targetTurn = manualTurn;
        }

        // Smoothly transition to the new turn input
        currentTurn = Mathf.Lerp(currentTurn, targetTurn, Time.deltaTime * 5f);

        // Apply movement
        transform.Translate(Vector3.up * move * speed * Time.deltaTime);
        transform.Rotate(Vector3.forward * currentTurn * turnSpeed * Time.deltaTime);
    }
}
