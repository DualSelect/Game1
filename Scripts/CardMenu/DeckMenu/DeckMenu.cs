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

public class DeckMenu : MonoBehaviour
{
    public DeckList[] deckLists;
    GameObject login;
    Client gs2;
    GameSession session;
    void Start()
    {
        login = GameObject.Find("Login");
        gs2 = login.GetComponent<LoginInitial>().GetClient();
        session = login.GetComponent<LoginInitial>().GetSession();
        for (int i = 0; i < deckLists.Length; i++)
        {
            deckLists[i].i = i+1;
            deckLists[i].num.text = (i + 1).ToString();
            //deckLists[i].title.text = PlayerPrefs.GetString("deckName" + (i+1),"DECK");
            StartCoroutine(DeckName(i));
        }
    }
    private IEnumerator DeckName(int i)
    {
        AsyncResult<EzPrepareDownloadOwnDataResult> asyncResult = null;
        var current = gs2.Datastore.PrepareDownloadOwnData(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "datastore",
              dataObjectName:"deck"+(i+1).ToString()
        );
        yield return current;
        if (asyncResult.Error != null)
        {
            OnError(asyncResult.Error);
            yield break;
        }
        UnityWebRequest www = UnityWebRequest.Get(asyncResult.Result.FileUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            deckLists[i].title.text = www.downloadHandler.text;
        }
    }
    private void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
}
