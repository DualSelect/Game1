using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MailLink : MonoBehaviour
{
    public void MailButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        var uri = new Uri("https://docs.google.com/forms/d/e/1FAIpQLSdlC3mgYOyBWkvXVrtTbwFJXL0WzCH-jjyVqPYsm8jQr7Gs6A/viewform");
        Application.OpenURL(uri.AbsoluteUri);
    }
}
