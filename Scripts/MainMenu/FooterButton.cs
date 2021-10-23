using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FooterButton : MonoBehaviour
{
    public GameObject home;
    public GameObject battle;
    public GameObject card;
    public GameObject shop;
    public GameObject option;
    public GameObject deck;
    public GameObject single;


    private void Start()
    {
        switch (PlayerPrefs.GetString("menu"))
        {
            case "home":
                home.SetActive(true);
                break;
            case "battle":
                battle.SetActive(true);
                break;
            case "card":
                card.SetActive(true);
                break;
            case "shop":
                shop.SetActive(true);
                break;
            case "option":
                option.SetActive(true);
                break;
            case "deck":
                deck.SetActive(true);
                break;
            case "single":
                single.SetActive(true);
                break;
            default:
                home.SetActive(true);
                break;
        }
    }
    public void HomeButton()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        ContentOff();
        home.SetActive(true);
    }
    public void BattleButton()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        ContentOff();
        battle.SetActive(true);
    }
    public void CardButton()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        ContentOff();
        card.SetActive(true);
    }
    public void ShopButton()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        ContentOff();
        shop.SetActive(true);
    }
    public void OptionButton()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        ContentOff();
        option.SetActive(true);
    }
    public void DeckButton()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        ContentOff();
        deck.SetActive(true);
    }
    public void SingleButton()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        ContentOff();
        single.SetActive(true);
    }

    private void ContentOff()
    {
        home.SetActive(false);
        battle.SetActive(false);
        card.SetActive(false);
        shop.SetActive(false);
        option.SetActive(false);
        deck.SetActive(false);
        single.SetActive(false);
    }
}
