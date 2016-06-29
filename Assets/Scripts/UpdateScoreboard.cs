using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdateScoreboard : MonoBehaviour {

    public string defaultName;

    Dictionary<string, int> playerScores;

    private int recentScore;
    private int[] topScores;
    private string[] topScoresNames;
    private TextMesh scoreBoardText;

    void Awake()
    {
        topScores = new int[9];
        topScoresNames = new string[9];
        scoreBoardText = this.GetComponent<TextMesh>();
    }

    void OnEnable()
    {
        CheckHighScore();
    }

    void Update()
    {
    }

    void CheckHighScore()
    {
        recentScore = TargetHitCounter.targetHitCount;

        for (int i = topScores.Length - 1; i >= 0; i--)
        {
            if (recentScore > topScores[i])
            {
                for (int k = 0; k < i; k++)
                {
                    topScores[k] = topScores[k + 1];
                    topScoresNames[k] = topScoresNames[k + 1];
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
}
