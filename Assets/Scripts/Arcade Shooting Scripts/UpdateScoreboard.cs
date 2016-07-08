using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdateScoreboard : MonoBehaviour
{

    public string defaultName;
    public GameObject playerScore;

    //Dictionary<string, int> playerScores;

    private int recentScore;
    public int[] topScores;
    public string[] topScoresNames;
    private TextMesh scoreBoardText;
    private GameObject[] goScore;
    private GameObject[] goName;

    void Awake()
    {
        //playerScores = new Dictionary<string, int>();

        topScores = new int[8];
        topScoresNames = new string[8];
        goScore = new GameObject[8];
        goName = new GameObject[8];
        scoreBoardText = this.GetComponent<TextMesh>();

        topScores[0] = 9999;
        topScores[1] = 1;

        for (int i = 0; i <= topScoresNames.Length - 1; i++)
        {
            topScoresNames[i] = defaultName;
        }

        InitScoreboard();
    }

    void OnEnable()
    {
        CheckHighScore();
        UpdateScores();

        this.GetComponent<UpdateScoreboard>().enabled = false;
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

    void InitScoreboard()
    {
        Vector3 scoreOffset = new Vector3(0f, -0.5f, 0f);

        for (int i = 0; i <= topScores.Length - 1; i++)
        {
            if (topScores[i] >= 0)
            {
                //Create 8 text prefabs for the scores
                goScore[i] = (GameObject)Instantiate(playerScore, scoreOffset * (i + 1), Quaternion.identity);
                goScore[i].transform.SetParent(this.transform.Find("Score Header"), false);

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
}
