using UnityEngine;
using System.Collections;

public class BulletholeDecay : MonoBehaviour {

    //Sets time for bulletholes to decay
    public float duration = 1.5f;

	void FixedUpdate () {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(gameObject);
        }
	}
}
