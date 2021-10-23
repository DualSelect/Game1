using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
//using Effekseer;
using System.Collections;

public class SpecialEffect : MonoBehaviour
{
    //public List<EffekseerEffectAsset> effectAssets = new List<EffekseerEffectAsset>();
    public Display display;
    public GameObject background;
    public Image specialImage;
    //public EffekseerEmitter emitter;
    public IEnumerator PlaySpecailEffect(Special special)
    {
        /*
        EffekseerEffectAsset effectColor = new EffekseerEffectAsset();
        switch (special.color)
        {
            case "青":
                effectColor = effectAssets[0];
                break;
            case "黄":
                effectColor = effectAssets[1];
                break;
            case "赤":
                effectColor = effectAssets[2];
                break;
            case "黒":
                effectColor = effectAssets[3];
                break;
            case "無":
                effectColor = effectAssets[4];
                break;
            default:
                break;
        }
        display.SpecialDisplay(special, specialImage);
        specialImage.gameObject.SetActive(false);
        background.SetActive(true);
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(9);
        emitter.Play(effectColor);
        yield return new WaitForSeconds(0.2f);
        specialImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        background.SetActive(false);
        */
        yield return new WaitForSeconds(0.5f);
    }
}
