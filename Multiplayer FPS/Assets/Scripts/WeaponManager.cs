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

    private WeaponGraphics currentGraphics; 

    private void Start()
    {
        EqiupWeapon(primaryweapon);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentweapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    void EqiupWeapon(PlayerWeapon _weapon)
    {
        currentweapon = _weapon;

        GameObject _weaponInst = Instantiate(_weapon.Graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponInst.transform.SetParent(weaponHolder);

        currentGraphics = _weaponInst.GetComponent<WeaponGraphics>();
        if (currentGraphics == null)
            Debug.LogError("No weapon Graphics on " + _weaponInst.name);

        if (isLocalPlayer)
            Util.SetLayerRecursively(_weaponInst, LayerMask.NameToLayer(weaponLayerName));
    }
}
