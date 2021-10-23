using Gs2.Core;
using Gs2.Core.Exception;
using Gs2.Unity;
using Gs2.Unity.Gs2Inbox.Result;
using Gs2.Unity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class MailControll : MonoBehaviour
{
    GameObject login;
    Client gs2;
    GameSession session;
    public GameObject mailWindow;
    public GameObject mailDetail;
    public GameObject content;
    public GameObject reseiveWindow;
    public AssetReference m_Ref;
    public Text title;
    public Text date;
    public Text detail;

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
        StartCoroutine(MailTest());
    }
    private IEnumerator MailTest()
    {
        AsyncResult<EzListResult> asyncResult2 = null;
        var current2 = gs2.Inbox.List(
            r => { asyncResult2 = r; },
            session: session,
            namespaceName: "inbox"
        );
        yield return current2;
        if (asyncResult2.Error != null)
        {
            OnError(asyncResult2.Error);
            yield break;
        }
        AsyncResult<EzReceiveGlobalMessageResult> asyncResult = null;
        var current = gs2.Inbox.ReceiveGlobalMessage(
            r => { asyncResult = r; },
            session: session,
            namespaceName: "inbox"
        );
        yield return current;
        if (asyncResult.Error != null)
        {
            OnError(asyncResult.Error);
            yield break;
        }

        asyncResult.Result.Item.AddRange(asyncResult2.Result.Items);

        var prefab = Addressables.LoadAssetAsync<GameObject>(m_Ref);
        yield return prefab;
        Vector2 sd = new Vector2(0, 180 * asyncResult.Result.Item.Count);
        content.GetComponent<RectTransform>().sizeDelta = sd;
        for (int i = 0; i < asyncResult.Result.Item.Count; i++)
        {
            Vector3 v = new Vector3(0, -100 - 200 * i, 0);
            GameObject gameObject = Instantiate(prefab.Result, v, Quaternion.identity);
            MailPrefab mailPrefab = gameObject.GetComponent<MailPrefab>();
            gameObject.transform.SetParent(content.transform, false);

            mailPrefab.title.text = asyncResult.Result.Item[i].Metadata;
            mailPrefab.date.text = asyncResult.Result.Item[i].ReceivedAt.ToString();
            mailPrefab.detail = asyncResult.Result.Item[i].Metadata;
            mailPrefab.messageId = asyncResult.Result.Item[i].MessageId;

            if (asyncResult.Result.Item[i].IsRead) mailPrefab.gift.interactable = false;
        }
    }
    public IEnumerator MailRead(string messageId)
    {
        {
            AsyncResult<EzReadResult> asyncResult = null;
            var current = gs2.Inbox.Read(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "inbox",
              messageName: messageId
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            string stampSheet = asyncResult.Result.StampSheet;
            var machine = new StampSheetStateMachine(stampSheet, gs2, "distributor", "grn:gs2:ap-northeast-1:uFLAkqDK-Development:key:inbox-key:key:inbox-key");
            yield return machine.Execute(m_events);
            reseiveWindow.SetActive(true);
        }
    }

    public void ReceiveWindowClose()
    {
        reseiveWindow.SetActive(false);
    }
    public void OpenMail()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        mailWindow.SetActive(true);
    }
    public void CloseMail()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        mailWindow.SetActive(false);
    }
    public void CloseMailDetail()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        mailDetail.SetActive(false);
        mailWindow.SetActive(true);
    }

    public void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
}
