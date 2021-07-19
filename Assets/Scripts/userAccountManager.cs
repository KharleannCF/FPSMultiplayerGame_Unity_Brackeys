using System.Collections;
using UnityEngine;
using DatabaseControl;
using UnityEngine.SceneManagement;
public class userAccountManager : MonoBehaviour
{
    public static userAccountManager instance;

    void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);



    }
    public static string LoggedIn_Data { get; protected set; }
    public static string PlayerUsername { get; protected set; }
    public static bool isLoggedIn { get; protected set; }
    private static string PlayerPassword = "";

    public string LoggedInScene = "Lobby";
    public string LoggedOutScene = "LoginMenu";
    public delegate void OnDataReceivedCallback(string data);


    public void LogOut()
    {
        PlayerUsername = "";
        PlayerPassword = "";
        isLoggedIn = false;
        SceneManager.LoadScene(LoggedOutScene);

    }

    public void LogIn(string UserName, string Password)
    {
        PlayerUsername = UserName;
        PlayerPassword = Password;
        isLoggedIn = true;
        SceneManager.LoadScene(LoggedInScene);
    }

    public void SendData(string data)
    {
        if (isLoggedIn)
        {
            StartCoroutine(SetData(data));
        }//Called when the player hits 'Set Data' to change the data string on their account. Switches UI to 'Loading...' and starts coroutine to set the players data string on the server

    }

    public void GetData(OnDataReceivedCallback onDataReceived)
    { //called when the 'Get Data' button on the data part is pressed

        if (isLoggedIn)
        {
            //ready to send request
            StartCoroutine(sendGetDataRequest(PlayerUsername, PlayerPassword, onDataReceived)); //calls function to send get data request
        }
    }
    IEnumerator sendGetDataRequest(string username, string password, OnDataReceivedCallback onDataReceived)
    {
        string data = "ERROR";

        IEnumerator e = DCF.GetUserData(username, password); // << Send request to get the player's data string. Provides the username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Error")
        {
            Debug.Log("Data Upload Error. Could be a server error. To check try again, if problem still occurs, contact us.");
        }
        else
        {
            data = response;
        }

        if (onDataReceived != null)
            onDataReceived.Invoke(data);
    }
    IEnumerator SetData(string data)
    {
        IEnumerator e = DCF.SetUserData(PlayerUsername, PlayerPassword, data); // << Send request to set the player's data string. Provides the username, password and new data string
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response != "Success")
        {
            //The data string was set correctly. Goes back to LoggedIn UI
            Debug.Log("There was an error");
        }

    }
}
