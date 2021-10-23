using Gs2.Core;
using Gs2.Core.Exception;
using Gs2.Unity;
using Gs2.Unity.Gs2Exchange.Result;
using Gs2.Unity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExchangePrefab : MonoBehaviour
{
    public string cardId;
    public Text rare;
    public new Text name;
    public Text button;
    public Button exchange;
    [System.Serializable]
    public class OnErrorCallback : UnityEngine.Events.UnityEvent<Gs2Exception>
    {

    }
    [SerializeField]
    private OnErrorCallback m_events = new OnErrorCallback();
    public void Exchange()
    {
        StartCoroutine(Exchange2());
    }
    private IEnumerator Exchange2()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        GameObject.Find("Loading").GetComponent<Loading>().LoadingStart();
        Client gs2 = GameObject.Find("Login").GetComponent<LoginInitial>().GetClient();
        GameSession session = GameObject.Find("Login").GetComponent<LoginInitial>().GetSession();


        AsyncResult<EzExchangeResult> asyncResult = null;
        var current = gs2.Exchange.Exchange(
          r => { asyncResult = r; },
          session: session,
          namespaceName: "exchange",
          rateName:cardId,
          count:1
        );
        yield return current;
        if (asyncResult.Error != null)
        {
            OnError(asyncResult.Error);
            yield break;
        }
        string stampSheet = asyncResult.Result.StampSheet;
        Debug.Log(stampSheet);

        var machine = new StampSheetStateMachine(stampSheet, gs2, "distributor", "grn:gs2:ap-northeast-1:uFLAkqDK-Development:key:kakin-key:key:kakin-key");

        yield return machine.Execute(m_events);

        PlayerPrefs.SetInt("PackNum", 1);
        PlayerPrefs.SetString("Pack1", cardId);
        SceneManager.LoadScene("CardPackResult");
    }
    public void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
}
