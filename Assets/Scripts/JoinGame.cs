using Mirror;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking.Match;
using System.Collections;
public class JoinGame : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text status;
    private NetworkManager networkManager;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private Transform roomListParent;

    List<GameObject> roomList = new List<GameObject>();
    void Start()
    {
        networkManager = NetworkManager.singleton;
        /* if (networkManager.matchMaker == null){
            networkManager.StartMatchMaker();
        } */

        RefreshRoomList();
    }

    public void RefreshRoomList()
    {
        /* if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        } */
        ClearRoomList();
        //networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
        status.text = "Loading...";
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        status.text = "";
        if (!success || matchList == null)
        {
            status.text = "Couldn't get room list";
            return;
        }

        ClearRoomList();
        foreach (MatchInfoSnapshot match in matchList)
        {
            GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
            _roomListItemGO.transform.SetParent(roomListParent);
            //Have component sit on game object that will take care of setting the data
            RoomListItem _roomListItem = _roomListItemGO.GetComponent<RoomListItem>();
            if (_roomListItem != null)
            {
                _roomListItem.Setup(match, JoinRoom);
            }

            roomList.Add(_roomListItemGO);
        }
        if (roomList.Count == 0)
        {
            status.text = "There are no rooms at the moment.";
        }
    }

    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }
        roomList.Clear();
    }

    public void JoinRoom(MatchInfoSnapshot _match)
    {
        Debug.Log("Joining " + _match.name);

        //networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        StartCoroutine(WaitForJoin());
    }

    IEnumerator WaitForJoin()
    {
        ClearRoomList();
        int countdown = 10;
        while (countdown > 0)
        {
            status.text = "Joining...(" + countdown + ")";
            yield return new WaitForSeconds(1);
            countdown--;
        }
        status.text = "Failed to connect";
        /*  MatchInfo matchInfo = networkManager.matchInfo;
         if (matchInfo != null)
         {
             //MatchInfo matchInfo = networkManager.matchInfo;
             //networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0 , networkManager.OnDropConnection);
             networkManager.StopHost();
         } */
        yield return new WaitForSeconds(1);
        RefreshRoomList();
    }

}
