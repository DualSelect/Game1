using Gs2.Core;
using Gs2.Unity;
using Gs2.Unity.Gs2Inventory.Model;
using Gs2.Unity.Gs2Inventory.Result;
using Gs2.Unity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ExchangeList : MonoBehaviour
{
    public GameObject exchangeWindow;
    public GameObject content;
    public AssetReference m_Ref;
    public CardMaster cardMaster;
    public ShieldMaster shieldMaster;
    GameObject login;
    Client gs2;
    GameSession session;
    List<EzItemSet> basic1;
    List<EzItemSet> basic2;
    List<EzItemSet> d1;
    List<EzItemSet> d1_2;
    public HeaderInitail headerInitail;

    public Button basicEx;
    public Button d1Ex;
    void Start()
    {
        login = GameObject.Find("Login");
        gs2 = login.GetComponent<LoginInitial>().GetClient();
        session = login.GetComponent<LoginInitial>().GetSession();
        StartCoroutine(InventoryLoad());
    }
    private IEnumerator InventoryLoad()
    {
        {
            AsyncResult<EzListItemsResult> asyncResult = null;
            var current = gs2.Inventory.ListItems(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "basic1",
              inventoryName: "basic1",
              limit: 50
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            basic1 = asyncResult.Result.Items;
        }
        {
            AsyncResult<EzListItemsResult> asyncResult = null;
            var current = gs2.Inventory.ListItems(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "basic2",
              inventoryName: "basic2",
              limit: 50
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            basic2 = asyncResult.Result.Items;
        }
        {
            AsyncResult<EzListItemsResult> asyncResult = null;
            var current = gs2.Inventory.ListItems(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "dan1",
              inventoryName: "dan1",
              limit: 50
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            d1 = asyncResult.Result.Items;
        }
        {
            AsyncResult<EzListItemsResult> asyncResult = null;
            var current = gs2.Inventory.ListItems(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "dan1_2",
              inventoryName: "dan1_2",
              limit: 50
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            d1_2 = asyncResult.Result.Items;
        }
        basicEx.interactable = true;
        d1Ex.interactable = true;
    }

    public void ExchangeListBasic()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        var cardlist = CardList(0);
        var shieldlist = ShieldList(0);
        StartCoroutine(ExchageContent(cardlist, shieldlist));
    }
    public void ExchangeListDan1()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        var cardlist = CardList(1);
        var shieldlist = ShieldList(1);
        StartCoroutine(ExchageContent(cardlist, shieldlist));
    }

    private IEnumerator ExchageContent(List<Card> cardlist , List<Shield> shieldlist)
    {
        exchangeWindow.SetActive(true);
        var prefab = Addressables.LoadAssetAsync<GameObject>(m_Ref);
        var transforms = content.GetComponentsInChildren<Transform>();
        var gameObjects = from t in transforms select t.gameObject;
        var cards = gameObjects.ToArray();
        for (int a = 1; a < cards.Length; a++) Destroy(cards[a]);
        Vector2 sd = new Vector2(0, 155 * (cardlist.Count+shieldlist.Count));
        content.GetComponent<RectTransform>().sizeDelta = sd;
        yield return prefab;
        int money = int.Parse(headerInitail.payStone.text) + int.Parse(headerInitail.freeStone.text);
        int i=0;
        for (i = 0; i < cardlist.Count; i++)
        {

            Vector3 v = new Vector3(0, -75 - 150 * i, 0);
            GameObject gameObject = Instantiate(prefab.Result, v, Quaternion.identity);
            ExchangePrefab exchangePrefab = gameObject.GetComponent<ExchangePrefab>();
            exchangePrefab.cardId = cardlist[i].itemId;
            exchangePrefab.rare.text = cardlist[i].rare;
            exchangePrefab.name.text = cardlist[i].name;
            switch (cardlist[i].inventory)
            {
                case "basic1":
                    if (basic1.Find(m => m.ItemName == cardlist[i].itemId) == null)
                    {
                        if (cardlist[i].rare == "LE")
                        {
                            exchangePrefab.button.text = "購入/ 400石";
                            if (money >= 400) exchangePrefab.exchange.interactable = true;
                        }
                        if (cardlist[i].rare == "SR")
                        {
                            exchangePrefab.button.text = "購入/ 300石";
                            if (money >= 300) exchangePrefab.exchange.interactable = true;
                        }
                        if (cardlist[i].rare == "R")
                        {
                            exchangePrefab.button.text = "購入/ 200石";
                            if (money >= 200) exchangePrefab.exchange.interactable = true;
                        }
                        if (cardlist[i].rare == "N")
                        {
                            exchangePrefab.button.text = "購入/ 100石";
                            if (money >= 100) exchangePrefab.exchange.interactable = true;
                        }
                    }
                    else
                    {
                        exchangePrefab.button.text = "所持済";
                    }
                    break;
                case "basic2":
                    if (basic2.Find(m => m.ItemName == cardlist[i].itemId) == null)
                    {
                        if (cardlist[i].rare == "LE")
                        {
                            exchangePrefab.button.text = "購入/ 400石";
                            if (money >= 400) exchangePrefab.exchange.interactable = true;
                        }
                        if (cardlist[i].rare == "SR")
                        {
                            exchangePrefab.button.text = "購入/ 300石";
                            if (money >= 300) exchangePrefab.exchange.interactable = true;
                        }
                        if (cardlist[i].rare == "R")
                        {
                            exchangePrefab.button.text = "購入/ 200石";
                            if (money >= 200) exchangePrefab.exchange.interactable = true;
                        }
                        if (cardlist[i].rare == "N")
                        {
                            exchangePrefab.button.text = "購入/ 100石";
                            if (money >= 100) exchangePrefab.exchange.interactable = true;
                        }
                    }
                    else
                    {
                        exchangePrefab.button.text = "所持済";
                    }
                    break;
                case "dan1":
                    if (d1.Find(m => m.ItemName == cardlist[i].itemId) == null)
                    {
                        if (cardlist[i].rare == "LE")
                        {
                            exchangePrefab.button.text = "購入/ 400石";
                            if (money >= 400) exchangePrefab.exchange.interactable = true;
                        }
                        if (cardlist[i].rare == "SR")
                        {
                            exchangePrefab.button.text = "購入/ 300石";
                            if (money >= 300) exchangePrefab.exchange.interactable = true;
                        }
                        if (cardlist[i].rare == "R")
                        {
                            exchangePrefab.button.text = "購入/ 200石";
                            if (money >= 200) exchangePrefab.exchange.interactable = true;
                        }
                        if (cardlist[i].rare == "N")
                        {
                            exchangePrefab.button.text = "購入/ 100石";
                            if (money >= 100) exchangePrefab.exchange.interactable = true;
                        }
                    }
                    else
                    {
                        exchangePrefab.button.text = "所持済";
                    }
                    break;
                case "dan1_2":
                    if (d1_2.Find(m => m.ItemName == cardlist[i].itemId) == null)
                    {
                        if (cardlist[i].rare == "LE")
                        {
                            exchangePrefab.button.text = "購入/ 400石";
                            if (money >= 400) exchangePrefab.exchange.interactable = true;
                        }
                        if (cardlist[i].rare == "SR")
                        {
                            exchangePrefab.button.text = "購入/ 300石";
                            if (money >= 300) exchangePrefab.exchange.interactable = true;
                        }
                        if (cardlist[i].rare == "R")
                        {
                            exchangePrefab.button.text = "購入/ 200石";
                            if (money >= 200) exchangePrefab.exchange.interactable = true;
                        }
                        if (cardlist[i].rare == "N")
                        {
                            exchangePrefab.button.text = "購入/ 100石";
                            if (money >= 100) exchangePrefab.exchange.interactable = true;
                        }
                    }
                    else
                    {
                        exchangePrefab.button.text = "所持済";
                    }
                    break;
            }
            gameObject.transform.SetParent(content.transform, false);
        }
        for (int j = 0; j < shieldlist.Count; j++)
        {

            Vector3 v = new Vector3(0, -75 - 150 * (i+j), 0);
            GameObject gameObject = Instantiate(prefab.Result, v, Quaternion.identity);
            ExchangePrefab exchangePrefab = gameObject.GetComponent<ExchangePrefab>();
            exchangePrefab.cardId = shieldlist[j].itemId;
            exchangePrefab.rare.text = shieldlist[j].rare;
            exchangePrefab.name.text = shieldlist[j].name;
            switch (shieldlist[j].inventory)
            {
                case "basic1":
                    if (basic1.Find(m => m.ItemName == shieldlist[j].itemId) == null)
                    {
                        if (shieldlist[j].rare == "LE")
                        {
                            exchangePrefab.button.text = "購入/ 400石";
                            if (money >= 400) exchangePrefab.exchange.interactable = true;
                        }
                        if (shieldlist[j].rare == "SR")
                        {
                            exchangePrefab.button.text = "購入/ 300石";
                            if (money >= 300) exchangePrefab.exchange.interactable = true;
                        }
                        if (shieldlist[j].rare == "R")
                        {
                            exchangePrefab.button.text = "購入/ 200石";
                            if (money >= 200) exchangePrefab.exchange.interactable = true;
                        }
                        if (shieldlist[j].rare == "N")
                        {
                            exchangePrefab.button.text = "購入/ 100石";
                            if (money >= 100) exchangePrefab.exchange.interactable = true;
                        }
                    }
                    else
                    {
                        exchangePrefab.button.text = "所持済";
                    }
                    break;
                case "basic2":
                    if (basic2.Find(m => m.ItemName == shieldlist[j].itemId) == null)
                    {
                        if (shieldlist[j].rare == "LE")
                        {
                            exchangePrefab.button.text = "購入/ 400石";
                            if (money >= 400) exchangePrefab.exchange.interactable = true;
                        }
                        if (shieldlist[j].rare == "SR")
                        {
                            exchangePrefab.button.text = "購入/ 300石";
                            if (money >= 300) exchangePrefab.exchange.interactable = true;
                        }
                        if (shieldlist[j].rare == "R")
                        {
                            exchangePrefab.button.text = "購入/ 200石";
                            if (money >= 200) exchangePrefab.exchange.interactable = true;
                        }
                        if (shieldlist[j].rare == "N")
                        {
                            exchangePrefab.button.text = "購入/ 100石";
                            if (money >= 100) exchangePrefab.exchange.interactable = true;
                        }
                    }
                    else
                    {
                        exchangePrefab.button.text = "所持済";
                    }
                    break;
                case "dan1":
                    if (d1.Find(m => m.ItemName == shieldlist[j].itemId) == null)
                    {
                        if (shieldlist[j].rare == "LE")
                        {
                            exchangePrefab.button.text = "購入/ 400石";
                            if (money >= 400) exchangePrefab.exchange.interactable = true;
                        }
                        if (shieldlist[j].rare == "SR")
                        {
                            exchangePrefab.button.text = "購入/ 300石";
                            if (money >= 300) exchangePrefab.exchange.interactable = true;
                        }
                        if (shieldlist[j].rare == "R")
                        {
                            exchangePrefab.button.text = "購入/ 200石";
                            if (money >= 200) exchangePrefab.exchange.interactable = true;
                        }
                        if (shieldlist[j].rare == "N")
                        {
                            exchangePrefab.button.text = "購入/ 100石";
                            if (money >= 100) exchangePrefab.exchange.interactable = true;
                        }
                    }
                    else
                    {
                        exchangePrefab.button.text = "所持済";
                    }
                    break;
                case "dan1_2":
                    if (d1_2.Find(m => m.ItemName == shieldlist[j].itemId) == null)
                    {
                        if (shieldlist[j].rare == "LE")
                        {
                            exchangePrefab.button.text = "購入/ 400石";
                            if (money >= 400) exchangePrefab.exchange.interactable = true;
                        }
                        if (shieldlist[j].rare == "SR")
                        {
                            exchangePrefab.button.text = "購入/ 300石";
                            if (money >= 300) exchangePrefab.exchange.interactable = true;
                        }
                        if (shieldlist[j].rare == "R")
                        {
                            exchangePrefab.button.text = "購入/ 200石";
                            if (money >= 200) exchangePrefab.exchange.interactable = true;
                        }
                        if (shieldlist[j].rare == "N")
                        {
                            exchangePrefab.button.text = "購入/ 100石";
                            if (money >= 100) exchangePrefab.exchange.interactable = true;
                        }
                    }
                    else
                    {
                        exchangePrefab.button.text = "所持済";
                    }
                    break;
            }
            gameObject.transform.SetParent(content.transform, false);
        }

    }

    private List<Card> CardList(int dan)
    {
        var cardlist = new List<Card>(cardMaster.CardList);
        if (dan!=0)
        {
            while (true)
            {
                int i = cardlist.FindIndex(m => m.pack == "ベーシック");
                if (i == -1)
                {
                    break;
                }
                else
                {
                    cardlist.RemoveAt(i);
                }
            }
        }
        if (dan != 1)
        {
            while (true)
            {
                int i = cardlist.FindIndex(m => m.pack == "1弾");
                if (i == -1)
                {
                    break;
                }
                else
                {
                    cardlist.RemoveAt(i);
                }
            }
        }
        if (dan != 2)
        {
            while (true)
            {
                int i = cardlist.FindIndex(m => m.pack == "2弾");
                if (i == -1)
                {
                    break;
                }
                else
                {
                    cardlist.RemoveAt(i);
                }
            }
        }
        return cardlist;
    }
    private List<Shield> ShieldList(int dan)
    {
        var shieldlist = new List<Shield>(shieldMaster.ShieldList);
        if (dan != 0)
        {
            while (true)
            {
                int i = shieldlist.FindIndex(m => m.pack == "ベーシック");
                if (i == -1)
                {
                    break;
                }
                else
                {
                    shieldlist.RemoveAt(i);
                }
            }
        }
        if (dan != 1)
        {
            while (true)
            {
                int i = shieldlist.FindIndex(m => m.pack == "1弾");
                if (i == -1)
                {
                    break;
                }
                else
                {
                    shieldlist.RemoveAt(i);
                }
            }
        }
        if (dan != 2)
        {
            while (true)
            {
                int i = shieldlist.FindIndex(m => m.pack == "2弾");
                if (i == -1)
                {
                    break;
                }
                else
                {
                    shieldlist.RemoveAt(i);
                }
            }
        }
        return shieldlist;
    }
    public void ExchangeListClose()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        exchangeWindow.SetActive(false);
    }
    private void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
}
