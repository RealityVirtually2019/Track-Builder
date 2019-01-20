using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAudioManager : MonoBehaviour {

    private AudioSource audioSource;
    private AudioClip rollingClip;
    private AudioClip clinkingClip;

    private float ballEnters = 0f;
    private bool ballHasEntered = false;
    private bool sourceHasStarted = false;
    

    private void OnCollisionEnter(Collision collision)
    {
        ballEnters = 0f;
        playBallCollisionEnter();
        print("Collision Out: " + collision.gameObject.name);
        audioSource.loop = false;
        startTimer();
        print("onCollEnter");
    }

    private void OnCollisionStay(Collision collision)
    {
        if (ballEnters > .2f) {
        audioSource.Pause();
            audioSource.loop = true;
        audioSource.clip = rollingClip;
        audioSource.Play();
            sourceHasStarted = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        ballEnters = 0f;
        print("onCollExit");
        sourceHasStarted = false;
    }

    private void startTimer() {
        ballHasEntered = true;
    }

    private void resetTimer() {
        ballHasEntered = false;
        ballEnters = 0f;
    }

    private void playBallCollisionEnter() {
        if (!sourceHasStarted)
        {
            audioSource.clip = clinkingClip;
            sourceHasStarted = true;
            print("playBallCollisionEnter");
            audioSource.Play();
        }
    }

    // Use this for initialization
    void Start () {
		
	}

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        rollingClip = (AudioClip) Resources.Load(ResourcePath.ROLLING);
        clinkingClip = (AudioClip)Resources.Load(ResourcePath.CLINKING);
    }

    // Update is called once per frame
    void Update () {
        if (ballHasEntered) {
            ballEnters += Time.deltaTime;
        }
	}
}
