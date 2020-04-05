using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStats : MonoBehaviour
{
    public TextMeshProUGUI PlayerScore;
    public TextMeshProUGUI BestScore;

    private void Start()
    {
        SoundManager.sm.PlayMusicForLevel(SceneManager.GetActiveScene());

        PlayerScore.text += PlayerPrefManager.GetScore().ToString();
        BestScore.text += PlayerPrefManager.GetHighscore().ToString();
    }
}


