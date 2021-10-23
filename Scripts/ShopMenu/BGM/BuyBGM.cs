using Gs2.Core;
using Gs2.Core.Exception;
using Gs2.Gs2Lottery.Result;
using Gs2.Unity;
using Gs2.Unity.Gs2Inventory.Result;
using Gs2.Unity.Gs2JobQueue.Result;
using Gs2.Unity.Gs2Lottery.Result;
using Gs2.Unity.Gs2Money.Model;
using Gs2.Unity.Gs2Money.Result;
using Gs2.Unity.Gs2Showcase.Result;
using Gs2.Unity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BuyBGM : MonoBehaviour
{
    GameObject login;
    Client gs2;
    GameSession session;
    public Button[] buttons;
    public Toggle[] toggles;
    public int dan;
    [System.Serializable]
    public class OnErrorCallback : UnityEngine.Events.UnityEvent<Gs2Exception>
    {

    }
    [SerializeField]
    private OnErrorCallback m_events = new OnErrorCallback();

    void Start()
    {
        login = GameObject.Find("Login");
        gs2 = login.GetComponent<LoginInitial>().GetClient();
        session = login.GetComponent<LoginInitial>().GetSession();
        for(int i = 0; i < buttons.Length; i++)
        {
            StartCoroutine(ButtonInteractive(i));
        }
        GameObject.Find("Loading").GetComponent<Loading>().LoadingEnd();
    }
    public void Buy1()
    {
        StartCoroutine(Buy(0));
    }
    public void Buy2()
    {
        StartCoroutine(Buy(1));
    }
    public void Buy3()
    {
        StartCoroutine(Buy(2));
    }
    public void Buy4()
    {
        StartCoroutine(Buy(3));
    }
    public void Buy5()
    {
        StartCoroutine(Buy(4));
    }
    private IEnumerator Buy(int num)
    {
        GameObject.Find("Loading").GetComponent<Loading>().LoadingStart();

        AsyncResult<EzGetShowcaseResult> asyncResult2 = null;
        var current2 = gs2.Showcase.GetShowcase(
          r => { asyncResult2 = r; },
          session: session,
          namespaceName: "BGM",
          showcaseName: "BGM"
        );
        yield return current2;
        if (asyncResult2.Error != null)
        {
            OnError(asyncResult2.Error);
            yield break;
        }
        AsyncResult<EzBuyResult> asyncResult = null;
        var current = gs2.Showcase.Buy(
          r => { asyncResult = r; },
          session: session,
          namespaceName: "BGM",
          showcaseName: "BGM",
          displayItemId: asyncResult2.Result.Item.DisplayItems.Find(m => m.SalesItem.Name == "BGM" + (num + 1 + (dan - 1) * 5)).DisplayItemId
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

        PlayerPrefs.SetInt("BGM" + (num + 1 + (dan - 1) * 5), 1);
        buttons[num].interactable = false;
        buttons[num].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "購入済み";
        toggles[num].isOn = true;
        toggles[num].interactable = true;
        SceneManager.LoadScene("BGM");
    }
    public void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
    public IEnumerator ButtonInteractive(int num)
    {
        {
            AsyncResult<EzGetItemWithSignatureResult> asyncResult = null;
            var current = gs2.Inventory.GetItemWithSignature(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "BGM",
              inventoryName: "BGM",
              keyId: "grn:gs2:ap-northeast-1:uFLAkqDK-Development:key:inventory-key:key:inventory-key",
              itemName: "BGM" + (num + 1 + (dan - 1) * 5)
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            if (asyncResult.Result.Items.Count != 0)
            {
                buttons[num].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "購入済み";
                if (PlayerPrefs.GetInt("BGM" + (num + 1 + (dan - 1) * 5), 0) == 1) toggles[num].isOn = true;
                toggles[num].interactable = true;
                yield break;
            }
        }
        int money = 0;
        {
            AsyncResult<EzGetResult> asyncResult = null;
            var current = gs2.Money.Get(
                  r => { asyncResult = r; },
                  session: session,
                  namespaceName: "money",
                  slot: 0
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            EzWallet ezWallet = asyncResult.Result.Item;
            money = ezWallet.Free + ezWallet.Paid;
        }
        if (money >= 500) buttons[num].interactable = true;
    }
    public void ToggleChange1(Boolean toggle)
    {
        if (toggle) PlayerPrefs.SetInt("BGM" + (0 + 1 + (dan - 1) * 5), 1);
        if (!toggle) PlayerPrefs.SetInt("BGM" + (0 + 1 + (dan - 1) * 5), 0);
    }
    public void ToggleChange2(Boolean toggle)
    {
        if (toggle) PlayerPrefs.SetInt("BGM" + (1 + 1 + (dan - 1) * 5), 1);
        if (!toggle) PlayerPrefs.SetInt("BGM" + (1 + 1 + (dan - 1) * 5), 0);
    }
    public void ToggleChange3(Boolean toggle)
    {
        if (toggle) PlayerPrefs.SetInt("BGM" + (2 + 1 + (dan - 1) * 5), 1);
        if (!toggle) PlayerPrefs.SetInt("BGM" + (2 + 1 + (dan - 1) * 5), 0);
    }
    public void ToggleChange4(Boolean toggle)
    {
        if (toggle) PlayerPrefs.SetInt("BGM" + (3 + 1 + (dan - 1) * 5), 1);
        if (!toggle) PlayerPrefs.SetInt("BGM" + (3 + 1 + (dan - 1) * 5), 0);
    }
    public void ToggleChange5(Boolean toggle)
    {
        if (toggle) PlayerPrefs.SetInt("BGM" + (4 + 1 + (dan - 1) * 5), 1);
        if (!toggle) PlayerPrefs.SetInt("BGM" + (4 + 1 + (dan - 1) * 5), 0);
    }
}
