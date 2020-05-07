using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscInput : MonoBehaviour
{
    public float Speed = .2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            this.gameObject.transform.position += this.gameObject.transform.forward * Speed * Time.deltaTime;
        }
        else if (Input.GetKey("s"))
        {
            this.gameObject.transform.position -= this.gameObject.transform.forward * Speed * Time.deltaTime;
        }
        else if (Input.GetKey("q") || Input.GetKeyDown(KeyCode.Escape))
        {
             #if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
             #else
             Application.Quit();
             #endif
        }
    }
}
