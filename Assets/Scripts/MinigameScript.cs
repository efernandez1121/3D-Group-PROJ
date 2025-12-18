using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigameScript : MonoBehaviour
{
    public Image fadeImage;
    public Image upKey;
    public Image downKey;
    public Image leftKey;
    public Image rightKey;

    public GameObject badFishPrefab;
    public GameObject goodFishPrefab;
    public GameObject greatFishPrefab;

    public bool canInput = false;
    private int currPatternLength = 3;

    private int attemptsLeft = 2;

    private const int upDir = 0;
    private const int downDir = 1;
    private const int leftDir = 2;
    private const int rightDir = 3;

    private bool caughtFish = false;
    private bool caughtGreatFish = false;

    private float fadeDuration = 1.5f;
    private float defaultGray = 60f;
    private float highlightRed = 131f;
    private float highlightGreen = 135f;
    private float highlightBlue = 239f;
    public List<int> patternList = new List<int>();


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
        //False is passed in so an easier pattern is played first
        StartCoroutine(StartMinigame(false));
    }

    // Update is called once per frame
    void Update()
    {   
        bool successfulAttempt = true;

        if(canInput && patternList.Count > 0)
        {
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

            if(patternList.Count == 0 && successfulAttempt)
            {
                if(caughtFish)
                {
                    caughtGreatFish = true;
                }
                else
                {
                    caughtFish = true;
                }
                attemptsLeft--;
            }

            if(attemptsLeft == 0)
            {
                canInput = false;
                CalcScore();
            }

            if((patternList.Count == 0 || !successfulAttempt) && attemptsLeft > 0)
            {
                canInput = false;
                StartCoroutine(StartMinigame(successfulAttempt));
            }
            
        }
    }

    public void CalcScore()
    {
        GameObject fish;
        if(caughtGreatFish)
        {
            Debug.Log("Big ass fish");
            fish = Instantiate<GameObject>(greatFishPrefab);
        }
        else if(caughtFish)
        {
            Debug.Log("Fish");
            fish = Instantiate<GameObject>(goodFishPrefab);
        }
        else
        {
            Debug.Log("No fish");
            fish = Instantiate<GameObject>(badFishPrefab);
        }

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        
        //Let canvas image fade in over 1.5 seconds as a transition
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
        
        //Let canvas image fade out over 1.5 seconds as a transition
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
        canInput = false;
        patternList = new List<int>();

        if(increaseDiff)
        {
            currPatternLength = 5;
        }

        for(int i = 0; i < currPatternLength; i++)
        {
            int randomKey = Random.Range(0, 4);
            patternList.Add(randomKey);
        }

        yield return new WaitForSeconds(2.0f); 

        for(int i = 0; i < patternList.Count; i++)
        {
            Debug.Log(patternList[i]);
            switch (patternList[i])
            {
                case upDir:
                    upKey.color = new Color(highlightRed / 255, highlightGreen / 255, highlightBlue / 255, 1f);
                    yield return new WaitForSeconds(1.0f);
                    upKey.color = new Color(defaultGray / 255, defaultGray / 255, defaultGray / 255, 1f);
                    break; 
                case downDir:
                    downKey.color = new Color(highlightRed / 255, highlightGreen / 255, highlightBlue / 255, 1f);
                    yield return new WaitForSeconds(1.0f);
                    downKey.color = new Color(defaultGray / 255, defaultGray / 255, defaultGray / 255, 1f);
                    break;
                case leftDir:
                    leftKey.color = new Color(highlightRed / 255, highlightGreen / 255, highlightBlue / 255, 1f);
                    yield return new WaitForSeconds(1.0f);
                    leftKey.color = new Color(defaultGray / 255, defaultGray / 255, defaultGray / 255, 1f);
                    break;
                default:
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
        yield return new WaitForSeconds(1.0f);
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
        if(!isRoundOver)
        {
            canInput = true;
        }
    }

    IEnumerator IncorrectInput(int direction)
    {
        canInput = false;
        yield return new WaitForSeconds(1.0f);
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
}