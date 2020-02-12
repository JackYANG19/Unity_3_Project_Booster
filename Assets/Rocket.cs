using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoad = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip Success;
    [SerializeField] AudioClip Death;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending};
    State state;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        state = State.Alive;
    }
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive)
        {
            RespondToThrust();
            RespondToRotate();
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(Success);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoad);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(Death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoad);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void RespondToThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    } // TODO: frame dependent

    private void ApplyThrust()
    {
        float ThrustThisFrame = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * ThrustThisFrame); // can rotate during thrusting
        if (!audioSource.isPlaying) // so it doesn't layer
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotate()
    {
        rigidBody.freezeRotation = true; // take manul control of the rocket
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false; // resume physics control of rotation
    }

    
}
