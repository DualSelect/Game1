using System.Collections;
using UnityEngine;
using BattleFolder;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using System.Text;
using System.IO;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour
{
    Display display;
    public CardMaster cardMaster;
    public ShieldMaster shieldMaster;
    public SpecialMaster specialMaster;
    public BattleStatus battleStatus;
    public BattleStatus battleStatusEnemy;
    public bool okSwitch;
    public bool specialSwitch=false;
    int time;
    BattleJson battleJson = new BattleJson();
    public bool[,] targetSelect = new bool[2,9];
    public int targetCount;
    public int targetShield;
    public string prevSetCardEnemy;
    public CutInEffect cutInEffect;
    public AreaEffect areaEffect;
    string json;
    public string action;
    void Start()
    {
        display = this.gameObject.GetComponent<Display>();
        display.okButton.interactable = false;
        StartCoroutine(Player());
    }
    public IEnumerator Player()
    {
        //クライアントから送信A(プレイヤーID、デッキ番号)
        ClientToServerA clientToServerA = new ClientToServerA(PlayerPrefs.GetString("ID"), 1);
        if (GameObject.Find("Login").GetComponent<LoginInitial>().test) clientToServerA = new ClientToServerA("cfd30423-79c0-4e09-bdc3-b5cc0b4fd6cc", 1);
        json = battleJson.toJsonA(clientToServerA);
        WriteFile(json);
        yield return RoadFileCheak();
        ServerToClient1 serverToClient1 = battleJson.toClass1(RoadFile());
        //盤面情報を初期化
        battleStatus = BattleStatusInitialize(serverToClient1);
        battleStatusEnemy = BattleStatusEnemyInitialize(serverToClient1);
        //盤面情報を画面に表示する
        StartCoroutine(DisplayInitialize(battleStatus, battleStatusEnemy));
        display.matchingWindow.SetActive(true);
        yield return BGMChange();
        GameObject.Find("Loading").GetComponent<Loading>().LoadingEnd();
        yield return display.matchingWindow.GetComponent<MatchingWindow>().MatchingDisplay();
        //バトル処理開始
        while (true)
        {
            Debug.Log("ターン開始");
            //ターン開始処理
            yield return SpPhase();
            //ターン終了処理
            yield return EndPhase();
            //セット処理
            yield return SetPhase();
            //オープン処理
            yield return OpenPhase();
            //リムーブ処理
            for (int i = 0; i < 9; i++)
            {
                if ((battleStatus.unitStatus[i].unitId != "" && battleStatus.unitStatus[i].close) || (battleStatusEnemy.unitStatus[i].unitId != "" && battleStatusEnemy.unitStatus[i].close))
                {
                    yield return RemovePhase();
                    break;
                }
            }
            //スタート処理
            for (int i = 0; i < 9; i++)
            {
                if (battleStatus.unitStatus[i].unitId != "" && !battleStatus.unitStatus[i].action && !battleStatus.unitStatus[i].close)
                {
                    if (cardMaster.CardList.Find(m => m.itemId == battleStatus.unitStatus[i].unitId).startSkillName!="") {
                        yield return StartPhase();
                        break;
                    }
                }
                if (battleStatusEnemy.unitStatus[i].unitId != "" && !battleStatusEnemy.unitStatus[i].action && !battleStatusEnemy.unitStatus[i].close)
                {
                    if (cardMaster.CardList.Find(m => m.itemId == battleStatusEnemy.unitStatus[i].unitId).startSkillName != "")
                    {
                        yield return StartPhase();
                        break;
                    }
                }
            }
            //バトル処理
            for (int i=0; i < 9; i++)
            {
                if ((battleStatus.unitStatus[i].unitId != "" && !battleStatus.unitStatus[i].action && !battleStatus.unitStatus[i].close)|| (battleStatusEnemy.unitStatus[i].unitId != "" && !battleStatusEnemy.unitStatus[i].action && !battleStatusEnemy.unitStatus[i].close))
                {
                    yield return BattlePhase();
                    break;
                }
            }
        }
    }
    private IEnumerator EndPhase()
    {
        StartCoroutine(DisplayPhase("準備フェーズ"));
        //セットカードの配置
        if (battleStatus.setCard != 99)
        {
            TargetList targetList = new TargetList();
            targetList.playerId = battleStatus.playerId;
            int targetNum = 0;
            for (int i = 0; i < 9; i++)
            {
                if (battleStatus.unitStatus[i].unitId == "")
                {
                    targetNum++;
                }
            }
            int[] fieldNumber = new int[targetNum];
            targetNum = 0;
            for (int i = 0; i < 9; i++)
            {
                if (battleStatus.unitStatus[i].unitId == "")
                {
                    fieldNumber[targetNum] = i;
                    targetNum++;
                }
            }
            targetList.list = fieldNumber;
            TargetList[] targetLists = new TargetList[1];
            targetLists[0] = targetList;
            yield return TargetSelect(targetLists,1);
            for (int i = 0; i < 9; i++)
            {
                if (targetSelect[0, i]) battleStatus.setPosition = i.ToString();
            }
            yield return display.CardDisplay(battleStatus.deckStatus[battleStatus.setCard].unitId, display.fieldUnit[int.Parse(battleStatus.setPosition)]);
            Card card = cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[battleStatus.setCard].unitId);
            battleStatus.unitStatus[int.Parse(battleStatus.setPosition)].SetUnit(card, battleStatus.setCard);
            display.setCard.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            battleStatus.setPosition = "";
        }
        //行動権復活
        for (int i = 0; i < 9; i++)
        {
            if (battleStatus.unitStatus[i].unitId != "" && !battleStatus.unitStatus[i].close)
            {
                UpdateActionEnd(false, i, false);
            }
            if (battleStatusEnemy.unitStatus[i].unitId != "" && !battleStatusEnemy.unitStatus[i].close)
            {
                UpdateActionEnd(true, i, false);
            }
        }
        //復活の選択
        {
            Target target = new Target();
            TargetList targetList = new TargetList();
            targetList.playerId = battleStatus.playerId;
            int targetNum = 0;
            for (int i = 0; i < 9; i++)
            {
                if (battleStatus.unitStatus[i].unitId != "" && battleStatus.unitStatus[i].close == true)
                {
                    targetNum++;
                }
            }
            if (targetNum > 0)
            {
                int[] fieldNumber = new int[targetNum];
                targetNum = 0;
                for (int i = 0; i < 9; i++)
                {
                    if (battleStatus.unitStatus[i].unitId != "" && battleStatus.unitStatus[i].close == true)
                    {
                        fieldNumber[targetNum] = i;
                        targetNum++;
                    }
                }
                targetList.list = fieldNumber;
                TargetList[] targetLists = new TargetList[1];
                targetLists[0] = targetList;
                target.targetList = targetLists;
                target.selectCount = 1;
                Target[] targets = new Target[1];
                targets[0] = target;
                yield return RevivalSelect(targets);
            }
        }
    }
    private IEnumerator AutoSelect(ServerToClient6 serverToClient6, UnityEngine.Events.UnityAction<ServerToClient6B> callback)
    {
        ClientToServerC clientToServerC = new ClientToServerC();
        clientToServerC.playerId = battleStatus.playerId;
        clientToServerC.phase = "autoSelect";
        if (serverToClient6.nextPlayerId == battleStatus.playerId)
        {
            Target[] target = new Target[0];
            yield return TargetSelect(serverToClient6.autoTargetList, (result) => target = result);
            clientToServerC.target = target;
            clientToServerC.fieldNumber = serverToClient6.nextFieldNumber;
        }
        string json = battleJson.toJsonC(clientToServerC);
        WriteFile(json);
        yield return RoadFileCheak();
        ServerToClient6B serverToClient6B = battleJson.toClass6B(RoadFile());
        callback(serverToClient6B);
    }
    private IEnumerator ShieldSelect(ServerToClient6B serverToClient6B,bool enemy,UnityEngine.Events.UnityAction<ServerToClient6B> callback)
    {
        ClientToServerC clientToServerC = new ClientToServerC();
        clientToServerC.playerId = battleStatus.playerId;
        clientToServerC.phase = "shieldSelect";
        if (enemy)
        {
            Target[] target = new Target[0];
            yield return TargetSelect(serverToClient6B.targetList, (result) => target = result);
            clientToServerC.target = target;
        }
        string json = battleJson.toJsonC(clientToServerC);
        WriteFile(json);
        yield return RoadFileCheak();
        serverToClient6B = battleJson.toClass6B(RoadFile());
        callback(serverToClient6B);
    }
    private IEnumerator ActionSelect(ServerToClient6B serverToClient6B,int nextFieldNumber,bool enemy, UnityEngine.Events.UnityAction<ServerToClient6B> callback)
    {
        ClientToServerC clientToServerC = new ClientToServerC();
        clientToServerC.playerId = battleStatus.playerId;
        if (serverToClient6B.battleAction == "actionSkill1") clientToServerC.phase = "skill1Select";
        if (serverToClient6B.battleAction == "actionSkill2") clientToServerC.phase = "skill2Select";
        clientToServerC.fieldNumber = nextFieldNumber;
        if (!enemy)
        {
            Target[] target = new Target[0];
            yield return TargetSelect(serverToClient6B.targetList, (result) => target = result);
            clientToServerC.target = target;
        }
        string json = battleJson.toJsonC(clientToServerC);
        WriteFile(json);
        yield return RoadFileCheak();
        serverToClient6B = battleJson.toClass6B(RoadFile());
        callback(serverToClient6B);
    }
    private IEnumerator BattlePhase()
    {
        StartCoroutine(DisplayPhase("戦闘フェーズ"));
        ClientToServerF clientToServerF = new ClientToServerF(battleStatus.playerId);
        string json = battleJson.toJsonF(clientToServerF);
        WriteFile(json);
        yield return RoadFileCheak();
        ServerToClient6 serverToClient6 = battleJson.toClass6(RoadFile());
        bool enemy = true;

        while (serverToClient6.nextPlayerId != "end")
        {
            int nextFieldNumber = serverToClient6.nextFieldNumber.Value;
            Card card = new Card();
            //行動選択
            if (serverToClient6.nextPlayerId == battleStatusEnemy.playerId)
            {
                enemy = true;
                card = cardMaster.CardList.Find(m => m.itemId == battleStatusEnemy.unitStatus[nextFieldNumber].unitId);
                if (card.autoSkillName != "")
                {
                    SkillCutIn(serverToClient6.nextPlayerId, nextFieldNumber, "auto");
                    UpdateSp(enemy, battleStatusEnemy.sp - card.autoSkillSp);
                    if (serverToClient6.autoTargetList!=null)
                    if (serverToClient6.autoTargetList.Length != 0)
                    {
                        ServerToClient6B serverToClient6B2 = new ServerToClient6B();
                        yield return AutoSelect(serverToClient6, (result) => serverToClient6B2 = result);
                        serverToClient6.autoUpdateInfo = serverToClient6B2.updateInfo;
                    }
                    else
                    {
                        yield return new WaitForSeconds(1f);
                    }
                    ServerToClient7 serverToClient7 = new ServerToClient7();
                    yield return SkillEffect(serverToClient6.nextPlayerId == battleStatusEnemy.playerId, card, "auto", serverToClient6.autoUpdateInfo, (result) => serverToClient7 = result);

                    if (battleStatusEnemy.unitStatus[nextFieldNumber].action || battleStatusEnemy.unitStatus[nextFieldNumber].close)
                    {
                            json = battleJson.toJsonF(clientToServerF);
                            WriteFile(json);
                            yield return RoadFileCheak();
                            serverToClient6 = battleJson.toClass6(RoadFile());
                            continue;
                    }

                }
                display.damageEnemy[nextFieldNumber].color = new Color(1f, 1f, 1f);
                display.damageEnemy[nextFieldNumber].text = "act";
                display.damageEnemy[nextFieldNumber].gameObject.SetActive(true);
                ClientToServerF1 clientToServerF1 = new ClientToServerF1(battleStatus.playerId);
                json = battleJson.toJsonF1(clientToServerF1);
                WriteFile(json);
            }
            if (serverToClient6.nextPlayerId == battleStatus.playerId)
            {
                enemy = false;
                ClientToServerF1 clientToServerF1 = new ClientToServerF1(battleStatus.playerId);
                clientToServerF1.fieldNumber = nextFieldNumber;
                card = cardMaster.CardList.Find(m => m.itemId == battleStatus.unitStatus[nextFieldNumber].unitId);
                if (card.autoSkillName != "")
                {
                    SkillCutIn(serverToClient6.nextPlayerId, nextFieldNumber, "auto");
                    UpdateSp(enemy, battleStatus.sp - card.autoSkillSp);
                    if (serverToClient6.autoTargetList != null)
                    if (serverToClient6.autoTargetList.Length != 0)
                    {
                        ServerToClient6B serverToClient6B2 = new ServerToClient6B();
                        yield return AutoSelect(serverToClient6, (result) => serverToClient6B2 = result);
                        serverToClient6.autoUpdateInfo = serverToClient6B2.updateInfo;
                    }
                    else
                    {
                        yield return new WaitForSeconds(1f);
                    }
                    ServerToClient7 serverToClient7 = new ServerToClient7();
                    yield return SkillEffect(serverToClient6.nextPlayerId == battleStatusEnemy.playerId, card, "auto", serverToClient6.autoUpdateInfo, (result) => serverToClient7 = result);
                    if (battleStatus.unitStatus[nextFieldNumber].action || battleStatus.unitStatus[nextFieldNumber].close)
                    {
                        json = battleJson.toJsonF(clientToServerF);
                        WriteFile(json);
                        yield return RoadFileCheak();
                        serverToClient6 = battleJson.toClass6(RoadFile());
                        continue;
                    }
                }
                
                display.damage[nextFieldNumber].color = new Color(1f, 1f, 1f);
                display.damage[nextFieldNumber].text = "act";
                display.damage[nextFieldNumber].gameObject.SetActive(true);
                bool enemyUnit = false;
                for (int i = 0; i < 9; i++)
                {
                    if (battleStatusEnemy.unitStatus[i].unitId != "" && !battleStatusEnemy.unitStatus[i].close)
                    {
                        enemyUnit = true;
                        break;
                    }
                }
                bool areaSpace = false;
                for (int i = 0; i < 9; i++)
                {
                    if (battleStatus.unitStatus[i].unitId == "")
                    {
                        areaSpace = true;
                        break;
                    }
                }
                for (int i = 0; i < display.action1.Length; i++)
                {
                    display.action1[i].GetComponent<Text>().text = card.actionSkillName1;
                    display.action1[i].transform.GetChild(0).GetComponent<Text>().text = card.actionSkillDetail1;
                    display.action2[i].GetComponent<Text>().text = card.actionSkillName2;
                    display.action2[i].transform.GetChild(0).GetComponent<Text>().text = card.actionSkillDetail2;
                    if (card.actionSkillName1 == "")
                    {
                        display.action1[i].GetComponent<Button>().interactable = false;
                    }
                    else
                    {
                        display.action1[i].GetComponent<Button>().interactable = true;
                    }
                    if (card.actionSkillName2 == "")
                    {
                        display.action2[i].GetComponent<Button>().interactable = false;
                    }
                    else
                    {
                        display.action2[i].GetComponent<Button>().interactable = true;
                    }
                    if (enemyUnit)
                    {
                        display.atackBreak[i].GetComponent<Text>().text = "攻撃";
                        display.atackBreak[i].GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        display.atackBreak[i].GetComponent<Text>().text = "ブレイク";
                        if (battleStatus.sp > 0)
                        {
                            display.atackBreak[i].GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            display.atackBreak[i].GetComponent<Button>().interactable = false;
                        }
                    }
                    if (areaSpace)
                    {
                        display.move[i].GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        display.move[i].GetComponent<Button>().interactable = false;
                    }
                }
                action = "";
                display.command.SetActive(true);
                while (action == "")
                {
                    yield return new WaitForSeconds(1.0f);
                }
                if (action == "atk" && !enemyUnit) action = "break";

                if (action == "break")
                {
                    if (battleStatusEnemy.life > 1)
                    {
                        for (int i = 0; i < display.shieldBreak.Length; i++)
                        {
                            if (battleStatusEnemy.shieldStatus[i].shieldId == "")
                            {
                                display.shieldBreak[i].SetActive(true);
                                display.shieldBreak[i].GetComponent<Button>().interactable = true;
                            }
                        }
                        okSwitch = false;
                        while (!okSwitch)
                        {
                            yield return new WaitForSeconds(1f);
                        }
                        for (int i = 0; i < display.shieldBreak.Length; i++)
                        {
                            display.shieldBreak[i].SetActive(false);
                            display.shieldBreak[i].GetComponent<Button>().interactable = false;
                            display.shieldBreak[i].gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        }
                        display.okButton.interactable = false;
                        clientToServerF1.battleAction = action + targetShield;
                    }
                    else
                    {
                        clientToServerF1.battleAction = action;
                    }
                }
                else if (action == "move")
                {
                    TargetList targetList = new TargetList();
                    targetList.playerId = battleStatus.playerId;
                    int targetNum = 0;
                    for (int i = 0; i < 9; i++)
                    {
                        if (battleStatus.unitStatus[i].unitId == "")
                        {
                            targetNum++;
                        }
                    }
                    int[] fieldNumber = new int[targetNum];
                    targetNum = 0;
                    for (int i = 0; i < 9; i++)
                    {
                        if (battleStatus.unitStatus[i].unitId == "")
                        {
                            fieldNumber[targetNum] = i;
                            targetNum++;
                        }
                    }
                    targetList.list = fieldNumber;
                    TargetList[] targetLists = new TargetList[1];
                    targetLists[0] = targetList;
                    yield return TargetSelect(targetLists, 1);
                    for (int i = 0; i < 9; i++)
                    {
                        if (targetSelect[0, i]) clientToServerF1.battleAction = action + i.ToString();
                    }
                }
                else
                {
                    clientToServerF1.battleAction = action;

                }
                json = battleJson.toJsonF1(clientToServerF1);
                WriteFile(json);
            }
            yield return RoadFileCheak();
            ServerToClient6B serverToClient6B = battleJson.toClass6B(RoadFile());
            //結果反映
            if (enemy)
            {
                display.damageEnemy[nextFieldNumber].text = "";
                if (serverToClient6B.battleAction.Contains("break"))
                {
                    int i = int.Parse(serverToClient6B.battleAction.Substring(5));
                    display.unitSkillWindowEnemy.GetComponent<SkillWindow>().UnitSkillWindow(card, "ブレイク", "マナ1消費してシールドにダメージを与える");
                    battleStatusEnemy.sp--;
                    display.spEnemy.text = battleStatusEnemy.sp.ToString();
                    GameObject shield = display.shieldCards[i].gameObject.transform.parent.parent.gameObject;
                    shield.transform.localPosition = new Vector3(shield.transform.localPosition.x, shield.transform.localPosition.y-20,0);
                    areaEffect.ShieldAreaEffect(enemy, i, card);
                    GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(7);
                    shield.transform.localPosition = new Vector3(shield.transform.localPosition.x, shield.transform.localPosition.y + 20, 0);
                    battleStatus.shieldStatus[i].shieldHp--;
                    battleStatus.life--;
                    display.life.text = battleStatus.life.ToString();
                    display.shieldCards[i].gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = battleStatus.shieldStatus[i].shieldHp.ToString();
                }
                if (serverToClient6B.battleAction == "atk") display.unitSkillWindowEnemy.GetComponent<SkillWindow>().UnitSkillWindow(card, "攻撃", "射程内のランダムな敵1体にATK分ダメージを与える");
                if (serverToClient6B.battleAction.Contains("move")) display.unitSkillWindowEnemy.GetComponent<SkillWindow>().UnitSkillWindow(card, "移動", "空いている別のエリアに移動する");
                if (serverToClient6B.battleAction == "rest") display.unitSkillWindowEnemy.GetComponent<SkillWindow>().UnitSkillWindow(card, "待機", "最大HPの1/4回復する");
                if (serverToClient6B.battleAction == "actionSkill1")
                {
                    display.unitSkillWindowEnemy.GetComponent<SkillWindow>().UnitSkillWindow(card, card.actionSkillName1, card.actionSkillDetail1);
                    UpdateSp(enemy, battleStatusEnemy.sp - card.actionSkillSp1);
                }
                if (serverToClient6B.battleAction == "actionSkill2")
                {
                    display.unitSkillWindowEnemy.GetComponent<SkillWindow>().UnitSkillWindow(card, card.actionSkillName2, card.actionSkillDetail2);
                    UpdateSp(enemy, battleStatusEnemy.sp - card.actionSkillSp2);
                }
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                display.damage[nextFieldNumber].text = "";
                if (serverToClient6B.battleAction.Contains("break"))
                {
                    int i = int.Parse(serverToClient6B.battleAction.Substring(5));
                    display.unitSkillWindow.GetComponent<SkillWindow>().UnitSkillWindow(card, "ブレイク", "マナ1消費してシールドにダメージを与える");
                    battleStatus.sp--;
                    display.sp.text = battleStatus.sp.ToString();
                    GameObject shield = display.shieldCardsEnemy[i].gameObject.transform.parent.parent.gameObject;
                    shield.transform.localPosition = new Vector3(shield.transform.localPosition.x, shield.transform.localPosition.y - 20, 0);
                    areaEffect.ShieldAreaEffect(enemy, i, card);
                    GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(7);
                    shield.transform.localPosition = new Vector3(shield.transform.localPosition.x, shield.transform.localPosition.y + 20, 0);
                    battleStatusEnemy.shieldStatus[i].shieldId = serverToClient6B.shieldId;
                    battleStatusEnemy.shieldStatus[i].shieldHp++;
                    battleStatusEnemy.life--;
                    display.lifeEnemy.text = battleStatusEnemy.life.ToString();
                    display.shieldCardsEnemy[i].gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = battleStatusEnemy.shieldStatus[i].shieldHp.ToString();
                }
                if (serverToClient6B.battleAction == "atk") display.unitSkillWindow.GetComponent<SkillWindow>().UnitSkillWindow(card, "攻撃", "射程内のランダムな敵1体にATK分ダメージを与える");
                if (serverToClient6B.battleAction.Contains("move")) display.unitSkillWindow.GetComponent<SkillWindow>().UnitSkillWindow(card, "移動", "空いている別のエリアに移動する");
                if (serverToClient6B.battleAction == "rest") display.unitSkillWindow.GetComponent<SkillWindow>().UnitSkillWindow(card, "待機", "最大HPの1/4回復する");
                if (serverToClient6B.battleAction == "actionSkill1")
                {
                    display.unitSkillWindow.GetComponent<SkillWindow>().UnitSkillWindow(card, card.actionSkillName1, card.actionSkillDetail1);
                    UpdateSp(enemy, battleStatus.sp - card.actionSkillSp1);
                }
                if (serverToClient6B.battleAction == "actionSkill2")
                {
                    display.unitSkillWindow.GetComponent<SkillWindow>().UnitSkillWindow(card, card.actionSkillName2, card.actionSkillDetail2);
                    UpdateSp(enemy, battleStatus.sp - card.actionSkillSp2);
                }
                yield return new WaitForSeconds(1.0f);
            }
            if (serverToClient6B.battleAction.Contains("action")) 
            { 
                if (serverToClient6B.targetList.Length != 0)
                {
                    ServerToClient6B serverToClient6B_2 = new ServerToClient6B();
                    yield return ActionSelect(serverToClient6B,nextFieldNumber,enemy,(result) => serverToClient6B_2 = result);
                    serverToClient6B.updateInfo = serverToClient6B_2.updateInfo;
                }
            }
            if (serverToClient6B.battleAction.Contains("break"))
            {
                int i = int.Parse(serverToClient6B.battleAction.Substring(5));
                if (serverToClient6B.shieldId!=null && serverToClient6B.targetList.Length == 0)
                {
                    if (enemy)
                    {

                        yield return display.openDisplay.GetComponent<OpenDisplay>().DoOpenDisplayPlayer(serverToClient6B.shieldId);
                        Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == battleStatus.shieldStatus[i].shieldId);
                        if (battleStatus.sp >= shield.skillSp)
                        {
                            battleStatus.sp = battleStatus.sp - shield.skillSp;
                            display.sp.text = battleStatus.sp.ToString();
                            yield return display.shieldSkillWindow.GetComponent<SkillWindow>().ShieldSkillWindow2(shield, shield.name, shield.skillDetail);
                        }
                        AddSpecial(false, 10);
                    }
                    else
                    {
                        yield return display.openDisplay.GetComponent<OpenDisplay>().DoOpenDisplayEnemy(serverToClient6B.shieldId);
                        Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == battleStatusEnemy.shieldStatus[i].shieldId);
                        yield return display.ShieldDisplay(shield, display.shieldCardsEnemy[i]);
                        if (battleStatusEnemy.sp >= shield.skillSp)
                        {
                            battleStatusEnemy.sp = battleStatusEnemy.sp - shield.skillSp;
                            display.spEnemy.text = battleStatusEnemy.sp.ToString();
                            yield return display.shieldSkillWindowEnemy.GetComponent<SkillWindow>().ShieldSkillWindow2(shield, shield.name, shield.skillDetail);
                        }
                        AddSpecial(true, 10);
                    }
                }
                else if (serverToClient6B.targetList.Length != 0)
                {
                    if (enemy)
                    {
                        yield return display.openDisplay.GetComponent<OpenDisplay>().DoOpenDisplayEnemy(serverToClient6B.shieldId);
                        Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == battleStatus.shieldStatus[i].shieldId);
                        if (battleStatus.sp >= shield.skillSp)
                        {
                            battleStatus.sp = battleStatus.sp - shield.skillSp;
                            display.sp.text = battleStatus.sp.ToString();
                            yield return display.shieldSkillWindow.GetComponent<SkillWindow>().ShieldSkillWindow2(shield, shield.name, shield.skillDetail);
                        }
                        AddSpecial(false, 10);
                        ServerToClient6B serverToClient6B_2 = new ServerToClient6B();
                        yield return ShieldSelect(serverToClient6B,enemy, (result) => serverToClient6B_2 = result);
                        serverToClient6B.updateInfo = serverToClient6B_2.updateInfo;
                    }
                    else
                    {
                        yield return display.openDisplay.GetComponent<OpenDisplay>().DoOpenDisplayPlayer(serverToClient6B.shieldId);
                        Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == battleStatusEnemy.shieldStatus[i].shieldId);
                        yield return display.ShieldDisplay(shield, display.shieldCardsEnemy[i]);
                        if (battleStatusEnemy.sp >= shield.skillSp)
                        {
                            battleStatusEnemy.sp = battleStatusEnemy.sp - shield.skillSp;
                            display.spEnemy.text = battleStatusEnemy.sp.ToString();
                            yield return display.shieldSkillWindowEnemy.GetComponent<SkillWindow>().ShieldSkillWindow2(shield, shield.name, shield.skillDetail);
                        }
                        AddSpecial(true, 10);
                        ServerToClient6B serverToClient6B_2 = new ServerToClient6B();
                        yield return ShieldSelect(serverToClient6B, enemy, (result) => serverToClient6B_2 = result);
                        serverToClient6B.updateInfo = serverToClient6B_2.updateInfo;
                    }
                }
            }
            ServerToClient7 serverToClient7_2 = new ServerToClient7();
            if (serverToClient6B.battleAction.Contains("move")) UpdateActionEnd(enemy, nextFieldNumber, true);
            yield return SkillEffect(enemy,card, serverToClient6B.battleAction, serverToClient6B.updateInfo, (result) => serverToClient7_2 = result);
            //行動ユニット終了
            if(!serverToClient6B.battleAction.Contains("move"))UpdateActionEnd(enemy, nextFieldNumber, true);
            //次のループに向けた準備
            json = battleJson.toJsonF(clientToServerF);
            WriteFile(json);
            yield return RoadFileCheak();
            serverToClient6 = battleJson.toClass6(RoadFile());
        }
    }
    private void SkillCutIn(string playerId ,int fieldNumber,string phase)
    {
        if (playerId == battleStatus.playerId)
        {
            Card card = cardMaster.CardList.Find(m => m.itemId == battleStatus.unitStatus[fieldNumber].unitId);
            if (phase == "start") display.unitSkillWindow.GetComponent<SkillWindow>().UnitSkillWindow(card, card.startSkillName, card.startSkillDetail);
            if (phase == "auto") display.unitSkillWindow.GetComponent<SkillWindow>().UnitSkillWindow(card, card.autoSkillName, card.autoSkillDetail);
            if (phase == "action1") display.unitSkillWindow.GetComponent<SkillWindow>().UnitSkillWindow(card, card.actionSkillName1, card.actionSkillDetail1);
            if (phase == "action2") display.unitSkillWindow.GetComponent<SkillWindow>().UnitSkillWindow(card, card.actionSkillName2, card.actionSkillDetail2);
            if (phase == "close") display.unitSkillWindow.GetComponent<SkillWindow>().UnitSkillWindow(card, card.closeSkillName, card.closeSkillDetail);
            if (phase == "text") display.unitSkillWindow.GetComponent<SkillWindow>().UnitSkillWindow(card, card.textSkillName, card.textSkillDetail);
        }
        else
        {
            Card card = cardMaster.CardList.Find(m => m.itemId == battleStatusEnemy.unitStatus[fieldNumber].unitId);
            if (phase == "start") display.unitSkillWindowEnemy.GetComponent<SkillWindow>().UnitSkillWindow(card, card.startSkillName, card.startSkillDetail);
            if (phase == "auto") display.unitSkillWindowEnemy.GetComponent<SkillWindow>().UnitSkillWindow(card, card.autoSkillName, card.autoSkillDetail);
            if (phase == "action1") display.unitSkillWindowEnemy.GetComponent<SkillWindow>().UnitSkillWindow(card, card.actionSkillName1, card.actionSkillDetail1);
            if (phase == "action2") display.unitSkillWindowEnemy.GetComponent<SkillWindow>().UnitSkillWindow(card, card.actionSkillName2, card.actionSkillDetail2);
            if (phase == "close") display.unitSkillWindowEnemy.GetComponent<SkillWindow>().UnitSkillWindow(card, card.closeSkillName, card.closeSkillDetail);
            if (phase == "text") display.unitSkillWindowEnemy.GetComponent<SkillWindow>().UnitSkillWindow(card, card.textSkillName, card.textSkillDetail);
        }
    }
    private IEnumerator StartSelect(ServerToClient5 serverToClient5, UnityEngine.Events.UnityAction<ServerToClient5> callback)
    {
        ClientToServerC clientToServerC = new ClientToServerC();
        clientToServerC.playerId = battleStatus.playerId;
        clientToServerC.phase = "startSelect";
        if (serverToClient5.nextPlayerId == battleStatus.playerId)
        {
            Target[] target = new Target[0];
            yield return TargetSelect(serverToClient5.targetList, (result) => target = result);
            clientToServerC.target = target;
            clientToServerC.fieldNumber = serverToClient5.nextFieldNumber;
        }
        string json = battleJson.toJsonC(clientToServerC);
        WriteFile(json);
        yield return RoadFileCheak();
        serverToClient5 = battleJson.toClass5(RoadFile());
        callback(serverToClient5);
    }
    private IEnumerator CloseSelect(ServerToClient7 serverToClient7, UnityEngine.Events.UnityAction<ServerToClient7> callback)
    {
        ClientToServerC clientToServerC = new ClientToServerC();
        clientToServerC.playerId = battleStatus.playerId;
        clientToServerC.phase = "closeSelect";
        if (serverToClient7.playerId == battleStatus.playerId)
        {
            Target[] target = new Target[0];
            yield return TargetSelect(serverToClient7.targetList, (result) => target = result);
            clientToServerC.target = target;
            clientToServerC.fieldNumber = serverToClient7.fieldNumber;
        }
        string json = battleJson.toJsonC(clientToServerC);
        WriteFile(json);
        yield return RoadFileCheak();
        serverToClient7 = battleJson.toClass7(RoadFile());
        callback(serverToClient7);
    }
    private IEnumerator StartPhase()
    {
        StartCoroutine(DisplayPhase("戦闘開始フェーズ"));
        ClientToServerE clientToServerE = new ClientToServerE(battleStatus.playerId);
        string json = battleJson.toJsonE(clientToServerE);
        WriteFile(json);
        yield return RoadFileCheak();
        ServerToClient5 serverToClient5 = battleJson.toClass5(RoadFile());
        serverToClient5.nextPlayerId = serverToClient5.nextPlayerId;
        while (serverToClient5.nextPlayerId != "end")
        {
            int nextFieldNumber = serverToClient5.nextFieldNumber.Value;
            SkillCutIn(serverToClient5.nextPlayerId, nextFieldNumber,"start");
            Card card = new Card();
            bool enemyCard = serverToClient5.nextPlayerId == battleStatusEnemy.playerId;
            if (!enemyCard)
            {
                card = cardMaster.CardList.Find(m => m.itemId == battleStatus.unitStatus[nextFieldNumber].unitId);
                UpdateSp(enemyCard, battleStatus.sp - card.startSkillSp);
            }
            else
            {
                card = cardMaster.CardList.Find(m => m.itemId == battleStatusEnemy.unitStatus[nextFieldNumber].unitId);
                UpdateSp(enemyCard, battleStatusEnemy.sp - card.startSkillSp);
            }
            if (serverToClient5.targetList.Length != 0)
            {
                yield return StartSelect(serverToClient5, (result) => serverToClient5 = result);
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
            ServerToClient7 serverToClient7 = new ServerToClient7();
            yield return SkillEffect(enemyCard, card, "start", serverToClient5.updateInfo, (result) => serverToClient7 = result);
            clientToServerE = new ClientToServerE(battleStatus.playerId);
            json = battleJson.toJsonE(clientToServerE);
            WriteFile(json);
            yield return RoadFileCheak();
            serverToClient5 = battleJson.toClass5(RoadFile());
        }
    }
    private IEnumerator RemoveSelect(ServerToClient4 serverToClient4, UnityEngine.Events.UnityAction<ServerToClient4> callback)
    {
        ClientToServerC clientToServerC = new ClientToServerC();
        clientToServerC.playerId = battleStatus.playerId;
        clientToServerC.phase = "shieldSelect";
        if (serverToClient4.playerId == battleStatus.playerId)
        {
            Target[] target = new Target[0];
            yield return TargetSelect(serverToClient4.targetList, (result) => target = result);
            clientToServerC.target = target;
            clientToServerC.fieldNumber = serverToClient4.fieldNumber;
        }
        string json = battleJson.toJsonC(clientToServerC);
        WriteFile(json);
        yield return RoadFileCheak();
        serverToClient4 = battleJson.toClass4(RoadFile());
        callback(serverToClient4);
    }
    private IEnumerator RemovePhase()
    {
        StartCoroutine(DisplayPhase("撤退フェーズ"));
        ClientToServerD clientToServerD = new ClientToServerD(battleStatus.playerId);
        string json = battleJson.toJsonD(clientToServerD);
        WriteFile(json);
        yield return RoadFileCheak();
        ServerToClient4 serverToClient4 = battleJson.toClass4(RoadFile());
        while (serverToClient4.playerId != "end")
        {
            int fieldNumber = serverToClient4.fieldNumber.Value;
            if (serverToClient4.playerId == battleStatusEnemy.playerId) UpdateSp(true, battleStatusEnemy.sp+ battleStatusEnemy.unitStatus[fieldNumber].defLV);
            if (serverToClient4.playerId == battleStatus.playerId) UpdateSp(false, battleStatus.sp + battleStatus.unitStatus[fieldNumber].defLV);
            GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(6);
            StartCoroutine(areaEffect.BattleAreaEffect(serverToClient4.playerId == battleStatusEnemy.playerId, fieldNumber, "リムーブ"));

            yield return UnitDeath(fieldNumber, serverToClient4.playerId == battleStatusEnemy.playerId);
            if (serverToClient4.shieldId != null)
            {
                if (serverToClient4.playerId == battleStatusEnemy.playerId)
                {
                    int i;
                    for (i = 0; i < battleStatusEnemy.shieldStatus.Length; i++)
                    {
                        if (battleStatusEnemy.shieldStatus[i].shieldId == "") break;
                    }
                    GameObject shieldObject = display.shieldCardsEnemy[i].gameObject.transform.parent.parent.gameObject;
                    shieldObject.transform.localPosition = new Vector3(shieldObject.transform.localPosition.x, shieldObject.transform.localPosition.y - 20, 0);
                    GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(8);
                    yield return new WaitForSeconds(0.5f);
                    shieldObject.transform.localPosition = new Vector3(shieldObject.transform.localPosition.x, shieldObject.transform.localPosition.y + 20, 0);
                    battleStatusEnemy.shieldStatus[i].shieldId = serverToClient4.shieldId;
                    battleStatusEnemy.shieldStatus[i].shieldHp++;
                    battleStatusEnemy.life--;
                    display.lifeEnemy.text = battleStatusEnemy.life.ToString();
                    display.shieldCardsEnemy[i].gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = battleStatusEnemy.shieldStatus[i].shieldHp.ToString();
                    yield return display.openDisplay.GetComponent<OpenDisplay>().DoOpenDisplayEnemy(serverToClient4.shieldId);
                    Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == battleStatusEnemy.shieldStatus[i].shieldId);
                    yield return display.ShieldDisplay(shield, display.shieldCardsEnemy[i]);
                    if (battleStatusEnemy.sp >= shield.skillSp)
                    {
                        battleStatusEnemy.sp = battleStatusEnemy.sp - shield.skillSp;
                        display.spEnemy.text = battleStatusEnemy.sp.ToString();
                        yield return display.shieldSkillWindowEnemy.GetComponent<SkillWindow>().ShieldSkillWindow2(shield, shield.name, shield.skillDetail);
                    }
                    AddSpecial(true, 10);
                    if (serverToClient4.targetList.Length != 0)
                    {
                        ServerToClient4 serverToClient4_2 = new ServerToClient4();
                        yield return RemoveSelect(serverToClient4, (result) => serverToClient4_2 = result);
                        serverToClient4.updateInfo = serverToClient4_2.updateInfo;
                    }
                    else
                    {
                        yield return new WaitForSeconds(1.0f);
                    }
                    ServerToClient7 serverToClient7 = new ServerToClient7();
                    yield return SkillEffect(true, null, "shield" + i, serverToClient4.updateInfo, (result) => serverToClient7 = result);
                }
                else
                {
                    int i;
                    for (i = 0; i < battleStatus.shieldStatus.Length; i++)
                    {
                        if (battleStatus.shieldStatus[i].shieldHp == 1) break;
                    }
                    GameObject shieldObject = display.shieldCardsEnemy[i].gameObject.transform.parent.parent.gameObject;
                    shieldObject.transform.localPosition = new Vector3(shieldObject.transform.localPosition.x, shieldObject.transform.localPosition.y - 20, 0);
                    GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(8);
                    yield return new WaitForSeconds(0.5f);
                    shieldObject.transform.localPosition = new Vector3(shieldObject.transform.localPosition.x, shieldObject.transform.localPosition.y + 20, 0);
                    battleStatus.shieldStatus[i].shieldHp--;
                    battleStatus.life--;
                    display.life.text = battleStatus.life.ToString();
                    display.shieldCards[i].gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = battleStatus.shieldStatus[i].shieldHp.ToString();
                    yield return display.openDisplay.GetComponent<OpenDisplay>().DoOpenDisplayPlayer(serverToClient4.shieldId);
                    Shield shield = shieldMaster.ShieldList.Find(m => m.itemId == battleStatus.shieldStatus[i].shieldId);
                    if (battleStatus.sp >= shield.skillSp)
                    {
                        battleStatus.sp = battleStatus.sp - shield.skillSp;
                        display.sp.text = battleStatus.sp.ToString();
                        yield return display.shieldSkillWindow.GetComponent<SkillWindow>().ShieldSkillWindow2(shield, shield.name, shield.skillDetail);
                    }
                    AddSpecial(false, 10);
                    if (serverToClient4.targetList.Length != 0)
                    {
                        ServerToClient4 serverToClient4_2 = new ServerToClient4();
                        yield return RemoveSelect(serverToClient4, (result) => serverToClient4_2 = result);
                        serverToClient4.updateInfo = serverToClient4_2.updateInfo;
                    }
                    else
                    {
                        yield return new WaitForSeconds(1.0f);
                    }
                    ServerToClient7 serverToClient7 = new ServerToClient7();
                    yield return SkillEffect(false, null, "shield" + i, serverToClient4.updateInfo, (result) => serverToClient7 = result);
                }
            }
            else
            {
                if (serverToClient4.playerId == battleStatusEnemy.playerId)
                {
                    int i;
                    for (i = 0; i < battleStatusEnemy.shieldStatus.Length; i++)
                    {
                        if (battleStatusEnemy.shieldStatus[i].shieldId == "") break;
                    }
                    battleStatusEnemy.shieldStatus[i].shieldHp++;
                    display.shieldCards[i].gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = battleStatusEnemy.shieldStatus[i].shieldHp.ToString();
                }
                else
                {
                    int i;
                    for (i = 0; i < battleStatus.shieldStatus.Length; i++)
                    {
                        if (battleStatus.shieldStatus[i].shieldHp > 1) break;
                    }
                    battleStatus.shieldStatus[i].shieldHp--;
                    display.shieldCards[i].gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = battleStatus.shieldStatus[i].shieldHp.ToString();

                }
            }
            WriteFile(json);
            yield return RoadFileCheak();
            serverToClient4 = battleJson.toClass4(RoadFile());
        }
    }
    private IEnumerator SkillEffectClose(bool enemyCard, Card card, string skillType, UpdateInfo[] updateInfo)
    {
        if (skillType.Contains("break"))
        {
            enemyCard = !enemyCard;
            skillType = "shield" + skillType.Substring(5);
        }
        Effect effect = new Effect();
        effect.cutin = "";
        effect.enemy = "";
        effect.player = "";
        switch (skillType)
        {
            case "atk":
                effect = battleJson.toEffect(card.attackEffect);
                break;
            case "open":
                effect = battleJson.toEffect(card.openEffect);
                break;
            case "start":
                effect = battleJson.toEffect(card.startEffect);
                break;
            case "auto":
                effect = battleJson.toEffect(card.autoEffect);
                break;
            case "action1":
                effect = battleJson.toEffect(card.action1Effect);
                break;
            case "action2":
                effect = battleJson.toEffect(card.action2Effect);
                break;
            case "close":
                effect = battleJson.toEffect(card.closeEffect);
                break;
            case "text":
                effect = battleJson.toEffect(card.textEffect);
                break;
            case "rest":
                effect.player = "回復黄";
                break;
            case "shield0":
                if (!enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatus.shieldStatus[0].shieldId).effect);
                if (enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatusEnemy.shieldStatus[0].shieldId).effect);
                break;
            case "shield1":
                if (!enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatus.shieldStatus[1].shieldId).effect);
                if (enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatusEnemy.shieldStatus[1].shieldId).effect);
                break;
            case "shield2":
                if (!enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatus.shieldStatus[2].shieldId).effect);
                if (enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatusEnemy.shieldStatus[2].shieldId).effect);
                break;
            case "shield3":
                if (!enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatus.shieldStatus[3].shieldId).effect);
                if (enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatusEnemy.shieldStatus[3].shieldId).effect);
                break;
            case "shield4":
                if (!enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatus.shieldStatus[4].shieldId).effect);
                if (enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatusEnemy.shieldStatus[4].shieldId).effect);
                break;
            case "special":
                if (!enemyCard) effect = battleJson.toEffect(specialMaster.SpecialList.Find(m => m.itemId == battleStatus.specialId).effect);
                if (enemyCard) effect = battleJson.toEffect(specialMaster.SpecialList.Find(m => m.itemId == battleStatusEnemy.specialId).effect);
                break;
            default:
                break;
        }
        Debug.Log("cutin:" + effect.cutin + " player:" + effect.player + " enemy:" + effect.enemy);
        //カットインエフェクト処理
        if (effect.cutin != "") yield return cutInEffect.StartCutInEffect(effect.cutin);
        //各更新内容処理

        if (updateInfo != null)
        {
            for (int i = 0; i < updateInfo.Length; i++)
            {
                if (updateInfo[i].sp != null)
                {
                    for (int j = 0; j < updateInfo[i].sp.Length; j++)
                    {
                        UpdateSp(updateInfo[i].sp[j].playerId == battleStatusEnemy.playerId, updateInfo[i].sp[j]);
                    }
                }
                if (updateInfo[i].field != null)
                {
                    yield return UpdateField(enemyCard, effect, updateInfo[i].field);
                }
                if (updateInfo[i].mana != null)
                {
                    for (int j = 0; j < updateInfo[i].mana.Length; j++)
                    {
                        UpdateMana(updateInfo[i].mana[j].playerId == battleStatusEnemy.playerId, updateInfo[i].mana[j]);
                    }
                }
                if (updateInfo[i].deckCemetery != null)
                {
                    for (int j = 0; j < updateInfo[i].deckCemetery.Length; j++)
                    {
                        if (updateInfo[i].deckCemetery[j].playerId == battleStatusEnemy.playerId)
                        {
                            TrushCard(updateInfo[i].deckCemetery[j].cardId, 2);
                        }
                        else
                        {
                            TrushCard(updateInfo[i].deckCemetery[j].deckNumber, 2);
                        }
                    }
                }
                if (updateInfo[i].deckLock != null)
                {
                    for (int j = 0; j < updateInfo[i].deckLock.Length; j++)
                    {
                        if (updateInfo[i].deckLock[j].playerId == battleStatus.playerId) yield return DeckLock(updateInfo[i].deckLock[j].deckNumber, updateInfo[i].deckLock[j].turn);
                    }
                }
                if (updateInfo[i].removeCemetery != null)
                {
                    for (int j = 0; j < updateInfo[i].removeCemetery.Length; j++)
                    {
                        int deckNum = battleStatus.trashDeck[0, updateInfo[i].removeCemetery[j].cemeteryNumber];
                        yield return RemoveCard(updateInfo[i].removeCemetery[j].cemeteryNumber, updateInfo[i].removeCemetery[j].playerId == battleStatusEnemy.playerId);
                        yield return TrushCard(deckNum, 3);
                    }
                }
                if (updateInfo[i].repair != null)
                {
                    for (int j = 0; j < updateInfo[i].repair.Length; j++)
                    {
                        yield return ReviveCard(updateInfo[i].repair[j].cemeteryNumber, updateInfo[i].repair[j].playerId == battleStatusEnemy.playerId);
                    }
                }
                if (updateInfo[i].special != null)
                {
                    for (int j = 0; j < updateInfo[i].special.Length; j++)
                    {
                        UpdateSpecial(updateInfo[i].special[j].playerId == battleStatusEnemy.playerId, updateInfo[i].special[j]);
                    }
                }
                if (updateInfo[i].countAdd != null)
                {
                    for (int j = 0; j < updateInfo[i].countAdd.Length; j++)
                    {
                        UpdateCount(updateInfo[i].countAdd[j].playerId == battleStatusEnemy.playerId, updateInfo[i].countAdd[j]);
                    }
                }
            }
        }
        display.unitSkillWindow.transform.GetChild(0).gameObject.SetActive(false);
        display.unitSkillWindowEnemy.transform.GetChild(0).gameObject.SetActive(false);
        display.shieldSkillWindow.transform.GetChild(0).gameObject.SetActive(false);
        display.shieldSkillWindowEnemy.transform.GetChild(0).gameObject.SetActive(false);
        display.specialSkillWindow.transform.GetChild(0).gameObject.SetActive(false);
        display.specialSkillWindowEnemy.transform.GetChild(0).gameObject.SetActive(false);
    }
    private IEnumerator SkillEffect(bool enemyCard,Card card,string skillType, UpdateInfo[] updateInfo, UnityEngine.Events.UnityAction<ServerToClient7> callback)
    {
        if (skillType.Contains("break"))
        {
            enemyCard = !enemyCard;
            skillType = "shield"+skillType.Substring(5);
        }
        Effect effect = new Effect();
        effect.cutin = "";
        effect.enemy = "";
        effect.player = "";
        switch (skillType)
        {
            case "atk":
                effect = battleJson.toEffect(card.attackEffect);
                break;
            case "open":
                effect = battleJson.toEffect(card.openEffect);
                break;
            case "start":
                effect = battleJson.toEffect(card.startEffect);
                break;
            case "auto":
                effect = battleJson.toEffect(card.autoEffect);
                break;
            case "actionSkill1":
                effect = battleJson.toEffect(card.action1Effect);
                break;
            case "actionSkill2":
                effect = battleJson.toEffect(card.action2Effect);
                break;
            case "close":
                effect = battleJson.toEffect(card.closeEffect);
                break;
            case "text":
                effect = battleJson.toEffect(card.textEffect);
                break;
            case "rest":
                effect.player = "回復黄";
                break;
            case "shield0":
                if(!enemyCard)effect= battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatus.shieldStatus[0].shieldId).effect);
                if (enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatusEnemy.shieldStatus[0].shieldId).effect);
                break;
            case "shield1":
                if (!enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatus.shieldStatus[1].shieldId).effect);
                if (enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatusEnemy.shieldStatus[1].shieldId).effect);
                break;
            case "shield2":
                if (!enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatus.shieldStatus[2].shieldId).effect);
                if (enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatusEnemy.shieldStatus[2].shieldId).effect);
                break;
            case "shield3":
                if (!enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatus.shieldStatus[3].shieldId).effect);
                if (enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatusEnemy.shieldStatus[3].shieldId).effect);
                break;
            case "shield4":
                if (!enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatus.shieldStatus[4].shieldId).effect);
                if (enemyCard) effect = battleJson.toEffect(shieldMaster.ShieldList.Find(m => m.itemId == battleStatusEnemy.shieldStatus[4].shieldId).effect);
                break;
            case "special":
                if (!enemyCard) effect = battleJson.toEffect(specialMaster.SpecialList.Find(m => m.itemId == battleStatus.specialId).effect);
                if (enemyCard) effect = battleJson.toEffect(specialMaster.SpecialList.Find(m => m.itemId == battleStatusEnemy.specialId).effect);
                break;
            default:
                break;
        }
        Debug.Log("cutin:" + effect.cutin　+" player:"+effect.player+" enemy:"+effect.enemy);
        //カットインエフェクト処理
        if(effect.cutin!="") yield return cutInEffect.StartCutInEffect(effect.cutin);
        //各更新内容処理

        if (updateInfo != null)
        {
            for (int i = 0; i < updateInfo.Length; i++)
            {
                if (updateInfo[i].sp != null)
                {
                    for (int j = 0; j < updateInfo[i].sp.Length; j++)
                    {
                        UpdateSp(updateInfo[i].sp[j].playerId==battleStatusEnemy.playerId, updateInfo[i].sp[j]);
                    }
                }
                if (updateInfo[i].field != null)
                {
                    yield return UpdateField(enemyCard, effect, updateInfo[i].field);
                }
                if (updateInfo[i].mana != null)
                {
                    for (int j = 0; j < updateInfo[i].mana.Length; j++)
                    {
                        UpdateMana(updateInfo[i].mana[j].playerId == battleStatusEnemy.playerId, updateInfo[i].mana[j]);
                    }
                }
                if (updateInfo[i].deckCemetery != null)
                {
                    for (int j = 0; j < updateInfo[i].deckCemetery.Length; j++)
                    {
                        if(updateInfo[i].deckCemetery[j].playerId == battleStatusEnemy.playerId)
                        {
                            TrushCard(updateInfo[i].deckCemetery[j].cardId,2);
                        }
                        else
                        {
                            TrushCard(updateInfo[i].deckCemetery[j].deckNumber,2);
                        }
                    }
                }
                if(updateInfo[i].deckLock != null)
                {
                    for (int j = 0; j < updateInfo[i].deckLock.Length; j++)
                    {
                        if (updateInfo[i].deckLock[j].playerId == battleStatus.playerId)yield return DeckLock(updateInfo[i].deckLock[j].deckNumber, updateInfo[i].deckLock[j].turn);
                    }
                }
                if (updateInfo[i].removeCemetery != null)
                {
                    for (int j = 0; j < updateInfo[i].removeCemetery.Length; j++)
                    {
                        int deckNum = battleStatus.trashDeck[0,updateInfo[i].removeCemetery[j].cemeteryNumber];
                        yield return RemoveCard(updateInfo[i].removeCemetery[j].cemeteryNumber, updateInfo[i].removeCemetery[j].playerId == battleStatusEnemy.playerId);
                        yield return TrushCard(deckNum, 3);
                    }
                }
                if (updateInfo[i].repair != null)
                {
                    for (int j = 0; j < updateInfo[i].repair.Length; j++)
                    {
                        yield return ReviveCard(updateInfo[i].repair[j].cemeteryNumber, updateInfo[i].repair[j].playerId == battleStatusEnemy.playerId);
                    }
                }
                if (updateInfo[i].special != null)
                {
                    for (int j = 0; j < updateInfo[i].special.Length; j++)
                    {
                        UpdateSpecial(updateInfo[i].special[j].playerId == battleStatusEnemy.playerId, updateInfo[i].special[j]);
                    }
                }
                if (updateInfo[i].countAdd != null)
                {
                    for (int j = 0; j < updateInfo[i].countAdd.Length; j++)
                    {
                        UpdateCount(updateInfo[i].countAdd[j].playerId == battleStatusEnemy.playerId, updateInfo[i].countAdd[j]);
                    }
                }
            }
        }

        //スキルウィンドウ消し
        display.unitSkillWindow.transform.GetChild(0).gameObject.SetActive(false);
        display.unitSkillWindowEnemy.transform.GetChild(0).gameObject.SetActive(false);
        display.shieldSkillWindow.transform.GetChild(0).gameObject.SetActive(false);
        display.shieldSkillWindowEnemy.transform.GetChild(0).gameObject.SetActive(false);
        display.specialSkillWindow.transform.GetChild(0).gameObject.SetActive(false);
        display.specialSkillWindowEnemy.transform.GetChild(0).gameObject.SetActive(false);
        ServerToClient7 serverToClient7 = null;
        for (int i = 0; i < 9; i++)
        {
            if(battleStatus.unitStatus[i].nowHP<=0 && battleStatus.unitStatus[i].close == false && battleStatus.unitStatus[i].unitId != "")
            {
                ClientToServerG clientToServerG = new ClientToServerG(battleStatus.playerId);
                string json = battleJson.toJsonG(clientToServerG);
                WriteFile(json);
                yield return RoadFileCheak();
                serverToClient7 = battleJson.toClass7(RoadFile());
                yield return CloseSkill(serverToClient7, (result) => serverToClient7 = result);
                i = -1;
            }
            if (i != -1)
            {
                if (battleStatusEnemy.unitStatus[i].nowHP <= 0 && battleStatusEnemy.unitStatus[i].close == false && battleStatusEnemy.unitStatus[i].unitId != "")
                {
                    ClientToServerG clientToServerG = new ClientToServerG(battleStatus.playerId);
                    string json = battleJson.toJsonG(clientToServerG);
                    WriteFile(json);
                    yield return RoadFileCheak();
                    serverToClient7 = battleJson.toClass7(RoadFile());
                    yield return CloseSkill(serverToClient7,(result) => serverToClient7 = result);
                    i = -1;
                }
            }
        }
        callback(serverToClient7);
    }
    private IEnumerator CloseSkill(ServerToClient7 serverToClient7, UnityEngine.Events.UnityAction<ServerToClient7> callback)
    {
        if (serverToClient7.playerId != "end")
        {
            int fieldNumber = serverToClient7.fieldNumber.Value;
            bool enemyCard = serverToClient7.playerId == battleStatusEnemy.playerId;
            Card card = new Card();
            if (!enemyCard)
            {
                card = cardMaster.CardList.Find(m => m.itemId == battleStatus.unitStatus[fieldNumber].unitId);
                UpdateSp(enemyCard, battleStatus.sp - card.closeSkillSp);
            }
            if (enemyCard)
            {
                card = cardMaster.CardList.Find(m => m.itemId == battleStatusEnemy.unitStatus[fieldNumber].unitId);
                UpdateSp(enemyCard, battleStatusEnemy.sp - card.closeSkillSp);
            }
            if (card.closeSkillName != "")
            {
                SkillCutIn(serverToClient7.playerId, fieldNumber, "close");
                yield return new WaitForSeconds(1.0f);
            }
            if (!enemyCard)
            {
                GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(6);
                battleStatus.unitStatus[fieldNumber].close = true;
                display.fieldUnit[fieldNumber].color = new Color(0f, 0f, 0f);
            }
            if (enemyCard)
            {
                GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(6);
                battleStatusEnemy.unitStatus[fieldNumber].close = true;
                display.fieldUnitEnemy[fieldNumber].color = new Color(0f, 0f, 0f);
            }
            if (serverToClient7.targetList.Length != 0) yield return CloseSelect(serverToClient7, (result) => serverToClient7 = result);
            yield return SkillEffectClose(enemyCard, card, "close", serverToClient7.updateInfo);
            callback(serverToClient7);
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                if (battleStatus.unitStatus[i].nowHP <= 0 && battleStatus.unitStatus[i].close == false && battleStatus.unitStatus[i].unitId != "")
                {
                    GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(6);
                    battleStatus.unitStatus[i].close = true;
                    display.fieldUnit[i].color = new Color(0f, 0f, 0f);
                }
                if (battleStatusEnemy.unitStatus[i].nowHP <= 0 && battleStatusEnemy.unitStatus[i].close == false && battleStatusEnemy.unitStatus[i].unitId != "")
                {
                    GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(6);
                    battleStatusEnemy.unitStatus[i].close = true;
                    display.fieldUnitEnemy[i].color = new Color(0f, 0f, 0f);
                }
            }
            callback(null);
        }
    }
    private IEnumerator OpenSkill(ServerToClient2 serverToClient2)
    {
        if (serverToClient2.specialId == "")
        {
            //行動カード
            Card card = new Card();
            if (serverToClient2.nextAction == battleStatus.playerId)
            {
                card = cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[battleStatus.setCard].unitId);
            }
            else if (serverToClient2.nextAction == battleStatusEnemy.playerId)
            {
                card = cardMaster.CardList.Find(m => m.itemId == prevSetCardEnemy);
            }
            //魔法墓地送り処理
            if (serverToClient2.nextAction == battleStatus.playerId && card.type == "魔法")
            {
                StartCoroutine(areaEffect.SetAreaEffect(false, "リムーブ"));
                display.setCard.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
                yield return TrushCard(battleStatus.setCard, 2);
                battleStatus.setCard = 99;
            }
            if (serverToClient2.nextAction == battleStatusEnemy.playerId && card.type == "魔法")
            {
                StartCoroutine(areaEffect.SetAreaEffect(true, "リムーブ"));
                yield return TrushCard(prevSetCardEnemy, 2);
                prevSetCardEnemy = "";
                display.setCardEnemy.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
            }
            ServerToClient7 serverToClient7 = new ServerToClient7();
            yield return SkillEffect(serverToClient2.nextAction == battleStatusEnemy.playerId, card, "open", serverToClient2.updateInfo, (result) => serverToClient7 = result);
        }
        else
        {
            if(serverToClient2.nextAction == battleStatusEnemy.playerId) battleStatusEnemy.specialId = serverToClient2.specialId;
            ServerToClient7 serverToClient7 = new ServerToClient7();
            yield return SkillEffect(serverToClient2.nextAction == battleStatusEnemy.playerId, null, "special", serverToClient2.updateInfo, (result) => serverToClient7 = result);
        }
    }
    private IEnumerator OpenCutIn(ServerToClient2 serverToClient2)
    {
        if (serverToClient2.specialId == "")
        {
            if (serverToClient2.nextAction == battleStatus.playerId)
            {
                Card card = cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[battleStatus.setCard].unitId);
                display.unitSkillWindow.GetComponent<SkillWindow>().UnitSkillWindow(card, card.openSkillName, card.openSkillDetail);
                UpdateSp(false, battleStatus.sp - card.openSkillSp);
            }
            else
            {
                Card card = cardMaster.CardList.Find(m => m.itemId == prevSetCardEnemy);
                display.unitSkillWindowEnemy.GetComponent<SkillWindow>().UnitSkillWindow(card, card.openSkillName, card.openSkillDetail);
                UpdateSp(true, battleStatusEnemy.sp - card.openSkillSp);
            }
        }
        else
        {
            Special special = specialMaster.SpecialList.Find(m => m.itemId == serverToClient2.specialId);
            if (serverToClient2.nextAction == battleStatus.playerId)
            {
                yield return display.specialEffect.PlaySpecailEffect(special);
                battleStatus.specialStock = battleStatus.specialStock - special.stock;
                display.stock.text = battleStatus.specialStock.ToString();
                display.specialSkillWindow.GetComponent<SkillWindow>().SpecialSkillWindow(special, special.name, special.skillDetail);
            }
            else
            {
                yield return display.specialEffect.PlaySpecailEffect(special);
                battleStatusEnemy.specialStock = battleStatusEnemy.specialStock - special.stock;
                display.stockEnemy.text = battleStatusEnemy.specialStock.ToString();
                display.specialSkillWindowEnemy.GetComponent<SkillWindow>().SpecialSkillWindow(special, special.name, special.skillDetail);
            }
        }
    }
    private IEnumerator OpenSelect(ServerToClient2 serverToClient2,bool special, UnityEngine.Events.UnityAction<ServerToClient2> callback)
    {
        ClientToServerC clientToServerC = new ClientToServerC();
        clientToServerC.playerId = battleStatus.playerId;
        if(!special)clientToServerC.phase = "openSelect";
        if (special) clientToServerC.phase = "specialSelect";
        if (serverToClient2.nextAction == battleStatus.playerId)
        {
            Target[] target = new Target[0];
            yield return TargetSelect(serverToClient2.targetList, (result) => target = result);
            clientToServerC.target = target;
        }
        string json = battleJson.toJsonC(clientToServerC);
        WriteFile(json);
        yield return RoadFileCheak();
        serverToClient2 = battleJson.toClass2(RoadFile());
        callback(serverToClient2);
    }
    private IEnumerator OpenPhase()
    {
        //Setの結果を受信2
        yield return RoadFileCheak();
        ServerToClient2 serverToClient2 = battleJson.toClass2(RoadFile());
        yield return CardOpen(serverToClient2);
        //オープン処理
        while (serverToClient2.nextAction != "end")
        {
            //カットイン
            yield return OpenCutIn(serverToClient2);
            //選択処理
            if (serverToClient2.targetList.Length != 0)
            {
                ServerToClient2 serverToClient2_2 = new ServerToClient2();
                if (serverToClient2.specialId == "") yield return OpenSelect(serverToClient2, false,(result) => serverToClient2_2 = result);
                if (serverToClient2.specialId != "") yield return OpenSelect(serverToClient2, true,(result) => serverToClient2_2 = result);
                serverToClient2.updateInfo = serverToClient2_2.updateInfo;
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
            //オープンスキル
            yield return OpenSkill(serverToClient2);
            //問い合わせ
            ClientToServerC clientToServerC = new ClientToServerC();
            clientToServerC.playerId = battleStatus.playerId;
            string json = battleJson.toJsonC(clientToServerC);
            WriteFile(json);
            yield return RoadFileCheak();
            serverToClient2 = battleJson.toClass2(RoadFile());
        }
    }
    private IEnumerator CardOpen(ServerToClient2 serverToClient2)
    {
        StartCoroutine(DisplayPhase("公開フェーズ"));
        //enemyRevival処理して
        for (int i = 0; i < serverToClient2.enemyRevivalNumber.Length; i++)
        {
            battleStatusEnemy.unitStatus[i].revival = serverToClient2.enemyRevivalNumber[i];
        }
        //復活させる
        for(int i = 0; i < 9; i++)
        {
            if (battleStatusEnemy.unitStatus[i].revival == true)
            {
                battleStatusEnemy.sp--;
                display.spEnemy.text = battleStatusEnemy.sp.ToString();
                TrushCard(battleStatusEnemy.unitStatus[i].unitId, 2);
                battleStatusEnemy.unitStatus[i].SetUnit(cardMaster.CardList.Find(m => m.itemId == battleStatusEnemy.unitStatus[i].unitId));
                UpdateActionEnd(true, i, false);
                display.hpBarEnemy[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = battleStatusEnemy.unitStatus[i].defHP + "/" + battleStatusEnemy.unitStatus[i].defHP;

            }
            if (battleStatus.unitStatus[i].revival == true)
            {
                battleStatus.unitStatus[i].SetUnit(cardMaster.CardList.Find(m => m.itemId == battleStatus.unitStatus[i].unitId));
                UpdateActionEnd(false, i, false);
                display.hpBar[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = battleStatus.unitStatus[i].defHP + "/" + battleStatus.unitStatus[i].defHP;
            }
        }


        display.setCardEnemy.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
        if (battleStatus.setPosition!="")
        {
            StartCoroutine(areaEffect.BattleAreaEffect(false, int.Parse(battleStatus.setPosition), "召喚"));
            display.hpBar[int.Parse(battleStatus.setPosition)].transform.GetChild(0).gameObject.GetComponent<Text>().text = battleStatus.unitStatus[int.Parse(battleStatus.setPosition)].defHP + "/" + battleStatus.unitStatus[int.Parse(battleStatus.setPosition)].defHP;
            display.hpBar[int.Parse(battleStatus.setPosition)].SetActive(true);
        }

        if (serverToClient2.enemySetPosition != "")
        {
            StartCoroutine(areaEffect.BattleAreaEffect(true, int.Parse(serverToClient2.enemySetPosition),"召喚"));
            yield return display.CardDisplay(prevSetCardEnemy,display.fieldUnitEnemy[int.Parse(serverToClient2.enemySetPosition)]);
            Card card = cardMaster.CardList.Find(m => m.itemId == prevSetCardEnemy);
            battleStatusEnemy.unitStatus[int.Parse(serverToClient2.enemySetPosition)].SetUnit(card);
            display.hpBarEnemy[int.Parse(serverToClient2.enemySetPosition)].transform.GetChild(0).gameObject.GetComponent<Text>().text = battleStatusEnemy.unitStatus[int.Parse(serverToClient2.enemySetPosition)].defHP + "/" + battleStatusEnemy.unitStatus[int.Parse(serverToClient2.enemySetPosition)].defHP;
            display.hpBarEnemy[int.Parse(serverToClient2.enemySetPosition)].SetActive(true);
        }
        //マナ関係
        for (int i = 0; i < 5; i++)
        {
            battleStatus.color[i] = battleStatus.color[i] + battleStatus.colorUp[i];
            battleStatus.colorUp[i] = 0;
        }
        Card card2 = cardMaster.CardList.Find(m => m.itemId == serverToClient2.enemyCardId);
        if (card2 != null) battleStatusEnemy.sp = battleStatusEnemy.sp - card2.level;
        battleStatusEnemy.sp = battleStatusEnemy.sp + battleStatusEnemy.color[0] + battleStatusEnemy.color[1] + battleStatusEnemy.color[2] + battleStatusEnemy.color[3] - serverToClient2.enemyBlueMana - serverToClient2.enemyYellowMana - serverToClient2.enemyRedMana - serverToClient2.enemyBlackMana;
        display.spEnemy.text = battleStatusEnemy.sp.ToString();
        battleStatusEnemy.color[0] = serverToClient2.enemyBlueMana;
        battleStatusEnemy.color[1] = serverToClient2.enemyYellowMana;
        battleStatusEnemy.color[2] = serverToClient2.enemyRedMana;
        battleStatusEnemy.color[3] = serverToClient2.enemyBlackMana;
        battleStatusEnemy.color[4] = battleStatusEnemy.color[0] + battleStatusEnemy.color[1] + battleStatusEnemy.color[2] + battleStatusEnemy.color[3];

        for (int i = 0; i < 5; i++)
        {
            display.colorEnemy[i].text = battleStatusEnemy.color[i].ToString();
        }
        yield return new WaitForSeconds(0.5f);
        //アニメーション
        if (serverToClient2.battingFlg)
        {
            yield return display.openDisplay.GetComponent<OpenDisplay>().DoOpenDisplayMatch(battleStatus.deckStatus[battleStatus.setCard].unitId, serverToClient2.enemyCardId);
        }
        else if (battleStatus.setCard != 99 && serverToClient2.enemyCardId != "")
        {
            yield return display.openDisplay.GetComponent<OpenDisplay>().DoOpenDisplay(battleStatus.deckStatus[battleStatus.setCard].unitId, serverToClient2.enemyCardId);
        }
        else if(battleStatus.setCard != 99)
        {
            yield return display.openDisplay.GetComponent<OpenDisplay>().DoOpenDisplayPlayer(battleStatus.deckStatus[battleStatus.setCard].unitId);
        }
        else if(serverToClient2.enemyCardId != "")
        {
            yield return display.openDisplay.GetComponent<OpenDisplay>().DoOpenDisplayEnemy(serverToClient2.enemyCardId);
        }
        //セットカード関係

        if (battleStatus.setCard != 99)
        {
            yield return DeckState(battleStatus.setCard, 1);
        }
        if (serverToClient2.battingFlg)
        {
            yield return TrushCard(battleStatus.setCard, 2);
            yield return TrushCard(serverToClient2.enemyCardId, 2);
            display.setCard.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
            battleStatus.setCard = 99;
            prevSetCardEnemy = "";
            battleStatusEnemy.deckNum--;
            display.deckEnemy.text = battleStatusEnemy.deckNum.ToString();

        }
        else
        {
            prevSetCardEnemy = serverToClient2.enemyCardId;
            if (serverToClient2.enemyCardId != "")
            {
                yield return display.CardDisplay(serverToClient2.enemyCardId, display.setCardEnemy);
                battleStatusEnemy.deckNum--;
                display.deckEnemy.text = battleStatusEnemy.deckNum.ToString();
            }
        }
    }
    private IEnumerator SetPhase()
    {
        StartCoroutine(DisplayPhase("選択フェーズ"));
        time = 75;
        battleStatus.setCard = 99;
        okSwitch = false;
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        display.okDeckButton.interactable = true;
        display.deckWindow.SetActive(true);
        display.colorUpDown.SetActive(true);

        display.colorUpDown.GetComponent<ColorUpDown>().BottunInitialize();

        okSwitch = false;
        display.setCard.gameObject.GetComponent<Button>().enabled = true;
        while (!okSwitch)
        {
            yield return new WaitForSeconds(1.0f);
            time--;
            display.time.text = time.ToString();
            if(time == 0)
            {
                //okSwitch = true;
            }
        }
        display.okDeckButton.interactable = false;
        display.setCard.gameObject.GetComponent<Button>().enabled = false;
        display.deckWindow.SetActive(false);
        display.colorUpDown.SetActive(false);
        display.deckCardsInteractive.DeckCardInteractableFalse();

        //setの結果を送信B
        ClientToServerB clientToServerB = new ClientToServerB(battleStatus);
        if (!specialSwitch) clientToServerB.specialUse = 0;
        if (specialSwitch)
        {
            clientToServerB.specialUse = 1;
            battleStatus.specialUsed = true;
            specialSwitch = false;
        }
        json = battleJson.toJsonB(clientToServerB);
        WriteFile(json);
    }
    private IEnumerator SpPhase()
    {
        display.phase.text = "ターン開始";
        battleStatus.turn = battleStatus.turn + 1;
        display.turn.text = battleStatus.turn + "ターン";
        battleStatus.sp = battleStatus.sp + battleStatus.spNext;
        battleStatus.spNext = 2;
        display.sp.text = battleStatus.sp.ToString();
        battleStatusEnemy.sp = battleStatusEnemy.sp + battleStatusEnemy.spNext;
        battleStatusEnemy.spNext = 2;
        display.spEnemy.text = battleStatusEnemy.sp.ToString();

        AddSpecial(true, 5);
        AddSpecial(false, 5);

        if (battleStatus.specialUsed==false && battleStatus.specialStock >= specialMaster.SpecialList.Find(m => m.itemId == battleStatus.specialId).stock)
        {
            display.specialCard.GetComponent<Button>().interactable = true;
        }
        else
        {
            display.specialCard.GetComponent<Button>().interactable = false;
            if (battleStatus.specialUsed == true)display.specialStatus.text = "使用済み";
        }
        display.turnDisplayText.text = battleStatus.turn.ToString();
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(0);
        display.turnDisplay.SetActive(true);
        yield return new WaitForSeconds(3f);
        display.turnDisplay.SetActive(false);
    }
    private IEnumerator DisplayInitialize(BattleStatus battleStatus,BattleStatus battleStatusEnemy)
    {
        display.phase.text = "読み込み中";
        display.life.text = battleStatus.life.ToString();
        display.lifeEnemy.text = battleStatusEnemy.life.ToString();
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(display.ShieldDisplay(battleStatus.shieldStatus[i].shieldId, display.shieldCards[i]));
            display.shieldCards[i].gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = battleStatus.shieldStatus[i].shieldHp.ToString();
        }
        for(int i = 0; i < 25; i++)
        {
            var card = cardMaster.CardList.Find(m => m.itemId == battleStatus.deckStatus[i].unitId);
            StartCoroutine(display.CardDisplay(card, display.deckUnits[i]));
            display.deckUnits[i].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = card.color + card.level;
        }
        var special = Addressables.LoadAssetAsync<Sprite>(specialMaster.SpecialList.Find(m => m.itemId == battleStatus.specialId).id);
        yield return special;
        display.special.sprite = special.Result;
        var special1 = Addressables.LoadAssetAsync<Sprite>("奥義ストック"+battleStatus.specialColor);
        yield return special1;
        display.specialCard.transform.parent.gameObject.GetComponent<Image>().sprite = special1.Result;
        display.stock.gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = special1.Result;
        var special2 = Addressables.LoadAssetAsync<Sprite>("奥義ゲージ" + battleStatus.specialColor);
        yield return special2;
        display.gauge.gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = special2.Result;
        display.specialStatus.gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = special2.Result;
        var special3 = Addressables.LoadAssetAsync<Sprite>("奥義ゲージ" + battleStatusEnemy.specialColor);
        yield return special3;
        display.gaugeEnemy.gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = special3.Result;
        var special4 = Addressables.LoadAssetAsync<Sprite>("奥義ストック" + battleStatusEnemy.specialColor);
        yield return special4;
        display.stockEnemy.gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = special4.Result;

    }
    private BattleStatus BattleStatusInitialize(ServerToClient1 serverToClient1)
    {
        BattleStatus battleStatus = new BattleStatus();
        battleStatus.StatusInitialize();
        battleStatus.playerId = PlayerPrefs.GetString("ID");
        if (GameObject.Find("Login").GetComponent<LoginInitial>().test) battleStatus.playerId = "cfd30423-79c0-4e09-bdc3-b5cc0b4fd6cc";
        battleStatus.specialId = serverToClient1.specialId;
        battleStatus.specialColor = specialMaster.SpecialList.Find(m => m.itemId == battleStatus.specialId).color;
        for (int i = 0; i < 25; i++)
        {
            DeckStatus deckStatus = new DeckStatus();
            deckStatus.unitId = serverToClient1.deckId[i];
            deckStatus.lockTurn = 0;
            deckStatus.playStatus = 0;
            battleStatus.deckStatus[i] = deckStatus;
        }
        battleStatus.life = 1;
        for (int i = 0; i < 5; i++)
        {
            ShieldStatus shieldStatus = new ShieldStatus();
            shieldStatus.shieldId = serverToClient1.shieldId[i];
            shieldStatus.shieldHp = shieldMaster.ShieldList.Find(m => m.itemId == shieldStatus.shieldId).life;
            battleStatus.life = battleStatus.life + shieldStatus.shieldHp;
            battleStatus.shieldStatus[i] = shieldStatus;
        }
        return battleStatus;
    }
    private BattleStatus BattleStatusEnemyInitialize(ServerToClient1 serverToClient1)
    {
        BattleStatus battleStatusEnemy = new BattleStatus();
        battleStatusEnemy.StatusInitialize();
        battleStatusEnemy.playerId = serverToClient1.enemyPlayerId;
        battleStatusEnemy.life = serverToClient1.enemyLife;
        battleStatusEnemy.specialColor = serverToClient1.enemySpecialColor;
        for (int i = 0; i < 5; i++)
        {
            ShieldStatus shieldStatus = new ShieldStatus();
            shieldStatus.shieldHp = 0;
            shieldStatus.shieldId = "";
            battleStatusEnemy.shieldStatus[i] = shieldStatus;
        }
        return battleStatusEnemy;
    }
    private void WriteFile(string json)
    {
        Encoding enc = Encoding.GetEncoding("shift_jis");
        //String path = @"C:\Users\81803\Desktop\AodemiServer\test\プレイヤー" + PlayerPrefs.GetString("Player") + @"\input.json";
        //if (GameObject.Find("Login").GetComponent<LoginInitial>().test) path = @"C:\Users\81803\Desktop\AodemiServer\test\プレイヤー" + "１" + @"\input.json";
        String path = @"C:\AodemiServer\test\プレイヤー" + PlayerPrefs.GetString("Player") + @"\input.json";
        if (GameObject.Find("Login").GetComponent<LoginInitial>().test) path = @"C:\AodemiServer\test\プレイヤー" + "１" + @"\input.json";
        if (File.Exists(path))
        {
            FileInfo file = new FileInfo(path);
            if ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                file.Attributes = FileAttributes.Normal;
            }
            file.Delete();
        }
        File.WriteAllText(path, json, enc);
        Debug.Log("inputを更新した");
        Debug.Log(json);
    }
    private string RoadFile()
    {

        Encoding enc = Encoding.GetEncoding("shift_jis");
        //String path = @"C:\Users\81803\Desktop\AodemiServer\test\プレイヤー" + PlayerPrefs.GetString("Player") + @"\output.json";
        //if (GameObject.Find("Login").GetComponent<LoginInitial>().test) path = @"C:\Users\81803\Desktop\AodemiServer\test\プレイヤー" + "１" + @"\output.json";
        String path = @"C:\AodemiServer\test\プレイヤー" + PlayerPrefs.GetString("Player") + @"\output.json";
        if (GameObject.Find("Login").GetComponent<LoginInitial>().test) path = @"C:\AodemiServer\test\プレイヤー" + "１" + @"\output.json";
        StreamReader sr = new StreamReader(path, enc);
        string str = sr.ReadToEnd();
        Debug.Log(str);
        Debug.Log("outputを読み込んだ");
        sr.Close();
        sr.Dispose();

        FileInfo file = new FileInfo(path);
        if ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
        {
            file.Attributes = FileAttributes.Normal;
        }
        file.Delete();

        ServerToClient serverToClient = battleJson.toClass(str);
        if(serverToClient.severCheack == "win" || serverToClient.severCheack == "lose")
        {
            PlayerPrefs.SetInt("AddRate", serverToClient.rate);
            PlayerPrefs.SetString("BattleResult", serverToClient.severCheack);
            StartCoroutine(BattleEnd());
            return null;
        }
        return str;
    }
    private IEnumerator BattleEnd()
    {
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(10);
        display.finishEffecseer.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("BattleResult");
    }
    private IEnumerator RoadFileCheak()
    {
        //String path = @"C:\Users\81803\Desktop\AodemiServer\test\プレイヤー" + PlayerPrefs.GetString("Player") + @"\output.json";
        //if (GameObject.Find("Login").GetComponent<LoginInitial>().test) path = @"C:\Users\81803\Desktop\AodemiServer\test\プレイヤー" + "１" + @"\output.json";
        String path = @"C:\AodemiServer\test\プレイヤー" + PlayerPrefs.GetString("Player") + @"\output.json";
        if (GameObject.Find("Login").GetComponent<LoginInitial>().test) path = @"C:\AodemiServer\test\プレイヤー" + "１" + @"\output.json";
        display.waitMessage.SetActive(true);
        Debug.Log("output待ち");
        while (true)
        {
            if (File.Exists(path))
            {
                break;
            }
            yield return new WaitForSeconds(0.5f);

        }
        display.waitMessage.SetActive(false);
        yield return new WaitForSeconds(1.0f);
    }
    private IEnumerator BGMChange()
    {
        GameObject audioObj = GameObject.Find("AudioBGM");
        System.Random r = new System.Random();
        switch (r.Next(1, 4))
        {
            case 2:
                yield return audioObj.GetComponent<AudioController>().BGMChange("02");
                break;

            case 3:
                yield return audioObj.GetComponent<AudioController>().BGMChange("05");
                break;

            default:
                yield return audioObj.GetComponent<AudioController>().BGMChange("01");
                break;
        }
    }
    private IEnumerator RemoveCard(int trashNum,bool enemy)
    {
        yield return ReviveCard(trashNum, enemy);
        //墓地のカードとデッキ番号を紐づけて墓地送りにする
    }
    public IEnumerator ReviveCard(int trashNum,bool enemy)
    {
        BattleStatus battleStatusX;
        battleStatusX = battleStatus;
        Debug.Log("ok1");
        if (enemy) battleStatusX = battleStatusEnemy;
        if (!enemy) StartCoroutine(DeckState(battleStatusX.trashDeck[0, trashNum], 0));
        Debug.Log("ok2");
        Image[] trashCard;
        trashCard = display.trashCard;
        if (enemy) trashCard = display.trashCardEnemy;

        for (int j = trashNum; j < 25; j++)
        {
            if (j != 24)
            {
                battleStatusX.trash[0, j] = battleStatusX.trash[0, j + 1];
                battleStatusX.trashDeck[0, j] = battleStatusX.trashDeck[0, j + 1];
                if (battleStatusX.trash[0, j] != "")
                {
                    StartCoroutine(display.CardDisplay(battleStatusX.trash[0, j], trashCard[j]));
                }
                else
                {
                    trashCard[j].gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
                }
            }
            else
            {
                battleStatusX.trash[0, j] = "";
                battleStatusX.trashDeck[0, j] = 99;
                display.trashCard[j].gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
        battleStatusX.trashNum[0]= battleStatusX.trashNum[0] -1;
        display.death.text = (battleStatus.trashNum[0] + battleStatus.trashNum[1]).ToString();
        display.deathEnemy.text = (battleStatusEnemy.trashNum[0] + battleStatusEnemy.trashNum[1]).ToString();
        yield return new WaitForSeconds(0);
    }
    public IEnumerator TrushCard(int deckNum, int type)
    {
        //墓地から消滅させる場合は一度デッキに戻してから消滅させればよい
        battleStatus.trash[type-2, battleStatus.trashNum[type-2]] = battleStatus.deckStatus[deckNum].unitId;
        battleStatus.trashDeck[type - 2, battleStatus.trashNum[type - 2]] = deckNum;
        if (type == 2) yield return display.CardDisplay(battleStatus.deckStatus[deckNum].unitId, display.trashCard[battleStatus.trashNum[type-2]]);
        if (type == 3) yield return display.CardDisplay(battleStatus.deckStatus[deckNum].unitId, display.vanishCard[battleStatus.trashNum[type-2]]);
        yield return DeckState(deckNum, type);
        battleStatus.trashNum[type-2]++;
        display.death.text = (battleStatus.trashNum[0] + battleStatus.trashNum[1]).ToString();
    }
    private IEnumerator TrushCard(string cardId,int type)
    {
        battleStatusEnemy.trash[type-2, battleStatusEnemy.trashNum[type-2]] = cardId;
        if (type == 2) yield return display.CardDisplay(cardId, display.trashCardEnemy[battleStatusEnemy.trashNum[type-2]]);
        if (type == 3) yield return display.CardDisplay(cardId, display.vanishCardEnemy[battleStatusEnemy.trashNum[type-2]]);
        battleStatusEnemy.trashNum[type-2]++;
        display.deathEnemy.text = (battleStatusEnemy.trashNum[0] + battleStatusEnemy.trashNum[1]).ToString();
    }
    private IEnumerator DeckLock(int deckNum, int turn)
    {
        battleStatus.deckStatus[deckNum].playStatus = 4;
        battleStatus.deckStatus[deckNum].lockTurn = turn;
        yield return display.DeckStateDisplay(deckNum, battleStatus.deckStatus[deckNum].playStatus, battleStatus.deckStatus[deckNum].lockTurn);
    }
    private IEnumerator DeckState(int deckNum,int state)
    {
        if(battleStatus.deckStatus[deckNum].playStatus == 0 || battleStatus.deckStatus[deckNum].playStatus == 4)
        {
            if(state == 1 || state == 2 || state == 3)
            {
                battleStatus.deckNum--;
                display.deck.text = battleStatus.deckNum.ToString();
            }
        }
        else
        {
            if (state == 0 || state == 4)
            {
                battleStatus.deckNum++;
                display.deck.text = battleStatus.deckNum.ToString();
            }
        }
        battleStatus.deckStatus[deckNum].playStatus = state;
        yield return display.DeckStateDisplay(deckNum, battleStatus.deckStatus[deckNum].playStatus);
    }
    private TargetList[] GetTargets()
    {
        int[] count = new int[2] { 0, 0 };
        for (int i = 0; i < 9; i++)
        {
            if (targetSelect[0, i]) count[0]++;
            if (targetSelect[1, i]) count[1]++;
        }
        int[] targetSelectReturn = new int[count[0]];
        int[] targetSelectReturnEnemy = new int[count[1]];
        int a = 0, b = 0;
        for (int i = 0; i < 9; i++)
        {
            if (targetSelect[0, i])
            {
                targetSelectReturn[a] = i;
                a++;
            }
            if (targetSelect[1, i])
            {
                targetSelectReturnEnemy[b] = i;
                b++;
            }
        }
        int k=0;
        if (a > 0) k++;
        if (b > 0) k++;
        TargetList[] targetList = new TargetList[k];
        TargetList target1 = new TargetList();
        TargetList target2 = new TargetList();
        target1.playerId = battleStatus.playerId;
        target2.playerId = battleStatusEnemy.playerId;
        target1.list = targetSelectReturn;
        target2.list = targetSelectReturnEnemy;
        int j = 0;
        if (a > 0)
        {
            targetList[j] = target1;
            j++;
        }
        if(b>0)targetList[j] = target2;
        return targetList;
    }
    private void UpdateSpecial(bool enemy, BattleFolder.Special special)
    {
        if (enemy)
        {
            if (special.stock != null)
            {
                battleStatusEnemy.specialStock = special.stock.Value;
                display.stockEnemy.text = battleStatusEnemy.specialStock.ToString();
            }
            if (special.gage != null)
            {
                battleStatusEnemy.specialGauge = special.gage.Value;
                display.gaugeEnemy.text = battleStatusEnemy.specialGauge.ToString();
            }
        }
        else
        {
            if (special.stock != null)
            {
                battleStatus.specialStock = special.stock.Value;
                display.stock.text = battleStatus.specialStock.ToString();
            }
            if (special.gage != null)
            {
                battleStatus.specialGauge = special.gage.Value;
                display.gauge.text = battleStatus.specialGauge.ToString();
            }
        }
    }
    private void AddSpecial(bool enemy,int num)
    {
        if (enemy)
        {
            int sum = battleStatusEnemy.specialGauge + battleStatusEnemy.specialStock * 20;
            sum = sum + num;
            battleStatusEnemy.specialStock = sum / 20;
            battleStatusEnemy.specialGauge = sum % 20;
            if (battleStatusEnemy.specialStock > 5) battleStatusEnemy.specialStock = 5;
            if (battleStatusEnemy.specialStock == 5) battleStatusEnemy.specialGauge = 0;
            display.gaugeEnemy.text = battleStatusEnemy.specialGauge.ToString();
            display.stockEnemy.text = battleStatusEnemy.specialStock.ToString();
        }
        else
        {
            int sum = battleStatus.specialGauge + battleStatus.specialStock * 20;
            sum = sum + num;
            battleStatus.specialStock = sum / 20;
            battleStatus.specialGauge = sum % 20;
            if (battleStatus.specialStock > 5) battleStatus.specialStock = 5;
            if (battleStatus.specialStock == 5) battleStatus.specialGauge = 0;
            display.gauge.text = battleStatus.specialGauge.ToString();
            display.stock.text = battleStatus.specialStock.ToString();
        }
    }
    private void UpdateCount(bool enemy, CountAdd countAdd)
    {
        if (enemy)
        {
            battleStatusEnemy.magicNum = battleStatusEnemy.magicNum + countAdd.magic;
        }
        else
        {
            battleStatus.magicNum = battleStatus.magicNum + countAdd.magic;
        }
    }
    private void UpdateMana(bool enemy, Mana mana)
    {
        if (enemy)
        {
            if (mana.blue != null)
            {
                battleStatusEnemy.color[0] = int.Parse(mana.blue);
                display.color[0].text = battleStatusEnemy.color[0].ToString();
            }
            if (mana.yellow != null)
            {
                battleStatusEnemy.color[1] = int.Parse(mana.blue);
                display.colorEnemy[1].text = battleStatusEnemy.color[1].ToString();
            }
            if (mana.red != null)
            {
                battleStatusEnemy.color[2] = int.Parse(mana.blue);
                display.colorEnemy[2].text = battleStatusEnemy.color[2].ToString();
            }
            if (mana.black != null)
            {
                battleStatusEnemy.color[3] = int.Parse(mana.blue);
                display.colorEnemy[3].text = battleStatusEnemy.color[3].ToString();
            }
            battleStatusEnemy.color[4] = battleStatusEnemy.color[0] + battleStatusEnemy.color[1] + battleStatusEnemy.color[2] + battleStatusEnemy.color[3];
            display.colorEnemy[4].text = battleStatusEnemy.color[4].ToString();
        }
        else
        {
            if (mana.blue != null)
            {
                battleStatus.color[0] = int.Parse(mana.blue);
                display.color[0].text = battleStatus.color[0].ToString();
            }
            if (mana.yellow != null)
            {
                battleStatus.color[1] = int.Parse(mana.blue);
                display.color[1].text = battleStatus.color[1].ToString();
            }
            if (mana.red != null)
            {
                battleStatus.color[2] = int.Parse(mana.blue);
                display.color[2].text = battleStatus.color[2].ToString();
            }
            if (mana.black != null)
            {
                battleStatus.color[3] = int.Parse(mana.blue);
                display.color[3].text = battleStatus.color[3].ToString();
            }
            battleStatus.color[4] = battleStatus.color[0] + battleStatus.color[1] + battleStatus.color[2] + battleStatus.color[3];
            display.color[4].text = battleStatus.color[4].ToString();
        }
    }
    private void UpdateSp(bool enemy, int sp)
    {
        if (enemy)
        {
                battleStatusEnemy.sp = sp;
                display.spEnemy.text = battleStatusEnemy.sp.ToString();
        }
        else
        {
                battleStatus.sp = sp;
                display.sp.text = battleStatus.sp.ToString();
        }
    }
    private void UpdateSp(bool enemy,Sp sp)
    {
        if (enemy)
        {
            if (sp.SP != null)
            {
                battleStatusEnemy.sp = sp.SP.Value;
                display.spEnemy.text = battleStatusEnemy.sp.ToString();
            }
            if (sp.nextSP != null)
            {
                battleStatusEnemy.spNext = sp.nextSP.Value;
            }
        }
        else
        {
            if (sp.SP != null)
            {
                battleStatus.sp = sp.SP.Value;
                display.sp.text = battleStatus.sp.ToString();
            }
            if (sp.nextSP != null)
            {
                battleStatus.spNext = sp.nextSP.Value;
            }
        }
    }
    private IEnumerator UnitReturn(int area,bool enemy)
    {
        if (enemy)
        {
            battleStatusEnemy.deckNum = battleStatusEnemy.deckNum + 1;
            display.deckEnemy.text = battleStatusEnemy.deckNum.ToString();
            battleStatusEnemy.unitStatus[area].unitId = "";
            display.hpBarEnemy[area].SetActive(false);
            display.fieldUnitEnemy[area].gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
            display.fieldUnitEnemy[area].color = new Color(1f, 1f, 1f);
        }
        else
        {
            yield return TrushCard(battleStatus.unitStatus[area].deckId, 2);
            yield return ReviveCard(battleStatus.trashNum[0]-1, false);
            battleStatus.unitStatus[area].unitId = "";
            display.hpBar[area].SetActive(false);
            display.fieldUnit[area].gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
            display.fieldUnit[area].color = new Color(1f, 1f, 1f);
        }
    }
    private IEnumerator UnitVanish(int area, bool enemy)
    {
        if (enemy)
        {
            yield return TrushCard(battleStatusEnemy.unitStatus[area].unitId, 3);
            battleStatusEnemy.unitStatus[area].unitId = "";
            display.hpBarEnemy[area].SetActive(false);
            display.fieldUnitEnemy[area].gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
            display.fieldUnitEnemy[area].color = new Color(1f, 1f, 1f);
        }
        else
        {
            yield return TrushCard(battleStatus.unitStatus[area].deckId, 3);
            battleStatus.unitStatus[area].unitId = "";
            display.hpBar[area].SetActive(false);
            display.fieldUnit[area].gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
            display.fieldUnit[area].color = new Color(1f, 1f, 1f);
        }

    }
    private IEnumerator UnitDeath(int area,bool enemy)
    {
        if (enemy)
        {
            yield return TrushCard(battleStatusEnemy.unitStatus[area].unitId, 2);
            battleStatusEnemy.unitStatus[area].unitId = "";
            display.hpBarEnemy[area].SetActive(false);
            display.fieldUnitEnemy[area].gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
            display.fieldUnitEnemy[area].color = new Color(1f, 1f, 1f);
        }
        else
        {
            yield return TrushCard(battleStatus.unitStatus[area].deckId, 2);
            battleStatus.unitStatus[area].unitId = "";
            display.hpBar[area].SetActive(false);
            display.fieldUnit[area].gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
            display.fieldUnit[area].color = new Color(1f, 1f, 1f);
        }

    }
    private IEnumerator UpdateMove(bool enemy, int pre,int aft)
    {
        if (enemy)
        {
            battleStatusEnemy.unitStatus[aft] = battleStatusEnemy.unitStatus[pre].Clone();
            battleStatusEnemy.unitStatus[pre].unitId = "";
            display.fieldUnitEnemy[pre].gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
            display.fieldUnitEnemy[pre].color=new Color(1f,1f,1f);
            display.hpBarEnemy[pre].SetActive(false);
            yield return display.CardDisplay(battleStatusEnemy.unitStatus[aft].unitId, display.fieldUnitEnemy[aft]);
            if(battleStatusEnemy.unitStatus[aft].action==true) display.fieldUnitEnemy[pre].color = new Color(0.5f, 0.5f, 0.5f);
            if (battleStatusEnemy.unitStatus[aft].close == true) display.fieldUnitEnemy[pre].color = new Color(0f, 0f, 0f);
            display.hpBarEnemy[aft].transform.GetChild(0).gameObject.GetComponent<Text>().text = battleStatusEnemy.unitStatus[aft].nowHP + "/" + (battleStatusEnemy.unitStatus[aft].defHP + battleStatusEnemy.unitStatus[aft].upHP + battleStatusEnemy.unitStatus[aft].tupHP);
            display.hpBarEnemy[aft].SetActive(true);
        }
        else
        {
            battleStatus.unitStatus[aft] = battleStatus.unitStatus[pre].Clone();
            battleStatus.unitStatus[pre].unitId = "";
            battleStatus.unitStatus[pre].deckId = 99;
            display.fieldUnit[pre].gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
            display.fieldUnit[pre].color = new Color(1f, 1f, 1f);
            display.hpBar[pre].SetActive(false);
            yield return display.CardDisplay(battleStatus.unitStatus[aft].unitId, display.fieldUnit[aft]);
            if (battleStatus.unitStatus[aft].action == true) display.fieldUnit[pre].color = new Color(0.5f, 0.5f, 0.5f);
            if (battleStatus.unitStatus[aft].close == true) display.fieldUnit[pre].color = new Color(0f, 0f, 0f);
            display.hpBar[aft].transform.GetChild(0).gameObject.GetComponent<Text>().text = battleStatus.unitStatus[aft].nowHP + "/" + (battleStatus.unitStatus[aft].defHP + battleStatus.unitStatus[aft].upHP + battleStatus.unitStatus[aft].tupHP);
            display.hpBar[aft].SetActive(true);
        }
    }
    private void UpdateActionEnd(bool enemy,int area,bool end)
    {
        if (enemy)
        {
            if (end)
            {
                display.fieldUnitEnemy[area].color = new Color(0.5f, 0.5f, 0.5f);
                battleStatusEnemy.unitStatus[area].action = true;
            }
            else
            {
                display.fieldUnitEnemy[area].color = new Color(1f, 1f, 1f);
                battleStatusEnemy.unitStatus[area].action = false;
            }
        }
        else
        {
            if (end)
            {
                display.fieldUnit[area].color = new Color(0.5f, 0.5f, 0.5f);
                battleStatus.unitStatus[area].action = true;
            }
            else
            {
                display.fieldUnit[area].color = new Color(1f, 1f, 1f);
                battleStatus.unitStatus[area].action = false;
            }
        }
    }
    private IEnumerator UpdateField(bool enemyCard, Effect effect, Field[] field)
    {
        for(int i = 0; i < field.Length; i++)
        {
            bool enemy = field[i].playerId == battleStatusEnemy.playerId;
            Debug.Log("enemycard:"+enemyCard);
            Debug.Log("enemy:" + enemy);
            if (enemyCard && enemy && effect.player != "") yield return areaEffect.BattleAreaEffect(enemy, field[i].fieldNumber, effect.player);
            if (!enemyCard && enemy && effect.enemy != "") yield return areaEffect.BattleAreaEffect(enemy, field[i].fieldNumber, effect.enemy);
            if (enemyCard && !enemy && effect.enemy != "") yield return areaEffect.BattleAreaEffect(enemy, field[i].fieldNumber, effect.enemy);
            if (!enemyCard && !enemy && effect.player != "") yield return areaEffect.BattleAreaEffect(enemy, field[i].fieldNumber, effect.player);
            yield return UpdateStatus(field[i],enemy);
            if (field[i].remove=="deck") yield return UnitReturn(field[i].fieldNumber, enemy);
            if (field[i].remove == "cemetery") yield return UnitDeath(field[i].fieldNumber, enemy);
            if (field[i].remove == "vanish") yield return UnitVanish(field[i].fieldNumber, enemy);
            if (field[i].remove == "action") UpdateActionEnd(enemy,field[i].fieldNumber, false);
            if (field[i].remove == "actionEnd") UpdateActionEnd(enemy, field[i].fieldNumber, true);
            if (field[i].move != null) yield return UpdateMove(enemy, field[i].fieldNumber,field[i].move.Value);
        }
    }
    private IEnumerator UpdateStatus(Field field,bool enemy)
    {
        UnitStatus unit = new UnitStatus();
        if (enemy) unit = battleStatusEnemy.unitStatus[field.fieldNumber];
        if (!enemy) unit = battleStatus.unitStatus[field.fieldNumber];
        if (field.upHP != null || field.tupHP != null) yield return UpdateMhp(field,unit,enemy);
        if (field.hp != null) yield return UpdateHp(field);
        if (field.upFRM != null)
        {
            unit.upSTK = field.upFRM.Value;
        }
        if (field.tupFRM != null)
        {
            unit.tupSTK = field.tupFRM.Value;
        }
        if (field.upATK != null)
        {
            unit.upATK = field.upATK.Value;
        }
        if (field.tupATK != null)
        {
            unit.tupATK = field.tupATK.Value;
        }
        if (field.upDEF != null)
        {
            unit.upDFE = field.upDEF.Value;
        }
        if (field.tupDEF != null)
        {
            unit.tupDFE = field.tupDEF.Value;
        }
        if (field.upLV != null)
        {
            unit.upLV = field.upLV.Value;
        }
        if (field.tupLV != null)
        {
            unit.tupLV = field.tupLV.Value;
        }
        if (field.upAGI != null)
        {
            unit.upAGI = field.upAGI.Value;
        }
        if (field.tupAGI != null)
        {
            unit.tupAGI = field.tupAGI.Value;
        }
        if (field.upRNG != null)
        {
            unit.upRNG = field.upRNG.Value;
        }
        if (field.tupRNG != null)
        {
            unit.tupRNG = field.tupRNG.Value;
        }
    }
    private IEnumerator UpdateMhp(Field field,UnitStatus unit,bool enemy)
    {
        int mhp = unit.defHP;
        if (field.upHP != null) mhp = mhp + field.upHP.Value;
        if (field.tupHP != null) mhp = mhp + field.tupHP.Value;
        if (unit.nowHP > mhp) unit.nowHP = mhp;
        if(enemy)display.hpBarEnemy[field.fieldNumber].transform.GetChild(0).gameObject.GetComponent<Text>().text = unit.nowHP + "/" + mhp;
        else display.hpBar[field.fieldNumber].transform.GetChild(0).gameObject.GetComponent<Text>().text = unit.nowHP + "/" + mhp;
        yield return new WaitForSeconds(0f);
    }
    private IEnumerator UpdateHp(Field field)
    {
        yield return new WaitForSeconds(0f);
        int hp = field.hp.Value;
        bool enemyArea = field.playerId == battleStatusEnemy.playerId;
        if (enemyArea)
        {
            int damage = hp - battleStatusEnemy.unitStatus[field.fieldNumber].nowHP;

            if (damage >= 0)
            {
                display.damageEnemy[field.fieldNumber].color = new Color(0f, 1f, 0f);
                display.damageEnemy[field.fieldNumber].text = damage.ToString();
            }
            else
            {
                display.damageEnemy[field.fieldNumber].color = new Color(1f, 1f, 1f);
                damage = damage * (-1);
                display.damageEnemy[field.fieldNumber].text = damage.ToString();
            }
            battleStatusEnemy.unitStatus[field.fieldNumber].nowHP = hp;
            display.hpBarEnemy[field.fieldNumber].transform.GetChild(0).gameObject.GetComponent<Text>().text = battleStatusEnemy.unitStatus[field.fieldNumber].nowHP + "/" + (battleStatusEnemy.unitStatus[field.fieldNumber].defHP + battleStatusEnemy.unitStatus[field.fieldNumber].upHP + battleStatusEnemy.unitStatus[field.fieldNumber].tupHP);
            if (hp > 0)
            {
                battleStatusEnemy.unitStatus[field.fieldNumber].close = false;
                if (battleStatusEnemy.unitStatus[field.fieldNumber].action == false) display.fieldUnitEnemy[field.fieldNumber].color = new Color(1f, 1f, 1f);
                if (battleStatusEnemy.unitStatus[field.fieldNumber].action == true) display.fieldUnitEnemy[field.fieldNumber].color = new Color(0.5f, 0.5f, 0.5f);
            }
            StartCoroutine(UpdateHpSub(true, field.fieldNumber));
        }
        else
        {

            int damage = hp - battleStatus.unitStatus[field.fieldNumber].nowHP;
            if (damage >= 0)
            {
                display.damage[field.fieldNumber].color = new Color(0f, 1f, 0f);
                display.damage[field.fieldNumber].text = damage.ToString();
            }
            else
            {
                display.damage[field.fieldNumber].color = new Color(1f, 1f, 1f);
                damage = damage * (-1);
                display.damage[field.fieldNumber].text = damage.ToString();
            }
            battleStatus.unitStatus[field.fieldNumber].nowHP = hp;
            display.hpBar[field.fieldNumber].transform.GetChild(0).gameObject.GetComponent<Text>().text = battleStatus.unitStatus[field.fieldNumber].nowHP + "/" + (battleStatus.unitStatus[field.fieldNumber].defHP + battleStatus.unitStatus[field.fieldNumber].upHP + battleStatus.unitStatus[field.fieldNumber].tupHP);
            if (hp > 0)
            {
                battleStatus.unitStatus[field.fieldNumber].close = false;
                if (battleStatus.unitStatus[field.fieldNumber].action == false) display.fieldUnit[field.fieldNumber].color = new Color(1f, 1f, 1f);
                if (battleStatus.unitStatus[field.fieldNumber].action==true) display.fieldUnit[field.fieldNumber].color = new Color(0.5f, 0.5f, 0.5f);
            }
            StartCoroutine(UpdateHpSub(false, field.fieldNumber));
        }
    }
    private IEnumerator UpdateHpSub(bool enemy, int fieldNumber)
    {
        if (enemy)
        {
            yield return new WaitForSeconds(1.0f);
            display.damageEnemy[fieldNumber].text = "";
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
            display.damage[fieldNumber].text = "";
        }
    }
    private IEnumerator TargetSelect(Target[] target, UnityEngine.Events.UnityAction<Target[]> callback)
    {
        Target[] select=new Target[target.Length];
        for (int i = 0; i < target.Length; i++)
        {
            yield return TargetSelect(target[i].targetList,target[i].selectCount);
            TargetList[] targetLists = GetTargets();
            Target targetTemp = new Target();
            targetTemp.targetList = targetLists;
            select[i] = targetTemp;
        }
        callback(select);
    }
    private IEnumerator TargetSelect(TargetList[] target, int num)
    {
        targetCount = num;
        for (int i = 0; i < 9; i++)
        {
            targetSelect[0, i] = false;
            targetSelect[1, i] = false;
            display.target[i].GetComponent<Button>().interactable = true;
            display.targetEnemy[i].GetComponent<Button>().interactable = true;
        }
        for (int i = 0; i < target.Length; i++)
        {
            if (target[i].playerId == battleStatus.playerId)
            {

                for (int j = 0; j < target[i].list.Length; j++)
                {
                    display.target[target[i].list[j]].SetActive(true);
                    display.target[target[i].list[j]].GetComponent<Button>().enabled = true;
                }
            }
            else
            {
                for (int j = 0; j < target[i].list.Length; j++)
                {
                    display.targetEnemy[target[i].list[j]].SetActive(true);
                    display.targetEnemy[target[i].list[j]].GetComponent<Button>().enabled = true;
                }
            }
        }
        okSwitch = false;
        while (!okSwitch)
        {
            yield return new WaitForSeconds(1.0f);
        }
        for (int i = 0; i < 9; i++)
        {
            display.target[i].transform.GetChild(0).gameObject.SetActive(false);
            display.target[i].SetActive(false);
            display.targetEnemy[i].transform.GetChild(0).gameObject.SetActive(false);
            display.targetEnemy[i].SetActive(false);
        }
    }
    private IEnumerator RevivalSelect(Target[] target)
    {
        for (int i = 0; i < 9; i++)
        {
            targetSelect[0, i] = false;

            if (battleStatus.sp > 0)
            {
                display.targetRevival[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                display.targetRevival[i].GetComponent<Button>().interactable = false;
                if (cardMaster.CardList.Find(m => m.itemId == battleStatus.unitStatus[i].unitId).textSkillName == "復帰") display.targetRevival[i].GetComponent<Button>().interactable = true;
            }
        }

        for (int j = 0; j < target[0].targetList[0].list.Length; j++)
        {
            for (int k = 0; k < battleStatus.deckStatus.Length; k++)
            {
                if(battleStatus.deckStatus[k].unitId== battleStatus.unitStatus[target[0].targetList[0].list[j]].unitId && battleStatus.deckStatus[k].playStatus==0)
                {
                    display.targetRevival[target[0].targetList[0].list[j]].SetActive(true);
                    display.targetRevival[target[0].targetList[0].list[j]].GetComponent<Button>().enabled = true;
                    break;
                }
            }
        }

        okSwitch = false;
        display.okButton.interactable = true;
        while (!okSwitch)
        {
            yield return new WaitForSeconds(1f);
        }
        display.okButton.interactable = false;
        for (int i = 0; i < 9; i++)
        {
            display.targetRevival[i].transform.GetChild(0).gameObject.SetActive(false);
            display.targetRevival[i].SetActive(false);
        }
        for(int i = 0; i < 9; i++)
        {
            if (targetSelect[0, i]) battleStatus.unitStatus[i].revival = true;
        }
    }
    private IEnumerator Surrender()
    {
        ClientToServer clientToServer = new ClientToServer();
        clientToServer.playerId = battleStatus.playerId;
        WriteFile(battleJson.toJson(clientToServer));
        yield return RoadFileCheak();
        RoadFile();
    }
    public void SurrenderButton()
    {
        StartCoroutine(Surrender());
    }
    private IEnumerator DisplayPhase(string phase)
    {
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().AnimeEffect(4);
        display.phase.text = "　　　　"+phase;
        display.phase.color = new Color(1f,0f,0f);
        yield return new WaitForSeconds(0.05f);
        display.phase.text = "　　　" + phase;
        display.phase.color = new Color(0.8f, 0f, 0f);
        yield return new WaitForSeconds(0.05f);
        display.phase.text = "　　" + phase;
        display.phase.color = new Color(0.5f, 0f, 0f);
        yield return new WaitForSeconds(0.05f);
        display.phase.text = "　" + phase;
        display.phase.color = new Color(0.3f, 0f, 0f);
        yield return new WaitForSeconds(0.05f);
        display.phase.text = phase;
        display.phase.color = new Color(0f, 0f, 0f);
    }
}
