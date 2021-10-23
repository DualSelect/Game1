using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialFlavorWindow : MonoBehaviour
{
    public Text specialName;
    public Image card;
    public Text flavor;
    public Text illust;
    public GameObject status;
    public Image frame;
    public void SpecialflavorWindowClose()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
    }
    public void SpecialStatus()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
        status.SetActive(true);
    }
}
