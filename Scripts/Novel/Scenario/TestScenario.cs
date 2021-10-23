using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScenario : MonoBehaviour
{
    public NovelInitial novel;
    public IEnumerator TestScenarioStart()
    {
        yield return GameObject.Find("AudioBGM").GetComponent<AudioController>().BGMChange("Minstrel2_Harp");
        novel.AreaChange("辺境の森");
        yield return novel.CharaAndLineChange("dummy", "システムメッセージ", "テスト開始");
        yield return novel.LineChange("1");
        yield return novel.LineChange("2");
        yield return novel.LineChange("3");
        string choise = "";
        yield return novel.SelectChoise("a", "b", (result) => choise = result);
        if (choise == "A")
        {
            yield return novel.LineChange("aを選択した");
        }
        if (choise == "B")
        {
            yield return novel.LineChange("bを選択した");
        }
        yield return novel.CharaAndLineChange("dummy", "システムメッセージ", "テスト終了");
    }
}
