using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gs2.Core;
using Gs2.Unity.Gs2Matchmaking.Model;
using Gs2.Unity.Gs2Matchmaking.Result;
using Gs2.Unity.Gs2Realtime.Result;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Gs2.Unity.Gs2Experience.Model;
using Gs2.Unity.Gs2Experience.Result;
using Gs2.Unity.Gs2Gateway.Result;
using Gs2.Unity.Gs2Gateway.Model;
using Gs2.Core.Model;

public class Matching : MonoBehaviour
{
    GameObject login;
    string gatheringName;
    public Button cancel;
    public InputField inputName;
    public GameObject window;
    public GameObject attention;
    public MainMenuInitial mainMenuInitial;
    private AsyncOperation async;
    Loading loading;
    bool complete=false;
    void Start()
    {
        complete = false;
        login = GameObject.Find("Login");
        loading = GameObject.Find("Loading").GetComponent<Loading>();
        cancel.enabled = false;
    }
    IEnumerator DoMatch()
    {
        //Debug.Log("Mode:"+PlayerPrefs.GetInt("MatchMode"));
        //Debug.Log("Rate:"+PlayerPrefs.GetInt("MatchRate"));
        //Debug.Log("Room:"+PlayerPrefs.GetInt("MatchRoom"));
        cancel.enabled = true;
        //yield return AsyncLoad();
        yield return DoMatching();//マッチングして
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (complete) break;
        }
        loading.LoadingStart();
        SceneManager.LoadScene("Battle");
        //async.allowSceneActivation = true;
    }



    public IEnumerator DoMatching()
    {
        var gs2 = login.GetComponent<LoginInitial>().GetClient();
        var session = login.GetComponent<LoginInitial>().GetSession();
        PlayerPrefs.SetString("Player", "２");
        gs2.Profile.Gs2Session.OnNotificationMessage -= PushNotificationHandler;
        gs2.Profile.Gs2Session.OnNotificationMessage += PushNotificationHandler;

        
        EzWebSocketSession socketSession = null;
        {
            AsyncResult<EzSetUserIdResult> asyncResult = null;
            var current = gs2.Gateway.SetUserId(
                  r => { asyncResult = r; },
                  session: session,
                  namespaceName: "gateway",
                  allowConcurrentAccess: false
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            socketSession = asyncResult.Result.Item;
        }
        



        //プレイヤー情報
        EzPlayer ezPlayer = new EzPlayer();
        ezPlayer.UserId = PlayerPrefs.GetString("ID");
        ezPlayer.RoleName = "default";
        EzAttribute ezAttributeMode = new EzAttribute();
        EzAttribute ezAttributeRate = new EzAttribute();
        EzAttribute ezAttributeRoom = new EzAttribute();
        ezAttributeMode.Name = "mode";
        ezAttributeMode.Value = PlayerPrefs.GetInt("MatchMode");
        ezAttributeRate.Name = "rate";
        ezAttributeRate.Value = PlayerPrefs.GetInt("MatchRate");
        ezAttributeRoom.Name = "room";
        ezAttributeRoom.Value = PlayerPrefs.GetInt("MatchRoom");
        var attributes = new List<EzAttribute>();
        attributes.Add(ezAttributeMode);
        attributes.Add(ezAttributeRate);
        attributes.Add(ezAttributeRoom);
        ezPlayer.Attributes = attributes;

        //募集人数
        var capacityOfRoles = new List<EzCapacityOfRole>();
        EzCapacityOfRole ezCapacityOfRole = new EzCapacityOfRole();
        ezCapacityOfRole.RoleName = "default";
        ezCapacityOfRole.Capacity = 2;
        capacityOfRoles.Add(ezCapacityOfRole);

        //参加資格
        var attributeRanges = new List<EzAttributeRange>();
        EzAttributeRange ezAttributeRangeMode = new EzAttributeRange();
        ezAttributeRangeMode.Name = "mode";
        ezAttributeRangeMode.Min = PlayerPrefs.GetInt("MatchMode");
        ezAttributeRangeMode.Max = PlayerPrefs.GetInt("MatchMode");
        attributeRanges.Add(ezAttributeRangeMode);
        if (PlayerPrefs.GetInt("MatchMode") == 1)
        {
            EzAttributeRange ezAttributeRangeRate = new EzAttributeRange();
            ezAttributeRangeRate.Name = "rate";
            ezAttributeRangeRate.Min = PlayerPrefs.GetInt("MatchRate") - 200;
            ezAttributeRangeRate.Max = PlayerPrefs.GetInt("MatchRate") + 200;
            attributeRanges.Add(ezAttributeRangeRate);
        }
        if (PlayerPrefs.GetInt("MatchMode") == 3)
        {
            EzAttributeRange ezAttributeRangeRoom = new EzAttributeRange();
            ezAttributeRangeRoom.Name = "rate";
            ezAttributeRangeRoom.Min = PlayerPrefs.GetInt("MatchRoom");
            ezAttributeRangeRoom.Max = PlayerPrefs.GetInt("MatchRoom");
            attributeRanges.Add(ezAttributeRangeRoom);
        }
        //許可するユーザー
        var allowUserIds = new List<string>();

        //部屋を検索
        {
            string matchmakingContextToken = null;
            for (int i = 0; i < 3; i++)
            {
                AsyncResult<EzDoMatchmakingResult> asyncResult = null;
                var current = gs2.Matchmaking.DoMatchmaking(
                      r => { asyncResult = r; },
                      session: session,
                      namespaceName: "matchmaking",
                      player: ezPlayer,
                      matchmakingContextToken: matchmakingContextToken
                );
                yield return current;
                if (asyncResult.Error != null)
                {
                    Debug.Log("条件に合う部屋がない");
                    break;
                }
                if (asyncResult.Result.Item != null)
                {
                    Debug.Log("マッチングに成功した");
                    gatheringName = asyncResult.Result.Item.Name;
                    PlayerPrefs.SetString("gatheringName", gatheringName);
                    yield break;
                }
                if (asyncResult.Result.MatchmakingContextToken != null)
                {
                    Debug.Log("部屋探し継続");
                    matchmakingContextToken = asyncResult.Result.MatchmakingContextToken;
                }
            }
        }
        //部屋作成
        {
            Debug.Log("部屋を作成する");
            PlayerPrefs.SetString("Player", "１");
            AsyncResult<EzCreateGatheringResult> asyncResult = null;
            var current = gs2.Matchmaking.CreateGathering(
                  r => { asyncResult = r; },
                  session: session,
                  namespaceName: "matchmaking",
                  player: ezPlayer,
                  attributeRanges: attributeRanges,
                  capacityOfRoles: capacityOfRoles,
                  allowUserIds: allowUserIds,
                  expiresAt: 180
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            gatheringName = asyncResult.Result.Item.Name;
            PlayerPrefs.SetString("gatheringName", gatheringName);
        }
        /*
        //マッチング確認
        {
            while (true)
            {
                AsyncResult<EzGetRoomResult> asyncResult = null;
                var current = gs2.Realtime.GetRoom(
                  r => { asyncResult = r; },
                  namespaceName: "realtime",
                  roomName: gatheringName
                );
                yield return current;
                if (asyncResult.Error == null)
                {
                    Debug.Log("マッチングした");
                    yield break;
                }
                yield return new WaitForSeconds(3.0f);
                Debug.Log("マッチング待ち");
                
                AsyncResult<EzGetGatheringResult> asyncResult = null;
                var current = gs2.Matchmaking.GetGathering(
                  r => { asyncResult = r; },
                  namespaceName: "matchmaking",
                  gatheringName: gatheringName
                  );
                yield return current;
                if (asyncResult.Error != null)
                {
                    Debug.Log("マッチングした");
                    yield break;
                }
                yield return new WaitForSeconds(3.0f);
                Debug.Log("マッチング待ち");
                
            }
        }
        */
    }

    private void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }

    public void RoomMatchButtunDown()
    {
        if (inputName.text.Length < 4)
        {
            attention.SetActive(true);
        }
        else
        {
            window.SetActive(true);
            GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
            int roomNumber;
            PlayerPrefs.SetInt("MatchMode", 3);
            Int32.TryParse(inputName.text, out roomNumber);
            PlayerPrefs.SetInt("MatchRoom", roomNumber);
            StartCoroutine(GetRate());
        }
    }

    public void RankMatchButtunDown()
    {
        window.SetActive(true);
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetInt("MatchMode", 1);
        StartCoroutine(GetRate());

    }
    private IEnumerator AsyncLoad()
    {
        async = SceneManager.LoadSceneAsync("Battle");
        async.allowSceneActivation = false;
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator GetRate()
    {
        var gs2 = login.GetComponent<LoginInitial>().GetClient();
        var session = login.GetComponent<LoginInitial>().GetSession();
        {
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
            PlayerPrefs.SetInt("MatchRate", (int)ezStatus.ExperienceValue);
            yield return DoMatch();
        }

    }
    public void FreeMatchButtunDown()
    {
        window.SetActive(true);
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetInt("MatchMode", 2);
        StartCoroutine(GetRate());
    }
    public void ReMatchButtunDown()
    {
        window.SetActive(true);
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(GetRate());
    }

    public void CancelButtonDown()
    {
        cancel.enabled = false;
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(CancelMatch());
    }
    public void CancelButtonDownHome()
    {
        cancel.enabled = false;
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(CancelMatchHome());
    }
    public void CancelButtonDownResult()
    {
        cancel.enabled = false;
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(CancelMatchResult());
    }
    public IEnumerator CancelMatch()
    {
        var gs2 = login.GetComponent<LoginInitial>().GetClient();
        var session = login.GetComponent<LoginInitial>().GetSession();
        AsyncResult<EzCancelMatchmakingResult> asyncResult = null;
        var current = gs2.Matchmaking.CancelMatchmaking(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "matchmaking",
              gatheringName: PlayerPrefs.GetString("gatheringName")

        );
        yield return current;
        PlayerPrefs.SetString("menu", "battle");
        SceneManager.LoadScene("MainMenu");
    }
    public IEnumerator CancelMatchHome()
    {
        var gs2 = login.GetComponent<LoginInitial>().GetClient();
        var session = login.GetComponent<LoginInitial>().GetSession();
        AsyncResult<EzCancelMatchmakingResult> asyncResult = null;
        var current = gs2.Matchmaking.CancelMatchmaking(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "matchmaking",
              gatheringName: PlayerPrefs.GetString("gatheringName")

        );
        yield return current;
        PlayerPrefs.SetString("menu", "home");
        SceneManager.LoadScene("MainMenu");
    }
    public IEnumerator CancelMatchResult()
    {
        var gs2 = login.GetComponent<LoginInitial>().GetClient();
        var session = login.GetComponent<LoginInitial>().GetSession();
        AsyncResult<EzCancelMatchmakingResult> asyncResult = null;
        var current = gs2.Matchmaking.CancelMatchmaking(
              r => { asyncResult = r; },
              session: session,
              namespaceName: "matchmaking",
              gatheringName: PlayerPrefs.GetString("gatheringName")

        );
        yield return current;
        window.SetActive(false);
    }

    public void PushNotificationHandler(NotificationMessage message)
    {
        if (!message.issuer.StartsWith("Gs2Matchmaking:")) return;

        if (message.issuer.EndsWith(":Complete"))
        {
            complete = true;
        }
    }
}
