using UnityEngine;
using Mirror;
using System.Collections;

[RequireComponent(typeof(PlayerSetupScript))]
public class PlayerManager : NetworkBehaviour
{

    // Start is called before the first frame update
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }
    [SyncVar]
    public string username = "Loading...";
    public int kills;
    public int deaths;


    [SerializeField]
    private Behaviour[] disableOnDeath;


    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;
    private bool[] wasEnabled;

    private bool firstSetup = true;

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    public void PlayerSetup()
    {

        if (isLocalPlayer)
        {
            GameManager.singleton.SetSceneCameraActive(false);
            GetComponent<PlayerSetupScript>().playerUIInstance.SetActive(true);

        }
        //Add player to all clients through server
        CmdBroadCastNewPlayerSetup();
        /*  wasEnabled = new bool[disableOnDeath.Length];
         for (int i = 0; i < wasEnabled.Length; i++)
         {
             wasEnabled[i] = disableOnDeath[i].enabled;
         }
         SetDefaults(); */
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcGetDamage(999999, username);
        }
    }


    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClient();
    }


    [ClientRpc]
    private void RpcSetupPlayerOnAllClient()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }
            firstSetup = false;
        }
        SetDefaults();
    }

    [ClientRpc]
    public void RpcGetDamage(int _amount, string _sourceID)
    {
        if (isDead) return;
        currentHealth -= _amount;
        Debug.Log(_sourceID + "now has " + currentHealth);

        if (currentHealth <= 0)
        {
            Died(_sourceID);
        }
    }

    public float getHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }

    private void Died(string _sourceID)
    {
        isDead = true;
        PlayerManager sourcePlayer = GameManager.GetPlayer(_sourceID);
        if (sourcePlayer != null)
        {
            sourcePlayer.kills++;
            GameManager.singleton.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
        }


        deaths++;
        //disable components after dead
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }
        //Disable the collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;
        }

        //Spawn death effect
        GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

        if (isLocalPlayer)
        {
            GameManager.singleton.SetSceneCameraActive(true);
            GetComponent<PlayerSetupScript>().playerUIInstance.SetActive(false);
        }
        Debug.Log(transform.name + "Dead!");

        //Call respawn
        StartCoroutine(Respawn());

    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.singleton.matchSettings.respawnTime);
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);
        /* 
                GameManager.singleton.SetSceneCameraActive(false);
                GetComponent<PlayerSetupScript>().playerUIInstance.SetActive(true);
         */
        PlayerSetup();


    }
    public void SetDefaults()
    {

        isDead = false;

        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = true;
        }
        //Spawn Effect
        GameObject _gfxIns = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
    }
    // Update is called once per frame

}
