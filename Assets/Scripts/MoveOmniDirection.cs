using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOmniDirection : MonoBehaviour
{
    public Transform CameraRelativeInput = null;
    public float MaxSpeed = 0.2f;
    public float Acceleration = 0.1f;
    public float Deacceleration = 0.075f;
    public bool DebugDraw = false;

    private Vector3 velocityVector = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {

    }

    void CheckForQuitSignal()
    {
        if (Input.GetKey("q") || Input.GetKeyDown(KeyCode.Escape))
        {
             #if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
             #else
             Application.Quit();
             #endif
        }
    }

    Vector3 GetInputVector()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (CameraRelativeInput != null)
        {
            input = CameraRelativeInput.transform.TransformVector(input);
        }
        input = Vector3.ProjectOnPlane(input, Vector3.up);
        return Vector3.ClampMagnitude(input, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForQuitSignal();

        Vector3 inputVector = GetInputVector();
        Vector3 accelerationVector = inputVector * Acceleration;
        Vector3 deaccelerationVector = velocityVector * -Deacceleration;

        // accelerate velocity
        velocityVector += accelerationVector * Time.deltaTime;
        // deaccelerate velocity
        velocityVector += deaccelerationVector * Time.deltaTime;

        velocityVector = Vector3.ClampMagnitude(velocityVector, MaxSpeed);

        // Debug draw input and velocity
        if(DebugDraw)
        {
            Vector3 currentPosition = this.gameObject.transform.position;
            Debug.DrawLine(currentPosition, currentPosition + (inputVector * 2f), Color.red);
            Debug.DrawLine(currentPosition, currentPosition + (velocityVector * 20f), Color.green);
        }

        // apply velocity to position
        this.gameObject.transform.position += velocityVector;
    }
}
