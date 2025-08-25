// EvolutionManager.cs
using System.Collections.Generic;
using UnityEngine;

public class EvolutionManager : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public GameObject carPrefab;
    public Transform spawnPoint;
    public int populationSize = 20;

    private readonly List<GameObject> population = new List<GameObject>();
    private readonly List<float> fitnessHistory = new List<float>();

    private void Start()
    {
        SpawnFirstGeneration();
    }

    private void Update()
    {
        FollowBestActiveCar();

        if (AllCarsInactive())
        {
            SpawnNextGeneration();
        }
    }

    private void SpawnFirstGeneration()
    {
        for (int i = 0; i < populationSize; i++)
        {
            GameObject car = Instantiate(carPrefab, spawnPoint.position, carPrefab.transform.rotation);
            population.Add(car);
        }
    }

    private void FollowBestActiveCar()
    {
        GameObject best = GetBestCar(true);
        if (best != null)
        {
            cameraFollow.target = best.transform;
        }
    }

    private bool AllCarsInactive()
    {
        foreach (GameObject car in population)
        {
            if (car != null && car.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    private GameObject GetBestCar(bool onlyActive)
    {
        GameObject best = null;
        float bestScore = float.MinValue;

        foreach (GameObject car in population)
        {
            if (car == null) continue;
            if (onlyActive && !car.activeSelf) continue;

            float score = car.GetComponent<FitnessTracker>().fitness;
            if (score > bestScore)
            {
                bestScore = score;
                best = car;
            }
        }
        return best;
    }

    private void SpawnNextGeneration()
    {
        GameObject champion = GetBestCar(false);
        if (champion == null) return;

        float championScore = champion.GetComponent<FitnessTracker>().fitness;
        fitnessHistory.Add(championScore);

        population.Clear();

        for (int i = 0; i < populationSize; i++)
        {
            GameObject offspring = Instantiate(carPrefab, spawnPoint.position, carPrefab.transform.rotation);

            CarBrain sourceBrain = champion.GetComponent<CarBrain>();
            CarBrain targetBrain = offspring.GetComponent<CarBrain>();

            CopyWeights(sourceBrain, targetBrain);

            if (i != 0)
            {
                MutateWeights(targetBrain, 0.5f, 0.8f); // best for visualisation is 0.5 and 0.8
            }

            population.Add(offspring);
        }

        Destroy(champion);
    }

    private static void CopyWeights(CarBrain source, CarBrain target)
    {
        for (int i = 0; i < source.InputCount; i++)
        {
            for (int j = 0; j < source.HiddenCount; j++)
            {
                target.inputToHiddenWeights[i, j] = source.inputToHiddenWeights[i, j];
            }
        }

        for (int j = 0; j < source.HiddenCount; j++)
        {
            for (int k = 0; k < source.Hidden2Count; k++)
            {
                target.hiddenToHidden2Weights[j, k] = source.hiddenToHidden2Weights[j, k];
            }
        }

        for (int j = 0; j < source.Hidden2Count; j++)
        {
            for (int o = 0; o < source.OutputCount; o++)
            {
                target.hiddenToOutputWeights[j, o] = source.hiddenToOutputWeights[j, o];
            }
        }
    }

    public void ChampionFinished(GameObject car)
    {
        if (!AllCarsInactive())
        {
            foreach (var c in population)
            {
                if (c != null) c.SetActive(false);
            }

            Debug.Log($"{car.name} triggered early generation end.");
        }
    }


    private static void MutateWeights(CarBrain brain, float rate, float strength)
    {
        // Input  Hidden
        for (int i = 0; i < brain.InputCount; i++)
        {
            for (int j = 0; j < brain.HiddenCount; j++)
            {
                if (Random.value < rate)
                {
                    brain.inputToHiddenWeights[i, j] += Random.Range(-strength, strength);
                }
            }
        }

        // Hidden Hidden2
        for (int j = 0; j < brain.HiddenCount; j++)
        {
            for (int k = 0; k < brain.Hidden2Count; k++)
            {
                if (Random.value < rate)
                {
                    brain.hiddenToHidden2Weights[j, k] += Random.Range(-strength, strength);
                }
            }
        }

        // Hidden2  Output
        for (int j = 0; j < brain.Hidden2Count; j++)
        {
            for (int o = 0; o < brain.OutputCount; o++)
            {
                // (Optional) Reduce mutation on throttle
                float localStrength = (o == 1) ? strength * 0.1f : strength;

                if (Random.value < rate)
                {
                    brain.hiddenToOutputWeights[j, o] += Random.Range(-localStrength, localStrength);
                }
            }
        }
    }

}

