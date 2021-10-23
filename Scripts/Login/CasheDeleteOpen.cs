using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasheDeleteOpen : MonoBehaviour
{
    public GameObject menu;
    public GameObject casheDeleteMenu;
    public void CasheDeleteMenuOpen()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        casheDeleteMenu.SetActive(true);
        menu.SetActive(false);
    }
}
