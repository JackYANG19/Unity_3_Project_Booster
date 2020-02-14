using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;
    float movementFactor;

    Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        // float cycle = period <= Mathf.Epsilon ? 0 : Time.time / period;
        if (period <= Mathf.Epsilon) { return; }

        const float tau = Mathf.PI * 2f;
        float cycles = Time.time / period;
        
        float rawSinWave = Mathf.Sin(cycles * tau - Mathf.PI / 2f); // (-1, 1) start from -1
        movementFactor = rawSinWave / 2f + 0.5f; // (0, 1) start from 0
        
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
	}
}
