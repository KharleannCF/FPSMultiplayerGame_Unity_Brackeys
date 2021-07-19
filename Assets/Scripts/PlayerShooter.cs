using Mirror;
using UnityEngine;

[RequireComponent(typeof(weaponManager))]
public class PlayerShooter : NetworkBehaviour
{
    // Start is called before the first frame update

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon activeWeapon;


    private weaponManager weaponManager;
    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced");
            this.enabled = false;
        }
        weaponManager = GetComponent<weaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        activeWeapon = weaponManager.GetCurrentWeapon();
        if (Pause_Menu.isOn) return;
        if (activeWeapon != null)
        {
            if (activeWeapon.bullets < activeWeapon.maxBullets)
            {
                if (Input.GetButtonDown("Reload"))
                {
                    weaponManager.Reload();
                    return;
                }
            }
            if (activeWeapon.fireRate <= 0)
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
                    InvokeRepeating("Shoot", 0f, 1f / activeWeapon.fireRate);
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    CancelInvoke("Shoot");
                }
            }
        }
    }

    [Command] //Called on the server when the player shoot
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    [ClientRpc] // Called in all clients when we need to shoot
    void RpcDoShootEffect()
    {
        weaponGraphics _currentWeapon = weaponManager.GetCurrentGraphics();
        _currentWeapon.muzzleFlash.Play();
        _currentWeapon.muzzleSound.Play();
    }
    //Call on hit in server
    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }

    //Call on hit in all clients after the hit (spawn effects)
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 2f);
    }

    [Client]
    private void Shoot()
    {
        Debug.Log("Current bullets " + activeWeapon.bullets);
        if (!isLocalPlayer || weaponManager.isReloading)
        {
            return;
        }

        if (activeWeapon.bullets <= 0)
        {
            weaponManager.Reload();
            //We are going to reload here
            return;
        }
        Debug.Log("Remaining bullets " + activeWeapon.bullets);

        activeWeapon.bullets--;
        //We are shooting call do shooting on server
        CmdOnShoot();
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, activeWeapon.range, mask))
        {
            //Something hit
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, activeWeapon.damage, transform.name);
            }
            //Something was hit
            CmdOnHit(_hit.point, _hit.normal);
        }
        if (activeWeapon.bullets <= 0)
        {
            weaponManager.Reload();
        }
    }

    [Command]
    void CmdPlayerShot(string _playerID, int _damage, string _sourceID)
    {
        Debug.Log(_playerID + " has been shot");
        PlayerManager _player = GameManager.GetPlayer(_playerID);
        _player.RpcGetDamage(_damage, _sourceID);
    }
}
