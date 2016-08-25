using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveScore {

    public static void SaveScores()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/scores.sav", FileMode.Create);

        PlayerScores localScores = new PlayerScores();

        formatter.Serialize(stream, localScores);
        stream.Close();
    }

    public static void LoadScores()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/scores.sav", FileMode.Open);

        PlayerScores localScores = formatter.Deserialize(stream) as PlayerScores;
        stream.Close();
    }
}

[Serializable]
public class PlayerScores
{
    public int[] scores;
    public string[] names;

    public PlayerScores()
    {
        scores = new int[10];
        names = new string[10];
    }
}