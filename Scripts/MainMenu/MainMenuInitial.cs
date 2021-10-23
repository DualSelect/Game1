using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gs2.Unity.Gs2Experience.Model;
using Gs2.Unity.Gs2Experience.Result;
using Gs2.Core;
using System;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class MainMenuInitial : MonoBehaviour
{
    GameObject login;
    public Image rankImage;
    public Text rankText;
    public Image blue;
    public Image yellow;
    public Image red;
    public Image black;
    public Image aibou;
    void Start()
    {

        login = GameObject.Find("Login");
        StartCoroutine(Rank());
        StartCoroutine(Aibou());
    }
    private IEnumerator Aibou()
    {
        var unit = Addressables.LoadAssetAsync<Sprite>(PlayerPrefs.GetString("aibou","dummy"));
        yield return unit;
        aibou.sprite = unit.Result;
    }
    private IEnumerator Rank()
    {
        var gs2 = login.GetComponent<LoginInitial>().GetClient();
        var session = login.GetComponent<LoginInitial>().GetSession();
        AsyncResult<EzGetStatusResult> asyncResult = null;
        var current = gs2.Experience.GetStatus(
                r => { asyncResult = r; },
                session: session,
                namespaceName: "experience",
                experienceName: "ratingRank",
                propertyId: "rate"
        );
        yield return current;
        if (asyncResult.Error != null)
        {
            OnError(asyncResult.Error);
            yield break;
        }
        long rate = asyncResult.Result.Item.ExperienceValue;

        if (rate >= 1000)
        {
            var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック黒" );
            yield return color;
            rankImage.sprite = color.Result;
            rankText.text = "Ⅰ";
        }
        if(rate <= 599)
        {
            var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック青");
            yield return color;
            rankImage.sprite = color.Result;
            rankText.text = "Ⅲ";
        }
        if(600 <= rate &&  rate <= 999)
        {
            if(600 <= rate && rate <= 699)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック青");
                yield return color;
                rankImage.sprite = color.Result;
            }
            if (700 <= rate && rate <= 799)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック黄");
                yield return color;
                rankImage.sprite = color.Result;
                yellow.color = new Color(0f, 0f, 0f);
            }
            if (800 <= rate && rate <= 899)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック赤");
                yield return color;
                rankImage.sprite = color.Result;
                yellow.color = new Color(0f, 0f, 0f);
                red.color = new Color(0f, 0f, 0f);
            }
            if (900 <= rate && rate <= 999)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック黒");
                yield return color;
                rankImage.sprite = color.Result;
                yellow.color = new Color(0f, 0f, 0f);
                red.color = new Color(0f, 0f, 0f);
                black.color = new Color(0f, 0f, 0f);
            }
            long l = rate % 100;
            if(0<=l && l <= 33)
            {
                rankText.text = "Ⅲ";
            }
            if (34 <= l && l <= 66)
            {
                rankText.text = "Ⅱ";
            }
            if (67 <= l && l <= 99)
            {
                rankText.text = "Ⅰ";
            }
        }
    }
    private void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
}
