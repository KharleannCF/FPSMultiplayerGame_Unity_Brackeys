using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KillFeed : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject killFeedItemPrefab;
    void Start()
    {
        GameManager.singleton.onPlayerKilledCallback += OnKill;
    }

    public void OnKill(string player, string source)
    {
        GameObject go = (GameObject)Instantiate(killFeedItemPrefab, this.transform);
        int lastChild = this.transform.childCount;
        go.GetComponent<KillFeedItem>().Setup(player, source);
        go.transform.SetAsFirstSibling();
        Destroy(go, 4f);
    }

}
