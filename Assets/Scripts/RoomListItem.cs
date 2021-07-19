using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{

    public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);
    private JoinRoomDelegate joinRoomDelegate;

    [SerializeField]
    private Text roomNameText;
    private MatchInfoSnapshot match;
    public void Setup(MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomDelegate)
    {
        match = _match;
        joinRoomDelegate = _joinRoomDelegate;
        roomNameText.text = match.name + " (" + match.currentSize + "/" + match.maxSize + ")";
    }

    public void JoinRoom()
    {
        joinRoomDelegate.Invoke(match);
    }
}
