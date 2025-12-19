using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigameScript : MonoBehaviour
{
    //references
    public ActionDetector detector;

    //UI images for minigame and fade transition
    public Image fadeImage;
    public Image upKey;
    public Image downKey;
    public Image leftKey;
    public Image rightKey;

    //References to different type of fish you can catch
    public GameObject badFishPrefab;
    public GameObject goodFishPrefab;
    public GameObject greatFishPrefab;

    //Reference to script that plays sounds during the minigame
    public FishingSoundScript fishingSoundScript;

    public bool canInput = false;
    private int currPatternLength = 3;

    //Player gets two attempts at the minigame before catching a fish
    private int attemptsLeft = 2;

    //Values used to indicate each direction
    private const int upDir = 0;
    private const int downDir = 1;
    private const int leftDir = 2;
    private const int rightDir = 3;

    //Booleans that determine what type of fish the player caught
    private bool caughtFish = false;
    private bool caughtGreatFish = false;

    private float fadeDuration = 1.5f;

    //Values for setting the colors of the buttons
    private float defaultGray = 60f;
    private float highlightRed = 131f;
    private float highlightGreen = 135f;
    private float highlightBlue = 239f;

    public List<int> patternList = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());

        //False is passed in so an easier pattern (length = 3) is played first
        StartCoroutine(StartMinigame(false));

        //Assign the action detector for stamina management
        if (detector == null)
        {
            detector = FindObjectOfType<ActionDetector>();
            if (detector == null)
            {
                Debug.LogError("No ActionDetector found in the scene!");
            }
        }
    }


    // Update is called once per frame
    void Update()
    {  
        bool successfulAttempt = true;

        //Limits player input to only work when there is a pattern and it has finished presenting to the player
        if(canInput && patternList.Count > 0)
        {
            //Get the player's input using the arrow keys
            //If it matches the first value in the patternlist, highlight the corresponding button green and remove from the list.
            //Otherwise, highlight it red, decrement the number of attempts left, and mark as a failed attempt
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                if(patternList[0] == upDir)
                {
                    upKey.color = new Color(0f, highlightGreen / 255, 0f, 1f);
                    StartCoroutine(CorrectInput(upDir, patternList.Count == 1));
                    patternList.Remove(patternList[0]);
                }
                else
                {
                    upKey.color = new Color(highlightRed / 255, 0f, 0f, 1f);
                    StartCoroutine(IncorrectInput(upDir));
                    attemptsLeft--;
                    successfulAttempt = false;
                }
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                if(patternList[0] == downDir)
                {
                    downKey.color = new Color(0f, highlightGreen / 255, 0f, 1f);
                    StartCoroutine(CorrectInput(downDir, patternList.Count == 1));
                    patternList.Remove(patternList[0]);
                }
                else
                {
                    downKey.color = new Color(highlightRed / 255, 0f, 0f, 1f);
                    StartCoroutine(IncorrectInput(downDir));
                    attemptsLeft--;
                    successfulAttempt = false;
                }
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if(patternList[0] == leftDir)
                {
                    leftKey.color = new Color(0f, highlightGreen / 255, 0f, 1f);
                    StartCoroutine(CorrectInput(leftDir, patternList.Count == 1));
                    patternList.Remove(patternList[0]);
                }
                else
                {
                    leftKey.color = new Color(highlightRed / 255, 0f, 0f, 1f);
                    StartCoroutine(IncorrectInput(leftDir));
                    attemptsLeft--;
                    successfulAttempt = false;
                }
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                if(patternList[0] == rightDir)
                {
                    rightKey.color = new Color(0f, highlightGreen / 255, 0f, 1f);
                    StartCoroutine(CorrectInput(rightDir, patternList.Count == 1));
                    patternList.Remove(patternList[0]);
                }
                else
                {
                    rightKey.color = new Color(highlightRed / 255, 0f, 0f, 1f);
                    StartCoroutine(IncorrectInput(rightDir));
                    attemptsLeft--;
                    successfulAttempt = false;
                }
            }

            //Run if the pattern has been completed successfully
            if(patternList.Count == 0 && successfulAttempt)
            {
                //If the player has already completed a pattern before, mark that a great fish was caught.
                //Otherwise, mark that a good fish was caught
                if(caughtFish)
                {
                    caughtGreatFish = true;
                }
                else
                {
                    caughtFish = true;
                }

                //Decrement attempt number with a fun noise
                fishingSoundScript.CallGoodJobSound();
                attemptsLeft--;
            }

            //If two minigames were run, stop all inputs and start calculating the final result
            if(attemptsLeft == 0)
            {
                canInput = false;
                StartCoroutine(Wait());
            }

            //If attempts are still left and the current minigame has ended, start another one
            if((patternList.Count == 0 || !successfulAttempt) && attemptsLeft > 0)
            {
                canInput = false;
                StartCoroutine(StartMinigame(successfulAttempt));
            }
        }
    }

    public void CalcScore()
    {
        //Create and assign the fish prefab depending on how successful the player was
        GameObject fish;
        if(caughtGreatFish)
        {
            Debug.Log("Big fish");
            fish = Instantiate<GameObject>(greatFishPrefab);

            //Stamina updates
            PersistentPlayerData.Instance.wonMiniGame = true;
            PersistentPlayerData.Instance.bigRestore = true;
        }
        else if(caughtFish)
        {
            Debug.Log("Fish");
            fish = Instantiate<GameObject>(goodFishPrefab);

            //Stamina updates
            PersistentPlayerData.Instance.wonMiniGame = true;
            PersistentPlayerData.Instance.smallRestore = true;
        }
        else
        {
            Debug.Log("bad fish");
            fish = Instantiate<GameObject>(badFishPrefab);

            //Stamina updates
            PersistentPlayerData.Instance.wonMiniGame = false;
            PersistentPlayerData.Instance.gotBadFish = true;
        }

        //Play a sound depending on the fish caught and fade out before changing scenes
        fishingSoundScript.CallFishSound(caughtGreatFish, caughtFish);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
       
        //Let canvas image fade out over 1 seconds as a transition by changing the alpha value of the fade image
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2.0f);
       
        float timer = 0f;
       
        //Let canvas image fade out over 1 seconds as a transition by changing the alpha value of the fade image
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }

        //Load back to the maze scene
        SceneManager.LoadScene("MazeScene");
    }

    IEnumerator StartMinigame(bool increaseDiff)
    {
        //Set patternList to an empty list. 
        //If the player has already successfully completed the first minigame, increase currPatternLength 
        canInput = false;
        patternList = new List<int>();

        if(increaseDiff)
        {
            currPatternLength = 5;
        }

        //Create a random pattern (direction indicated by the value of the integer)
        for(int i = 0; i < currPatternLength; i++)
        {
            int randomKey = Random.Range(0, 4);
            patternList.Add(randomKey);
        }

        yield return new WaitForSeconds(2.0f);

        //Loop through the pattern, highlighting the corresponding keys for 1 second each with the appropriate sound
        for(int i = 0; i < patternList.Count; i++)
        {
            Debug.Log(patternList[i]);
            switch (patternList[i])
            {
                case upDir:
                    fishingSoundScript.CallUpSound();
                    upKey.color = new Color(highlightRed / 255, highlightGreen / 255, highlightBlue / 255, 1f);
                    yield return new WaitForSeconds(1.0f);
                    upKey.color = new Color(defaultGray / 255, defaultGray / 255, defaultGray / 255, 1f);
                    break;
                case downDir:
                    fishingSoundScript.CallDownSound();
                    downKey.color = new Color(highlightRed / 255, highlightGreen / 255, highlightBlue / 255, 1f);
                    yield return new WaitForSeconds(1.0f);
                    downKey.color = new Color(defaultGray / 255, defaultGray / 255, defaultGray / 255, 1f);
                    break;
                case leftDir:
                    fishingSoundScript.CallLeftSound();
                    leftKey.color = new Color(highlightRed / 255, highlightGreen / 255, highlightBlue / 255, 1f);
                    yield return new WaitForSeconds(1.0f);
                    leftKey.color = new Color(defaultGray / 255, defaultGray / 255, defaultGray / 255, 1f);
                    break;
                default:
                    fishingSoundScript.CallRightSound();
                    rightKey.color = new Color(highlightRed / 255, highlightGreen / 255, highlightBlue / 255, 1f);
                    yield return new WaitForSeconds(1.0f);
                    rightKey.color = new Color(defaultGray / 255, defaultGray / 255, defaultGray / 255, 1f);
                    break;
            }
            yield return new WaitForSeconds(0.5f);
        }
        canInput = true;
    }

    IEnumerator CorrectInput(int direction, bool isRoundOver)
    {
        canInput = false;

        //Play the corresponding key and sound based on player input, then revert back to base color
        switch(direction)
        {
            case upDir:
                fishingSoundScript.CallUpSound();
                yield return new WaitForSeconds(1.0f);
                upKey.color = new Color(defaultGray / 255, defaultGray / 255, defaultGray / 255, 1f);
                break;
            case downDir:
                fishingSoundScript.CallDownSound();
                yield return new WaitForSeconds(1.0f);
                downKey.color = new Color(defaultGray / 255, defaultGray / 255, defaultGray / 255, 1f);
                break;
            case leftDir:
                fishingSoundScript.CallLeftSound();
                yield return new WaitForSeconds(1.0f);
                leftKey.color = new Color(defaultGray / 255, defaultGray / 255, defaultGray / 255, 1f);
                break;
            default:
                fishingSoundScript.CallRightSound();
                yield return new WaitForSeconds(1.0f);
                rightKey.color = new Color(defaultGray / 255, defaultGray / 255, defaultGray / 255, 1f);
                break;
        }

        //Give the player input back if they still need to finish the minigame 
        if(!isRoundOver)
        {
            canInput = true;
        }
    }


    IEnumerator IncorrectInput(int direction)
    {
        //Play a sound for an incorrect button press
        canInput = false;
        fishingSoundScript.CallWrongSound();
        yield return new WaitForSeconds(1.0f);

        //Revert key back to base color
        switch(direction)
        {
            case upDir:
                upKey.color = new Color(defaultGray / 255, defaultGray / 255, defaultGray / 255, 1f);
                break;
            case downDir:
                downKey.color = new Color(defaultGray / 255, defaultGray / 255, defaultGray / 255, 1f);
                break;
            case leftDir:
                leftKey.color = new Color(defaultGray / 255, defaultGray / 255, defaultGray / 255, 1f);
                break;
            default:
                rightKey.color = new Color(defaultGray / 255, defaultGray / 255, defaultGray / 255, 1f);
                break;
        }
    }

    //Wait a bit after the last puzzle is finished before bringing up the fish
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        CalcScore();
    }
}
