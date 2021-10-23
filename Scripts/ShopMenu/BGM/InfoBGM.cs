using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBGM : MonoBehaviour
{
    public GameObject info;
    public void InfoButton()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        info.SetActive(true);
    }
    public void InfoClose()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        info.SetActive(false);
    }
}
