using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    //******************************************************
    //
    //      VARIABLES
    //
    //******************************************************

    public enum GizmoType { Never, SelectedOnly, Always }

    public Boid prefab;
    public float spawnRadius = 10;
    public int spawnCount = 10;
    public GizmoType showSpawnRegion;

    //******************************************************
    //
    //      FUNCTIONS
    //
    //******************************************************

    /// <summary>
    //  Called before the first frame after the object is enabled.
    /// </summary>
    void Start() {

        // Instantiate boids
        for (int i = 0; i < spawnCount; i++) {

            // Create boid instance
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            Boid boid = Instantiate(prefab);
            boid.transform.position = pos;
            boid.transform.forward = Random.insideUnitSphere;

            // Set random colour for the boid
            Color col = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            boid.SetColour(col);
        }
    }
}