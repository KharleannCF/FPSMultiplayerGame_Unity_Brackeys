using Mirror;
using UnityEngine;
using System.Collections;
public class weaponManager : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon activeWeapon;
    private weaponGraphics activeGraphics;

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private string weaponLayerName = "Weapon";
    public bool isReloading = false;

    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    void EquipWeapon(PlayerWeapon _weapon)
    {
        activeWeapon = _weapon;
        GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);

        activeGraphics = _weaponIns.GetComponent<weaponGraphics>();
        if (activeGraphics == null)
        {
            Debug.LogError("No WeaponGraphics on current weapon " + _weaponIns.name);

        }
        if (isLocalPlayer)
        {
            Util.SetLayerRecursively(_weaponIns, LayerMask.NameToLayer(weaponLayerName));
        }
    }
    // Update is called once per frame

    public PlayerWeapon GetCurrentWeapon()
    {
        return activeWeapon;
    }
    public weaponGraphics GetCurrentGraphics()
    {
        return activeGraphics;
    }

    public void Reload()
    {
        if (isReloading)
        {
            return;
        }
        StartCoroutine(Reload_Coroutine());

    }

    private IEnumerator Reload_Coroutine()
    {
        isReloading = true;
        CmdOnReload();
        yield return new WaitForSeconds(activeWeapon.reloadTime);
        Debug.Log("Reloading...");
        activeWeapon.bullets = activeWeapon.maxBullets;
        isReloading = false;
    }

    [Command]
    void CmdOnReload()
    {
        RpcOnReload();
    }
    [ClientRpc]
    void RpcOnReload()
    {
        Animator anim = activeGraphics.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Reload");
        }
    }

}

