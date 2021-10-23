using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuClose : MonoBehaviour
{
    public GameObject menu;
    public void MenuCloseDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        menu.SetActive(false);
    }
}
