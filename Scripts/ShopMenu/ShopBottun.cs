using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ShopBottun : MonoBehaviour
{
    public GameObject stoneWindow; 
    public void PackButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        SceneManager.LoadScene("CardPack");
    }
    public void StoneButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        stoneWindow.SetActive(true);
    }
    public void BGMButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        SceneManager.LoadScene("BGM");
    }
    public void CloseButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        stoneWindow.SetActive(false);
    }
    public void SikinButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        var uri = new Uri("https://docs.google.com/document/d/1kAMmepT5_tVrrCBgitSnkr9C-eXkRoxRh8pFXRhGqHE/edit?usp=sharing");
        Application.OpenURL(uri.AbsoluteUri);
    }
    public void TokuteiButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        var uri = new Uri("https://docs.google.com/document/d/11wqtbWakaYPdtF0VDd9HFoBrJ1fi4T59KMTPWlQldkg/edit?usp=sharing");
        Application.OpenURL(uri.AbsoluteUri);
    }
}
