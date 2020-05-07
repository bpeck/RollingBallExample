using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TireRotationFromMovement : MonoBehaviour
{
    // GameObject that is the parent of all tires. This object's movement corresponds to movement of the tires
    public GameObject wheelChassisPivot;

    // GameObjects that will be rotated in order to rotate the tire due to movement
    public GameObject[] wheelPivotObjects;

    public float[] wheelRadii;

    // Axis in local space of the tire pivot object around which we will rotate the tire on it's axle.
    public Axis wheelAxleLocalAxis = Axis.X;

    private Vector3[] _wheelPrevPos = new Vector3[0];

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject wheel in wheelPivotObjects)
        {
            Debug.Log(string.Format("Found wheel {0}", wheel.name));
        }

        _wheelPrevPos = new Vector3[wheelPivotObjects.Length];
    }

    private Vector3 GetAxisFromEnum(Transform t, Axis a)
    {
        switch(a)
        {
            case Axis.X: return t.right;
            case Axis.Y: return t.up;
            case Axis.Z: return t.forward;
            default: return Vector3.zero;
        }
    }

    void Update()
    {
        int i = 0;
        foreach (GameObject wheel in wheelPivotObjects)
        {
            // How far has the wheel travelled this tick
            Vector3 deltaPosition = wheel.transform.position - _wheelPrevPos[i];
            float distanceTravelled = deltaPosition.magnitude;

            if (deltaPosition.magnitude > 0.00001f)
            {
                // Get a vector that runs down the axle of the wheel
                Vector3 axle = GetAxisFromEnum(wheel.transform, wheelAxleLocalAxis);

                // Use arc length = radius * theta to get angle of wheel travel
                float angleOfTravel = distanceTravelled / wheelRadii[i];
                // Convert to degrees
                angleOfTravel *= Mathf.Rad2Deg;

                // Derive a forward vector from world up cross axle
                Vector3 wheelFwd = Vector3.Cross(-Vector3.up, axle);
                // Get heading of movement based
                Vector3 motionHeading = Vector3.Normalize(Vector3.ProjectOnPlane(deltaPosition, Vector3.up));
                float forwardMovement = Mathf.Sign(Vector3.Dot(motionHeading, wheelFwd));

                // Apply rotation to wheel by making a quaternion from an angle-axis
                Quaternion wheelRotationIncrement = Quaternion.AngleAxis(angleOfTravel * forwardMovement, axle);
                // Compose the rotation with the existing wheel rotation
                wheel.transform.rotation = wheelRotationIncrement * wheel.transform.rotation;

                /*
                Debug.DrawLine(wheel.transform.position, wheel.transform.position + (motionHeading * .25f));
                Debug.DrawLine(wheel.transform.position, wheel.transform.position + (wheelFwd * .25f), Color.red);
                Debug.DrawLine(wheel.transform.position, wheel.transform.position + (axle * .25f), Color.green);
                Debug.DrawLine(wheel.transform.position, wheel.transform.position + (Vector3.up * .25f), Color.blue);
                */
            }

            _wheelPrevPos[i] = wheel.transform.position;
            i++;
        }
    }
}
