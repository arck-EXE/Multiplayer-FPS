using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    //constant and static variable to store the playerID
    private const string PLAYER_TAG = "Player";
    [SerializeField]
    private string weaponLayerName = "Weapon";

    //Weapon class which store info about the current weapon
    [SerializeField]
    private PlayerWeapon weapon;

    [SerializeField]
    private GameObject weaponGFX;

    [SerializeField]
    private Camera cam;
    //layer mask to avoid the layers that player should not hit.
    [SerializeField]
    private LayerMask mask;

    private void Start()
    {
        if(cam == null)
        {
            //disables the camera when game starts
            Debug.LogError("No Camera Found : PlayerShoot!!");
            this.enabled = false;
        }

        weaponGFX.layer = LayerMask.NameToLayer(weaponLayerName);
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }


    //callef on the client only
    [Client]
    void Shoot()
    {
        RaycastHit _hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            if(_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, weapon.damage);
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
