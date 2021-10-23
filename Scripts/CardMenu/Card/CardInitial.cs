using Gs2.Core;
using Gs2.Unity.Gs2Inventory.Model;
using Gs2.Unity.Gs2Inventory.Result;
using Gs2.Unity.Gs2Limit.Result;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardInitial : MonoBehaviour
{
    GameObject login;
    public CardMaster cardMaster;
    public AssetReference m_Ref;
    GameObject m_Result = null;
    public GameObject unitStatusWindow;
    public GameObject unitFlavorWindow;
    public GameObject filterWindow;
    public Toggle yesHave;
    public Toggle noHave;
    public Toggle le;
    public Toggle sr;
    public Toggle r;
    public Toggle n;
    public Toggle basic;
    public Toggle dan1;
    public Toggle dan2;
    public Toggle blue;
    public Toggle yellow;
    public Toggle red;
    public Toggle black;
    public Toggle none;
    public Display display;
    List<EzItemSet> basic1;
    List<EzItemSet> basic2;
    List<EzItemSet> d1;
    List<EzItemSet> d1_2;

    void Start()
    {
        login = GameObject.Find("Login");
        StartCoroutine(ListItem());
    }

    private IEnumerator ListItem()
    {
        var gs2 = login.GetComponent<LoginInitial>().GetClient();
        var session = login.GetComponent<LoginInitial>().GetSession();
        {
            AsyncResult<EzListItemsResult> asyncResult = null;
            var current = gs2.Inventory.ListItems(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "basic1",
              inventoryName: "basic1",
              limit:50
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

        yield return FilterSearch();
    }

    private IEnumerator FilterSearch()
    {
        var prefab = Addressables.LoadAssetAsync<GameObject>(m_Ref);
        yield return prefab;
        m_Result = prefab.Result;
        var cardlist = new List<Card>(cardMaster.CardList);
        var comp1 = new Comparison<Card>(C1);
        {
            if (!le.isOn)
            {
                while (true)
                {
                    int i = cardlist.FindIndex(m => m.rare == "LE");
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
            if (!sr.isOn)
            {
                while (true)
                {
                    int i = cardlist.FindIndex(m => m.rare == "SR");
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
            if (!r.isOn)
            {
                while (true)
                {
                    int i = cardlist.FindIndex(m => m.rare == "R");
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
            if (!n.isOn)
            {
                while (true)
                {
                    int i = cardlist.FindIndex(m => m.rare == "N");
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
            if (!basic.isOn)
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
            if (!dan1.isOn)
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
            if (!dan2.isOn)
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
            if (!blue.isOn)
            {
                while (true)
                {
                    int i = cardlist.FindIndex(m => m.color == "青");
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
            if (!yellow.isOn)
            {
                while (true)
                {
                    int i = cardlist.FindIndex(m => m.color == "黄");
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
            if (!red.isOn)
            {
                while (true)
                {
                    int i = cardlist.FindIndex(m => m.color == "赤");
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
            if (!black.isOn)
            {
                while (true)
                {
                    int i = cardlist.FindIndex(m => m.color == "黒");
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
            if (!none.isOn)
            {
                while (true)
                {
                    int i = cardlist.FindIndex(m => m.color == "無");
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
        }
        cardlist.Sort(comp1);
        GameObject content = GameObject.Find("Content");
        var transforms = content.GetComponentsInChildren<Transform>();
        var gameObjects = from t in transforms select t.gameObject;
        var cards = gameObjects.ToArray();
        for (int i = 1; i < cards.Length; i++) Destroy(cards[i]);
        Vector2 sd = new Vector2(0, 180 * ((cardlist.Count - 1) / 4 + 1));
        content.GetComponent<RectTransform>().sizeDelta = sd;
        int minus = 0;
        for (int i = 0; i < cardlist.Count; i++)
        {
            bool cont = false;

            if (!yesHave.isOn)
            {
                switch (cardlist[i].inventory)
                {
                    case "basic1":
                        if (basic1.Find(m => m.ItemName == cardlist[i].itemId) != null) cont = true;
                        break;
                    case "basic2":
                        if (basic2.Find(m => m.ItemName == cardlist[i].itemId) != null) cont = true;
                        break;
                    case "dan1":
                        if (d1.Find(m => m.ItemName == cardlist[i].itemId) != null) cont = true;
                        break;
                    case "dan1_2":
                        if (d1_2.Find(m => m.ItemName == cardlist[i].itemId) != null) cont = true;
                        break;
                    default:
                        break;
                }
            }
            if (cont)
            {
                minus++;
                continue;
            }
            if (!noHave.isOn)
            {
                switch (cardlist[i].inventory)
                {
                    case "basic1":
                        if (basic1.Find(m => m.ItemName == cardlist[i].itemId) == null) cont = true;
                        break;
                    case "basic2":
                        if (basic2.Find(m => m.ItemName == cardlist[i].itemId) == null) cont = true;
                        break;
                    case "dan1":
                        if (d1.Find(m => m.ItemName == cardlist[i].itemId) == null) cont = true;
                        break;
                    case "dan1_2":
                        if (d1_2.Find(m => m.ItemName == cardlist[i].itemId) != null) cont = true;
                        break;
                    default:
                        break;
                }
            }
            if (cont)
            {
                minus++;
                continue;
            }
            Vector3 v = new Vector3(-250 + ((i-minus) % 4) * 180, -90 - ((i - minus) / 4) * 180, 0);
            GameObject gameObject = Instantiate(m_Result, v, Quaternion.identity);
            gameObject.GetComponent<CardPrefab>().cardId = cardlist[i].itemId;
            gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
            gameObject.transform.SetParent(content.transform, false);
            switch (cardlist[i].inventory)
            {
                case "basic1":
                    if (basic1.Find(m => m.ItemName == cardlist[i].itemId) == null && cardlist[i].type != "魔法") gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    if (basic1.Find(m => m.ItemName == cardlist[i].itemId) == null && cardlist[i].type == "魔法") gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    break;
                case "basic2":
                    if (basic2.Find(m => m.ItemName == cardlist[i].itemId) == null && cardlist[i].type != "魔法") gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    if (basic2.Find(m => m.ItemName == cardlist[i].itemId) == null && cardlist[i].type == "魔法") gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    break;
                case "dan1":
                    if (d1.Find(m => m.ItemName == cardlist[i].itemId) == null && cardlist[i].type != "魔法") gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    if (d1.Find(m => m.ItemName == cardlist[i].itemId) == null && cardlist[i].type == "魔法") gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    break;
                case "dan1_2":
                    if (d1_2.Find(m => m.ItemName == cardlist[i].itemId) == null && cardlist[i].type != "魔法") gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    if (d1_2.Find(m => m.ItemName == cardlist[i].itemId) == null && cardlist[i].type == "魔法") gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    break;
                default:
                    if (cardlist[i].type != "魔法") gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    if (cardlist[i].type == "魔法") gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    break;
            }
            StartCoroutine(display.CardDisplay(cardlist[i], gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>()));
            StartCoroutine(DisplayLV(cardlist[i].level, gameObject.transform.GetChild(2).gameObject.GetComponent<Image>()));
        }
        filterWindow.SetActive(false);
    }
    private IEnumerator DisplayLV(int lv,Image image)
    {
        var unit = Addressables.LoadAssetAsync<Sprite>("サンセリフホワイト48_"+lv);
        yield return unit;
        image.sprite = unit.Result;
    }
    static int C1(Card a,Card b)
    {
        int x = 0, y = 0;
        if (a.rare == "LE") x = x + 1000;
        if (a.rare == "SR") x = x + 2000;
        if (a.rare == "R") x = x + 3000;
        if (a.rare == "N") x = x + 4000;
        if (b.rare == "LE") y = y + 1000;
        if (b.rare == "SR") y = y + 2000;
        if (b.rare == "R") y = y + 3000;
        if (b.rare == "N") y = y + 4000;
        if (a.pack == "ベーシック") x = x + 100;
        if (a.pack == "1段") x = x + 200;
        if (a.pack == "2段") x = x + 300;
        if (a.pack == "3段") x = x + 400;
        if (a.pack == "4段") x = x + 500;
        if (a.pack == "5段") x = x + 600;
        if (b.pack == "ベーシック") y = y + 100;
        if (b.pack == "1段") y = y + 200;
        if (b.pack == "2段") y = y + 300;
        if (b.pack == "3段") y = y + 400;
        if (b.pack == "4段") y = y + 500;
        if (b.pack == "5段") y = y + 600;
        if (a.color == "無") x = x + 10;
        if (a.color == "青") x = x + 20;
        if (a.color == "黄") x = x + 30;
        if (a.color == "赤") x = x + 40;
        if (a.color == "黒") x = x + 50;
        if (b.color == "無") y = y + 10;
        if (b.color == "青") y = y + 20;
        if (b.color == "黄") y = y + 30;
        if (b.color == "赤") y = y + 40;
        if (b.color == "黒") y = y + 50;
        x = x + a.level;
        y = y + b.level;
        return x - y;
    }
    public void FilterSearchDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(FilterSearch());
    }
    public void FilterCloseDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        filterWindow.SetActive(false);
    }
    public void FilterButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        filterWindow.SetActive(true);
    }
    public void FilterResetDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        yesHave.isOn = true;
        noHave.isOn = true;
        le.isOn = true;
        sr.isOn = true;
        r.isOn = true;
        n.isOn = true;
        basic.isOn = true;
        dan1.isOn = true;
        blue.isOn = true;
        yellow.isOn = true;
        red.isOn = true;
        black.isOn = true;
        none.isOn = true;
    }
    public void BackButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetString("menu", "card");
        SceneManager.LoadScene("MainMenu");
    }
    public void ShieldButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        SceneManager.LoadScene("Shield");
    }
    private void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
    public IEnumerator CardWin(Card card,Text cardWin,Button aibouButton)
    {
        var gs2 = login.GetComponent<LoginInitial>().GetClient();
        var session = login.GetComponent<LoginInitial>().GetSession();
        string nameSpace = null;

        switch (card.inventory)
        {
            case "basic1":
                nameSpace = "basic1";
                break;
            case "basic2":
                nameSpace = "basic2";
                break;
            case "d1":
                nameSpace = "dan1";
                break;
            case "d1_2":
                nameSpace = "dan1_2";
                break;
            default:
                break;
        }

        AsyncResult<EzGetCounterResult> asyncResult = null;
        var current = gs2.Limit.GetCounter(
          r => { asyncResult = r; },
          session: session,
          namespaceName: nameSpace,
          limitName:card.itemId,
          counterName:card.itemId
        );
        yield return current;
        if (asyncResult.Error != null)
        {
            OnError(asyncResult.Error);
            yield break;
        }
        cardWin.text = asyncResult.Result.Item.Count.ToString();
        //if (asyncResult.Result.Item.Count > 49) aibouButton.interactable = true;
    }
}
