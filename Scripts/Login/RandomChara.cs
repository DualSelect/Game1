using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
public class RandomChara : MonoBehaviour
{
    System.Random r = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        ///StartCoroutine(RandomLoad());
    }
    public IEnumerator RandomLoad()
    {
        string load = r.Next(1,181).ToString("000");
        var op = Addressables.LoadAssetAsync<Sprite>(load);
        //非同期なのでopがCompleteするまで待つ
        yield return op;
        this.GetComponent<Image>().sprite = op.Result;
    }
}
