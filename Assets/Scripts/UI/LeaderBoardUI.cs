using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LeaderBoardUI : MonoBehaviour
{
    #region Variables
    public GameObject GameoverUI;

    public GameManager Manager;

    public TextMeshProUGUI thatRunsScore;

    public TextMeshProUGUI Multiplier;

    public TextMeshProUGUI scores;
    
    public string scoretext;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
      GameObject GameoverUI = GetComponent<GameObject>();

      GameManager Manager = GetComponent<GameManager>();

      TextMeshProUGUI  thatRunsScore = GetComponent<TextMeshProUGUI>();

      TextMeshProUGUI scores = GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI Multiplier = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        OnAndOff();
        UpdateScore();

    }

    private void OnAndOff()
    {
       if(Manager.CurrentState == GameManager.GameState.End)
        {
            GameoverUI.SetActive(true);
        }
        else
        {
            GameoverUI.SetActive(false);
        }
    }

    private void UpdateScore()
    {
        thatRunsScore.SetText(scoretext + Manager.Runscore.ToString());

        scores.SetText("1. " + Manager.score1.ToString() + "\n" + "2. " + Manager.score2.ToString() + "\n" + "3. " + Manager.score3.ToString() + "\n" + "4. " + Manager.score4.ToString());

        Multiplier.SetText("x" + Manager.multiplier.ToString());

        if(Manager.multiplier <= 1)
        {
            Multiplier.gameObject.SetActive(false);
        }
        else
        {
            Multiplier.gameObject.SetActive(true);
        }
    }
}
