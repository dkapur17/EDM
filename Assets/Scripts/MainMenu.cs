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
        SceneManager.LoadScene("Level_YouAndI");
    }
    
    public void PlayYouAndI(){
        SceneManager.LoadScene("Level_YouAndI");
    }

    public void PlaySamurai(){
        SceneManager.LoadScene("Level_Samurai");    
    }

    public void PlayDioma(){
        SceneManager.LoadScene("Level_Dioma");        
    }

    public void PlayWarMachine(){
        SceneManager.LoadScene("Level_WarMachine");
    }

    public void PlayRedemption(){
        SceneManager.LoadScene("Level_Redemption");
    }

    public void PlayEgo(){
        SceneManager.LoadScene("Level_Ego");
    }

    public void PlayVideo () {
        beginning = Time.time;
        videoPlayer.Play();
        // StartCoroutine(waitmethod());
        Invoke("PlayGame", (float)videoPlayer.length);
    }

    // IEnumerator waitmethod()
    // {
    //     while (Time.time - beginning < videoPlayer.length)
    //     {
    //         yield return null;
    //     }
    //     PlayGame();
    // }

    public void QuitGame () {
        Application.Quit();
    }
}
