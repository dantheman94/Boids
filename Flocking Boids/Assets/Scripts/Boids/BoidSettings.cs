using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoidSettings : ScriptableObject {

    //******************************************************
    //
    //      VARIABLES
    //
    //******************************************************

    public float MinimumSpeed = 10f;
    public float MaximumSpeed = 20f;
    public float PerceptionRadius = 2.5f;
    public float AvoidanceRadius = 1f;
    public float MaxSteeringForce = 3;
    
    public float AlignWeight = 1f;
    public float CohesionWeight = 1f;
    public float SeperationWeight = 1f;
    
    public float TargetWeight = 1f;

    [Header("Collisions")]
    public LayerMask ObstacleMask;
    public float BoundsRadius = 0.27f;
    public float AvoidCollisionWeight = 10.0f;
    public float CollisionAvoidanceDistance = 5f;

    const int _NumViewDirections = 300;
    public readonly Vector3[] Directions;

    //******************************************************
    //
    //      FUNCTIONS
    //
    //******************************************************
    
    /// <summary>
    //  Constructor
    /// </summary>
    BoidSettings() {

        Directions = new Vector3[_NumViewDirections];

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < _NumViewDirections; i++) {

            float t = (float)i / _NumViewDirections;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            Directions[i] = new Vector3(x, y, z);
        }
    }
}
