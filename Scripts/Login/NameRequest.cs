using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Gs2.Core;
using Gs2.Unity.Gs2Friend.Result;

public class NameRequest : MonoBehaviour
{
    public InputField inputName;
    GameObject login;
    public GameObject attention;
    void Start()
    {
        login = GameObject.Find("Login");
    }
    public void NameRequestButtunDown()
    {
        if (inputName.text.Length < 2)
        {
            attention.SetActive(true);
        }
        else
        {
            this.gameObject.GetComponent<Button>().interactable = false;
            StartCoroutine(CreateProfile());
        }
    }
    public IEnumerator CreateProfile()
    {
        GameObject.Find("Loading").GetComponent<Loading>().LoadingStart();
        //yield return GameObject.Find("Loading").GetComponent<Loading>().FadeIn();
        {
            //Debug.Log("プロフィールを作成");

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
        }
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(1);
        PlayerPrefs.SetString("menu","home");
        SceneManager.LoadScene("MainMenu");
    }
    private void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
}