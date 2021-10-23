using BattleFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShieldBreak : MonoBehaviour
{
    public int areaNumber;
    public GameObject battleManager;
    public Display display;
    public void TargetClick()
    {
        Battle battle = battleManager.GetComponent<Battle>();
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(2);
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        this.gameObject.GetComponent<Button>().enabled = false;
        battle.targetShield = areaNumber;
        display.okButton.gameObject.GetComponent<Button>().interactable = true;
        for (int i = 0; i < display.shieldBreak.Length; i++)
        {
            display.shieldBreak[i].GetComponent<Button>().interactable = false;
        }
    }
    public void CheckClick()
    {
        Battle battle = battleManager.GetComponent<Battle>();
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        this.gameObject.GetComponent<Button>().enabled = true;
        battle.targetShield = 99;
        display.okButton.gameObject.GetComponent<Button>().interactable = false;
        for (int i = 0; i < display.shieldBreak.Length; i++)
        {
            display.shieldBreak[i].GetComponent<Button>().interactable = true;
        }
    }
}
