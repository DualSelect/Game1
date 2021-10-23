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

public class TakeOverRequest : MonoBehaviour
{
    public InputField inputId;
    public InputField inputPass;
    public Text resultText;
    GameObject login;
    void Start()
    {
        login = GameObject.Find("Login");
    }
    public void TakeOverRequestButtunDown()
    {
        this.gameObject.GetComponent<Button>().interactable = false;
        StartCoroutine(TakeOver());
    }
    public IEnumerator TakeOver()
    {
        {
            GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
            var gs2 = login.GetComponent<LoginInitial>().GetClient();
            var session = login.GetComponent<LoginInitial>().GetSession();
            //Debug.Log("引継ぎ処理");
            AsyncResult<EzDoTakeOverResult> asyncResult = null;
            var current = gs2.Account.DoTakeOver(
                 r => { asyncResult = r; },
                 namespaceName: "account",
                 type: 1,
                 userIdentifier: inputId.text,
                 password: inputPass.text
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                this.gameObject.GetComponent<Button>().interactable = true;
                resultText.text = "ID/PASSが違います";
                yield break;
            }
            resultText.text = "成功しました";
            EzAccount account = asyncResult.Result.Item;
            PlayerPrefs.SetString("ID", account.UserId);
            PlayerPrefs.SetString("PASS", account.Password);
        }
    }
}
