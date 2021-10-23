using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace BattleFolder
{
    public class BattleJson
    {
        public string toJson(ClientToServer clientToServer)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ClientToServer));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, clientToServer);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        public string toJsonA(ClientToServerA clientToServerA)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ClientToServerA));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, clientToServerA);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        public string toJsonB(ClientToServerB clientToServerB)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ClientToServerB));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, clientToServerB);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        public string toJsonC(ClientToServerC clientToServerC)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ClientToServerC));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, clientToServerC);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        public string toJsonD(ClientToServerD clientToServerD)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ClientToServerD));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, clientToServerD);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        public string toJsonE(ClientToServerE clientToServerE)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ClientToServerE));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, clientToServerE);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        public string toJsonF(ClientToServerF clientToServerF)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ClientToServerF));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, clientToServerF);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        public string toJsonF1(ClientToServerF1 clientToServerF1)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ClientToServerF1));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, clientToServerF1);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        public string toJsonF2(ClientToServerF2 clientToServerF2)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ClientToServerF2));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, clientToServerF2);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        public string toJsonG(ClientToServerG clientToServerG)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ClientToServerG));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, clientToServerG);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        public ServerToClient toClass(string str)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServerToClient));
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream(bytes);
            return (ServerToClient)serializer.ReadObject(ms);
        }
        public ServerToClient1 toClass1(string string1)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServerToClient1));
            byte[] bytes = Encoding.UTF8.GetBytes(string1);
            MemoryStream ms = new MemoryStream(bytes);
            return (ServerToClient1)serializer.ReadObject(ms);
        }
        public ServerToClient2 toClass2(string string2)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServerToClient2));
            byte[] bytes = Encoding.UTF8.GetBytes(string2);
            MemoryStream ms = new MemoryStream(bytes);
            return (ServerToClient2)serializer.ReadObject(ms);
        }

        public ServerToClient3 toClass3(string string3)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServerToClient3));
            byte[] bytes = Encoding.UTF8.GetBytes(string3);
            MemoryStream ms = new MemoryStream(bytes);
            return (ServerToClient3)serializer.ReadObject(ms);
        }
        public ServerToClient4 toClass4(string string4)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServerToClient4));
            byte[] bytes = Encoding.UTF8.GetBytes(string4);
            MemoryStream ms = new MemoryStream(bytes);
            return (ServerToClient4)serializer.ReadObject(ms);
        }
        public ServerToClient5 toClass5(string string5)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServerToClient5));
            byte[] bytes = Encoding.UTF8.GetBytes(string5);
            MemoryStream ms = new MemoryStream(bytes);
            return (ServerToClient5)serializer.ReadObject(ms);
        }
        public ServerToClient6 toClass6(string string6)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServerToClient6));
            byte[] bytes = Encoding.UTF8.GetBytes(string6);
            MemoryStream ms = new MemoryStream(bytes);
            return (ServerToClient6)serializer.ReadObject(ms);
        }
        public ServerToClient6A toClass6A(string string6A)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServerToClient6A));
            byte[] bytes = Encoding.UTF8.GetBytes(string6A);
            MemoryStream ms = new MemoryStream(bytes);
            return (ServerToClient6A)serializer.ReadObject(ms);
        }
        public ServerToClient6B toClass6B(string string6B)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServerToClient6B));
            byte[] bytes = Encoding.UTF8.GetBytes(string6B);
            MemoryStream ms = new MemoryStream(bytes);
            return (ServerToClient6B)serializer.ReadObject(ms);
        }
        public ServerToClient7 toClass7(string string7)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServerToClient7));
            byte[] bytes = Encoding.UTF8.GetBytes(string7);
            MemoryStream ms = new MemoryStream(bytes);
            return (ServerToClient7)serializer.ReadObject(ms);
        }
        public Effect toEffect(string effect)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Effect));
            byte[] bytes = Encoding.UTF8.GetBytes(effect);
            MemoryStream ms = new MemoryStream(bytes);
            return (Effect)serializer.ReadObject(ms);
        }
        public RankUp toRankUp(string rankUp)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RankUp));
            byte[] bytes = Encoding.UTF8.GetBytes(rankUp);
            MemoryStream ms = new MemoryStream(bytes);
            return (RankUp)serializer.ReadObject(ms);
        }
    }

    //サレンダー
    [System.Runtime.Serialization.DataContract]
    public class ClientToServer
    {
        [System.Runtime.Serialization.DataMember()]
        public string phase { get; set; } = "surrender";
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
    }
    //勝敗の結果
    [System.Runtime.Serialization.DataContract]
    public class ServerToClient
    {
        [System.Runtime.Serialization.DataMember()]
        public string severCheack { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int rate { get; set; }
    }
    //初期情報
    [System.Runtime.Serialization.DataContract]
    public class ClientToServerA
    {
        [System.Runtime.Serialization.DataMember()]
        public string phase { get; set; } = "init";
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int deckNumber { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string playerName { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int playerRate { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string playerIcon { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int playerNum { get; set; }
        public ClientToServerA(string playerId,int deckNumber)
        {
            this.playerId = playerId;
            this.deckNumber = deckNumber;
        }
    }
    //初期情報の返し
    [System.Runtime.Serialization.DataContract]
    public class ServerToClient1
    {
        [System.Runtime.Serialization.DataMember()]
        public string player { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string[] deckId { get; set; } = new string[25];
        [System.Runtime.Serialization.DataMember()]
        public string[] shieldId { get; set; } = new string[5];
        [System.Runtime.Serialization.DataMember()]
        public string specialId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string enemyPlayerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int enemyLife { get; set; } = 1;
        [System.Runtime.Serialization.DataMember()]
        public string enemySpecialColor { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string enemyPlayerName { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int enemyPlayerRate { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string enemyPlayerIcon { get; set; }
    }
    //セット内容
    [System.Runtime.Serialization.DataContract]
    public class ClientToServerB
    {
        [System.Runtime.Serialization.DataMember()]
        public string phase { get; set; } = "set";
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public bool[] revivalNumber { get; set; } = new bool[9];
        [System.Runtime.Serialization.DataMember()]
        public int setCard { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int specialUse { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int blueMana { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int yellowMana { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int redMana { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int blackMana { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string setPosition { get; set; } = "";
        public ClientToServerB(BattleStatus battleStatus)
        {
            playerId = battleStatus.playerId;
            for (int i = 0; i < 9; i++)
            {
                revivalNumber[i] = battleStatus.unitStatus[i].revival;
            }
            setCard = battleStatus.setCard;
            blueMana = battleStatus.colorUp[0];
            yellowMana = battleStatus.colorUp[1];
            redMana = battleStatus.colorUp[2];
            blackMana = battleStatus.colorUp[3];
            setPosition = battleStatus.setPosition;
        }
    }
    //セットの返し
    [System.Runtime.Serialization.DataContract]
    public class ServerToClient2 
    {
        [System.Runtime.Serialization.DataMember()]
        public string severCheack { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int enemyBlueMana { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int enemyYellowMana { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int enemyRedMana { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int enemyBlackMana { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string enemyCardId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public bool battingFlg { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string enemySetPosition { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public bool[] enemyRevivalNumber { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string nextAction { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Target[] targetList { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public UpdateInfo[] updateInfo { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string specialId { get; set; }
    }

    //選択結果の送信
    [System.Runtime.Serialization.DataContract]
    public class ClientToServerC
    {
        [System.Runtime.Serialization.DataMember()]
        public string phase { get; set; } = "open";
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Target[] target { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? fieldNumber { get; set; }
    }
    //オープンの結果
    [System.Runtime.Serialization.DataContract]
    public class ServerToClient3
    {
        [System.Runtime.Serialization.DataMember()]
        public string severCheack { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string nextAction { get; set; }

        [System.Runtime.Serialization.DataMember()]
        public Target[] targetList { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public UpdateInfo[] updateInfo { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string specialId { get; set; }
    }

    //リムーブ開始
    [System.Runtime.Serialization.DataContract]
    public class ClientToServerD
    {
        [System.Runtime.Serialization.DataMember()]
        public string phase { get; set; } = "remove";
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        public ClientToServerD(string playerId)
        {
            this.playerId = playerId;
        }
    }
    //リムーブ開始の返し
    [System.Runtime.Serialization.DataContract]
    public class ServerToClient4
    {
        [System.Runtime.Serialization.DataMember()]
        public string severCheack { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string shieldId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? fieldNumber { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public UpdateInfo[] updateInfo { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Target[] targetList { get; set; }
    }
    //スタート開始
    [System.Runtime.Serialization.DataContract]
    public class ClientToServerE
    {
        [System.Runtime.Serialization.DataMember()]
        public string phase { get; set; } = "start";
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        public ClientToServerE(string playerId)
        {
            this.playerId = playerId;
        }
    }

    //スタート開始の返し
    [System.Runtime.Serialization.DataContract]
    public class ServerToClient5
    {
        [System.Runtime.Serialization.DataMember()]
        public string severCheack { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string nextPlayerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? nextFieldNumber { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public UpdateInfo[] updateInfo { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Target[] targetList { get; set; }
    }
    //バトル開始
    [System.Runtime.Serialization.DataContract]
    public class ClientToServerF
    {
        [System.Runtime.Serialization.DataMember()]
        public string phase { get; set; } = "battle";
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        public ClientToServerF(string playerId)
        {
            this.playerId = playerId;
        }
    }
    //バトル開始の返し
    [System.Runtime.Serialization.DataContract]
    public class ServerToClient6
    {
        [System.Runtime.Serialization.DataMember()]
        public string severCheack { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string nextPlayerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? nextFieldNumber { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public UpdateInfo[] autoUpdateInfo { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Target[] autoTargetList { get; set; }
    }
    //バトル行動選択
    [System.Runtime.Serialization.DataContract]
    public class ClientToServerF1
    {
        [System.Runtime.Serialization.DataMember()]
        public string phase { get; set; } = "battleAction";
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string battleAction { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? fieldNumber { get; set; }
        public ClientToServerF1(string playerId)
        {
            this.playerId = playerId;
        }
    }
    //バトル選択候補
    [System.Runtime.Serialization.DataContract]
    public class ServerToClient6A
    {
        [System.Runtime.Serialization.DataMember()]
        public string severCheack { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Target[] targetList { get; set; }
    }
    //バトル対象選択
    [System.Runtime.Serialization.DataContract]
    public class ClientToServerF2
    {
        [System.Runtime.Serialization.DataMember()]
        public string phase { get; set; } = "actionSelect";
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Target[] targetList { get; set; }
        public ClientToServerF2(string playerId)
        {
            this.playerId = playerId;
        }
    }
    //行動の結果
    [System.Runtime.Serialization.DataContract]
    public class ServerToClient6B
    {
        [System.Runtime.Serialization.DataMember()]
        public string severCheack { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string battleAction { get; set; }
        /*
        [System.Runtime.Serialization.DataMember()]
        public string nextPlayerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? nextFieldNumber { get; set; }
        */
        [System.Runtime.Serialization.DataMember()]
        public UpdateInfo[] updateInfo { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string shieldId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Target[] targetList{ get; set; }
        [System.Runtime.Serialization.DataMember()]
        public UpdateInfo[] autoUpdateInfo { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Target[] autoTargetList { get; set; }
    }
    [System.Runtime.Serialization.DataContract]
    public class ClientToServerG
    {
        [System.Runtime.Serialization.DataMember()]
        public string phase { get; set; } = "close";
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        public ClientToServerG(string playerId)
        {
            this.playerId = playerId;
        }
    }
    [System.Runtime.Serialization.DataContract]
    public class ServerToClient7
    {
        [System.Runtime.Serialization.DataMember()]
        public string severCheack { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? fieldNumber { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public UpdateInfo[] updateInfo { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Target[] targetList { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public UpdateInfo[] autoUpdateInfo { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Target[] autoTargetList { get; set; }
    }


    [System.Runtime.Serialization.DataContract]
    public class Target
    {
        [System.Runtime.Serialization.DataMember()]
        public int selectCount { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public TargetList[] targetList { get; set; }
    }
    [System.Runtime.Serialization.DataContract]
    public class TargetList
    {
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int[] list { get; set; }
    }
    [System.Runtime.Serialization.DataContract]
    public class UpdateInfo
    {
        [System.Runtime.Serialization.DataMember()]
        public Sp[] sp { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Field[] field { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Mana[] mana { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public DeckCemetery[] deckCemetery { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public DeckLock[] deckLock { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Remove[] removeCemetery { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Repair[] repair { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public Special[] special { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public CountAdd[] countAdd { get; set; }
    }

    [System.Runtime.Serialization.DataContract]
    public class Sp
    {
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? SP { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? nextSP { get; set; }
    }
    [System.Runtime.Serialization.DataContract]
    public class Field
    {
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int fieldNumber { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? hp { get; set; }
        [System.Runtime.Serialization.DataMember()]
        //up:永続変化 tup:ターン中変化
        public int? upLV { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? tupLV { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? upHP { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? tupHP { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? upFRM { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? tupFRM { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? upATK { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? tupATK { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? upDEF { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? tupDEF { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? upAGI { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? tupAGI { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? upRNG { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? tupRNG { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? move { get; set; }//移動後のエリア番号
        [System.Runtime.Serialization.DataMember()]
        public string remove { get; set; }//"cemetery"墓地送り"deck"返却"vanish"消滅"actionEnd"アクション終了"action"アクション復活
    }
    [System.Runtime.Serialization.DataContract]
    public class Mana
    {
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int fieldNumber { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string yellow { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string blue { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string red { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string black { get; set; }
    }
    [System.Runtime.Serialization.DataContract]
    public class DeckCemetery
    {
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int deckNumber { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string cardId { get; set; }

    }
    [System.Runtime.Serialization.DataContract]
    public class DeckLock
    {
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int deckNumber { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int turn { get; set; }
    }

    [System.Runtime.Serialization.DataContract]
    public class Remove
    {
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int cemeteryNumber { get; set; }
    }
    [System.Runtime.Serialization.DataContract]
    public class Repair
    {
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int cemeteryNumber { get; set; }
    }
    [System.Runtime.Serialization.DataContract]
    public class Special
    {
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? gage { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int? stock { get; set; }
    }
    public class CountAdd
    {
        [System.Runtime.Serialization.DataMember()]
        public string playerId { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int magic { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int divine { get; set; }
    }
    [System.Runtime.Serialization.DataContract]
    public class Effect
    {
        [System.Runtime.Serialization.DataMember()]
        public string cutin { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string player { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public string enemy { get; set; }
    }
    [System.Runtime.Serialization.DataContract]
    public class RankUp
    {
        [System.Runtime.Serialization.DataMember()]
        public string color { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int num { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int level { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int stock { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int hp { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int atk { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int dfe { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int agi { get; set; }
        [System.Runtime.Serialization.DataMember()]
        public int rng { get; set; }
    }

}