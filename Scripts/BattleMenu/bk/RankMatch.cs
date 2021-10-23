using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Gs2.Core;
using Gs2.Unity.Gs2Experience.Model;
using Gs2.Unity.Gs2Experience.Result;

public class RankMatch : MonoBehaviour
{
    GameObject login;
    public void RankMatchButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        login = GameObject.Find("Login");
        PlayerPrefs.SetInt("MatchMode", 1);
        StartCoroutine(GetRate());

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
            SceneManager.LoadScene("Matching");
        }

    }
    private void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
}
