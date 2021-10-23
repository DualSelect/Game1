using Gs2.Core;
using Gs2.Unity.Gs2Inventory.Model;
using Gs2.Unity.Gs2Inventory.Result;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShieldInitial : MonoBehaviour
{
    GameObject login;
    public ShieldMaster shieldMaster ;
    public AssetReference m_Ref;
    GameObject m_Result = null;
    public GameObject shieldStatusWindow;
    public GameObject shieldFlavorWindow;
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
    public Toggle life1;
    public Toggle life2;
    public Toggle life3;
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
        yield return FilterSearch();
    }

    private IEnumerator FilterSearch()
    {
        var prefab = Addressables.LoadAssetAsync<GameObject>(m_Ref);
        yield return prefab;
        m_Result = prefab.Result;
        var cardlist = new List<Shield>(shieldMaster.ShieldList);
        var comp1 = new Comparison<Shield>(C1);
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
            if (!life1.isOn)
            {
                while (true)
                {
                    int i = cardlist.FindIndex(m => m.life == 1);
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
            if (!life2.isOn)
            {
                while (true)
                {
                    int i = cardlist.FindIndex(m => m.life == 2);
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
            if (!life3.isOn)
            {
                while (true)
                {
                    int i = cardlist.FindIndex(m => m.life == 3);
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
            Vector3 v = new Vector3(-250 + ((i - minus) % 4) * 180, -90 - ((i - minus) / 4) * 180, 0);
            GameObject gameObject = Instantiate(m_Result, v, Quaternion.identity);
            gameObject.GetComponent<ShieldPrefab>().shieldId = cardlist[i].itemId;
            gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
            gameObject.transform.SetParent(content.transform, false);
            StartCoroutine(display.ShieldDisplay(cardlist[i],gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>()));
            gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = cardlist[i].life.ToString();
            switch (cardlist[i].inventory)
            {
                case "basic1":
                    if (basic1.Find(m => m.ItemName == cardlist[i].itemId) == null) gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    break;
                case "basic2":
                    if (basic2.Find(m => m.ItemName == cardlist[i].itemId) == null) gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    break;
                case "dan1":
                    if (d1.Find(m => m.ItemName == cardlist[i].itemId) == null) gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    break;
                case "dan1_2":
                    if (d1_2.Find(m => m.ItemName == cardlist[i].itemId) == null) gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    break;
                default:
                    gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                    break;
            }
        }
        filterWindow.SetActive(false);
    }
    static int C1(Shield a, Shield b)
    {
        int x = 0, y = 0;
        if (a.rare == "LE") x = x + 100;
        if (a.rare == "SR") x = x + 200;
        if (a.rare == "R") x = x + 300;
        if (a.rare == "N") x = x + 400;
        if (b.rare == "LE") y = y + 100;
        if (b.rare == "SR") y = y + 200;
        if (b.rare == "R") y = y + 300;
        if (b.rare == "N") y = y + 400;
        if (a.pack == "ベーシック") x = x + 10;
        if (a.pack == "1段") x = x + 20;
        if (a.pack == "2段") x = x + 30;
        if (a.pack == "3段") x = x + 40;
        if (a.pack == "4段") x = x + 50;
        if (a.pack == "5段") x = x + 60;
        if (b.pack == "ベーシック") y = y + 10;
        if (b.pack == "1段") y = y + 20;
        if (b.pack == "2段") y = y + 30;
        if (b.pack == "3段") y = y + 40;
        if (b.pack == "4段") y = y + 50;
        if (b.pack == "5段") y = y + 60;
        x = x + a.life;
        y = y + b.life;
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
        life1.isOn = true;
        life2.isOn = true;
        life3.isOn = true;
    }
    public void BackButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetString("menu", "card");
        SceneManager.LoadScene("MainMenu");
    }
    public void CardButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        SceneManager.LoadScene("Card");
    }
    private void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
}
