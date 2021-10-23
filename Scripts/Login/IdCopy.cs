using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IdCopy : MonoBehaviour
{
    public GameObject menu;
    public GameObject idCopyMenu;
    public Text id;
    public void IdCopyMenuOpen()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        idCopyMenu.SetActive(true);
        menu.SetActive(false);
        GUIUtility.systemCopyBuffer = id.text;
    }
}
