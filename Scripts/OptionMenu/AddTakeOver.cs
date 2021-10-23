using Gs2.Core;
using Gs2.Unity.Gs2Account.Result;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddTakeOver : MonoBehaviour
{
    public GameObject window;
    public GameObject window2;
    public Text tid;
    public Text tpass;
    GameObject login;
    void Start()
    {
        login = GameObject.Find("Login");
    }


    public void Close()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        window.SetActive(false);
        window2.SetActive(false);
    }
    public void Window()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        window.SetActive(true);
    }
    public void TakeOver()
    {
        StartCoroutine(TakeOver2());
    }
    public IEnumerator TakeOver2()
    {
        var gs2 = login.GetComponent<LoginInitial>().GetClient();
        var session = login.GetComponent<LoginInitial>().GetSession();

        string id = Guid.NewGuid().ToString("N").Substring(0, 8);
        string pass = Guid.NewGuid().ToString("N").Substring(0, 8);



        AsyncResult<EzAddTakeOverSettingResult> asyncResult = null;
        var current = gs2.Account.AddTakeOverSetting(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "account",
              type: 1,
              userIdentifier:id,
              password:pass
        );
        yield return current;
        if (asyncResult.Error != null)
        {
            OnError(asyncResult.Error);
            yield break;
        }
        Debug.Log(asyncResult.Result.Item.UserId);
        Debug.Log(asyncResult.Result.Item.UserIdentifier);
        tid.text = "ID:"+id;
        tpass.text = "PASS:"+pass;

        window.SetActive(false);
        window2.SetActive(true);
    }
    private void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
}
