using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardMenuButton : MonoBehaviour
{
    public void DeckButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        SceneManager.LoadScene("DeckMenu");
    }
    public void CardButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        SceneManager.LoadScene("Card");
    }
    public void ShieldButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        SceneManager.LoadScene("Shield");
    }

}
