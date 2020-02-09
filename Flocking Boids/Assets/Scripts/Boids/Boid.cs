using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

    //******************************************************
    //
    //      VARIABLES
    //
    //******************************************************

    private Vector3 _Position;
    private Vector3 _Forward;
    private Vector3 _Velocity;

    private Vector3 _Acceleration;
    private Vector3 _AvgFlockHeading;
    private Vector3 _AvgAvoidanceHeading;
    private Vector3 _CenterOfFlock;
    private int _FlockSize;

    private Material _Material;
    Transform _CachedTransform;
    Transform _TargetTransform;

    //******************************************************
    //
    //      FUNCTIONS
    //
    //******************************************************

    /// <summary>
    //  Called when the object is created, before Start().
    /// </summary>
    private void Awake() {

        _Material = transform.GetComponentInChildren<MeshRenderer>().material;
        _CachedTransform = transform;
    }

    /// <summary>
    //  Initializes the boid.
    /// </summary>
    //  <param name="target"></param>
    public void Init(Transform target) {

        _TargetTransform = target;
        _Position = _CachedTransform.position;
        _Forward = _CachedTransform.forward;

        // Initial velocity
        float startSpeed = (BoidManager.Instance.BoidSettings.MinimumSpeed + BoidManager.Instance.BoidSettings.MaximumSpeed) / 2;
        _Velocity = transform.forward * startSpeed;

    }

    /// <summary>
    //  Called once every frame from the Boid compute shader.
    /// </summary>
    public void UpdateBoid() {

        Vector3 acceleration = Vector3.zero;

        if (_TargetTransform != null) {

            Vector3 offsetToTarget = (_TargetTransform.position - _Position);
            acceleration = SteerTowards(offsetToTarget) * BoidManager.Instance.BoidSettings.TargetWeight;
        }

        if (_FlockSize != 0) {

            // Determine the center position of the current flock
            _CenterOfFlock /= _FlockSize;

            Vector3 offsetToFlockCenter = (_CenterOfFlock - _Position);

            var alignmentForce = SteerTowards(_AvgFlockHeading) * BoidManager.Instance.BoidSettings.AlignWeight;
            var cohesionForce = SteerTowards(offsetToFlockCenter) * BoidManager.Instance.BoidSettings.CohesionWeight;
            var seperationForce = SteerTowards(_AvgAvoidanceHeading) * BoidManager.Instance.BoidSettings.SeperationWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;
        }

        // Try to steer away from the incoming collision
        if (IsHeadingForCollision()) {

            Vector3 collisionAvoidDir = ObstacleRays();
            Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * BoidManager.Instance.BoidSettings.AvoidCollisionWeight;
            acceleration += collisionAvoidForce;
        }

        // Calculate velocity
        _Velocity += acceleration * Time.deltaTime;
        float speed = _Velocity.magnitude;
        Vector3 dir = _Velocity / speed;
        speed = Mathf.Clamp(speed, BoidManager.Instance.BoidSettings.MaximumSpeed, BoidManager.Instance.BoidSettings.MinimumSpeed);
        _Velocity = dir * speed;

        _CachedTransform.position += _Velocity * Time.deltaTime;
        _CachedTransform.forward = dir;
        _Position = _CachedTransform.position;
        _Forward = dir;
    }
    
    /// <summary>
    //  Sets the boid's colour.
    /// </summary>
    public void SetColour(Color color) {

        if (_Material != null) { _Material.color = color; }
    }

    /// <summary>
    //  Returns if whether the boid is about to collide with something or not.
    /// </summary>
    private bool IsHeadingForCollision() {

        // Sphere cast is more reliable than a single ray
        return Physics.SphereCast(_Position, BoidManager.Instance.BoidSettings.BoundsRadius, _Forward, out RaycastHit hit, BoidManager.Instance.BoidSettings.CollisionAvoidanceDistance, BoidManager.Instance.BoidSettings.ObstacleMask);
    }

    /// <summary>
    //  Fires multiple rays for obstacle avoidance.
    /// </summary>
    private Vector3 ObstacleRays() {

        Vector3[] rayDirs = BoidManager.Instance.BoidSettings.Directions;
        for (int i = 0; i < rayDirs.Length; i++) {

            Vector3 dir = _CachedTransform.TransformDirection(rayDirs[i]);
            Ray ray = new Ray(_Position, dir);
            if (!Physics.SphereCast(ray, BoidManager.Instance.BoidSettings.BoundsRadius, BoidManager.Instance.BoidSettings.CollisionAvoidanceDistance, BoidManager.Instance.BoidSettings.ObstacleMask)) {

                return dir;
            }
        }

        return _Forward;
    }

    /// <summary>
    //  Steer towards the vector specified.
    /// </summary>
    private Vector3 SteerTowards(Vector3 vector) {

        Vector3 v = vector.normalized * BoidManager.Instance.BoidSettings.MaximumSpeed - _Velocity;
        return Vector3.ClampMagnitude(v, BoidManager.Instance.BoidSettings.MaxSteeringForce);
    }

    /// <summary>
    //  Returns reference to the _Position variable.
    /// </summary>
    //  <returns></returns>
    public Vector3 GetPosition() { return _Position; }

    /// <summary>
    //  Returns reference to the _Forward variable.
    /// </summary>
    //  <returns></returns>
    public Vector3 GetForward() { return _Forward; }

    /// <summary>
    //  Sets the _AvgFlockHeading variable vector value.
    /// </summary>
    //  <param name="value"></param>
    public void SetAverageFlockHeading(Vector3 value) { _AvgFlockHeading = value; }

    /// <summary>
    //  Sets the AvgAvoidanceHeading variable vector value.
    /// </summary>
    //  <param name="value"></param>
    public void SetAverageAvoidanceHeading(Vector3 value) { _AvgAvoidanceHeading = value; }

    /// <summary>
    //  Sets the _CenterOffFlock vector value.
    /// </summary>
    //  <param name="value"></param>
    public void SetCenterOfFlock(Vector3 value) { _CenterOfFlock = value; }
    
    /// <summary>
    //  Sets the _FlockSize integer value.
    /// </summary>
    //  <param name="value"></param>
    public void SetFlockSize(int value) { _FlockSize = value; }
}
