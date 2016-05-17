using UnityEngine;
using System.Collections;

public class FireGun : MonoBehaviour
{

    private Animation anim;
    public int currentAmmo;

    void Start()
    {
        currentAmmo = 17;
        anim = GetComponent<Animation>();
    }


    void Update()
    {
        if (transform.parent == null)
        {
            return;
        }

        // Only play the animation if the player is holding the gun
        if (transform.parent != null && transform.parent.name == ("Camera"))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                anim.Play("Shooting");
            }
        }
    }
}
