using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shooting : MonoBehaviour
{

    private GUIStyle guiFont = new GUIStyle();

    public GameObject bulletDecal;
    public Text targetHitText;
    public GameObject bulletShell;
    public GameObject targets;

    private int targetHitCounter;
    private bool holdingGun;
    private bool gunCanPickUp;
    private static int ammoCount;
    private GameObject heldGun;
    private Vector3 gunEjectChamber;
    private Animation anim;

    void Awake()
    {
        UpdateHitCounter();
    }

    void Start()
    {
        Animation setAnim = targets.GetComponent<Animation>();
        setAnim.playAutomatically = false;
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Ray targetRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100, Color.green);

        if (holdingGun == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DropWeapon(heldGun);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ammoCount = 17;
                Debug.Log("Gun reloaded");
            }
        }

        if (Physics.Raycast(targetRay, out hit))
        {

            //Only guns can be picked up if no gun is equipped
            if (hit.collider.tag == "Gun")
            {
                gunCanPickUp = true;
                if (holdingGun == false && Input.GetKeyDown(KeyCode.E))
                {
                    PickUpWeapon(hit.collider.gameObject);
                    heldGun = hit.collider.gameObject;
                }
            }

            else if (hit.collider.tag != "Gun")
            {
                gunCanPickUp = false;
            }


            //If a gun is equipped, allow shooting actions
            if (holdingGun == true && Input.GetButtonDown("Fire1"))
            {
                // Do not fire gun if there is no ammo
                if (ammoCount == 0)
                {
                    return;
                }

                else if (ammoCount != 0)
                {
                    // Eject a bullet shell with each shot
                    BulletEject(bulletShell, heldGun);

                    // Play the gun's fire animation
                    anim = heldGun.GetComponent<Animation>();
                    anim.Play("Shooting");

                    // Update ammo count with each shot
                    ammoCount = ammoCount - 1;
                    Debug.Log("Ammo: " + ammoCount);


                    //Track what has been shot
                    Debug.Log("Object Hit: " + hit.collider.gameObject.name);


                    //Place bullet hole at area shot
                    if (hit.collider.tag != ("Target"))
                    {
                        Instantiate(bulletDecal, hit.point, Quaternion.identity);
                    }


                    //If target is shot, update score, break target, and respawn target
                    else if (hit.collider.tag == ("Target"))
                    {
                        StartCoroutine(TargetHitReset(hit.collider.gameObject));
                        
                        ++targetHitCounter;
                        UpdateHitCounter();
                    }
                }
            }
        }
    }

    void PickUpWeapon(GameObject gun)
    {
        //Remove the collider and rigidbody of the gun
        Destroy(gun.GetComponent<Rigidbody>());
        Destroy(gun.GetComponent<BoxCollider>());

        //Attach gun to camera point
        gun.transform.SetParent(Camera.main.transform);
        holdingGun = true;

        //Set ammo of gun
        if (gun.name == "Glock18_Simple")
        {
            ammoCount = 17;
        }

        if (gun.name == "HK45")
        {
            ammoCount = 10;
        }

        //Position for original "detailed" model
        //gun.transform.localPosition = new Vector3(0.48f, -0.43f, 0.84f);
        //gun.transform.localRotation = Quaternion.Euler(0, 270, 0);

        //Position for simple animated model
        gun.transform.localPosition = new Vector3(0.6f, -0.5f, 1.1f);
        gun.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void DropWeapon(GameObject gun)
    {
        gun.AddComponent<Rigidbody>();
        BoxCollider gunCollider = gun.AddComponent<BoxCollider>();
        gunCollider.size = new Vector3(0.1f, 0.49f, 0.88f);
        gunCollider.center = new Vector3(0f, 0.01f, -0.05f); 

        Camera.main.transform.DetachChildren();
        holdingGun = false;
    }

    void UpdateHitCounter()
    {
        targetHitText.text = "Targets Hit: " + targetHitCounter.ToString();
    }

    void BulletEject(GameObject bullet, GameObject gun)
    {
        float randVelocity = Random.Range(200.0f, 300.0f);

        gunEjectChamber = gun.transform.TransformPoint(1.0f, 1.0f, 1.0f) + new Vector3(-1.0f, -1.0f, -0.7f);
        GameObject bulletShell = Instantiate(bullet, gunEjectChamber, Quaternion.Euler(0f, 180f, 0f)) as GameObject;
        bulletShell.AddComponent<BulletholeDecay>();

        bulletShell.AddComponent<Rigidbody>();
        bulletShell.GetComponent<Rigidbody>().AddRelativeForce(randVelocity, randVelocity, 0f);
        bulletShell.GetComponent<Rigidbody>().AddRelativeTorque(10000.0f, 10000.0f, 10000.0f);
    }

    IEnumerator TargetHitReset(GameObject target)
    {
        float duration = .280f;
        Vector3 targetPosition = target.transform.position;

        Animation targetBreakAnim = target.GetComponent<Animation>();
        targetBreakAnim.Play("TargetBreak");

        yield return new WaitForSeconds(duration);

        DestroyObject(target);

        yield return new WaitForSeconds(3);

        SpawnTarget(targets, targetPosition);

        yield return null;
    }

    void SpawnTarget(GameObject target, Vector3 spawnLocation)
    {
        GameObject newTarget = (GameObject)Instantiate(targets, spawnLocation, Quaternion.identity);

        newTarget.tag = ("Target");
        newTarget.AddComponent<BoxCollider>();
        BoxCollider targetHitBox = newTarget.GetComponent<BoxCollider>();
        targetHitBox.center = new Vector3(0f, 0f, -0.17f);
        targetHitBox.size = new Vector3(1f, 1f, 0.1f);
    }

    void OnGUI()
    {
        guiFont.fontSize = 20;
        float xMin = (Screen.width / 2);
        float yMin = (Screen.height / 1.5f);
        float yMax = (Screen.height / 1f);

        if (gunCanPickUp)
        {
            GUI.Label(new Rect(xMin, yMin, 500, 200), "Press E to pick up gun", guiFont);
        }
        if (holdingGun)
        {
            GUI.Label(new Rect(0, yMax, 200, 200), "Ammo: " + ammoCount, guiFont);
        }
    }
}
