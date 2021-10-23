using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsPrefab : MonoBehaviour
{
    NewsInitial news;
    public Text date;
    public Text title;
    public string detail;
    public string url;
    public Button site;
    void Start()
    {
        news = GameObject.Find("News").GetComponent<NewsInitial>();
    }
    public void DetailOpen()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        news.title.text = title.text;
        news.date.text = date.text;
        news.detail.text = detail;
        news.newsDetail.SetActive(true);
        news.newsWindow.SetActive(false);
    }
    public void UrlOpen()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        var uri = new Uri(url);
        Application.OpenURL(uri.AbsoluteUri);
    }
    
}
