using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveScore {

    public static void SaveScores(UpdateScoreboard scores)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        //if (!Directory.Exists(Application.persistentDataPath))
        //    Directory.CreateDirectory(Application.persistentDataPath);

        FileStream stream = File.Create(Application.persistentDataPath + "/scores.bin");

        PlayerScores localScores = new PlayerScores(scores);
        PlayerNames localNames = new PlayerNames(scores);

        formatter.Serialize(stream, localScores);
        stream.Close();
        Debug.Log("File Saved in: " + Application.persistentDataPath);
    }

    public static int[] LoadScores()
    {
        Debug.Log("Loading file from: " + Application.persistentDataPath);
        if (File.Exists(Application.persistentDataPath + "/score.bin"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(Application.persistentDataPath + "/scores.bin", FileMode.Open);

            PlayerScores localScores = formatter.Deserialize(stream) as PlayerScores;
            stream.Close();
            return localScores.savedScores;
        }
        else
        {
            Debug.Log("No file was found, writing default scores");
            return new int[8];
        }
    }

    public static string[] LoadNames()
    {
        if (File.Exists(Application.persistentDataPath + "/score.bin"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(Application.persistentDataPath + "/scores.bin", FileMode.Open);

            PlayerNames localNames = formatter.Deserialize(stream) as PlayerNames;
            stream.Close();
            return localNames.savedNames;
        }
        else
        {
            Debug.Log("No file was found, writing default names");
            return null;
        }
    }
}

[Serializable]
public class PlayerScores
{
    public int[] savedScores = new int[8];

    public PlayerScores(UpdateScoreboard scores)
    {
        for (int i = 0; i < scores.topScores.Length - 1; i++)
        {
            savedScores[i] = scores.topScores[i];
        }
    }
}

[Serializable]
public class PlayerNames
{
    public string[] savedNames = new string[8];

    public PlayerNames(UpdateScoreboard scores)
    {
        for (int i = 0; i <= scores.topScoresNames.Length - 1; i++)
        {
            savedNames[i] = scores.topScoresNames[i];
        }
    }
}