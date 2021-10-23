using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitFlavorWindow : MonoBehaviour
{
    public Text unitName;
    public Image card;
    public Text flavor;
    public Text illust;
    public GameObject status;
    public Image frame;
    public string cardId;
    public Text cardWin;
    public Button aibouButton;
    public void UnitflavorWindowClose()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
    }
    public void UnitStatus()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.SetActive(false);
        status.SetActive(true);
    }
    public void AibouButton()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetString("aibou", cardId);
    }
}
