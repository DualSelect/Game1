using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Collections;

public class NewsInitial : MonoBehaviour
{
    public GameObject newsWindow;
    public GameObject newsDetail;
    public GameObject content;
    public GameObject prefab;
    public AssetReference m_Ref;
    public Text title;
    public Text date;
    public Text detail;

    const string URL = "https://script.google.com/macros/s/AKfycbxldDU2oOQWKKX1vI4jvxorwy5CdXb_o3DpuUPykOn0VdBzSlcJ/exec";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(News());
    }
    public IEnumerator News()
    {
        var result = Addressables.LoadAssetAsync<GameObject>(m_Ref);
        yield return result;
        prefab = result.Result;
        _ = LoadNews("");
    }
    public async Cysharp.Threading.Tasks.UniTask<string> LoadNews(string masterName)
    {
        // シート名を追加パラメータとしてAPIを叩くURLを決定。
        // GASでは "exec"のあとに "?" をつけて "hoge=fuga" などと追記することでGETにパラメータを付与できる
        GameObject.Destroy(GameObject.Find("MainThreadDispatcher"));
        string sheetName = "News";
        var url = URL + "?sheetName=" + sheetName;
        Debug.Log(url);
        var result = await GetMasterAsync(url);
        try
        {
            Debug.Log(result);
            List<NewsCard> newsList = JsonHelper.ListFromJson<NewsCard>(result);
            if (newsList != null)
            {
                newsList.Sort(CompareByDate);
                Vector2 sd = new Vector2(0, 180 * newsList.Count);
                content.GetComponent<RectTransform>().sizeDelta = sd;
                for (int i = 0; i < newsList.Count; i++)
                {
                    Vector3 v = new Vector3(0, -100 - 200 * i, 0);
                    GameObject gameObject = Instantiate(prefab, v, Quaternion.identity);
                    NewsPrefab newsPrefab = gameObject.GetComponent<NewsPrefab>();
                    gameObject.transform.SetParent(content.transform, false);
                    newsPrefab.title.text = newsList[i].title;
                    newsPrefab.date.text = newsList[i].dateTitle;
                    newsPrefab.detail = newsList[i].content;
                    newsPrefab.url = newsList[i].url;
                    if (newsList[i].url == "" || newsList[i].url == null) newsPrefab.site.interactable = false;
                }
            }
            return masterName;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return e.Message;
        }
    }
    /*
    public IEnumerator LoadNews()
    {
        GameObject.Destroy(GameObject.Find("MainThreadDispatcher"));

        string sheetName = "News";
        var url = URL + "?sheetName=" + sheetName;
        Debug.Log(url);


        var prefab = Addressables.LoadAssetAsync<GameObject>(m_Ref);
        yield return prefab;
        ObservableWWW.GetWWW(url)
            .Subscribe(www =>
            {
                List<NewsCard> newsList = JsonHelper.ListFromJson<NewsCard>(www.text);
                if (newsList != null)
                {
                    newsList.Sort(CompareByDate);
                    Vector2 sd = new Vector2(0, 180 * newsList.Count);
                    content.GetComponent<RectTransform>().sizeDelta = sd;
                    for (int i = 0; i < newsList.Count; i++)
                    {
                        Vector3 v = new Vector3(0, -100 - 200 * i, 0);
                        GameObject gameObject = Instantiate(prefab.Result, v, Quaternion.identity);
                        NewsPrefab newsPrefab = gameObject.GetComponent<NewsPrefab>();
                        gameObject.transform.SetParent(content.transform, false);
                        newsPrefab.title.text = newsList[i].title;
                        newsPrefab.date.text = newsList[i].dateTitle;
                        newsPrefab.detail = newsList[i].content;
                        newsPrefab.url = newsList[i].url;
                        if (newsList[i].url == "" || newsList[i].url == null) newsPrefab.site.interactable = false;
                    }
                }
                else
                {
                    // Jsonの取得に失敗している
                    Debug.LogError(www.text);
                }
            });

    }
    */
    private static int CompareByDate(NewsCard a, NewsCard b)
    {
        return b.date - a.date;
    }
    public void OpenNews()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        newsWindow.SetActive(true);
    }
    public void CloseNews()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        newsWindow.SetActive(false);
    }
    public void OpenNewsDetail()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        newsDetail.SetActive(true);
        newsWindow.SetActive(false);
    }
    public void CloseNewsDetail()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        newsDetail.SetActive(false);
        newsWindow.SetActive(true);
    }
    private static async UniTask<string> GetMasterAsync(string url)
    {
        var request = UnityWebRequest.Get(url);

        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
        {
            throw new Exception(request.error);
        }

        return request.downloadHandler.text;
    }
}
