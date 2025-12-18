using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PondScript : MonoBehaviour
{
    public GameObject text;
    public Image fadeImage;
    private bool seesPlayer = false; 
    private float fadeDuration = 1.5f;

    // Update is called once per frame
    void Update()
    {
        if(seesPlayer && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(FadeOut());
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Player"))
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

    IEnumerator FadeOut()
    {
        float timer = 0f;
        
        //Let canvas image fade out over 1.5 seconds as a transition
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }

        //Load scene for fishing minigame
        SceneManager.LoadScene("FishingScene");
    }
}