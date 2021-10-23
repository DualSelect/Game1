using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FooterButton2 : MonoBehaviour
{
    public void HomeButton2()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetString("menu", "home");
        SceneManager.LoadScene("MainMenu");
    }
    public void BattleButton2()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetString("menu", "battle");
        SceneManager.LoadScene("MainMenu");
    }
    public void CardButton2()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetString("menu", "card");
        SceneManager.LoadScene("MainMenu");
    }
    public void ShopButton2()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetString("menu", "shop");
        SceneManager.LoadScene("MainMenu");
    }
    public void OptionButton2()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetString("menu", "option");
        SceneManager.LoadScene("MainMenu");
    }
}
