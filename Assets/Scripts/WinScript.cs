using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    private TextMesh WinText;
    private GameObject confetti;
    private AudioSource audioSource;
    private AudioClip audioClip;

    private void Awake()
    {
        confetti = transform.Find("confetti").gameObject;
        WinText = gameObject.GetComponentInChildren<TextMesh>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.clip = (AudioClip)Resources.Load(ResourcePath.WIN);
        WinText.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            print("Box collider");
            WinText.text = "You win!";
            StartCoroutine(StartConfetti());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            print("Box collider");
            WinText.text = "You win!";
            StartCoroutine(StartConfetti());
        }
    }

    IEnumerator StartConfetti() {
        confetti.SetActive(true);
        confetti.GetComponent<ParticleSystem>().Play();
        confetti.transform.Find("confetti2").GetComponent<ParticleSystem>().Play();
        audioSource.Play();
        yield return new WaitForSeconds(10f);
        confetti.SetActive(false);
        audioSource.Stop();
        confetti.GetComponent<ParticleSystem>().Stop();
        SceneManager.LoadScene("Scene1");
    }
}