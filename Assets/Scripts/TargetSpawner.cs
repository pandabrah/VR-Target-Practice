using UnityEngine;
using System.Collections;

public class TargetSpawner : MonoBehaviour {

    public int maxTargetCount = 6;
    public GameObject target;
    public Transform[] spawnPoints;

    public static int currentTargetCount = 0;

    void Awake()
    {
        Animation setAnim = target.GetComponent<Animation>();
        setAnim.playAutomatically = false;
    }

    void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        if (currentTargetCount != maxTargetCount)
        {
            CreateTargetObject(spawnPointIndex);

            currentTargetCount += 1;
        }

        else
            return;
    }

    void CreateTargetObject(int sIndex)
    {
        GameObject t = Instantiate(target, spawnPoints[sIndex].position, spawnPoints[sIndex].rotation) as GameObject;

        t.AddComponent<TargetInteraction>();
        t.tag = ("Target");

        BoxCollider targetHitBox = t.gameObject.AddComponent<BoxCollider>();
        targetHitBox.center = new Vector3(0f, 0f, -0.17f);
        targetHitBox.size = new Vector3(1f, 1f, 0.1f);
    }
}
