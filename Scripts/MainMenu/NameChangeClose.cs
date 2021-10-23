using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameChangeClose : MonoBehaviour
{
    public GameObject nameChange;
    public void NameChangeCloseDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        nameChange.SetActive(false);
    }
}
