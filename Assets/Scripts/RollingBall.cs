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

        // TODO Use arc length = radius * theta to get angle of wheel travel
        float angleOfTravel = /* ??? */ 0;

        // TODO Make a unity length (ie normalized) direction vector for forward motion of the ball
        Vector3 ballFwd = Vector3.Normalize(Vector3.ProjectOnPlane(/* ??? */, /* ??? */));
        // TODO Make a vector about which the ball will need to rotate in order to move forward for this tick
        Vector3 axle  = Vector3.Cross(/* ??? */, Vector3.down);

        // TODO Create quaternion for the rotation of the ball on this tick
        Quaternion rotationIncrement = /* ??? */;

        // TODO Compose the rotation with the existing wheel rotation
        BallObj.transform.rotation = /* ??? */ * /* ??? */;

        prevPosition = BallObj.transform.position;

        if (DebugDraw)
        {
            Debug.DrawLine(BallObj.transform.position, BallObj.transform.position + (axle * .25f), Color.yellow);
        }
    }
}
