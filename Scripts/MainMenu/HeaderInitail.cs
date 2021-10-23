using System;                       //exceptionを使うため必要
using System.Collections;           //Unity必須
using System.Collections.Generic;   //Unity必須
using UnityEngine;                  //Unity必須
using UnityEngine.UI;               //UI(text)を使うため必要
using Gs2.Core;                     //gs2必須
using Gs2.Unity.Util;               //gs2必須
using Gs2.Unity.Gs2Friend.Model;    //gs2のフレンドメソッドを使うため必要
using Gs2.Unity.Gs2Friend.Result;   //gs2のフレンドメソッドを使うため必要
using Gs2.Unity.Gs2Money;
using Gs2.Unity.Gs2Money.Model;    
using Gs2.Unity.Gs2Money.Result;
using Gs2.Unity.Gs2Experience.Model;
using Gs2.Unity.Gs2Experience.Result;
using Gs2.Unity.Gs2Ranking.Result;

public class HeaderInitail : MonoBehaviour
{
    public Text playerName;         //textオブジェクトPlayerNameをインスペクター上でアタッチ
    public Text freeStone;
    public Text payStone;
    public Text ratePoint;
    GameObject login;               //Loginオブジェクトを入れるインスタンスを用意
    void Start()
    {
        login = GameObject.Find("Login");   //Loginオブジェクトをloginに入れる
        StartCoroutine(MenuInitailize());   //コルーチンで関数を呼ぶ  
    }
 
    public IEnumerator MenuInitailize()
    {
        yield return GameObject.Find("AudioBGM").GetComponent<AudioController>().BGMChange("Minstrel2");
        var gs2 = login.GetComponent<LoginInitial>().GetClient();           //LoginオブジェクトのLoginInitialスクリプトのGetClientメソッドを呼んでgs2を取得
        var session = login.GetComponent<LoginInitial>().GetSession();      //LoginオブジェクトのLoginInitialスクリプトのGetSessionメソッドを呼んでsessionを取得
        {
            AsyncResult<EzGetProfileResult> asyncResult = null;                 //引数に必要なAsyncResult<EZ+メソッド名+Result>を宣言
            var current = gs2.Friend.GetProfile(                                //GetProfileメソッドを呼ぶ
                  r => { asyncResult = r; },                                    //引数1定型文
                  session: session,                                             //引数2session
                  namespaceName: "friend"                                       //引数3ネームスペース名friend
            );
            yield return current;                                               //定型文
            if (asyncResult.Error != null)                                      //エラーだった場合
            {
                OnError(asyncResult.Error);                                     //エラーメッセージを表示
                yield break;                                                    //MenuInitailizeメソッドをここで終了する
            }
            EzProfile ezProfile = asyncResult.Result.Item;                      //asyncResult.Result.ItemにGetProfileの結果が入る
            playerName.text = ezProfile.PublicProfile;                          //textオブジェクトPlayerNameに公開プロフィール(名前が入っている)を入れる
        }
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
            freeStone.text = ezWallet.Free.ToString("0");
            payStone.text = ezWallet.Paid.ToString("0");
        }
        long rate;
        {
            //Debug.Log("rateの取得");
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
            EzStatus ezStatus = asyncResult.Result.Item;
            rate = ezStatus.ExperienceValue;
            ratePoint.text = ezStatus.ExperienceValue.ToString("0");
        }
        GameObject.Find("Loading").GetComponent<Loading>().LoadingEnd();
        yield return GameObject.Find("Loading").GetComponent<Loading>().LoadingRamdom();
        {
            //Debug.Log("rateをscoreに");
            AsyncResult<EzPutScoreResult> asyncResult = null;
            var current = gs2.Ranking.PutScore(
                  r => { asyncResult = r; },
                  session: session,
                  namespaceName: "ranking",
                  categoryName: "rate",
                  score: rate
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
        }
    }
    private void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
}
