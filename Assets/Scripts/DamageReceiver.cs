using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour, IEntity
{
    public float playerHP = 100;
    public PlayerController playerController;
    public WeaponManager weaponManager;

    public void ApplyDamage(float points)
    {
        playerHP -= points;

        if(playerHP <= 0)
        {
            //Player is dead
            playerController.canMove = false;
            playerHP = 0;
        }
    }
}