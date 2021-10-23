using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolume : MonoBehaviour
{
    public Text[] bgm;
    public Text[] se;
    int bgmVolume;
    int seVolume;
    public GameObject window;
    void Start()
    {
        ColorReset();

    }
    private void ColorReset()
    {
        for(int i = 0; i < bgm.Length; i++)
        {
            bgm[i].color = new Color(1f, 1f, 1f);
            se[i].color = new Color(1f, 1f, 1f);
        }
                bgm[PlayerPrefs.GetInt("BGMVolume")].color = new Color(0f, 0f, 0f);
        se[PlayerPrefs.GetInt("SEVolume")].color = new Color(0f, 0f, 0f);
    }
    public void BGM0()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetInt("BGMVolume", 0);
        ColorReset();
        GameObject.Find("AudioBGM").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("BGMVolume"));
    }
    public void BGM1()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetInt("BGMVolume", 1);
        ColorReset();
        GameObject.Find("AudioBGM").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("BGMVolume"));
    }
    public void BGM2()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetInt("BGMVolume", 2);
        ColorReset();
        GameObject.Find("AudioBGM").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("BGMVolume"));
    }
    public void BGM3()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetInt("BGMVolume", 3);
        ColorReset();
        GameObject.Find("AudioBGM").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("BGMVolume"));
    }
    public void BGM4()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetInt("BGMVolume", 4);
        ColorReset();
        GameObject.Find("AudioBGM").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("BGMVolume"));
    }
    public void SE0()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetInt("SEVolume", 0);
        ColorReset();
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("SEVolume"));
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("SEVolume"));
    }
    public void SE1()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetInt("SEVolume", 1);
        ColorReset();
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("SEVolume"));
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("SEVolume"));
    }
    public void SE2()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetInt("SEVolume", 2);
        ColorReset();
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("SEVolume"));
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("SEVolume"));
    }
    public void SE3()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetInt("SEVolume", 3);
        ColorReset();
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("SEVolume"));
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("SEVolume"));
    }
    public void SE4()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetInt("SEVolume", 4);
        ColorReset();
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("SEVolume"));
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("SEVolume"));
    }

    public void Decide()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        window.SetActive(false);
    }
    public void VolumeBottun()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        window.SetActive(true);
    }
}
