using System.Collections;

using UnityEngine;
using Mirror;
using UnityEngine.Networking.Match;

public class Pause_Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isOn = false;

    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    public void LeaveRoom()
    {
        //MatchInfo matchInfo = networkManager.matchInfo;
        //networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0 , networkManager.OnDropConnection);
        networkManager.StopHost();
    }
}
