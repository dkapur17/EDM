using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    private float beginning;

    public void PlayGame () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void PlayVideo () {
        beginning = Time.time;
        videoPlayer.Play();
        StartCoroutine(waitmethod());
    }

    IEnumerator waitmethod()
    {
        while (Time.time - beginning < videoPlayer.length)
        {
            yield return null;
        }
        PlayGame();
    }

    public void QuitGame () {
        Application.Quit();
    }
}
