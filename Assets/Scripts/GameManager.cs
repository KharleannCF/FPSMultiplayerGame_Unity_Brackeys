using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    #region Player tracking


    private const string PLAYER_PREFIX = "Player ";
    private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();

    public static void RegisterPlayer(string _netID, PlayerManager _player)
    {
        string _playerID = PLAYER_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
        Debug.Log(players[_playerID].transform.name);
    }

    public static void UnregisterPlayer(string _playerID)
    {
        Debug.Log("Player gone " + _playerID);
        players.Remove(_playerID);
    }


    public static PlayerManager GetPlayer(string _PlayerID)
    {
        return players[_PlayerID];
    }
    /* void OnGui()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();
        foreach (string _playerID in players.Keys)
        {
            GUILayout.Label(_playerID + "  -  " + players[_playerID].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    } */
    #endregion

    public static GameManager singleton;
    public MatchSettings matchSettings;

    [SerializeField]
    private GameObject sceneCamera;


    public delegate void OnPlayerKilledCallback(string player, string source);
    public OnPlayerKilledCallback onPlayerKilledCallback;
    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("There is more than one Game manager");
        }
        else
        {
            singleton = this;
        }
    }

    public void SetSceneCameraActive(bool isActive)
    {
        if (sceneCamera == null) return;
        sceneCamera.SetActive(isActive);
    }

    public static PlayerManager[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }
}
