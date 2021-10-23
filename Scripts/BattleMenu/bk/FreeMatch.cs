using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FreeMatch : MonoBehaviour
{
    public void FreeMatchButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        PlayerPrefs.SetInt("MatchMode", 2);
        SceneManager.LoadScene("Matching");
    }
}
