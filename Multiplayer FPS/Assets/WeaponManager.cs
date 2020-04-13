using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private PlayerWeapon primaryweapon;

    private PlayerWeapon currentweapon;

    private void Start()
    {
        EqiupWeapon(primaryweapon);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentweapon;
    }

    void EqiupWeapon(PlayerWeapon _weapon)
    {
        currentweapon = _weapon;

        GameObject _weaponInst = Instantiate(_weapon.Graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponInst.transform.SetParent(weaponHolder);

        if (isLocalPlayer)
            _weaponInst.layer = LayerMask.NameToLayer(weaponLayerName);
    }
}
