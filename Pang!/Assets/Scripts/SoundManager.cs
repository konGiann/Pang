using UnityEngine;
using UnityEngine.SceneManagement;

public class  SoundManager : MonoBehaviour
{
    public static SoundManager sm;

    [Header("Main Menu Sounds")]
    public AudioClip MainMenuTrack;
    public AudioClip MenuItemToggle;

    [Header("Cyber City Tracks")]
    public AudioClip CyberCityTrack_1;

    [Header("Dark City Tracks")]
    public AudioClip DarkCityTrack_1;

    [Header("Level SFX")]
    public AudioClip BallExplosion;
    public AudioClip LostLife;
    public AudioClip NewHighscore;
    public AudioClip CountDownBeep;
    public AudioClip CountDownOver;
    public AudioClip LevelCompleted;
    public AudioClip ComboBroke;    
    public AudioClip GameOverMusic;
    public AudioClip GameWin;

    [HideInInspector]
    public AudioSource audioController;

    // private fields
    Scene currentScene; 
    
    void Awake()
    {
        if (sm == null)
            sm = GetComponent<SoundManager>();

        DontDestroyOnLoad(gameObject);

        // get current screen
        currentScene = SceneManager.GetActiveScene();        

        // add an audiosource controller
        audioController = gameObject.AddComponent<AudioSource>();

        // start music for main menu
        PlayMusicForLevel(currentScene);

        // loop music
        audioController.loop = true;
    }    

    // play the music clip assigned for each level when the level loads
    public void PlayMusicForLevel(Scene scene)
    {
        switch (scene.name)
        {
            case "MainMenu":
                audioController.Stop();
                audioController.clip = MainMenuTrack;
                audioController.Play();                
                break;

            case "Level 1":
                audioController.Stop();
                audioController.clip = CyberCityTrack_1;
                audioController.Play();                
                break;

            case "Level 2":
                audioController.Stop();
                audioController.clip = DarkCityTrack_1;
                audioController.Play();                
                break;

            case "GameOver":
                audioController.Stop();
                audioController.clip = GameOverMusic;
                audioController.Play();
                break;

            default:
                break;
        }
    }
    
    // menu items sfx
    public void PlayMenuToggleSound()
    {
        audioController.PlayOneShot(MenuItemToggle);
    }

    public void PlayMenuSelectedSound()
    {
        audioController.PlayOneShot(MenuItemToggle);
    }

}
