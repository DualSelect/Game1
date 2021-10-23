using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEnemyWindow : MonoBehaviour
{
    public GameObject deathWindow;
    public void DeathButtunDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        if (deathWindow.activeSelf)
        {
            deathWindow.SetActive(false);
        }
        else
        {
            deathWindow.SetActive(true);
        }
    }
}
