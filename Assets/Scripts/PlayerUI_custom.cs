using UnityEngine;
using UnityEngine.UI;

public class PlayerUI_custom : MonoBehaviour
{
    [SerializeField]
    RectTransform thrusterFuelFill;
    [SerializeField]
    RectTransform HealthBarFill;

    [SerializeField]
    Text AmmoText;

    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject ScoreBoard;

    private PlayerManager player;
    private Player_Controller controller;
    private weaponManager weaponManager;
    public void SetPlayer(PlayerManager _player)
    {
        player = _player;
        controller = player.GetComponent<Player_Controller>();
        weaponManager = player.GetComponent<weaponManager>();
    }
    void SetFuelAmount(float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
    }
    void SetHealthAmount(float _amount)
    {
        HealthBarFill.localScale = new Vector3(1f, _amount, 1f);
    }
    void SetAmmoAmount(int _amount)
    {
        AmmoText.text = _amount.ToString() + "/" + weaponManager.GetCurrentWeapon().maxBullets.ToString();
    }

    void Start()
    {
        Pause_Menu.isOn = false;
    }

    void Update()
    {
        if (controller != null)
        {
            SetFuelAmount(controller.GetThrusterFuelAmount());
            SetHealthAmount(player.getHealthPercentage());
            SetAmmoAmount(weaponManager.GetCurrentWeapon().bullets);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ScoreBoard.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            ScoreBoard.SetActive(false);
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Pause_Menu.isOn = pauseMenu.activeSelf;
    }
}
