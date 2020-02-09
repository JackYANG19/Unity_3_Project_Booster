using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

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
            Thrust();
            Rotate();
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
                state = State.Transcending;
                Invoke("loadNextLevel", 1f);
                break;
            default:
                state = State.Dying;
                audioSource.Pause();
                Invoke("loadFirstLevel", 3f);
                break;
        }
    }

    private void loadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void loadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust); // can rotate during thrusting
            if (!audioSource.isPlaying) // so it doesn't layer
            {
                audioSource.Play(0);
            }
        }
        else
        {
            audioSource.Pause();
        }
    }
    private void Rotate()
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
