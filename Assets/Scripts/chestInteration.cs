using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class chestInteration : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip winAudio;
    public float volume;

    public GameObject text;
    private bool seesPlayer = false;
    private float endDelay = 2.5f;
    // Update is called once per frame

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // makees the chest the audio 
    }
    void Update()
    {
        if (seesPlayer && Input.GetKeyDown(KeyCode.F))
        {
            audioSource.PlayOneShot(winAudio, volume);
            StartCoroutine(WinCase());
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            seesPlayer = true;
            text.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        seesPlayer = false;
        text.SetActive(false);
    }

    IEnumerator WinCase()
    {
        
        float timer = 0f;

        //pause before 
        while (timer < endDelay)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        //Load scene for fishing minigame
        SceneManager.LoadScene("GameOver");
    }
}
