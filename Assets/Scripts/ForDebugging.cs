using UnityEngine;
using System.Collections;

public class ForDebugging : MonoBehaviour {

    void Update()
    {
        Debug.Log("targetHitCount = " + TargetHitCounter.targetHitCount);
        Debug.Log("currentTargetCount = " + TargetSpawner.currentTargetCount);
    }

}
