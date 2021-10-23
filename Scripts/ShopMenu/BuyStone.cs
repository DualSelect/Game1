using System;
using System.Collections;
using UnityEngine;
using Gs2.Core;
using Gs2.Unity.Gs2Showcase.Result;
using Gs2.Unity.Util;
using Gs2.Core.Exception;
using System.Collections.Generic;
using Gs2.Unity.Gs2Showcase.Model;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BuyStone : MonoBehaviour
{
    GameObject login;
    public GameObject stoneWindow;
    public GameObject waitWindow;
    public GameObject okButtun;
    public Text message;
    [System.Serializable]
    public class OnErrorCallback : UnityEngine.Events.UnityEvent<Gs2Exception>
    {

    }
    [SerializeField]
    private OnErrorCallback m_events = new OnErrorCallback();

    void Start()
    {
        login = GameObject.Find("Login");
    }

    public void Buy120()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(Buy("kakin120"));
    }
    public void Buy370()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(Buy("kakin370"));
    }
    public void Buy980()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(Buy("kakin980"));
    }
    public void Buy2440()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(Buy("kakin2440"));
    }
    public void Buy4900()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(Buy("kakin4900"));
    }
    private IEnumerator Buy(string productId)
    {
        stoneWindow.SetActive(false);
        waitWindow.SetActive(true);

        var gs2 = login.GetComponent<LoginInitial>().GetClient();
        var session = login.GetComponent<LoginInitial>().GetSession();


        AsyncResult<EzGetShowcaseResult> asyncResult2 = null;
        var current2 = gs2.Showcase.GetShowcase(
          r => { asyncResult2 = r; },
          session: session,
          namespaceName: "kakin",
          showcaseName: "kakin"
        );
        yield return current2;
        if (asyncResult2.Error != null)
        {
            OnError(asyncResult2.Error);
            yield break;
        }

        string receipt = null;

        AsyncResult<PurchaseParameters> result = null;
        yield return new IAPUtil().Buy(r => { result = r; }, productId);

        if (result.Error != null)
        {
            OnError(result.Error);
            yield break;
        }

        receipt = result.Result.receipt;
        Debug.Log(receipt);
        string stampSheet = null;
        AsyncResult<EzBuyResult> asyncResult = null;
        var current = gs2.Showcase.Buy(
          r => { asyncResult = r; },
          session: session,
          namespaceName: "kakin",
          showcaseName: "kakin",
          displayItemId: asyncResult2.Result.Item.DisplayItems.Find(m => m.SalesItem.Name == productId).DisplayItemId,
          config: new List<EzConfig>
                    {
                        new EzConfig
                        {
                            Key = "receipt",
                            Value = receipt,
                        },
                    }
        );
        yield return current;
        if (asyncResult.Error != null)
        {
            OnError(asyncResult.Error);
            yield break;
        }
        stampSheet = asyncResult.Result.StampSheet;
        Debug.Log(stampSheet);
        
        var machine = new StampSheetStateMachine(stampSheet,gs2, "distributor", "grn:gs2:ap-northeast-1:uFLAkqDK-Development:key:kakin-key:key:kakin-key");
        yield return machine.Execute(m_events);
        message.text = "購入完了しました！";
        okButtun.SetActive(true);
    }



    public void OnError(Exception e)
    {
        message.text = "購入がキャンセルされました";
        okButtun.SetActive(true);
        Debug.Log(e.ToString());
    }

    public void OkButton()
    {
        PlayerPrefs.SetString("menu", "shop");
        SceneManager.LoadScene("MainMenu");
    }
}
