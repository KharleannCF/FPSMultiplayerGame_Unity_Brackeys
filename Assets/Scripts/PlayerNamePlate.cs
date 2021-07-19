
using UnityEngine;
using UnityEngine.UI;

public class PlayerNamePlate : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text usernameText;
    [SerializeField]
    private RectTransform healthBarFill;
    [SerializeField]
    private PlayerManager player;


    // Update is called once per frame
    void Update()
    {
        usernameText.text = player.username;
        healthBarFill.localScale = new Vector3(player.getHealthPercentage(), 1f, 1f);

    }
}
