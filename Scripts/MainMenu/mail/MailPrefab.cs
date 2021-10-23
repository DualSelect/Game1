using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailPrefab : MonoBehaviour
{
    MailControll mail;
    public Text date;
    public Text title;
    public string detail;
    public string messageId;
    public Button gift;
    void Start()
    {
        mail = GameObject.Find("News").GetComponent<MailControll>();
    }
    public void DetailOpen()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        mail.title.text = title.text;
        mail.date.text = date.text;
        mail.detail.text = detail;
        mail.mailDetail.SetActive(true);
        mail.mailWindow.SetActive(false);
    }
    public void GiftOpen()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        mail.MailRead(messageId);
        gift.interactable = false;
    }

}
