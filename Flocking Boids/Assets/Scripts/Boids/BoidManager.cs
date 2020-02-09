using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour {

    //******************************************************
    //
    //      VARIABLES
    //
    //******************************************************

    public static BoidManager Instance;

    public BoidSettings BoidSettings;
    public ComputeShader ComputeShader;

    private Boid[] _Boids;
    private const int _ThreadGroupSize = 1024;

    public struct BoidData {

        public Vector3 position;
        public Vector3 direction;

        public Vector3 flockHeading;
        public Vector3 flockCenter;
        public Vector3 avoidanceHeading;
        public int numFlockmates;

        public static int Size {

            get { return sizeof(float) * 3 * 5 + sizeof(int); }
        }
    }

    //******************************************************
    //
    //      FUNCTIONS
    //
    //******************************************************

    /// <summary>
    //  Called when the object is created, before Start().
    /// </summary>
    private void Awake() {

        // Initialize singleton
        if (Instance != null && Instance != this) {

            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    //  Called before the first frame after the object is enabled.
    /// </summary>
    private void Start() {

        // Find all the boid instances in the scene
        _Boids = FindObjectsOfType<Boid>();
        for (int i = 0; i < _Boids.Length; i++) {

            _Boids[i].Init(null);
        }
    }

    /// <summary>
    //  Called once for each frame.
    /// </summary>
    private void Update() {

        if (_Boids != null) {

            // Extract the vector position and direction only from the boids (since were calculating on the GPU)
            int numBoids = _Boids.Length;
            var boidData = new BoidData[numBoids];

            for (int i = 0; i < _Boids.Length; i++) {

                boidData[i].position = _Boids[i].GetPosition();
                boidData[i].direction = _Boids[i].GetForward();
            }

            // Using a compute shader for optimization 
            var boidBuffer = new ComputeBuffer(numBoids, BoidData.Size);
            boidBuffer.SetData(boidData);

            ComputeShader.SetBuffer(0, "boids", boidBuffer);
            ComputeShader.SetInt("numBoids", numBoids);
            ComputeShader.SetFloat("viewRadius", BoidSettings.PerceptionRadius);
            ComputeShader.SetFloat("avoidRadius", BoidSettings.AvoidanceRadius);

            int threadGroups = Mathf.CeilToInt(numBoids / (float)_ThreadGroupSize);
            ComputeShader.Dispatch(0, threadGroups, 1, 1);

            boidBuffer.GetData(boidData);

            // Update individual boid instances with the data pulled from the compute shader
            for (int i = 0; i < _Boids.Length; i++) {

                _Boids[i].SetAverageAvoidanceHeading(boidData[i].avoidanceHeading);
                _Boids[i].SetAverageFlockHeading(boidData[i].flockHeading);
                _Boids[i].SetCenterOfFlock(boidData[i].flockCenter);
                _Boids[i].SetFlockSize(boidData[i].numFlockmates);

                _Boids[i].UpdateBoid();
            }
            boidBuffer.Release();
        }
    }

    /// <summary>
    //  Returns reference to the _Boids variable.
    /// </summary>
    /// <returns>
    //  Boid[]
    /// </returns>
    public Boid[] GetBoids() { return _Boids; }
}
