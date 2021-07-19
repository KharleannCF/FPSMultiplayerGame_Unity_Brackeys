using System.Collections;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField]
    GameObject playerScoreboardItem;
    [SerializeField]
    Transform playerScoreboardList;
    void OnEnable()
    {
        //GetArrayPlayers
        PlayerManager[] players = GameManager.GetAllPlayers();

        foreach (PlayerManager player in players)
        {
            Debug.Log(player.username + "|" + player.kills + "|" + player.deaths);
            GameObject itemGO = Instantiate(playerScoreboardItem, playerScoreboardList);
            PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();
            if (item != null)
            {
                item.Setup(player.username, player.kills, player.deaths);
            }
        }
        //Set UI elements 
    }

    void OnDisable()
    {
        //Clean up list of items
        foreach (Transform child in playerScoreboardList)
        {
            Destroy(child.gameObject);
        }
    }
}
