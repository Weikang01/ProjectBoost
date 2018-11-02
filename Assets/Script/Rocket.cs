using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 150f;
    [SerializeField] float mainThrust = 300f;
    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip levelSound;

    Rigidbody rb;
    AudioSource myAudioSource;

    enum State { Alive, Dying, Transcending}
    State currentState = State.Alive;

    void Start () {
        rb = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }
	
	void Update () {
        if(currentState == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
}

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            myAudioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!myAudioSource.isPlaying)
        {
            myAudioSource.PlayOneShot(mainEngineSound);
        }
    }

    void RespondToRotateInput()
    {
        rb.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rb.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState != State.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish ":
                StartLevelSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartLevelSequence()
    {
        currentState = State.Transcending;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(levelSound);
        Invoke("LoadNextScene", 1f);
    }

    private void StartDeathSequence()
    {
        currentState = State.Dying;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(deathSound);
        Invoke("LoadFirstScene", 1f);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }
}