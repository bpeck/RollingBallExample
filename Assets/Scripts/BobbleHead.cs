using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbleHead : MonoBehaviour
{
    public Transform HeadPivot;
    private Vector3 prevPosition;

    void Start()
    {
        if (HeadPivot != null)
        {
            prevPosition = HeadPivot.transform.position;
        }
    }

    void Update()
    {
        if(HeadPivot == null)
        {
            return;
        }

        Vector3 deltaPosition = HeadPivot.transform.position - prevPosition;
        // project onto ground plane and normalize to get movement heading on ground plane
        Vector3 movementHeading = Vector3.Normalize(Vector3.ProjectOnPlane(deltaPosition, Vector3.up));

        // Twist head to look along the vector of movement heading
        Quaternion FacingGoal = Quaternion.identity;
        if (movementHeading.sqrMagnitude > 0)
        {
            Quaternion.LookRotation(movementHeading, Vector3.up);
        }

        // Pitch head down according to current speed, to lean into the movement
        float speedSqr = deltaPosition.sqrMagnitude;
        // Lean somewhere between 0 and 45 degrees based on current speed.
        // TODO: replace this lerp with a hand-tuned float curve?
        // TODO: what if we are decelerating? Shouldn't we be leaning back, "against" the speed?
        float leanAngle = Mathf.Lerp(0.0f, 45.0f, speedSqr * 2000.0f);
        Quaternion LeaningGoal = Quaternion.AngleAxis(leanAngle, Vector3.right);

        // Combine goal rotations into one
        Quaternion OverallGoalRotation = FacingGoal * LeaningGoal;

        // TODO: use interpolation to find intermediate value between current rotation and goal rotation

        // TODO: decompose into swing and twist rotations and enforce "speed limits"
        // so that movements feel more realisitic, eg simulate limitations of real-life servo motor speeds

        HeadPivot.rotation = OverallGoalRotation;

        prevPosition = HeadPivot.transform.position;
    }

    // Swing-Twist decomposition by Allen Chou at http://allenchou.net/2018/05/game-math-swing-twist-interpolation-sterp/
    void DecomposeSwingTwist(Quaternion q, Vector3 twistAxis, out Quaternion swing, out Quaternion twist)
    {
        Vector3 r = new Vector3(q.x, q.y, q.z);

        // Handle special case: rotation by 180 degree
        if (r.sqrMagnitude < Mathf.Epsilon)
        {
            Vector3 rotatedTwistAxis = q * twistAxis;
            Vector3 swingAxis = Vector3.Cross(twistAxis, rotatedTwistAxis);

            if (swingAxis.sqrMagnitude > Mathf.Epsilon)
            {
                float swingAngle = Vector3.Angle(twistAxis, rotatedTwistAxis);
                swing = Quaternion.AngleAxis(swingAngle, swingAxis);
            }
            else
            {
                // more singularity:
                // rotation axis parallel to twist axis
                swing = Quaternion.identity; // no swing
            }

            // always twist 180 degree on singularity
            twist = Quaternion.AngleAxis(180.0f, twistAxis);
            return;
        }

        // meat of swing-twist decomposition
        Vector3 p = Vector3.Project(r, twistAxis);
        twist = new Quaternion(p.x, p.y, p.z, q.w);
        twist = Quaternion.Normalize(twist);
        swing = q * Quaternion.Inverse(twist);
    }
}
