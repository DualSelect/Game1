using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckWindow : MonoBehaviour
{
    public GameObject deckWindow;
    public void DeckButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        if (deckWindow.activeSelf)
        {
            deckWindow.SetActive(false);
        }
        else
        {
            deckWindow.SetActive(true);
        }
    }
}
