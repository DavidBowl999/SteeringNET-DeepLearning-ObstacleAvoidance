using UnityEngine;

public class CarSensors : MonoBehaviour
{
    public float sensorRange = 15f;
    public LayerMask obstacleLayer;

    private readonly Vector2[] directions =
    {
        Vector2.left,                   // left
        new Vector2(-1f, 1f),           // front-left (45°)
        new Vector2(-0.5f, 1f),         // slight front-left (~22.5°)
        Vector2.up,                     // front (straight)
        new Vector2(0.5f, 1f),          // slight front-right (~22.5°)
        new Vector2(1f, 1f),            // front-right (45°)
        Vector2.right                   // right
    };

    public float[] GetSensorReadings()
    {
        float[] readings = new float[directions.Length];

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2 origin = transform.position;
            Vector2 worldDir = transform.TransformDirection(directions[i].normalized);

            RaycastHit2D hit = Physics2D.Raycast(origin, worldDir, sensorRange, obstacleLayer);

            float dist;
            if (hit.collider != null)
            {
                Debug.DrawRay(origin, worldDir * hit.distance, Color.red);   // Obstacle detected
                dist = hit.distance;
            }
            else
            {
                Debug.DrawRay(origin, worldDir * sensorRange, Color.green);  // Clear path
                dist = sensorRange;
            }

            readings[i] = dist / sensorRange;
        }

        return readings;
    }

    private void Update()
    {
        GetSensorReadings(); // Draw rays every frame for debugging
    }
}




