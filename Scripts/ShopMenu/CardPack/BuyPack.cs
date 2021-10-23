using Gs2.Core;
using Gs2.Core.Exception;
using Gs2.Gs2Lottery.Result;
using Gs2.Unity;
using Gs2.Unity.Gs2JobQueue.Result;
using Gs2.Unity.Gs2Lottery.Result;
using Gs2.Unity.Gs2Money.Model;
using Gs2.Unity.Gs2Money.Result;
using Gs2.Unity.Gs2Showcase.Result;
using Gs2.Unity.Util;
using Gs2.Util.LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BuyPack : MonoBehaviour
{
    GameObject login;
    public string boxName;
    public string one;
    public string ten;
    public Button oneButton;
    public Button tenButton;
    public Text remain;
    Client gs2;
    GameSession session;
    public GameObject percentWindow;
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
        StartCoroutine(GameObject.Find("Loading").GetComponent<Loading>().LoadingRamdom());
        StartCoroutine(ButtonInteractive());
    }
    public void Buy1()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(Buy(one));
    }
    public void Buy10()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(Buy(ten));
    }

    private IEnumerator Buy(string productId)
    {
        GameObject.Find("Loading").GetComponent<Loading>().LoadingStart();


        AsyncResult<EzGetShowcaseResult> asyncResult2 = null;
        var current2 = gs2.Showcase.GetShowcase(
          r => { asyncResult2 = r; },
          session: session,
          namespaceName: "gacha",
          showcaseName: "gacha"
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
          namespaceName: "gacha",
          showcaseName: "gacha",
          displayItemId: asyncResult2.Result.Item.DisplayItems.Find(m => m.SalesItem.Name == productId).DisplayItemId
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

        machine.OnCompleteStampSheet.AddListener(Machine_OnCompleteStampSheet);
        yield return machine.Execute(m_events);
    }

    private void Machine_OnCompleteStampSheet(EzStampSheet sheet, Gs2.Unity.Gs2Distributor.Result.EzRunStampSheetResult result)
    {
        var jsonData = JsonMapper.ToObject(result.Result);
        var jsonResult = DrawByUserIdResult.FromDict(jsonData);
        PlayerPrefs.SetInt("PackNum", jsonResult.items.Count);
        for (int i = 0; i < jsonResult.items.Count; i++)
        {
            Debug.Log(jsonResult.items[i].acquireActions[0].request);
            itemId itemId = ToClass(jsonResult.items[i].acquireActions[0].request);
            PlayerPrefs.SetString("Pack" + (i+1), itemId.itemName);
        }
        StartCoroutine(GetCard(jsonResult.stampSheet));
    }
    private IEnumerator GetCard(string stampSheet)
    {
        Debug.Log(stampSheet);
        var machine = new StampSheetStateMachine(stampSheet, gs2, "distributor", "grn:gs2:ap-northeast-1:uFLAkqDK-Development:key:lottery_key:key:lottery_key");
        yield return machine.Execute(m_events);
        yield return JobQueue();
        SceneManager.LoadScene("CardPackResult");
    }

    public void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }

    public void PercentOpen()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        percentWindow.SetActive(true);
    }
    public void PercentClose()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        percentWindow.SetActive(false);
    }
    public itemId ToClass(string str)
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(itemId));
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        MemoryStream ms = new MemoryStream(bytes);
        return (itemId)serializer.ReadObject(ms);
    }
    [System.Runtime.Serialization.DataContract]
    public class itemId
    {
        [System.Runtime.Serialization.DataMember()]
        public string itemName { get; set; }
    }
    public IEnumerator JobQueue()
    {
        while (true)
        {
            AsyncResult<EzRunResult> asyncResult = null;

            var current = gs2.JobQueue.Run(
                        r => asyncResult = r,
                        session,
                        "queue"
                    );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            if (asyncResult.Result.IsLastJob) break;
        }
    }
    public IEnumerator ButtonInteractive()
    {
        int num=0;
        {
            AsyncResult<EzDescribeBoxesResult> asyncResult = null;
            var current = gs2.Lottery.DescribeBoxes(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "lottery_card"
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            int i;
            bool ok=false;
            for (i = 0; i < asyncResult.Result.Items.Count; i++)
            {
                if (asyncResult.Result.Items[i].PrizeTableName == boxName)
                {
                    ok = true;
                    break;
                }
            }
            if(ok && boxName == "basic")num = 100 - asyncResult.Result.Items[i].DrawnIndexes.Count;
            if (!ok && boxName == "basic") num = 100;
            if (ok && boxName == "dan1") num = 54 - asyncResult.Result.Items[i].DrawnIndexes.Count;
            if (!ok && boxName == "dan1") num = 54;
        }
        remain.text = "残り" + num + "枚";
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

        if (num >= 10 && money >= 1000) tenButton.interactable = true;
        if (num >= 1 && money >= 100) oneButton.interactable = true;
    }
}
