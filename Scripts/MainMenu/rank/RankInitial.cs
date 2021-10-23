using Gs2.Core;
using Gs2.Core.Exception;
using Gs2.Unity;
using Gs2.Unity.Gs2Friend.Model;
using Gs2.Unity.Gs2Friend.Result;
using Gs2.Unity.Gs2Inbox.Result;
using Gs2.Unity.Gs2Ranking.Result;
using Gs2.Unity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class RankInitial : MonoBehaviour
{
    GameObject login;
    Client gs2;
    GameSession session;
    public GameObject rankWindow;
    public GameObject content;
    public AssetReference m_Ref;
    public RankPrefab own;
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
        StartCoroutine(RankDisplay());
    }
    IEnumerator RankDisplay()
    {
        {
            AsyncResult<EzGetRankResult> asyncResult = null;
            var current = gs2.Ranking.GetRank(
                r => { asyncResult = r; },
                session: session,
                namespaceName: "ranking",
                categoryName: "rate",
                scorerUserId: PlayerPrefs.GetString("ID")
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            own.rank.text = asyncResult.Result.Item.Rank.ToString();
            own.rate.text = asyncResult.Result.Item.Score.ToString();
            StartCoroutine(RankImage(asyncResult.Result.Item.Score, own));
            StartCoroutine(RankPlayer(asyncResult.Result.Item.UserId, own));
        }
        {
            AsyncResult<EzGetRankingResult> asyncResult = null;
            var current = gs2.Ranking.GetRanking(
                r => { asyncResult = r; },
                session: session,
                namespaceName: "ranking",
                categoryName: "rate",
                limit: 100
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            var prefab = Addressables.LoadAssetAsync<GameObject>(m_Ref);
            yield return prefab;
            Vector2 sd = new Vector2(0, 150 * asyncResult.Result.Items.Count);
            content.GetComponent<RectTransform>().sizeDelta = sd;
            for (int i = 0; i < asyncResult.Result.Items.Count; i++)
            {
                Vector3 v = new Vector3(0, -100 - 150 * i, 0);
                GameObject gameObject = Instantiate(prefab.Result, v, Quaternion.identity);
                RankPrefab rankPrefab = gameObject.GetComponent<RankPrefab>();
                gameObject.transform.SetParent(content.transform, false);

                rankPrefab.rank.text = asyncResult.Result.Items[i].Rank.ToString();
                rankPrefab.rate.text = asyncResult.Result.Items[i].Score.ToString();
                StartCoroutine(RankImage(asyncResult.Result.Items[i].Score, rankPrefab));
                StartCoroutine(RankPlayer(asyncResult.Result.Items[i].UserId, rankPrefab));
            }
        }
    }
    IEnumerator RankImage(long rate,RankPrefab rankPrefab)
    {
        if (rate >= 1000)
        {
            var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック黒");
            yield return color;
            rankPrefab.rankImage.sprite = color.Result;
            rankPrefab.rankText.text = "Ⅰ";
        }
        if (rate <= 599)
        {
            var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック青");
            yield return color;
            rankPrefab.rankImage.sprite = color.Result;
            rankPrefab.rankText.text = "Ⅲ";
        }
        if (600 <= rate && rate <= 999)
        {
            if (600 <= rate && rate <= 699)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック青");
                yield return color;
                rankPrefab.rankImage.sprite = color.Result;
            }
            if (700 <= rate && rate <= 799)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック黄");
                yield return color;
                rankPrefab.rankImage.sprite = color.Result;
            }
            if (800 <= rate && rate <= 899)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック赤");
                yield return color;
                rankPrefab.rankImage.sprite = color.Result;
            }
            if (900 <= rate && rate <= 999)
            {
                var color = Addressables.LoadAssetAsync<Sprite>("奥義ストック黒");
                yield return color;
                rankPrefab.rankImage.sprite = color.Result;
            }
            long l = rate % 100;
            if (0 <= l && l <= 33)
            {
                rankPrefab.rankText.text = "Ⅲ";
            }
            if (34 <= l && l <= 66)
            {
                rankPrefab.rankText.text = "Ⅱ";
            }
            if (67 <= l && l <= 99)
            {
                rankPrefab.rankText.text = "Ⅰ";
            }
        }
    }
    IEnumerator RankPlayer(String id,RankPrefab rankPrefab)
    {
        AsyncResult<EzGetPublicProfileResult> asyncResult = null;              
        var current = gs2.Friend.GetPublicProfile(                       
              r => { asyncResult = r; },                                    
              namespaceName: "friend",
              userId:id
        );
        yield return current;                                           
        if (asyncResult.Error != null)                                   
        {
            OnError(asyncResult.Error);                                    
            yield break;                                                   
        }
        EzPublicProfile ezProfile = asyncResult.Result.Item;                      
        rankPrefab.player.text = ezProfile.PublicProfile;                          
    }
    public void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
    public void CloseRank()
    {
        rankWindow.SetActive(false);
    }
    public void OpenRank()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        rankWindow.SetActive(true);
    }
}
