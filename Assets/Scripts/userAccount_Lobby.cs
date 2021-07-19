using UnityEngine;
using UnityEngine.UI;

public class userAccount_Lobby : MonoBehaviour
{
    // Start is called before the first frame update
    public Text usernameText;
    void Start()
    {
        if (userAccountManager.isLoggedIn)
            usernameText.text = userAccountManager.PlayerUsername;
    }

    public void LogOut()
    {
        if (userAccountManager.isLoggedIn)
            userAccountManager.instance.LogOut();
    }
}
