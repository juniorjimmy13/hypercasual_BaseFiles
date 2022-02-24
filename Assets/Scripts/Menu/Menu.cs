using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI Leaderboard;

    public TextMeshProUGUI audiobut;
    
    public TextMeshProUGUI Sfxbut;
    
    // Start is called before the first frame update
    void Start()
    {
      TextMeshProUGUI  Leaderboard = GetComponent<TextMeshProUGUI>();
      TextMeshProUGUI audiobut = GetComponent<TextMeshProUGUI>();
      TextMeshProUGUI Sfxbut = GetComponent<TextMeshProUGUI>();
      
    
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 1;
        updateLeaderboard();
    }

    void updateLeaderboard()
    {
        Leaderboard.SetText("1. " + PlayerPrefs.GetInt("score1").ToString() + "\n" + "2. " + PlayerPrefs.GetInt("score2").ToString() + "\n" + "3. " + PlayerPrefs.GetInt("score3").ToString() + "\n" + "4. " +PlayerPrefs.GetInt("score4").ToString());
    }

    public void loadNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void quit()
    {
        Application.Quit();
    }

    public void onAndOffVfx()
    {
        if (VfxManager.instance.Player.volume > 0)
        {
            Sfxbut.SetText("SFX:OFF");
            VfxManager.instance.Player.volume = 0;
        }
        else if (VfxManager.instance.Player.volume == 0)
        {
            Sfxbut.SetText("SFX:ON");
            VfxManager.instance.Player.volume = VfxManager.instance.oldvolume;
        }
    }
    public void onAndOffAudio()
    {
        if (AudioManager.instance.Player.volume > 0)
        {
            audiobut.SetText("Audio:OFF");
            AudioManager.instance.Player.volume = 0;
        }
        else if(AudioManager.instance.Player.volume == 0)
        {
            audiobut.SetText("Audio:ON");
            AudioManager.instance.Player.volume = AudioManager.instance.oldVolume;
        }
    }

    public void playClick()
    {
        VfxManager.instance.Player.PlayOneShot(VfxManager.instance.Click);
        
    }
    public void playtoggle()
    {
        VfxManager.instance.Player.PlayOneShot(VfxManager.instance.toggle);
        
    }
}
