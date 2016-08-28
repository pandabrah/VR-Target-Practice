using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class UpdateScoreboard : MonoBehaviour
{

    public string defaultName;
    public GameObject playerScore;

    //Dictionary<string, int> playerScores;

    public int[] topScores;
    public string[] topScoresNames;
    public GameObject[] goScore;
    public GameObject[] goName;

    private int recentScore;


    void Awake()
    {
        //playerScores = new Dictionary<string, int>();
        topScores = new int[8];
        topScoresNames = new string[8];
        goScore = new GameObject[8];
        goName = new GameObject[8];

        //Temporary until save/load working
        topScores[0] = 9999;
        for (int i = 0; i <= topScoresNames.Length - 1; i++)
            topScoresNames[i] = defaultName;

        InitScoreboard();
    }

    void OnEnable()
    {
        CheckHighScore();
        UpdateScores();
        SaveScore.SaveScores(this);
        SaveScore.SaveNames(this);

        this.GetComponent<UpdateScoreboard>().enabled = false;
    }

    void InitScoreboard()
    {
        Vector3 scoreOffset = new Vector3(0f, -0.5f, 0f);

        Load();

        for (int i = 0; i <= topScores.Length - 1; i++)
        {
            if (topScores[i] >= 0)
            {
                if (goScore[i] == null)
                {
                    //Create 8 text prefabs for the scores
                    goScore[i] = (GameObject)Instantiate(playerScore, scoreOffset * (i + 1), Quaternion.identity);
                    goScore[i].transform.SetParent(this.transform.Find("Score Header"), false);
                }
                
                //Format score text to match right side
                goScore[i].GetComponent<TextMesh>().text = ("" + topScores[i].ToString());
                goScore[i].GetComponent<TextMesh>().alignment = TextAlignment.Right;
                goScore[i].GetComponent<TextMesh>().anchor = TextAnchor.UpperRight;

                //Create 8 text prefabs for the usernames
                goName[i] = (GameObject)Instantiate(playerScore, scoreOffset * (i + 1), Quaternion.identity);
                goName[i].transform.SetParent(this.transform.Find("Name Header"), false);
                goName[i].GetComponent<TextMesh>().text = ("" + topScoresNames[i].ToString());
            }
        }
    }

    void CheckHighScore()
    {
        recentScore = TargetHitCounter.targetHitCount;

        for (int i = 0; i <= topScores.Length - 1; i++)
        {
            if (recentScore >= topScores[i] && recentScore != 0)
            {
                for (int k = topScores.Length - 1; k >= i; k--)
                {
                    topScores[k] = topScores[k - 1];
                    //topScoresNames[k] = topScoresNames[k - 1];
                }

                topScores[i] = recentScore;
                break;
            }

            else if (recentScore < topScores[7])
            {
                break;
            }
        }
    }

    void UpdateScores()
    {
        for (int i = 0; i <= topScores.Length - 1; i++)
        {
            if (topScores[i] >= 0)
            {
                goScore[i].GetComponent<TextMesh>().text = ("" + topScores[i].ToString());
                goName[i].GetComponent<TextMesh>().text = ("" + topScoresNames[i].ToString());
            }

            else
                break;
        }
    }

    void Load()
    {
        int[] loadedScores = SaveScore.LoadScores();
        string[] loadedNames = SaveScore.LoadNames();

        Debug.Log("loadedNames: " + loadedNames[0]);

        if (loadedScores[0] != 0)
        {
            for (int i = 0; i <= topScores.Length - 1; i++)
            {
                topScores[i] = loadedScores[i];
                topScoresNames[i] = loadedNames[i];
                Debug.Log("Loaded scores: " + topScores[i]);
            }
        }
        else
            return;
    }

    public void Reset()
    {
        for (int i = 0; i <= topScores.Length - 1; i++)
        {
            topScores[i] = 0;
            topScoresNames[i] = defaultName;
        }
    }
}
