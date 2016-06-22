using UnityEngine;
using System.Collections;

public class TargetHitCounter : MonoBehaviour {

    TextMesh targetCountText;
    public static int targetHitCount;

    void Awake()
    {
        targetCountText = transform.Find("Score").GetComponent<TextMesh>();
        targetHitCount = 0;
    }

    void Update()
    {
        targetCountText.text = ("Targets Hit: " + targetHitCount);
    }
}
