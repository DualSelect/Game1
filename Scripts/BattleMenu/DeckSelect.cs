using Gs2.Core;
using Gs2.Unity;
using Gs2.Unity.Gs2Datastore.Result;
using Gs2.Unity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckSelect : MonoBehaviour
{
    GameObject login;
    Client gs2;
    GameSession session;
    int load = 0;
    public Dropdown dropdown;
    void Start()
    {
        login = GameObject.Find("Login");
        gs2 = login.GetComponent<LoginInitial>().GetClient();
        session = login.GetComponent<LoginInitial>().GetSession();
        for (int i = 0; i < 30; i++)
        {
            StartCoroutine(DeckDropDown(i));
        }
    }
    private IEnumerator DeckDropDown(int i)
    {
        AsyncResult<EzPrepareDownloadOwnDataResult> asyncResult = null;
        var current = gs2.Datastore.PrepareDownloadOwnData(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "datastore",
              dataObjectName: "deck" + (i + 1).ToString()
        );
        yield return current;
        while (true) {
            yield return new WaitForSeconds(0.1f);
            if (i == load) break;
        }

        if (asyncResult.Error == null)
        {
            UnityWebRequest www = UnityWebRequest.Get(asyncResult.Result.FileUrl);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                dropdown.options.Add(new Dropdown.OptionData { text = (i + 1).ToString() + ":" + "読込エラー" });
            }
            else
            {
                dropdown.options.Add(new Dropdown.OptionData { text = (i + 1).ToString() + ":" + www.downloadHandler.text });
            }
        }
        else
        {
            dropdown.options.Add(new Dropdown.OptionData { text = (i + 1).ToString() + ":" + "使用不可" });
        }
        load++;
        if (load == 29)
        {
            dropdown.RefreshShownValue();
            dropdown.value = PlayerPrefs.GetInt("BattleDeck");
        }
    }
    public void DeckChange(Int32 num)
    {
        if (dropdown.options[num].text.Contains("使用不可"))
        {
            dropdown.value = PlayerPrefs.GetInt("BattleDeck")-1;
        }
        else
        {
            PlayerPrefs.SetInt("BattleDeck", num + 1);
        }
    }
    public void DeckEdit()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetInt("deckNum", PlayerPrefs.GetInt("BattleDeck"));
        GameObject.Find("Loading").GetComponent<Loading>().LoadingStart();
        PlayerPrefs.SetString("deckEdit", "battle");
        SceneManager.LoadScene("DeckEdit");
    }
}
