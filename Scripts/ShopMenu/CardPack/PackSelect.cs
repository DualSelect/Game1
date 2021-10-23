using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackSelect : MonoBehaviour
{
    public GameObject basic;
    public GameObject dan1;

    private void Start()
    {
        if (PlayerPrefs.GetString("Box") == "dan1") dan1Open();
    }
    public void basicOpen()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        AllClose();
        basic.SetActive(true);
        PlayerPrefs.SetString("Box", "basic");
    }
    public void dan1Open()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        AllClose();
        dan1.SetActive(true);
        PlayerPrefs.SetString("Box", "dan1");
    }
    private void AllClose()
    {
        basic.SetActive(false);
        dan1.SetActive(false);
    }
}
