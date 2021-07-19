using UnityEngine;
using Mirror;

[RequireComponent(typeof(PlayerManager))]
public class PlayerSetupScript : NetworkBehaviour
{

    [SerializeField]
    Behaviour[] componentsToDisable;
    [SerializeField]
    string remoteLayerName = "Remote Player";
    [SerializeField]
    string dontDrawLayerName = "DontDraw";
    [SerializeField]
    GameObject playerGraphics;


    [SerializeField]
    GameObject playerUIPrefab;
    [HideInInspector]
    public GameObject playerUIInstance;


    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {

            //Disable player graphics for local
            Util.SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            //Create playerUI
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;
            //Confi PUI
            PlayerUI_custom ui = playerUIInstance.GetComponent<PlayerUI_custom>();
            if (ui == null)
            {
                Debug.LogError("No player UI on the prefab");
            }
            PlayerManager poller = GetComponent<PlayerManager>();
            if (poller != null)
            {
                ui.SetPlayer(poller);
            }
            //ui.SetController(GetComponent<Player_Controller>());
            GetComponent<PlayerManager>().PlayerSetup();
            string _username = "Loading...";
            if (userAccountManager.isLoggedIn)
            {
                _username = userAccountManager.PlayerUsername;
            }
            else
            {
                _username = transform.name;
            }
            CmdSetUsername(transform.name, _username);
        }

    }

    [Command]
    void CmdSetUsername(string _playerID, string username)
    {
        PlayerManager player = GameManager.GetPlayer(_playerID);
        if (player != null)
        {
            player.username = username;
        }
    }

    void SetLayerRecursively(GameObject gameObject, int newLayer)
    {
        gameObject.layer = newLayer;
        foreach (Transform child in gameObject.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        PlayerManager _player = GetComponent<PlayerManager>();
        GameManager.RegisterPlayer(_netID, _player);
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }
    void OnDisable()
    {

        Destroy(playerUIInstance);
        if (isLocalPlayer)
        {
            GameManager.singleton.SetSceneCameraActive(true);
        }
        GameManager.UnregisterPlayer(transform.name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
