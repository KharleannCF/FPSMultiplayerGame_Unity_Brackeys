
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public Text killCount;
    public Text deathCount;
    // Start is called before the first frame update
    void Start()
    {
        if (userAccountManager.isLoggedIn)
            userAccountManager.instance.GetData(OnReceiveData);
    }

    void OnReceiveData(string data)
    {
        killCount.text = DataTranslator.DataToKills(data).ToString() + " Kills";
        deathCount.text = DataTranslator.DataToDeaths(data).ToString() + " Deaths";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
