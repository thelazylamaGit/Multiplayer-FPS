using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayer_player : MonoBehaviour
{
    public CWeapon weapon;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<WeaponHandler>().WeaponLoad(weapon);
        }
    }
}
