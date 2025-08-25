using UnityEngine;
using System;

public class CarBrain : MonoBehaviour
{
    public CarSensors sensors;

    public readonly int InputCount = 7;       // e.g., 5 sensors + speed + time
    public readonly int HiddenCount = 12;     // First hidden layer (wider)
    public readonly int Hidden2Count = 8;     // Second hidden layer
    public readonly int OutputCount = 2;      // Steering, Throttle

    public float[,] inputToHiddenWeights;
    public float[,] hiddenToHidden2Weights;
    public float[,] hiddenToOutputWeights;

    private float minSpeed = 10f;
    private float maxSpeed = 100f;  // best for visualisation is 50
    private float turnSpeed = 300f; // best for visualisation is 120

    private void Awake()
    {
        inputToHiddenWeights = new float[InputCount, HiddenCount];
        hiddenToHidden2Weights = new float[HiddenCount, Hidden2Count];
        hiddenToOutputWeights = new float[Hidden2Count, OutputCount];

        RandomizeWeights();
    }

    private void RandomizeWeights()
    {
        for (int i = 0; i < InputCount; i++)
            for (int j = 0; j < HiddenCount; j++)
                inputToHiddenWeights[i, j] = UnityEngine.Random.Range(-1f, 1f);

        for (int j = 0; j < HiddenCount; j++)
            for (int k = 0; k < Hidden2Count; k++)
                hiddenToHidden2Weights[j, k] = UnityEngine.Random.Range(-1f, 1f);

        for (int j = 0; j < Hidden2Count; j++)
            for (int o = 0; o < OutputCount; o++)
                hiddenToOutputWeights[j, o] = UnityEngine.Random.Range(-1f, 1f);
    }

    private void Update()
    {
        float[] inputs = sensors.GetSensorReadings(); // 7 sensor reading are retrieved here
        float[] hidden1 = new float[HiddenCount];
        float[] hidden2 = new float[Hidden2Count];

        // Input to Hidden 1
        for (int h1 = 0; h1 < HiddenCount; h1++)
        {
            float sum = 0f;
            for (int i = 0; i < InputCount; i++)
                sum += inputs[i] * inputToHiddenWeights[i, h1];
            hidden1[h1] = ReLU(sum);
        }

        
        for (int h2 = 0; h2 < Hidden2Count; h2++)
        {
            float sum = 0f;
            for (int h1 = 0; h1 < HiddenCount; h1++)
                sum += hidden1[h1] * hiddenToHidden2Weights[h1, h2];
            hidden2[h2] = ReLU(sum);
        }

        // Hidden 2  Outputs
        float steerRaw = 0f;
        float throttleRaw = 0f;

        for (int h2 = 0; h2 < Hidden2Count; h2++)
        {
            steerRaw += hidden2[h2] * hiddenToOutputWeights[h2, 0];
            throttleRaw += hidden2[h2] * hiddenToOutputWeights[h2, 1];
        }

        float steer = Mathf.Clamp((float)Math.Tanh(steerRaw), -1f, 1f);
        float throttle = Mathf.Clamp01((float)Math.Tanh(throttleRaw));
        float speed = Mathf.Lerp(minSpeed, maxSpeed, throttle);

        transform.Rotate(0f, 0f, -steer * turnSpeed * Time.deltaTime);
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private float ReLU(float x) => Mathf.Max(0f, x);
}



