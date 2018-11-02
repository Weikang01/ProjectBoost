using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCamera : MonoBehaviour {

    [SerializeField] GameObject cameraObject;

	// Use this for initialization
	void Start () {
        transform.position = cameraObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = cameraObject.transform.position;
    }
}