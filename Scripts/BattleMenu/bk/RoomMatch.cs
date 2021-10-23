using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class RoomMatch : MonoBehaviour
{
    public InputField inputName;
    public void RoomMatchButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        int roomNumber;
        PlayerPrefs.SetInt("MatchMode", 3);
        Int32.TryParse(inputName.text, out roomNumber);
        PlayerPrefs.SetInt("MatchRoom", roomNumber);
        SceneManager.LoadScene("Matching");
    }
}
