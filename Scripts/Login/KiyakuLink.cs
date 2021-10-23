using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class KiyakuLink : MonoBehaviour
{
    public GameObject nameRequest;
    public void KiyakuButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        var uri = new Uri("https://docs.google.com/document/d/1uKTTZLbiMt0EPlCqbDijEZngOhnUxB7e89yBoBFnv_w/edit?usp=sharing");
        Application.OpenURL(uri.AbsoluteUri);
    }
    public void PolicyButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        var uri = new Uri("https://docs.google.com/document/d/1-wLmPupqbHHk-iI7mMH5kkNdTbldyE5TOZyzoP_nLhs/edit?usp=sharing");
        Application.OpenURL(uri.AbsoluteUri);
    }
    public void KiyakuOk()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
        nameRequest.SetActive(true);
    }
}
