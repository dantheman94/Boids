using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    //******************************************************
    //
    //      VARIABLES
    //
    //******************************************************

    private static float movementSpeed = 1.0f;
    private bool _bLocked = false;
    private Transform _TargetBoidTransform = null;

    //******************************************************
    //
    //      FUNCTIONS
    //
    //******************************************************

    void Update() {

        // Free camera movement (if not locked)
        if (!_bLocked) {

            movementSpeed = Mathf.Max(movementSpeed += Input.GetAxis("Mouse ScrollWheel"), 0.0f);
            transform.position += (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical") + transform.up * Input.GetAxis("Depth")) * movementSpeed;
            transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        }

        // Unlock camera
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)) {

            _bLocked = false;
            _TargetBoidTransform = null;
        }

        // If theres a target boid transform to follow
        if (_TargetBoidTransform != null) {

            // Follow it
            transform.SetPositionAndRotation(_TargetBoidTransform.position, _TargetBoidTransform.rotation);
        }

        // Boid view
        if (Input.GetKeyDown(KeyCode.Alpha1)) {

            _bLocked = true;
            _TargetBoidTransform = BoidManager.Instance.GetBoids()[1].transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {

            _bLocked = true;
            _TargetBoidTransform = BoidManager.Instance.GetBoids()[2].transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {

            _bLocked = true;
            _TargetBoidTransform = BoidManager.Instance.GetBoids()[3].transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {

            _bLocked = true;
            _TargetBoidTransform = BoidManager.Instance.GetBoids()[4].transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {

            _bLocked = true;
            _TargetBoidTransform = BoidManager.Instance.GetBoids()[5].transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {

            _bLocked = true;
            _TargetBoidTransform = BoidManager.Instance.GetBoids()[6].transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {

            _bLocked = true;
            _TargetBoidTransform = BoidManager.Instance.GetBoids()[7].transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8)) {

            _bLocked = true;
            _TargetBoidTransform = BoidManager.Instance.GetBoids()[8].transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) {

            _bLocked = true;
            _TargetBoidTransform = BoidManager.Instance.GetBoids()[9].transform;
        }
    }
}
