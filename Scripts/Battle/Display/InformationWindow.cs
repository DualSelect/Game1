using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationWindow : MonoBehaviour
{
    public void InformationWindowClose()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
    }
    public void InformationWindowOpen()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(true);
    }
}
