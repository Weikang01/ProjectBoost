using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [Header("Power System")]
    [SerializeField] float rcsThrust = 150f;
    [SerializeField] float mainThrust = 300f;

    [Header("Sound Effect")]
    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip levelSound;

    [Header("Partical System")]
    [SerializeField] ParticleSystem rocketJetParticles;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem successParticles;

    Rigidbody rb;
    AudioSource myAudioSource;
    bool collisionAreEnabled = true;

    enum State { Alive, Dying, Transcending }
    State currentState = State.Alive;

    void Start() {
        rb = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (currentState == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
            if (Debug.isDebugBuild)
            {
                RespondToDebugKeys();
            }
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
            rocketJetParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!myAudioSource.isPlaying)
        {
            myAudioSource.PlayOneShot(mainEngineSound);
        }
        rocketJetParticles.Play();
    }

    void RespondToRotateInput()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            RotateManually(rotationThisFrame);
        }
        if (Input.GetKey(KeyCode.D))
        {
            RotateManually(-rotationThisFrame);
        }
    }

    private void RotateManually(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame);
        rb.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState != State.Alive || !collisionAreEnabled) { return; }

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
        rocketJetParticles.Stop();
        successParticles.Play();
        myAudioSource.PlayOneShot(levelSound);
        Invoke("LoadNextScene", 1f);
    }

    private void StartDeathSequence()
    {
        currentState = State.Dying;
        myAudioSource.Stop();
        rocketJetParticles.Stop();
        explosionParticles.Play();
        myAudioSource.PlayOneShot(deathSound);
        Invoke("LoadFirstScene", 1f);
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionAreEnabled = !collisionAreEnabled;
        }
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        if(currentSceneIndex == sceneCount - 1)
        {
            SceneManager.LoadScene(0);
        } else if(currentSceneIndex < sceneCount - 1)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }

    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }
}