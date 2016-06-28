using UnityEngine;
using System.Collections;

public class VRShooting : MonoBehaviour
{
    public GameObject bulletDecal;
    public GameObject bullet;

    public static int ammoCount;

    public AudioClip gunShotSound;

    private int ammo;
    private Vector3 gunEjectChamber;

    private Animation anim;

    private AudioSource source;

    private SteamVR_Controller.Device controller;

    void Start()
    {
        InitGun();
        controller = VRControls.device;
    }

    void InitGun()
    {
        ammo = ammoCount;
    }

    void Update()
    {
        Ray targetRay = new Ray(transform.Find("Aim").position, transform.Find("Aim").forward);
        RaycastHit hit;

        Quaternion gunRotation = transform.rotation;

        if (controller.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            source = GetComponent<AudioSource>();
            source.PlayOneShot(gunShotSound, 1f);

            StartCoroutine(GunShotAnimation());
            BulletEject();

            //Check if gun has animation, if it does play it every trigger pull
            if (GetComponent<Animation>())
            {
                Animation anim = GetComponent<Animation>();
                anim.Play("Shooting");
            }

            if (Physics.Raycast(targetRay, out hit))
            {
                if (hit.collider.tag != ("Target"))
                {
                    Instantiate(bulletDecal, hit.point, Quaternion.identity);
                }

                //If target is the start button, do not add to hit counter
                else if (hit.collider.tag == ("Target") && hit.collider.name == ("Start Button"))
                {
                    StartCoroutine(hit.collider.gameObject.GetComponent<TargetInteraction>().StartButtonInteraction());
                }

                //If target is shot, update score, break target, and respawn target
                else if (hit.collider.tag == ("Target"))
                {
                    StartCoroutine(hit.collider.gameObject.GetComponent<TargetInteraction>().TargetHitReset());
                    TargetHitCounter.targetHitCount += 1;
                }
            }

            //Confirm trigger is released before allowing another shot
            if (controller.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                return;
            }
        }

        //Check if there is a scoreboard and timer object attached to gun
        if (transform.Find("Scoreboard").gameObject != null && transform.Find("Timer").gameObject != null)
        {

            //If the left side of the gun is rotated towards the user, show the scoreboard and timer
            if (gunRotation.eulerAngles.y >= 260 && gunRotation.eulerAngles.y <= 300)
            {
                transform.Find("Scoreboard").gameObject.SetActive(true);
                transform.Find("Timer").gameObject.SetActive(true);
            }

            else
            {
                transform.Find("Scoreboard").gameObject.SetActive(false);
                transform.Find("Timer").gameObject.SetActive(false);
            }
        }
    }

    IEnumerator GunShotAnimation()
    {
        float animDuration = 0.03f;

        GameObject gunSlide = this.transform.Find("Slide").gameObject;

        Vector3 originalPos = new Vector3(0.0004795636f, 0.0824099f, 0.05370728f);
        Vector3 midPos = new Vector3(originalPos.x, originalPos.y, -0.01f);

        for (float i = 0; i < animDuration; i += Time.deltaTime)
        {
            Vector3 newPosition = Vector3.Lerp(originalPos, midPos, i / animDuration);

            gunSlide.transform.localPosition = newPosition;

            yield return null;
        }

        for (float i = 0; i < animDuration; i += Time.deltaTime)
        {
            Vector3 newPosition = Vector3.Lerp(midPos, originalPos, i / animDuration);

            gunSlide.transform.localPosition = newPosition;

            yield return null;
        }

        gunSlide.transform.localPosition = originalPos;

    }

    void BulletEject()
    {
        float randVelocity = Random.Range(50.0f, 300.0f);
        Quaternion gunRotation = this.transform.rotation;

        Vector3 gunEjectChamber = this.transform.Find("Bullet").transform.position;
        GameObject bulletShell = Instantiate(bullet, gunEjectChamber, gunRotation) as GameObject;
        bulletShell.AddComponent<BulletholeDecay>();

        bulletShell.AddComponent<Rigidbody>();
        bulletShell.GetComponent<Rigidbody>().AddRelativeForce(randVelocity, randVelocity, 0f);
        bulletShell.GetComponent<Rigidbody>().AddRelativeTorque(10000.0f, 10000.0f, 10000.0f);
    }
}

