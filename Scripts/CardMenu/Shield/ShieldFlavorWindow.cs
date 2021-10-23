using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShieldFlavorWindow : MonoBehaviour
{
    public Text shieldName;
    public Image card;
    public Text flavor;
    public Text illust;
    public GameObject status;
    public Image frame;
    public void ShieldFlavorWindowClose()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
    }
    public void ShieldStatus()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
        status.SetActive(true);
    }
}
