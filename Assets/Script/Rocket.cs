using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] float rocketSpeed = 10;
    [SerializeField] AudioSource thrustSound;
    Quaternion objectRotation;
    Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        thrustSound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
}

    void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up);
            if (!thrustSound.isPlaying)
            {
                thrustSound.Play();
            }
        }
        else
        {
            thrustSound.Stop();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up);
            thrustSound.Stop();
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(Vector3.right);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(-Vector3.right);
        }
    }
}
