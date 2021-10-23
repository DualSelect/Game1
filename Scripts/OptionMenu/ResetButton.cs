using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public void Reset()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        GameObject.Destroy(GameObject.Find("Login"));
        GameObject.Destroy(GameObject.Find("AudioBGM"));
        GameObject.Destroy(GameObject.Find("AudioTapEffect"));
        GameObject.Destroy(GameObject.Find("AudioAnimeEffect"));
        GameObject.Destroy(GameObject.Find("Particle System"));
        GameObject.Destroy(GameObject.Find("Camera"));
        GameObject.Destroy(GameObject.Find("Loading"));
        SceneManager.LoadScene(0);
    }
}