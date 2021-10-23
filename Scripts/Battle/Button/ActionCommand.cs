using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCommand : MonoBehaviour
{
    public GameObject uponFrame;
    public GameObject upoffFrame;
    public GameObject dwononFrame;
    public GameObject dwonoffFrame;
    public GameObject battleManager;


    public void UpDwon()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        if (uponFrame.activeSelf)
        {
            uponFrame.SetActive(false);
            dwononFrame.SetActive(true);
        }
        else if(upoffFrame.activeSelf)
        {
            upoffFrame.SetActive(false);
            dwonoffFrame.SetActive(true);
        }
        else if (dwononFrame.activeSelf)
        {
            dwononFrame.SetActive(false);
            uponFrame.SetActive(true);
        }
        else if (dwonoffFrame.activeSelf)
        {
            dwonoffFrame.SetActive(false);
            upoffFrame.SetActive(true);
        }
    }
    public void LeftRight()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        if (uponFrame.activeSelf)
        {
            uponFrame.SetActive(false);
            upoffFrame.SetActive(true);
        }
        else if (upoffFrame.activeSelf)
        {
            upoffFrame.SetActive(false);
            uponFrame.SetActive(true);
        }
        else if (dwononFrame.activeSelf)
        {
            dwononFrame.SetActive(false);
            dwonoffFrame.SetActive(true);
        }
        else if (dwonoffFrame.activeSelf)
        {
            dwonoffFrame.SetActive(false);
            dwononFrame.SetActive(true);
        }
    }
    public void Attack()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(2);
        battleManager.GetComponent<Battle>().action = "atk";
        uponFrame.SetActive(false);
        upoffFrame.SetActive(false);
        dwononFrame.SetActive(false);
        dwonoffFrame.SetActive(false);
    }
    public void Move()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(2);
        battleManager.GetComponent<Battle>().action = "move";
        uponFrame.SetActive(false);
        upoffFrame.SetActive(false);
        dwononFrame.SetActive(false);
        dwonoffFrame.SetActive(false);
    }
    public void Stop()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(2);
        battleManager.GetComponent<Battle>().action = "rest";
        uponFrame.SetActive(false);
        upoffFrame.SetActive(false);
        dwononFrame.SetActive(false);
        dwonoffFrame.SetActive(false);
    }
    public void Action1()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(2);
        battleManager.GetComponent<Battle>().action = "actionSkill1";
        uponFrame.SetActive(false);
        upoffFrame.SetActive(false);
        dwononFrame.SetActive(false);
        dwonoffFrame.SetActive(false);
    }
    public void Action2()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(2);
        battleManager.GetComponent<Battle>().action = "actionSkill2";
        uponFrame.SetActive(false);
        upoffFrame.SetActive(false);
        dwononFrame.SetActive(false);
        dwonoffFrame.SetActive(false);
    }
}
