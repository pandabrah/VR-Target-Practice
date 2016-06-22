using UnityEngine;
using System.Collections;

public class TargetSpawner : MonoBehaviour {

    public int maxTargetCount = 6;
    public GameObject target;
    //public Transform[] spawnPoints;
    public int xSize;
    public int ySize;

    public static int currentTargetCount = 0;

    private Vector3[] spawnPoints;
    private int spawnPointIndex;
    private int lastIndex = 0;

    void Awake()
    {
        Animation setAnim = target.GetComponent<Animation>();
        setAnim.playAutomatically = false;
        CreateSpawnPoints();
    }

    //void OnDrawGizmos()
    //{
    //    if (newSpawnPoints == null)
    //    {
    //        return;
    //    }

    //    Gizmos.color = Color.black;
    //    for (int i = 0; i < newSpawnPoints.Length; i++)
    //    {
    //        Gizmos.DrawSphere(transform.TransformPoint(newSpawnPoints[i]), 0.1f);
    //    }
    //}

    void Update()
    {
        Spawn();
    }

    void CreateSpawnPoints()
    {
        spawnPoints = new Vector3[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                spawnPoints[i] = transform.TransformPoint(new Vector3(x, y));
            }
        }
    }

    void Spawn()
    {
        spawnPointIndex = Random.Range(0, spawnPoints.Length);

        if (spawnPointIndex == lastIndex)
        {
            return;
        }

        else if (spawnPointIndex != lastIndex)
        {
            if (currentTargetCount != maxTargetCount)
            {
                CreateTargetObject(spawnPointIndex);

                currentTargetCount += 1;
            }

            else
                return;
        }
    }

    void CreateTargetObject(int sIndex)
    {
        GameObject t = Instantiate(target, spawnPoints[sIndex], Quaternion.identity) as GameObject;

        t.AddComponent<TargetInteraction>();
        t.tag = ("Target");

        BoxCollider targetHitBox = t.gameObject.AddComponent<BoxCollider>();
        targetHitBox.center = new Vector3(0f, 0f, -0.17f);
        targetHitBox.size = new Vector3(1f, 1f, 0.1f);
    }
}
