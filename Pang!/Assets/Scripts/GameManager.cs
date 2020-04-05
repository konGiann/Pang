using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    [Header("Game Settings")]      
    public int PlayerLives = 3;
    public int PlayerScore = 0;
    public int PlayerHighscore = 0;    
    public string NextLevelToLoad;
    public string GameOverScreen;    
    [Tooltip("Assign all possible balloon prefabs.")]
    [HideInInspector]
    public List<GameObject> BalloonsPrefabs;    

    [Header("UI Settings")]
    public TextMeshProUGUI UIScore;
    public TextMeshProUGUI UILevel;
    public TextMeshProUGUI UIHighScore;
    public TextMeshProUGUI UILives;
    public TextMeshProUGUI UICountDownNumber;
    public TextMeshProUGUI UIComboCounter;

    #region private variables
    private Scene _currentScene;
    // save all ball positions and their name for level reset    
    private Dictionary<Vector3, string> _startingBallsPosition;
    // check if player has achieved a new score in this session and handle it              
    private static bool _hasNewHighscore = false; 
    private SoundManager _sound;
    private GameObject _player;
    #endregion

    #region player specifics    
    [HideInInspector]
    // to implement combos/multipliers
    public int comboCounter;

    [HideInInspector]
    // check if player missed a shot or not
    public bool flawlessVictory = true;

    [HideInInspector]
    // to calculate win condition
    public int hitsToWin;

    [HideInInspector]
    // to calculate previous succesful hit
    public float timeHit;
    #endregion

    // setup defaults and assign references
    private void Awake()
    {
        if (gm == null)
            gm = GetComponent<GameManager>();
        
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player == null)
            Debug.LogError("Could not find Player gameobject!");

        if (_sound == null)            
        _sound = SoundManager.sm;

        if (BalloonsPrefabs == null || !BalloonsPrefabs.Any())
            Debug.LogWarning("There are no prefabs attatched to the editor!");

               
        
        // store balls' starting positions into a list so we can later reset them
        _startingBallsPosition = new Dictionary<Vector3, string>();

        // Find all balloons in scene and save their starting position, balloon type and direction
        var allBalloons = FindAllBalloonsInScene();
        foreach (var ballloon in allBalloons)
        {
            
            _startingBallsPosition.Add(new Vector3(ballloon.transform.position.x, 
                                                    ballloon.transform.position.y), 
                                                    ballloon.GetComponent<BalloonController>().BalloonType + "," 
                                                    + ballloon.GetComponent<BalloonController>().BalloonDirection.ToString());
        }

        SetupDefaults();

        // play level music
        _sound.PlayMusicForLevel(_currentScene);
    }

    // set gamemanager defaults
    private void SetupDefaults()
    {   
        // get current scene
        _currentScene = SceneManager.GetActiveScene();

        // load the same level again if we didn't specify the next level
        if (NextLevelToLoad == "")
            NextLevelToLoad = _currentScene.name;

        // load the same level again if we didn't specify the gameover level
        if (GameOverScreen == "")
            GameOverScreen = _currentScene.name;        
                
        // get stored stat values
        RefreshPlayerState();

        // ready UI
        RefreshGUI();

        CalculateHitsForWin();
    }

    private void Start()
    {
        // stop game time and initiate countdown
        Time.timeScale = 0;
        StartCoroutine(CountDown());        
    }

    // calculate maximum hits needed to complete the level
    private void CalculateHitsForWin()
    {        
        hitsToWin = 0;
        foreach (var balloon in _startingBallsPosition)
        {
            var parameters = balloon.Value.Split(',');
            switch (parameters[0])
            {
                case "Big":
                    hitsToWin += 15;
                    break;
                case "Medium":
                    hitsToWin += 7;
                    break;
                case "Small":
                    hitsToWin += 3;
                    break;
                case "Tiny":
                    hitsToWin += 1;
                    break;
                default:
                    break;
            }
        }
        Debug.Log(hitsToWin);
    }

    private void Update()
    {   // always check for combos             
        CheckCombo();        
    }

    // check if gameover, else reset level
    public void ResetLevel()
    {   
        // subtract from player lives
        PlayerLives--;        
        // store lives
        PlayerPrefManager.SetLives(PlayerLives);
                
        RefreshGUI();

        // if game over
        if (PlayerLives <= 0) 
        {
            // Save stats
            PlayerPrefManager.SavePlayerStats(PlayerLives, PlayerScore, PlayerHighscore);

            // load game over screen
            SceneManager.LoadScene(GameOverScreen);
        }
        // else respawn player and balloons at starting point
        else
        {
            // load default weapon
            _player.GetComponent<PlayerShoot>().ActiveWeapon = PlayerShoot.Weapons.Anchor;
            
            // respawn player
            _player.GetComponent<CharacterController2D>().RespawnPlayer();

            // respawn balloons
            RespawnBalloons();

            // reset hit points needed to win
            CalculateHitsForWin();
        }
    }

    // respawn balls at starting position when we reset the level
    private void RespawnBalloons()
    {
        // Find all balls in scene and delete them
        List<GameObject> allBalloons = FindAllBalloonsInScene();

        foreach (var balloon in allBalloons)
        {
            Destroy(balloon);
        }

        // foreach starting position, instantiate the matching balloon prefab and set its direction
        foreach (KeyValuePair<Vector3, string> balloon in _startingBallsPosition)
        {
            var parameters = balloon.Value.Split(',');
            for (int i = 0; i < BalloonsPrefabs.Count; i++)
            {
                if (parameters[0] == BalloonsPrefabs[i].GetComponent<BalloonController>().BalloonType)
                {
                    var obj = Instantiate(BalloonsPrefabs[i], balloon.Key, BalloonsPrefabs[i].transform.rotation);
                    obj.GetComponent<BalloonController>().BalloonDirection = int.Parse(parameters[1]);
                }
            }
        }
    }

    // use it to find all balloons in the level
    private List<GameObject> FindAllBalloonsInScene()
    {
        var allBalloons = new List<GameObject>(GameObject.FindGameObjectsWithTag("Balloon"));        
        return allBalloons;
    }

    // add to player's score
    public void AddScore(int amount)
    {
        PlayerScore += amount;
        UIScore.text = "Score: " + PlayerScore.ToString();

        // check if it is a new highscore
        if (PlayerScore > PlayerHighscore)
        {
            PlayerHighscore = PlayerScore;
            UIHighScore.text = "Best Score: " + PlayerHighscore.ToString();
            PlayerPrefManager.SetHighscore(PlayerHighscore);

            if (!_hasNewHighscore)
            {
                _sound.audioController.PlayOneShot(_sound.NewHighscore);
                _hasNewHighscore = true;
            }
        }        
    }

    // used on setup defaults and level reset
    private void RefreshGUI()
    {
        UIScore.text = "Score: " + PlayerScore.ToString();
        UIHighScore.text = "Best Score: " + PlayerHighscore.ToString();
        UILives.text = "Lives: " + PlayerLives.ToString();
        UILevel.text = _currentScene.name;
        UIComboCounter.text = "";
        comboCounter = 0;
    }

    // used on setup defaults
    private void RefreshPlayerState()
    {       
        PlayerLives = PlayerPrefManager.GetLives();
        PlayerScore = PlayerPrefManager.GetScore();
        PlayerHighscore = PlayerPrefManager.GetHighscore();        
    }

    // increament combocounter if the next bullet does not miss in a timeframe of 2 seconds
    public void CheckCombo()
    {
        // check when was the last time the player hit a balloon and check if combo can continue
        float timeNow = Time.time;
        float timeDif = timeNow - timeHit;

        if (comboCounter > 0)
        {
            UIComboCounter.enabled = true;
            UIComboCounter.text = comboCounter.ToString() + "x Combo!";
        }

        // check if combo breaks
        if (timeDif > 2f && comboCounter != 0)
        {
            UIComboCounter.enabled = false;
            comboCounter = 0;

            // play broke combo sound
            _sound.audioController.PlayOneShot(_sound.ComboBroke);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }    
       
    #region Coroutines
    // countdown for level to start
    IEnumerator CountDown()
    {
        if (!PauseMenu.GameIsPaused)
        {            
            _sound.audioController.PlayOneShot(_sound.CountDownBeep);
            UICountDownNumber.text = "3";
            yield return new WaitForSecondsRealtime(1);

            _sound.audioController.PlayOneShot(_sound.CountDownBeep);
            UICountDownNumber.text = "2";            
            yield return new WaitForSecondsRealtime(1);

            _sound.audioController.PlayOneShot(_sound.CountDownBeep);
            UICountDownNumber.text = "1";
            yield return new WaitForSecondsRealtime(1);

            _sound.audioController.PlayOneShot(_sound.CountDownOver);
            UICountDownNumber.text = "GO!";
            yield return new WaitForSecondsRealtime(1);
            Time.timeScale = 1;
            UICountDownNumber.gameObject.SetActive(false); 
        }
    }

    public IEnumerator LoadNextLevel()
    {
        // save stats
        PlayerPrefManager.SavePlayerStats(PlayerLives, PlayerScore, PlayerHighscore);
        // play level completed sound
        _sound.audioController.PlayOneShot(_sound.LevelCompleted);

        yield return new WaitForSeconds(2.5f);
        // load next level
        SceneManager.LoadScene(NextLevelToLoad);                
    }        
    #endregion
}
