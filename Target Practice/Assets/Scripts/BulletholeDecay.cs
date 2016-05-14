using UnityEngine;
using System.Collections;

public class BulletholeDecay : MonoBehaviour {

    public float duration = 1f;

	void FixedUpdate () {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(gameObject);
        }
	}
}
