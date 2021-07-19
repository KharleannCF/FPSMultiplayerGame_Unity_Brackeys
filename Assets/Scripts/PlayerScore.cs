using System.Collections;

using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class PlayerScore : MonoBehaviour
{
    // Start is called before the first frame update
    int lastKills = 0;
    int lastDeaths = 0;
    PlayerManager player;
    void Start()
    {
        player = GetComponent<PlayerManager>();
        StartCoroutine(SyncScoreLoop());
    }
    IEnumerator SyncScoreLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            SyncNow();


        }
    }
    void OnDestroy()
    {
        if (player != null)
            SyncNow();
    }

    void SyncNow()
    {
        if (userAccountManager.isLoggedIn)
        {
            userAccountManager.instance.GetData(OnDataReceived);
        }
    }
    void OnDataReceived(string data)
    {
        if (player.kills <= lastKills && player.deaths <= lastDeaths) return;

        int killsSinceLast = player.kills - lastKills;
        int deathsSinceLast = player.deaths - lastDeaths;

        if (killsSinceLast == 0 && deathsSinceLast == 0) return;

        int kills = DataTranslator.DataToKills(data);
        int deaths = DataTranslator.DataToDeaths(data);

        int newKills = killsSinceLast + kills;
        int newDeaths = deathsSinceLast + deaths;

        string newData = DataTranslator.ValuesToData(newKills, newDeaths);
        Debug.Log("Syncing: " + newData);
        lastKills = player.kills;
        lastDeaths = player.deaths;
        userAccountManager.instance.SendData(newData);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
