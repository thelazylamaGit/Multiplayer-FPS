using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public enum Ammo
    {
        LIGHT, MEDIUM, HEAVY
    }

    public enum FireSelect
    {
        SEMI, AUTO
    }

    public enum CastType
    {
        INST, RAY
    }


    //numerical
    private int[] allAmmo = new int[3];
    private int ammoLight = 25;
    private int ammoMedium = 25;
    private int ammoHeavy = 25;
    private float nextShot = 0;
    private float reloadTime = 0;
    float grenadeAccel = 0;

    //bool
    [HideInInspector]
    public bool loaded = false;

    //misc
    public GameObject missile;
    public GameObject grenade;
    public Vector3 modelPos;

    //current equipped
    public CWeapon activeWeapon;


    public void Update()
    {
        if(activeWeapon != null)
        {
            //update loaded bool
            if (allAmmo[(int)activeWeapon.ammoType] > 0)
                loaded = true;

            FireSelection();
        }
    }


    //========================================
    //FireSelection
    //Differentiate between FireSelect
    //========================================
    private void FireSelection()
    {
        if(activeWeapon != null)
        {
            switch (activeWeapon.fireSelect)
            {
                case FireSelect.AUTO:
                    {
                        //full auto
                        if (Input.GetMouseButton(0) && loaded)
                        {
                            if (Input.GetMouseButton(0) && ((Time.time - nextShot) > (1f / (float)activeWeapon.fireRate)))
                            {
                                nextShot = Time.time;
                                PrimaryAttack();
                            }
                        }
                        break;
                    }
                case FireSelect.SEMI:
                    {
                        //semi auto
                        if (Input.GetMouseButtonDown(0) && loaded)
                        {
                            PrimaryAttack();
                        }
                        break;
                    }
            }
        }
    }


    //========================================
    //PrimaryAttack
    //Primary attack func
    //========================================
    private void PrimaryAttack()
    {
        //play weapon audio
        try
        {
            GetComponent<AudioSource>().clip = activeWeapon.fireClip;
            GetComponent<AudioSource>().Play();
        }
        catch
        {
            Debug.LogError("Unable to play fireClip");
        }

        //subtract relevant ammo
        int index = (int)activeWeapon.ammoType;
        allAmmo[index]--;

        switch (activeWeapon.castType)
        {
            case CastType.INST:
                {
                    //instantiate obj, missile, grenade etc
                    GameObject projectile;
                    if (activeWeapon.rocket)
                    {
                        //is missile
                        projectile = Instantiate(missile, transform.position, transform.rotation);
                    }
                    else
                    {
                        //is grenade
                        projectile = Instantiate(grenade, transform.position, transform.rotation);
                        projectile.GetComponent<Rigidbody>().AddForce(transform.forward * 500, ForceMode.Acceleration);
                    }

                    break;
                }
            case CastType.RAY:
                {
                    //raycast bullet
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
                    {
                        Debug.Log("weapon:" + activeWeapon.name + "hit->" + hit.collider.gameObject.name);

                        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        marker.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        marker.transform.position = hit.point;
                    }

                    break;
                }
        }
    }


    //========================================
    //WeaponLoad
    //Weapon init, load model etc
    //========================================
    public void WeaponLoad(CWeapon weapon)
    {
        activeWeapon = weapon;

        //remove old view model
        try
        {
            Destroy(GameObject.Find("v_model"));
        }
        catch{ }

        //spawn weapon view model
        GameObject v_model;
        v_model = Instantiate(activeWeapon.model, transform.position, transform.rotation);
        v_model.transform.localPosition = modelPos;
        v_model.name = "v_model";
    }


    private void Start()
    {
        allAmmo[0] = ammoLight;
        allAmmo[1] = ammoMedium;
        allAmmo[2] = ammoHeavy;
    }
}
