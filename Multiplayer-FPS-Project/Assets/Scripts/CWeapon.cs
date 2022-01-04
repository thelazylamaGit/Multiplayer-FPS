using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapon")]
public class CWeapon : ScriptableObject
{
    public WeaponHandler.Ammo ammoType;
    public WeaponHandler.CastType castType;
    public WeaponHandler.FireSelect fireSelect;
    public string name;
    public AudioClip fireClip;
    public GameObject model;
    public float fireRate;
    public bool rocket;
}
