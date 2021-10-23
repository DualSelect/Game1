using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Gs2.Core;
using Gs2.Unity.Gs2Matchmaking.Model;
using Gs2.Unity.Gs2Matchmaking.Result;

public class CancelMatching : MonoBehaviour
{
    GameObject login;
    private void Start()
    {
        login = GameObject.Find("Login");
    }

    public void CancelButtonDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(CancelMatch());
    }
    public IEnumerator CancelMatch()
    {
        var gs2 = login.GetComponent<LoginInitial>().GetClient();
        var session = login.GetComponent<LoginInitial>().GetSession();
        AsyncResult<EzCancelMatchmakingResult> asyncResult = null;
        var current = gs2.Matchmaking.CancelMatchmaking(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "matchmaking",
              gatheringName: PlayerPrefs.GetString("gatheringName")

        );
        yield return current;

        SceneManager.LoadScene("BattleMenu");
    }

}
