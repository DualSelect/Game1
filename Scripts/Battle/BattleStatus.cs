using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleFolder
{
    public class BattleStatus
    {
        public string playerId { get; set; }
        public string playerNumber { get; set; }
        public int life { get; set; }
        public int turn { get; set; }
        public int sp { get; set; }
        public int spNext { get; set; }//次ターンでもらえるSP
        public int[] color { get; set; } = new int[5] { 0, 0, 0, 0, 0 };
        public int[] colorUp { get; set; } = new int[5] { 0, 0, 0, 0, 0 };
        public string specialId { get; set; }
        public string specialColor { get; set; }
        public int setCard { get; set; } = 99;
        public int specialGauge { get; set; }
        public int specialStock { get; set; }
        public bool specialUsed { get; set; }
        public bool specialTurn { get; set; }//奥義を使ったターンかどうか
        public int specialLock { get; set; }
        public int magicNum { get; set; }//魔法回数
        public string[,] trash { get; set; } = new string[2,25];
        public int[,] trashDeck { get; set; } = new int[2, 25];
        public int[] trashNum { get; set; } = new int[2];
        public DeckStatus[] deckStatus { get; set; } = new DeckStatus[25];
        public int deckNum { get; set; }
        public UnitStatus[] unitStatus { get; set; } = new UnitStatus[9];
        public ShieldStatus[] shieldStatus { get; set; } = new ShieldStatus[5];
        public string setPosition { get; set; } = "";

        //デッキ内容にかかわらず初期化できる部分を初期化
        //初期化しない要素：プレイヤーID、ライフ、奥義ID、奥義カラー、デッキのカードID、シールドのカードIDおよびHP、盤面ステータスのユニットID以外
        public void StatusInitialize()
        {
            turn = 0;
            sp = 3;
            spNext = 2;
            specialGauge = 0;
            specialStock = 0;
            specialUsed = false;
            specialTurn = false;
            specialLock = 0;
            trashNum[0] = 0;
            trashNum[1] = 0;
            deckNum = 25;
            magicNum = 0;
            for (int i = 0; i < 25; i++)
            {
                trash[0,i] = "0";
                trash[1,i] = "0";
                DeckStatus deckStatus1 = new DeckStatus();
                deckStatus1.playStatus = 0;
                deckStatus1.lockTurn = 0;
                deckStatus[i] = deckStatus1;
                trash[0, i] = "";
                trash[1, i] = "";
            }
            for (int i = 0; i < 9; i++)
            {
                    UnitStatus unitStatus1 = new UnitStatus();
                    unitStatus1.unitId = "";
                    unitStatus1.deckId = 99;
                    unitStatus[i] = unitStatus1;
            }
            for (int i = 0; i < 5; i++)
            {
                ShieldStatus shieldStatus1 = new ShieldStatus();
                shieldStatus1.shieldId = "";
                shieldStatus1.shieldHp = 0;
                shieldStatus[i] = shieldStatus1;
            }
        }
    }
}