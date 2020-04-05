using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject HowToPlayPanel;

    bool isHowToPlayPanelActive;

    public void StartNewGame()
    {
        PlayerPrefManager.ResetPlayerState(3);
        StartCoroutine(LoadFirstLevel());        
    }    

    public void QuitGame()
    {
        Application.Quit();
    }   
    
    public void HowToPlay()
    {
        HowToPlayPanel.SetActive(!isHowToPlayPanelActive);
        isHowToPlayPanelActive = !isHowToPlayPanelActive;
    }

    IEnumerator LoadFirstLevel()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level 1");
    }
}
