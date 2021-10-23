using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OkButton : MonoBehaviour
{
    public GameObject battleManager;
    public bool deck; 
    public void OkButtonDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(2);
        this.gameObject.GetComponent<Button>().interactable = false;
        battleManager.GetComponent<Battle>().okSwitch = true;
    }
    private void Start()
    {
        if(!deck)StartCoroutine(SizeChange());  
    }
    private IEnumerator SizeChange()
    {
        while (true)
        {
            if(this.GetComponent<Button>().IsInteractable())this.transform.localScale = new Vector3(1.2f, 1.2f, 1);
            yield return new WaitForSeconds(0.2f);
            this.transform.localScale = new Vector3(1, 1, 1);
            yield return new WaitForSeconds(1f);
        }
    }
}
