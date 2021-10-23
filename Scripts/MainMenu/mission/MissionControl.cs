using Gs2.Core;
using Gs2.Core.Exception;
using Gs2.Unity;
using Gs2.Unity.Gs2Mission.Result;
using Gs2.Unity.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MissionControl : MonoBehaviour
{
    GameObject login;
    Client gs2;
    GameSession session;
    public AssetReference m_Ref;
    public GameObject mission;
    public GameObject dailyList;
    public GameObject monthlyList;
    public GameObject totalList;
    public GameObject dailyListReceive;
    public GameObject monthlyListReceive;
    public GameObject totalListReceive;
    public GameObject dailyListContent;
    public GameObject monthlyListContent;
    public GameObject totalListContent;
    public GameObject dailyListReceiveContent;
    public GameObject monthlyListReceiveContent;
    public GameObject totalListReceiveContent;

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
        MissionList();
    }
    private void MissionList()
    {
        StartCoroutine(MissionList("daily"));
        StartCoroutine(MissionList("monthly"));
        StartCoroutine(MissionList("total"));
    }
    private IEnumerator MissionList(string group)
    {
        AsyncResult<EzGetMissionGroupModelResult> asyncResult = null;
        var current = gs2.Mission.GetMissionGroupModel(
          r => { asyncResult = r; },
          namespaceName: "mission",
          missionGroupName: group
        );
        yield return current;
        if (asyncResult.Error != null)
        {
            OnError(asyncResult.Error);
            yield break;
        }
        var task = asyncResult.Result.Item.Tasks;

        AsyncResult<EzGetCompleteResult> asyncResult2 = null;
        var current2 = gs2.Mission.GetComplete(
          r => { asyncResult2 = r; },
          session: session,
          namespaceName: "mission",
          missionGroupName: group
        );
        yield return current2;
        List<string> clear;
        List<string> receive;
        if (asyncResult2.Error != null)
        {
            OnError(asyncResult2.Error);
            clear = null;
            receive = null;
        }
        else
        {
            clear = asyncResult2.Result.Item.CompletedMissionTaskNames;
            receive = asyncResult2.Result.Item.ReceivedMissionTaskNames;
        }
        var prefab = Addressables.LoadAssetAsync<GameObject>(m_Ref);
        yield return prefab;
        //受け取り済み
        {
            if (receive != null)
            {
                GameObject content = new GameObject();
                if (group == "daily") content = dailyListReceiveContent;
                if (group == "monthly") content = monthlyListReceiveContent;
                if (group == "total") content = totalListReceiveContent;
                var transforms = content.GetComponentsInChildren<Transform>();
                var gameObjects = from t in transforms select t.gameObject;
                var tasks = gameObjects.ToArray();
                for (int i = 1; i < tasks.Length; i++) Destroy(tasks[i]);
                Vector2 sd = new Vector2(0, 180 * receive.Capacity);
                content.GetComponent<RectTransform>().sizeDelta = sd;
                for (int i = 0; i < receive.Capacity; i++)
                {
                    var receiveTask = task.Find(m => m.Name == receive[i]);
                    task.Remove(receiveTask);
                    Vector3 v = new Vector3(0, -100 - 200 * i, 0);
                    GameObject gameObject = Instantiate(prefab.Result, v, Quaternion.identity);
                    MissionPrefab missionPrefab = gameObject.GetComponent<MissionPrefab>();
                    missionPrefab.missionName = receiveTask.Name;
                    missionPrefab.missionGroup = group;
                    missionPrefab.rule.text = GetBetweenStrings("(", ")", receiveTask.Metadata);
                    missionPrefab.reward.text = GetBetweenStrings("[", "]", receiveTask.Metadata);
                    missionPrefab.count.text = "受取済み";
                    missionPrefab.receive.interactable = false;
                    gameObject.transform.SetParent(content.transform, false);
                }
            }
        }
        //未達成&達成済
        {
            int j = 0;
            GameObject content = new GameObject();
            if (group == "daily") content = dailyListContent;
            if (group == "monthly") content = monthlyListContent;
            if (group == "total") content = totalListContent;
            var transforms = content.GetComponentsInChildren<Transform>();
            var gameObjects = from t in transforms select t.gameObject;
            var tasks = gameObjects.ToArray();
            for (int i = 1; i < tasks.Length; i++) Destroy(tasks[i]);
            Vector2 sd = new Vector2(0, 180 * task.Count);
            content.GetComponent<RectTransform>().sizeDelta = sd;
            if (clear != null)
            {
                for (int i = 0; i < clear.Count; i++)
                {
                    var clearTask = task.Find(m => m.Name == clear[i]);
                    if (clearTask != null)
                    {
                        task.Remove(clearTask);
                        Vector3 v = new Vector3(0, -100 - 200 * i, 0);
                        GameObject gameObject = Instantiate(prefab.Result, v, Quaternion.identity);
                        MissionPrefab missionPrefab = gameObject.GetComponent<MissionPrefab>();
                        missionPrefab.missionName = clearTask.Name;
                        missionPrefab.missionGroup = group;
                        missionPrefab.rule.text = GetBetweenStrings("(", ")", clearTask.Metadata);
                        missionPrefab.reward.text = GetBetweenStrings("[", "]", clearTask.Metadata);
                        missionPrefab.count.text = "達成済み";
                        gameObject.transform.SetParent(content.transform, false);
                        j++;
                    }
                }
            }
            for (int i = 0; i < task.Count; i++)
            {
                var notclearTask = task[i];
                Vector3 v = new Vector3(0, -100 - 200 * (i+j), 0);
                GameObject gameObject = Instantiate(prefab.Result, v, Quaternion.identity);
                MissionPrefab missionPrefab = gameObject.GetComponent<MissionPrefab>();
                missionPrefab.missionName = notclearTask.Name;
                missionPrefab.missionGroup = group;
                missionPrefab.rule.text = GetBetweenStrings("(", ")", notclearTask.Metadata);
                missionPrefab.reward.text = GetBetweenStrings("[", "]", notclearTask.Metadata);
                long count = 0;
                var cor = StartCoroutine(Counter(notclearTask.CounterName, (result) => count = result));
                yield return cor;
                missionPrefab.count.text = "進行度:"+count + "/" + notclearTask.TargetValue;
                missionPrefab.receive.interactable = false;
                gameObject.transform.SetParent(content.transform, false);
            }
        }
    }
    private IEnumerator Counter(string countName, UnityEngine.Events.UnityAction<long> callback)
    {
        AsyncResult<EzGetCounterResult> asyncResult = null;
        var current = gs2.Mission.GetCounter(
          r => { asyncResult = r; },
          session: session,
          namespaceName: "mission",
          counterName: countName
        );
        yield return current;
        if (asyncResult.Error != null)
        {
            OnError(asyncResult.Error);
            callback(0);
        }
        else
        {
            callback(asyncResult.Result.Item.Values[0].Value);
        }
    }
    public IEnumerator Receive(string group,string name)
    {
        GameObject.Find("Loading").GetComponent<Loading>().LoadingStart();
        AsyncResult<EzReceiveRewardsResult> asyncResult = null;
        var current = gs2.Mission.ReceiveRewards(
          r => { asyncResult = r; },
          session: session,
          namespaceName: "mission",
          missionGroupName: group,
          missionTaskName: name
        );
        yield return current;
        if (asyncResult.Error != null)
        {
            OnError(asyncResult.Error);
            yield break;
        }
        var machine = new StampSheetStateMachine(asyncResult.Result.StampSheet, gs2, "distributor", "grn:gs2:ap-northeast-1:uFLAkqDK-Development:key:mission-key:key:mission-key");
        yield return machine.Execute(m_events);
        GameObject.Find("Loading").GetComponent<Loading>().LoadingEnd();
        StartCoroutine(MissionList(group));
    }
    private void ListReset()
    {
        dailyList.SetActive(false);
        monthlyList.SetActive(false);
        totalList.SetActive(false);
        dailyListReceive.SetActive(false);
        monthlyListReceive.SetActive(false);
        totalListReceive.SetActive(false);
    }
    public void DailyButton()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        if (!(dailyList.activeSelf || dailyListReceive.activeSelf))
        {
            ListReset();
            dailyList.SetActive(true);
        }
    }
    public void MonthlyButton()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        if (!(monthlyList.activeSelf || monthlyListReceive.activeSelf))
        {
            ListReset();
            monthlyList.SetActive(true);
        }
    }
    public void TotalButton()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        if (!(totalList.activeSelf || totalListReceive.activeSelf))
        {
            ListReset();
            totalList.SetActive(true);
        }
    }
    public void NotClear()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        if (dailyListReceive.activeSelf)
        {
            ListReset();
            dailyList.SetActive(true);
        }
        if(monthlyListReceive.activeSelf)
        {
            ListReset();
            monthlyList.SetActive(true);
        }
        if (totalListReceive.activeSelf)
        {
            ListReset();
            totalList.SetActive(true);
        }
    }
    public void OkClear()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        if (dailyList.activeSelf)
        {
            ListReset();
            dailyListReceive.SetActive(true);
        }
        if (monthlyList.activeSelf)
        {
            ListReset();
            monthlyListReceive.SetActive(true);
        }
        if (totalList.activeSelf)
        {
            ListReset();
            totalListReceive.SetActive(true);
        }
    }
    public void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
    public string GetBetweenStrings(string str1, string str2, string orgStr)
    {
        int str1Len = str1.Length; //str1の長さ
        int str1Num = orgStr.IndexOf(str1); //str1が原文のどの位置にあるか
        string s = ""; //返す文字列
        //例外処理
        try
        {
            s = orgStr.Remove(0, str1Num + str1Len); //原文の初めからstr1のある位置まで削除
            int str2Num = s.IndexOf(str2); //str2がsのどの位置にあるか
            s = s.Remove(str2Num); //sのstr2のある位置から最後まで削除
        }
        catch (Exception)
        {
            return orgStr; //原文を返す
        }
        return s; //戻り値
    }

    public void OpenMission()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        Vector3 pos = new Vector3(0, 0, 0);
        mission.transform.position = pos;
    }
    public void CloseMission()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        Vector3 pos = new Vector3(10, 0, 0);
        mission.transform.position = pos;
    }
    
}
