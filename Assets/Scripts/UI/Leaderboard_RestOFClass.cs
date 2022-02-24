using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard_RestOFClass : MonoBehaviour
{
    #region Leaderboard Variables

   [Tooltip("This is the score for that specific playthrough")] public int Runscore;

    // these are the highscores for the leaderboard
    public int score1;
    public int score2;
    public int score3;
    public int score4;

    //Bool to help sort the list
    public static bool sortingstop;

    //List that stores these values
    public static List<int> top3;
    #endregion

    private void Awake()
    {
        top3 = new List<int>();

        // Populate list using by creating playerprefs that dont currently exist

        top3.Add(PlayerPrefs.GetInt("score1", 0));
        top3.Add(PlayerPrefs.GetInt("score2", 0));
        top3.Add(PlayerPrefs.GetInt("score3", 0));
        top3.Add(PlayerPrefs.GetInt("score4", 0));

        sortingstop = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

//this function checks whether the score youve gotten is greater than the last score on the list the replaces that score then sorts the list;
    void sortlist()
    {

        if (!sortingstop)
        {
            if (Runscore > top3[3])
            {
                top3[3] = Runscore;

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


  
    // Updatesthescorevariables with the variables in the list
    void updatescore()
    {
        score1 = PlayerPrefs.GetInt("score1", 0);
        score2 = PlayerPrefs.GetInt("score2", 0);
        score3 = PlayerPrefs.GetInt("score3", 0);
        score4 = PlayerPrefs.GetInt("score4", 0);

    }
}
