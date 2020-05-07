using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBall : MonoBehaviour
{
    public Transform BallObj;
    // the radius of the ball mesh we are rolling
    public float Radius = 0.25f;
    public bool DebugDraw = false;
    private Vector3 prevPosition;

    void Start()
    {
        prevPosition = BallObj.transform.position;
    }

    void Update()
    {
        // Note: this is not velocity because it is not with respect to time
        Vector3 deltaPosition = BallObj.transform.position - prevPosition;
        float distanceTravelled = deltaPosition.magnitude;

        // Use arc length = radius * theta to get angle of wheel travel
        float angleOfTravel = (distanceTravelled / Radius) * Mathf.Rad2Deg;

        Vector3 ballFwd = Vector3.Normalize(Vector3.ProjectOnPlane(deltaPosition, Vector3.up));
        Vector3 axle  = Vector3.Cross(ballFwd, Vector3.down);

        // Apply rotation to wheel by making a quaternion from an angle-axis
        Quaternion rotationIncrement = Quaternion.AngleAxis(angleOfTravel, axle);

        // Compose the rotation with the existing wheel rotation
        BallObj.transform.rotation = rotationIncrement * BallObj.transform.rotation;

        prevPosition = BallObj.transform.position;

        if (DebugDraw)
        {
            Debug.DrawLine(BallObj.transform.position, BallObj.transform.position + (axle * .25f), Color.yellow);
        }
    }
}
