using UnityEngine;
using System.Collections;

public class TargetHitCounter : MonoBehaviour {

    TextMesh targetCountText;
    public static int targetHitCount = 0;

    void Awake()
    {
        targetCountText = transform.Find("Score").GetComponent<TextMesh>();
    }

    void Update()
    {
        targetCountText.text = ("Targets Hit: " + targetHitCount);
    }
}
