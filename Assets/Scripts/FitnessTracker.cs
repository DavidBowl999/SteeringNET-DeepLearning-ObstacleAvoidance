using UnityEngine;

public class FitnessTracker : MonoBehaviour
{
    public float fitness;
    private Vector3 previousPosition;
    private float startTime;
    private bool finished = false;

    private void Start()
    {
        previousPosition = transform.position;
        fitness = 0f;
        startTime = Time.time;
    }

    private void Update()
    {
        if (finished) return;

        float distanceMoved = Vector3.Distance(transform.position, previousPosition);
        fitness += distanceMoved;
        previousPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Finish") && !finished)
        {
            finished = true;

            // Give a big fitness bonus for reaching the end
            fitness += 10000f;

            float timeTaken = Time.time - startTime;
            float timeBonus = Mathf.Max(0f, 1000f - (timeTaken * 100f)); // Fast = better bonus
            fitness += timeBonus;

            Debug.Log($"{gameObject.name} finished in {timeTaken:F2}s with bonus {timeBonus:F0}. Total fitness: {fitness:F1}");

            // Stop movement
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // Notify EvolutionManager to end early
            EvolutionManager manager = FindObjectOfType<EvolutionManager>();
            if (manager != null)
            {
                manager.ChampionFinished(gameObject);
            }

            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!finished)
        {
            gameObject.SetActive(false);
        }
    }
}






