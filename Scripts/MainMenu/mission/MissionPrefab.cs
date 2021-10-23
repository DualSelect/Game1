using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionPrefab : MonoBehaviour
{
    public string missionGroup;
    public string missionName;
    public Text rule;
    public Text reward;
    public Text count;
    public Button receive;
    public MissionControl mission; 

    void Start()
    {
        mission = GameObject.Find("Mission").GetComponent<MissionControl>();
    }
    public void ReseiveBottun()
    {
        StartCoroutine(mission.Receive(missionGroup,missionName));
    }
}
