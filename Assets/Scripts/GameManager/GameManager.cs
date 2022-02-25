using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public ProceduralSpawning Spawner;

    public GameObject countdown;

    public TextMeshProUGUI countdownText;
    int Maxdrones, MaxAngle, Maxhealth;

    float WaitTime, Maxspeed;

    public int IntroScore;
    
    public int BeginnerScore;

    public int MediumScore;

    public float timescale;

    private float counttime = 3;
    public TextMeshProUGUI scoreUI;

    private bool countdownReady;
    bool Spawned;

    public ModuleSpawner moduleSpawner;

    public PlayerHealth playerhealth;

    public Volume Post;

    private int Chance;
    public enum GameState
    {
        Countdown,
        Play,
        Pause,
        SpawnMoreDrones,
        Cooldown,
        End,
    }
    #region Leaderboard Variables
    public int score1;
    public int score2;
    public int score3;
    public int score4;
  
    public static List<int> top3;
    #endregion

    public static bool sortingstop;

    public GameState CurrentState;

    public int[] LeaderboardScores = new int[4];

    public int Runscore;

    public int multiplier;

    public float vingettechange;

    public GameObject objective;

    private bool relaxing;
    private void Awake()
    {
        relaxing = false;
        GameObject countdown = GetComponent<GameObject>();
        TextMeshProUGUI countdownText = GetComponent<TextMeshProUGUI>();


       Volume Post = GetComponent<Volume>();

        top3 = new List<int>();
        top3.Add(PlayerPrefs.GetInt("score1", 0));
        top3.Add(PlayerPrefs.GetInt("score2", 0));
        top3.Add(PlayerPrefs.GetInt("score3", 0));
        top3.Add(PlayerPrefs.GetInt("score4", 0));
       
        ProceduralSpawning  Spawner = GetComponent<ProceduralSpawning>();

        ModuleSpawner  moduleSpawner = GetComponent<ModuleSpawner>();

        GameObject objective = GetComponent<GameObject>();
        PlayerHealth  playerhealth = GetComponent<PlayerHealth>();

        sortingstop = false;
       
        // SetValues();

        CurrentState = GameState.Countdown;
    }

    private void SetValues()
    {
        score1 = PlayerPrefs.GetInt("score1", 0);
        score2 = PlayerPrefs.GetInt("score2", 0);
        score3 = PlayerPrefs.GetInt("score3", 0);
        score4 = PlayerPrefs.GetInt("score4", 0);

        LeaderboardScores[0] = score1;
        LeaderboardScores[1] = score2;
        LeaderboardScores[2] = score3;
        LeaderboardScores[3] = score4;

    }

    // Start is called before the first frame update
    void Start()
    {
    TextMeshProUGUI  scoreUI = GetComponent<TextMeshProUGUI>();
    Chance = 1;

    }

    // Update is called once per frame
    void Update()
    {

       
       timescale =  Time.timeScale;
        deathcheck();
        updatescore();
        //countdownTexter();
        switch (CurrentState)
        {
            case GameState.Countdown:

                Time.timeScale = 1;
                if (AudioManager.instance.Player.isPlaying == false)
                {
                    AudioManager.instance.Player.PlayOneShot(AudioManager.instance.Introsound);
                }
             
                
                StartCoroutine(Countdowntime());
                OpenEyeEffect();
                break;

            case GameState.Play:
                Time.timeScale = 1;
                Spawned = false;
                counttime = 3;
                CheckDronesinScene();
                break;

            case GameState.SpawnMoreDrones:
                if (!Spawned)
                {
                    DifficultyCheck();
                    Spawned = true;
                    CurrentState = GameState.Play;
                }
                break;

            case GameState.Pause:
                if (counttime > 0 && countdownReady)
                {
                    countdown.SetActive(true);
                     
            
                    countdownText.SetText(((int)(counttime -= Time.unscaledDeltaTime)).ToString());
                }

                break;

            case GameState.Cooldown:
                break;

            case GameState.End:
                Time.timeScale = 0;
                restart();
                SortList(Runscore);


                break;
                


        }
    }

    private void countdownTexter()
    {
        if (countdownReady == true)
        {
            countdown.gameObject.SetActive(true);
            countdownText.SetText(((counttime - Time.unscaledDeltaTime)).ToString());
        }
        else if (countdownReady!=true)
        {
            counttime = 3f;
            countdown.SetActive(false);
        }
    }

    private void SpawnSetOfDrones()
    {

    }
    

    private void OpenEyeEffect()
    {
        if(Post.profile.TryGet<Vignette>(out var vin))
        {
            vin.intensity.overrideState = true;

         if (vin.intensity.value > 0.4f)
            {
                vin.intensity.value -= vingettechange * Time.deltaTime;

            }else
            {
                vin.intensity.value = 0.4f;
            }

            
        }
           
        
    }

    private void SortList(int score)
    {
        if (!sortingstop)
        {
            if (score > top3[3])
            {
                top3[3] = score;

                top3.Sort();
                top3.Reverse();
            }

            PlayerPrefs.SetInt("score1", top3[0]);
            PlayerPrefs.SetInt("score2", top3[1]);
            PlayerPrefs.SetInt("score3", top3[2]);
            PlayerPrefs.SetInt("score4", top3[3]);
            sortingstop = true;
        }
    }

    private void restart()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene(0);
        }
    }

   

    private void deathcheck()
    {
        if (playerhealth.dead)
        {
            CurrentState = GameState.End;
        }
       
    }
   
    private void updatescore()
    {
        scoreUI.SetText(Runscore.ToString());

        score1 = PlayerPrefs.GetInt("score1", 0);
        score2 = PlayerPrefs.GetInt("score2", 0);
        score3 = PlayerPrefs.GetInt("score3", 0);
        score4 = PlayerPrefs.GetInt("score4", 0);



    }

    private IEnumerator Countdowntime()
    {
        
        yield return new WaitForSeconds(3f);
        objective.gameObject.SetActive(false);
        CurrentState = GameState.Play;
        
        yield break;



    }
   private void CheckDronesinScene()
    {
       Spawner.dronesinscene = GameObject.FindGameObjectsWithTag("Drones");

        if (Spawner.dronesinscene.Length == 0)
        {
            Chance += 1;
            Spawner.maxdrone = 0;
            CurrentState = GameState.SpawnMoreDrones;
        }
    }

 private void DifficultyCheck()
    {
        if (!relaxing)
        {
            int Chance = UnityEngine.Random.Range(0, 10);

            if (moduleSpawner.Hard_Mid_modules_row == 5)
            {
                StartCoroutine(Relaxtime());
            
            }
            else if (Runscore < IntroScore)
            {
                if (AudioManager.instance.Player.isPlaying == false)
                {
                    AudioManager.instance.Player.PlayOneShot(AudioManager.instance.Introsound);
                }

                moduleSpawner.SpawnIntro();
            }
            else if (Runscore < BeginnerScore)
            {
                if (AudioManager.instance.Player.isPlaying == true)
                {
                    AudioManager.instance.Player.Stop();
                    AudioManager.instance.Player.PlayOneShot(AudioManager.instance.hype);
                }
                if (Chance < 2)
                {
                    moduleSpawner.SpawnEasy();
                }
                else if (Chance < 4)
                {
                    moduleSpawner.SpawnMedium();
                }
                else
                {
                    moduleSpawner.SpawnHard();
                }
            }
            else if (Runscore < MediumScore)
            {
                if (AudioManager.instance.Player.isPlaying == true)
                {
                    AudioManager.instance.Player.Stop();
                    AudioManager.instance.Player.PlayOneShot(AudioManager.instance.hype);
                }
                if (Chance < 2)
                {
                    moduleSpawner.SpawnHard();
                }
                else if (Chance < 4)
                {
                    moduleSpawner.SpawnEasy();
                }
                else
                {
                    moduleSpawner.SpawnMedium();
                }
           
            }
            else if (Runscore > MediumScore)
            {
                if (AudioManager.instance.Player.isPlaying == true)
                {
                    AudioManager.instance.Player.Stop();
                    AudioManager.instance.Player.PlayOneShot(AudioManager.instance.hype);
                }
                if (Chance < 2)
                {
                    moduleSpawner.SpawnEasy();
                }
                else if(Chance < 4)
                {
                    moduleSpawner.SpawnMedium();
                }
                else
                {
                    moduleSpawner.SpawnHard();
                }
            

            
            }
        }
       
    }

 private IEnumerator Relaxtime()
 {
      relaxing = true;
     moduleSpawner.Hard_Mid_modules_row = 0;
     yield return new WaitForSeconds(3);
     relaxing = false;

 }

 private void HardValues()
    {
        Maxhealth = 3;
        Maxdrones = 6;
        Maxspeed = 5f;
        MaxAngle = 40;
        WaitTime = 3f;
    }

    private void MediumValues()
    {
        Maxhealth = 3;
        Maxdrones = 4;
        Maxspeed = 3f;
        MaxAngle = 30;
        WaitTime = 3f;
    }

    private void BeginerValues()
    {
        Maxhealth = 2;
        Maxdrones = 4;
        Maxspeed = 2f;
        MaxAngle = 20;
        WaitTime = 3f;

    }

    public void PauseGame()
    {
        CurrentState = GameState.Pause;
        
        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        StartCoroutine(unpause());

    }

    IEnumerator unpause()
    {
        countdownReady = true;
        yield return new WaitForSecondsRealtime(3f);
        countdown.SetActive(false);
        CurrentState = GameState.Play;
        countdownReady = false;
        
    }

   public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

   public void ToMenu()
   {
       SceneManager.LoadScene(0);

   }

   
}
