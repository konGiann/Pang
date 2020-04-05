using UnityEngine;

public static class PlayerPrefManager
{
    public static int GetLives()
    {
        if (PlayerPrefs.HasKey("Lives"))
            return PlayerPrefs.GetInt("Lives");
        else
            return 0;
    }

    public static void SetLives(int lives)
    {
        PlayerPrefs.SetInt("Lives", lives);
    }

    public static int GetScore()
    {
        if (PlayerPrefs.HasKey("Score"))
            return PlayerPrefs.GetInt("Score");
        else
            return 0;
    }

    public static void SetScore(int score)
    {
        PlayerPrefs.SetInt("Score", score);
    }

    public static int GetHighscore()
    {
        if (PlayerPrefs.HasKey("Highscore"))
            return PlayerPrefs.GetInt("Highscore");
        else
            return 0;
    }

    public static void SetHighscore(int highscore)
    {
        PlayerPrefs.SetInt("Highscore", highscore);
    }

    // Save player stats and keep them when moving to the next level
    public static void SavePlayerStats(int lives, int score, int highscore)
    {
        PlayerPrefs.SetInt("Lives", lives);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("Highscore", highscore);
    }
    
    public static void ResetPlayerState(int startingLives)
    {
        Debug.Log("Reseting player stats");
        PlayerPrefs.SetInt("Lives", startingLives);
        PlayerPrefs.SetInt("Score", 0);
    }
}
