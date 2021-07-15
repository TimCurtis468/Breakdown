using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public static int score;

    public void ChangeMenuScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void buySoundtrack()
    {
        Application.OpenURL("https://penniesonoureyes.bandcamp.com/album/virtually-vintage-soundtrack");
    }
}
