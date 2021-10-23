using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AudioController : MonoBehaviour
{
    public AudioClip[] tapEffect;
    public AudioClip[] animeEffect;
    public AudioClip[] cutinEffect;
    public AudioClip[] battleEffect;

    void Start()
    {
        DontDestroyOnLoad(this);
    }
    public IEnumerator BGMChange(string title)
    {
        if (this.gameObject.GetComponent<AudioSource>().clip.name != title)
        {
            this.gameObject.GetComponent<AudioSource>().Pause();
            var bgm = Addressables.LoadAssetAsync<AudioClip>(title);
            yield return bgm;
            this.gameObject.GetComponent<AudioSource>().clip = bgm.Result;
            this.gameObject.GetComponent<AudioSource>().Play();
        }
    }
    public void VolumeChange(int volume)
    {
        this.gameObject.GetComponent<AudioSource>().volume = volume * 0.25f;
    }
    public void TapEffect(int effect)
    {
        if (tapEffect[effect] != null) this.gameObject.GetComponent<AudioSource>().PlayOneShot(tapEffect[effect]);
    }
    public void AnimeEffect(int effect)
    {
        if (animeEffect[effect] != null) this.gameObject.GetComponent<AudioSource>().PlayOneShot(animeEffect[effect]);
    }
    public void CutinEffect(int effect)
    {
        if (cutinEffect[effect] != null) this.gameObject.GetComponent<AudioSource>().PlayOneShot(cutinEffect[effect]);
    }
    public void BattleEffect(int effect)
    {
        if(battleEffect[effect]!=null) this.gameObject.GetComponent<AudioSource>().PlayOneShot(battleEffect[effect]);
    }
    public void BGMStop()
    {
        this.gameObject.GetComponent<AudioSource>().Pause();
    }
    public void BGMStart()
    {
        this.gameObject.GetComponent<AudioSource>().Play();
    }
}
