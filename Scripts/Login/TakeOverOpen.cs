using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeOverOpen : MonoBehaviour
{
    public GameObject menu;
    public GameObject takeOverMenu;
    public void TakeOverMenuOpen()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        takeOverMenu.SetActive(true);
        menu.SetActive(false);
    }
}
