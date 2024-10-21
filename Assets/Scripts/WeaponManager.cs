using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Camera playerCamera;
    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;

    [HideInInspector]
    public Weapon selectedWeapon;

    void Start()
    {
        primaryWeapon.ActivateWeapon(true);
        secondaryWeapon.ActivateWeapon(false);
        selectedWeapon = primaryWeapon;
        primaryWeapon.manager = this;
        secondaryWeapon.manager = this;
    }

    void Update()
    {
        //Arma secundaria apretando "1"
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            primaryWeapon.ActivateWeapon(false);
            secondaryWeapon.ActivateWeapon(true);
            selectedWeapon = secondaryWeapon;
        }

        //Arma primaria apretando "2"
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            primaryWeapon.ActivateWeapon(true);
            secondaryWeapon.ActivateWeapon(false);
            selectedWeapon = primaryWeapon;
        }
    }
}