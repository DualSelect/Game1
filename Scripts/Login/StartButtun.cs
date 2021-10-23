using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Gs2.Core;
using Gs2.Unity.Gs2Account.Model;
using Gs2.Unity.Gs2Account.Result;
using Gs2.Unity.Util;
using Gs2.Unity.Gs2Friend.Model;
using Gs2.Unity.Gs2Friend.Result;


public class StartButtun : MonoBehaviour
{
    public GameObject kiyakuRequest;
    GameObject login;
    void Start()
    {
        login = GameObject.Find("Login");
    }
    public void StartButtunDown()
    {
        this.gameObject.GetComponent<Button>().interactable = false;
        StartCoroutine(CheckProfile());
    }
    public IEnumerator CheckProfile()
    {
        GameObject.Find("Loading").GetComponent<Loading>().LoadingStart();
        //yield return GameObject.Find("Loading").GetComponent<Loading>().FadeIn();
        {
            //Debug.Log("プロフィールを確認");

            var gs2 = login.GetComponent<LoginInitial>().GetClient();
            var session = login.GetComponent<LoginInitial>().GetSession();
            AsyncResult<EzGetProfileResult> asyncResult = null;
            var current = gs2.Friend.GetProfile(
                  r => { asyncResult = r; },
                  session: session,
                  namespaceName: "friend"
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                GameObject.Find("Loading").GetComponent<Loading>().LoadingEnd();
                kiyakuRequest.SetActive(true);
                yield break;
            }
        }
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(1);

        PlayerPrefs.SetString("menu", "home");
        SceneManager.LoadScene("MainMenu");
    }
}
