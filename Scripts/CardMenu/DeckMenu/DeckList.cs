using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckList : MonoBehaviour
{

    public int i;
    public Text num;
    public Text title;

    public void DeckEditDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetInt("deckNum", i);
        GameObject.Find("Loading").GetComponent<Loading>().LoadingStart();
        PlayerPrefs.SetString("deckEdit", "deck");
        SceneManager.LoadScene("DeckEdit");
    }
}
