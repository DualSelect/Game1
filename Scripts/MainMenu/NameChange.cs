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

public class NameChange : MonoBehaviour
{
    public Text playerName;
    public InputField inputName;
    GameObject login;
    public GameObject nameChange;
    void Start()
    {
        login = GameObject.Find("Login");
    }
    public void NameChangeButtunDown()
    {
        StartCoroutine(DoNameChange());
    }
    public IEnumerator DoNameChange()
    {
        {
            GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
            //Debug.Log("名前を変更");

            var gs2 = login.GetComponent<LoginInitial>().GetClient();
            var session = login.GetComponent<LoginInitial>().GetSession();
            AsyncResult<EzUpdateProfileResult> asyncResult = null;
            var current = gs2.Friend.UpdateProfile(
                  r => { asyncResult = r; },
                  session: session,
                  namespaceName: "friend",
                  publicProfile: inputName.text,
                  followerProfile: "",
                  friendProfile: ""
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            playerName.text = inputName.text;
        }
        nameChange.SetActive(false);
    }
    private void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }


}
