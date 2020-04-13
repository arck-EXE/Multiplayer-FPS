using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    //constant and static variable to store the playerID
    private const string PLAYER_TAG = "Player";

    //Weapon class which store info about the current weapon
    private PlayerWeapon currentweapon;

    [SerializeField]
    private Camera cam;

    //layer mask to avoid the layers that player should not hit.
    [SerializeField]
    private LayerMask mask;

    private WeaponManager weaponManager;

    private void Start()
    {
        if(cam == null)
        {
            //disables the camera when game starts
            Debug.LogError("No Camera Found : PlayerShoot!!");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>(); 
    }

    private void Update()
    {
        currentweapon = weaponManager.GetCurrentWeapon();

        if (currentweapon.firerate <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / currentweapon.firerate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
    }


    //callef on the client only
    [Client]
    void Shoot()
    {
        Debug.Log("Shoot");

        RaycastHit _hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentweapon.range, mask))
        {
            if(_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, currentweapon.damage);
            }
        }
    }

    //called on the server
    [Command]
    void CmdPlayerShot(string _ID, int _damage)
    {
        Debug.Log(_ID + " has been Shot.");
        Player _player = GameManager.GetPlayer(_ID);
        //updates the client to assign the damage the player
        _player.RpcTakeDamage(_damage);
    }
}
