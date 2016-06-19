using UnityEngine;
using System.Collections;

public class TargetInteraction : MonoBehaviour {

    public GameObject targets;

    public IEnumerator TargetHitReset()
    {
        float duration = .280f;
        //Vector3 targetPosition = transform.position;

        Animation targetBreakAnim = GetComponent<Animation>();
        targetBreakAnim.Play("TargetBreak");
        gameObject.GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(duration);

        DestroyObject(gameObject);

        TargetSpawner.currentTargetCount -= 1;
        //Debug.Log("Object Destroyed: " + gameObject);

    //    yield return new WaitForSeconds(3);

    //    SpawnTarget(targets, targetPosition);

    //    yield return null;
    }

    //public void SpawnTarget(GameObject target, Vector3 spawnLocation)
    //{
    //    GameObject newTarget = (GameObject)Instantiate(targets, spawnLocation, Quaternion.identity);

    //    newTarget.AddComponent<TargetInteraction>();
    //    newTarget.tag = ("Target");

    //    Animation setAnim = targets.GetComponent<Animation>();
    //    setAnim.playAutomatically = false;

    //    newTarget.AddComponent<BoxCollider>();
    //    BoxCollider targetHitBox = newTarget.GetComponent<BoxCollider>();
    //    targetHitBox.center = new Vector3(0f, 0f, -0.17f);
    //    targetHitBox.size = new Vector3(1f, 1f, 0.1f);
    //}
}
