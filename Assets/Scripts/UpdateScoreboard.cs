using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdateScoreboard : MonoBehaviour {

    public string defaultName;
    public GameObject playerScore;

    //Dictionary<string, int> playerScores;

    private int recentScore;
    public int[] topScores;
    private string[] topScoresNames;
    private TextMesh scoreBoardText;

    void Awake()
    {
        //playerScores = new Dictionary<string, int>();

        topScores = new int[9];
        topScoresNames = new string[9];
        scoreBoardText = this.GetComponent<TextMesh>();
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
                    topScoresNames[k] = topScoresNames[k - 1];
                }

                topScores[i] = recentScore;
                this.GetComponent<UpdateScoreboard>().enabled = false;

                break;
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
        Debug.Log("AddToScoreboard ran");

        for (int i = 0; i <= topScores.Length; i++)
        {
            if (topScores[i] > 0)
            {
                GameObject go = (GameObject)Instantiate(playerScore, scoreOffset * (i + 1), Quaternion.identity);
                go.transform.SetParent(this.transform, false);
                go.GetComponent<TextMesh>().text = ("" + topScores[i].ToString());

                Debug.Log("Updated top score to: " + topScores[i]);
            }

            else
                break;
        }
    }
}
