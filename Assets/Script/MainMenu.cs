using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Play");   
    }

    public void OpenGuide()
    {
        SceneManager.LoadScene("Guide"); 
    }
    public void BackToMenu()
    {
    SceneManager.LoadScene("Menu");
    }

}
