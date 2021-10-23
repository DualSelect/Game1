using System;                      
using System.Collections;         
using System.Collections.Generic;   
using UnityEngine;                  
using UnityEngine.UI;               
using Gs2.Core;                    
using Gs2.Unity.Util;              
using Gs2.Unity.Gs2Money;
using Gs2.Unity.Gs2Money.Model;
using Gs2.Unity.Gs2Money.Result;

public class ShopMenuInitial : MonoBehaviour
{
    public Text freeStone;
    public Text payStone;
    GameObject login;              
    void Start()
    {
        login = GameObject.Find("Login"); 
        StartCoroutine(ShopMenuInitailize());  
    }
    public IEnumerator ShopMenuInitailize()
    {
        var gs2 = login.GetComponent<LoginInitial>().GetClient();           
        var session = login.GetComponent<LoginInitial>().GetSession();   
        {
            AsyncResult<EzGetResult> asyncResult = null;
            var current = gs2.Money.Get(
                  r => { asyncResult = r; },
                  session: session,
                  namespaceName: "money",
                  slot: 0
            );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            EzWallet ezWallet = asyncResult.Result.Item;
            freeStone.text = ezWallet.Free.ToString("0");
            payStone.text = ezWallet.Paid.ToString("0");
        }
    }
    private void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
}