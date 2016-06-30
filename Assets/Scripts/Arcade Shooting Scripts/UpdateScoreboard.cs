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

    void Awake()
    {
        //playerScores = new Dictionary<string, int>();

        topScores = new int[8];
        topScoresNames = new string[8];
        scoreBoardText = this.GetComponent<TextMesh>();

        topScores[0] = 9999;
        topScores[1] = 1;

        for (int i = 0; i <= topScoresNames.Length - 1; i++)
        {
            topScoresNames[i] = defaultName;
        }
    }

    void OnEnable()
    {
        CheckHighScore();
        AddToScoreboard();
    }

    void CheckHighScore()
    {
        recentScore = TargetHitCounter.targetHitCount;

        for (int i = 0; i <= topScores.Length - 1; i++)
        {
            if (recentScore > topScores[i])
            {
                for (int k = topScores.Length - 1; k > i; k--)
                {
                    topScores[k] = topScores[k - 1];
                    //topScoresNames[k] = topScoresNames[k - 1];
                }

                topScores[i] = recentScore;
                this.GetComponent<UpdateScoreboard>().enabled = false;
            }

            else if (recentScore < topScores[0])
            {
                this.GetComponent<UpdateScoreboard>().enabled = false;
                break;
            }
        }
    }

    void AddToScoreboard()
    {
        Vector3 scoreOffset = new Vector3(0f, -0.5f, 0f);

        for (int i = 0; i <= topScores.Length - 1; i++)
        {
            if (topScores[i] >= 0)
            {
                GameObject goScore = (GameObject)Instantiate(playerScore, scoreOffset * (i + 1), Quaternion.identity);
                goScore.transform.SetParent(this.transform.Find("Score Header"), false);

                goScore.GetComponent<TextMesh>().text = ("" + topScores[i].ToString());
                goScore.GetComponent<TextMesh>().alignment = TextAlignment.Right;
                goScore.GetComponent<TextMesh>().anchor = TextAnchor.UpperRight;

                Debug.Log("Updated top score to: " + topScores[i]);

                GameObject goName = (GameObject)Instantiate(playerScore, scoreOffset * (i + 1), Quaternion.identity);
                goName.transform.SetParent(this.transform.Find("Name Header"), false);
                goName.GetComponent<TextMesh>().text = ("" + topScoresNames[i].ToString());
            }

            else
                break;
        }
    }
}
