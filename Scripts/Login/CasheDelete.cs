using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CasheDelete : MonoBehaviour
{
    public GameObject casheDeleteMenu;
    public GameObject login;
    public GameObject bgm;
    public GameObject tap;
    public GameObject anime;
    public GameObject particle;
    public GameObject camera2;
    public GameObject load;


    public void DoCasheDelete()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        Caching.ClearCache();
        PlayerPrefs.DeleteAll();
        casheDeleteMenu.SetActive(false);
        GameObject.Destroy(login);
        GameObject.Destroy(bgm);
        GameObject.Destroy(tap);
        GameObject.Destroy(anime);
        GameObject.Destroy(particle);
        GameObject.Destroy(camera2);
        GameObject.Destroy(load);

        SceneManager.LoadScene("Login");
    }
}
