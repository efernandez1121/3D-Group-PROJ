using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PondScript : MonoBehaviour
{
    [Header("References")]
    public SimplePlayerMovement player;
    public Functions staminaData;
    public GameObject text;


    //Acts as transition before switching scenes
    public Image fadeImage;

    private bool seesPlayer = false;
    private float fadeDuration = 1.5f;


    // Update is called once per frame
    void Update()
    {
        //If the player is close to the pond and presses F, start transition to minigame scene
        if(seesPlayer && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(FadeOut());
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //When player is close enough to start the minigame, make text appear to tell player what to do
        if(collider.gameObject.CompareTag("Player"))
        {
            seesPlayer = true;
            text.SetActive(true);
        }
    }


    private void OnTriggerExit(Collider collider)
    {
        //Make text disappear and make sure player can't start the minigame
        seesPlayer = false;
        text.SetActive(false);
    }


    IEnumerator FadeOut()
    {
        float timer = 0f;
       
        //Let canvas image fade out over 1 seconds as a transition by changing the alpha value of the fade image
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }
        //Save player data
        PersistentPlayerData.Instance.savedPlayerPosition = player.CurrPosition();
        PersistentPlayerData.Instance.savedStamina = staminaData.currStamina;


        //Load scene for fishing minigame
        SceneManager.LoadScene("MinigameScene");
    }
}