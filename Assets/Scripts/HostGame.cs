using Mirror;
using UnityEngine;
public class HostGame : MonoBehaviour
{
    [SerializeField]
    private uint roomSize = 6;
    private NetworkManager networkManager;
    private string roomName;

    void Start()
    {
        networkManager = NetworkManager.singleton;
        /* if (networkManager.matchMaker == null)
        {
        } */

    }

    public void SetRoomName(string _name)
    {
        roomName = _name;
    }

    public void CreateRoom()
    {
        /* if (roomName != "" && roomName != null)
        {
            Debug.Log("Create the room " + roomName + " for " + roomSize);
            //create room
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        } */
    }
}

