# SteeringNET: A Deep Learning-Based Obstacle Avoidance System

This repository contains **SteeringNET**, my final-year Computer Science dissertation project at Cardiff University.  

The system demonstrates how an **Evolutionary Artificial Neural Network (EANN)** can be built entirely from scratch in **Unity (C#)** to enable cars to learn obstacle avoidance behaviours through trial and error, without any pre-programmed rules.  

**Demo Video:** [Watch the cars evolve to drive](https://youtu.be/iVFnwgIQQnc)  

---

## What Is SteeringNET?
SteeringNET is a **deep learning approach to obstacle avoidance**, where a car agent learns how to steer and accelerate by evolving its own neural network weights across many generations.  

Unlike manually coded driving logic (e.g., “if wall on right → turn left”), a neural network is implemented to abstract the complexity and rules. The neural network starts with **random weights**, where the cars initially perform poorly, but over successive generations they evolve to drive smoothly around a track, slow for corners, speed up for the straights and complete laps.  

The result is a clear, visual demonstration of how **AI can learn behaviours without human intervention**.  

---

## How It Works (Step by Step)

### 1. Sensors (CarSensors.cs)
- Each car has **7 raycast “sensors”** that detect the distance to nearby obstacles.  
- These distances are normalised (0–1) and fed into the neural network as input values.  
- Red ray = collision detected, Green ray = clear path.  

**[]**

---

### 2. Neural Network “Brain” (CarBrain.cs)
- A feed-forward neural network built from scratch in C#.  
- Architecture: **7 input neurons → hidden layers (12 in the first) + (8 in the second ) → 2 output neurons**.  
- Outputs:  
  - **Steering** (turn left/right)  
  - **Throttle** (accelerate/slow down)  
- Activation: **ReLU** (for non-linearity).  

**[]**

---

### 3. Evolutionary Training (EvolutionManager.cs)
- Each generation spawns a **population** of cars with randomised weights.  
- After cars crash or finish:  
  - **The Best car** = one that travelled furthest, and when the cars continuously finish the track, it uses the brain of the car that completed the lap fastest.  
  - Neural net weights from the best car are copied and mutated to create the next generation.  
- Mutation rate controls variation:  
  - Too low = stagnation (no learning).  
  - Too high = chaos (cars never converge).   

---

### 4. Fitness Tracking (FitnessTracker.cs)
- Distance travelled + lap completion time used to score cars.  
- If a car completes the track, it’s prioritised as the “champion” of the generation.  
- Cars are despawned after finishing or crashing, keeping the simulation efficient.  

**[Insert Screenshot of Unity scene with cars mid-race + fitness tracker overlay]**

---

### 5. Camera System (CameraFollow.cs)
- Always follows the **current best car**.  
- Lets you visually watch the “smartest” car each generation.  

📸 **[Insert Screenshot of game view showing camera locked to best car]**

---

## Results
- Cars typically **learn to complete the track and drive around it very effectively for the cars given limits within 5–10 generations**.  
- Mutation rate strongly affects learning speed:  
  - Low = no improvement.  
  - Medium (0.3–0.5) = fast convergence.  
  - High (0.8+) = unstable behaviour.  
- Larger populations evolve faster due to more diversity within each generation, especially effective as the neural network itself becomes larger and more complex (more parameters that need to be updated).  

---

## Threat Model (Conceptual)
Even though this is a **simulation**, I designed a simple threat model as if it were real-world:  
- **Sensor spoofing**: false inputs could mislead the car.  
- **Tampered fitness scores**: cars incorrectly selected as “best.”  
- **Code injection**: unsafe modification of evolution logic.  

Mitigation: modular design, input validation, debugging tools, environment sanitisation.  

---

## Beyond the Simulation
This project wasn’t just about coding. It also explored:  
- **Ethical implications**: safety, fairness, transparency.  
- **Legal challenges**: liability in accidents, GDPR compliance.  
- **Societal impact**: job displacement, “Spotify for cars” ownership models, industrial revolution 4.0 parallels.  

---

## Running the Project
1. Clone this repository:
   ```bash
   git clone https://github.com/DavidBowl999/SteeringNET-DeepLearning-ObstacleAvoidance.git
