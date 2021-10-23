using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
public class NovelInitial : MonoBehaviour
{
    public Text area;
    public new Text name;
    public Text line;
    public Image chara;
    bool click=true;
    bool skip = false;
    bool choiseA = false;
    bool choiseB = false;
    public GameObject logWindow;
    public TestScenario testScenario;
    public AssetReference m_Ref;
    public GameObject content;
    int logNum;
    public ScrollRect scrollRect;
    public GameObject selectWindow;
    public Text selectA;
    public Text selectB;

    void Start()
    {
        switch (PlayerPrefs.GetString("novel"))
        {
            case "tuto1":
                break;
            default:
                StartCoroutine(testScenario.TestScenarioStart());
                break;
        }
    }

    public IEnumerator CharaAndLineChange(string chara,string name,string line)
    {
        while (!click && !skip)
        {
            yield return new WaitForSeconds(0.1f);
        }
        click = false;
        var image = Addressables.LoadAssetAsync<Sprite>(chara);
        yield return image;
        this.chara.sprite = image.Result;
        this.name.text = name;
        this.line.text = line;
        logNum++;
        Vector2 sd = new Vector2(0, 250 * logNum);
        content.GetComponent<RectTransform>().sizeDelta = sd;
        var prefab = Addressables.LoadAssetAsync<GameObject>(m_Ref);
        yield return prefab;
        Vector3 v = new Vector3(0, -250 * (logNum - 1), 0);
        GameObject gameObject = Instantiate(prefab.Result, v, Quaternion.identity);
        LogPrefab logPrefab = gameObject.GetComponent<LogPrefab>();
        logPrefab.name.text = this.name.text;
        logPrefab.line.text = this.line.text;
        gameObject.transform.SetParent(content.transform, false);
    }
    public IEnumerator LineChange(string line)
    {
        while (!click && !skip)
        {
            yield return new WaitForSeconds(0.1f);
        }
        click = false;
        this.line.text = line;
        logNum++;
        Vector2 sd = new Vector2(0, 250 * logNum);
        content.GetComponent<RectTransform>().sizeDelta = sd;
        var prefab = Addressables.LoadAssetAsync<GameObject>(m_Ref);
        yield return prefab;
        Vector3 v = new Vector3(0,  - 250 * (logNum-1), 0);
        GameObject gameObject = Instantiate(prefab.Result, v, Quaternion.identity);
        LogPrefab logPrefab = gameObject.GetComponent<LogPrefab>();
        logPrefab.name.text = this.name.text;
        logPrefab.line.text = this.line.text;
        gameObject.transform.SetParent(content.transform, false);
    }
    public void AreaChange(string area)
    {
        this.area.text = area;
    }

    public IEnumerator SelectChoise(string a,string b, UnityEngine.Events.UnityAction<string> callback)
    {
        selectWindow.SetActive(true);
        selectA.text = a;
        selectB.text = b;
        while (!choiseA && !choiseB)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (choiseA) callback("A");
        if (choiseB) callback("B");
        choiseA = false;
        choiseB = false;
        selectWindow.SetActive(false);
    }
    public void LineClick()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        click = true;
    }
    public void SkipClick()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        skip = true;
    }
    public void LogClick()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        scrollRect.verticalNormalizedPosition = 0f;
        logWindow.SetActive(true);
    }
    public void LogClose()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        logWindow.SetActive(false);
    }
    public void ChoiseClickA()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        choiseA = true;
        click = true;
    }
    public void ChoiseClickB()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        choiseB = true;
        click = true;
    }
}
