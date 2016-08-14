using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetSpawner : MonoBehaviour
{

    public int maxTargetCount = 6;
    public GameObject target;
    public int xSize;
    public int ySize;

    public static int currentTargetCount = 0;
    public GameObject tgt;

    public List<GameObject> spawnedTargets;
    private Vector3[] spawnPoints;
    private int spawnPointIndex;
    private int lastIndex = 0;


    void Awake()
    {
        Animation setAnim = target.GetComponent<Animation>();
        setAnim.playAutomatically = false;
        CreateSpawnPoints();
        spawnedTargets = new List<GameObject>();
    }

    void OnEnable()
    {
        InitialSpawn();
    }

    void Update()
    {
        Spawn();
        ClearNull();
    }

    void CreateSpawnPoints()
    {
        spawnPoints = new Vector3[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
                spawnPoints[i] = transform.TransformPoint(new Vector3(x, y));
        }
    }

    void InitialSpawn()
    {
        //Checks if target counter is reset; if not, reset it back to zero.
        if (currentTargetCount != 0)
            currentTargetCount = 0;

        while (currentTargetCount != maxTargetCount)
        {
            spawnPointIndex = Random.Range(0, spawnPoints.Length);
            bool validPoint = checkValidPoints(spawnPointIndex, lastIndex);

            if (!validPoint)
                return;

            else if (validPoint)
            {
                CreateTargetObject(spawnPointIndex);
                currentTargetCount += 1;
                lastIndex = spawnPointIndex;
            }

            else
                return;
        }
    }

    void Spawn()
    {
        spawnPointIndex = Random.Range(0, spawnPoints.Length);
        bool validPoint = checkValidPoints(spawnPointIndex, lastIndex);

        if (!validPoint)
            return;

        else if (validPoint)
        {
            if (currentTargetCount != maxTargetCount)
            {
                StartCoroutine(SpawnDelay());
                currentTargetCount += 1;
                lastIndex = spawnPointIndex;
            }

            else
                return;
        }
    }

    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(Random.Range(1, 2));
        CreateTargetObject(spawnPointIndex);
    }

    bool checkValidPoints(int current, int last)
    {
        //Check left, right, and center
        if (current == last || current == (last - 1) || current == (last + 1))
            return false;

        //Check top and bottom
        else if (current == (last - xSize + 1) || current == (last + xSize + 1))
            return false;

        //Check bottom left and bottom right
        else if (current == (last - xSize) || current == (last - xSize + 2))
            return false;

        //Check top left and top right
        else if (current == (last + xSize + 1) || current == (last + xSize + 2))
            return false;

        //Return true if there is no target spawned on or around planned point
        else
            return true;
    }

    void CreateTargetObject(int sIndex)
    {
        tgt = Instantiate(target, spawnPoints[sIndex], Quaternion.identity) as GameObject;

        tgt.AddComponent<TargetInteraction>();
        tgt.tag = ("Target");

        BoxCollider targetHitBox = tgt.gameObject.AddComponent<BoxCollider>();
        targetHitBox.center = new Vector3(0f, 0f, -0.17f);
        targetHitBox.size = new Vector3(1f, 1f, 0.1f);

        spawnedTargets.Add(tgt);
    }

    void ClearNull()
    {
        for (int i = spawnedTargets.Count - 1; i >= 0; i--)
        {
            if(spawnedTargets[i] == null)
                spawnedTargets.RemoveAt(i);
        }
    }
}
