using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace MasterLoader
{
    public enum MasterType
    {
        Card,
        Shield,
        Special,
    }
    /// <summary>
    /// スプレッドシートからマスタを取得して自動生成したScriptableObjectに流し込むクラス
    /// </summary>
    public class MasterLoader : Editor
    {
        /// <summary>
        /// マスタのURLは不変なのでconstにして編集できないようにしておく
        /// URLはスプレッドシートのコードを公開したときに表示されるものを入れる。
        /// </summary>
        private const string gas =
            "https://script.google.com/macros/s/AKfycbxldDU2oOQWKKX1vI4jvxorwy5CdXb_o3DpuUPykOn0VdBzSlcJ/exec";
        /// <summary>
        /// doGet時の独自変数
        /// 読み込むシートの判断用
        /// </summary>
        private const string sheetName = "?sheetName=";
        /// <summary>
        /// マスタを配置するパス。ResoucesディレクトリとMasterディレクトリをあらかじめ作成しておく
        /// ディレクトリのパスはほんの一例。
        /// </summary>
        const string path = "Assets/Scripts/CardMaster/";

        /// <summary>
        /// スプレッドシートからマスタを取得する
        /// </summary>
        /// <param name="masterName">取得するマスタ名</param>
        /// <returns>エラー時の警告またはロードしたマスタ名</returns>
        public static async Cysharp.Threading.Tasks.UniTask<string> LoadMaster(string masterName)
        {
            // シート名を追加パラメータとしてAPIを叩くURLを決定。
            // GASでは "exec"のあとに "?" をつけて "hoge=fuga" などと追記することでGETにパラメータを付与できる
            var url = $"{gas}{sheetName}{masterName}";
            Debug.Log(url);
            var result = await GetMasterAsync(url);
            var assetPath = $"{path}{masterName}.asset";
            try
            {
                Debug.Log(result);
                switch (masterName)
                {
                    case "CardMaster":
                        var cardList = JsonHelper.ListFromJson<Card>(result);
                        if (cardList != null)
                        {
                            // すでにマスタが作成されているかを確認するために取得してみる
                            var master = AssetDatabase.LoadAssetAtPath<CardMaster>(assetPath);
# if UNITY_EDITOR
                            if (master == null)
                            {
                                master = CreateInstance<CardMaster>();
                                AssetDatabase.CreateAsset(master, assetPath);
                                EditorUtility.SetDirty(master);
                            }
                            master.CardList = cardList;
                            EditorUtility.SetDirty(master);
                            AssetDatabase.SaveAssets();
#endif
                        }
                        break;
                    case "ShieldMaster":
                        var shieldList = JsonHelper.ListFromJson<Shield>(result);
                        if (shieldList != null)
                        {
                            // すでにマスタが作成されているかを確認するために取得してみる
                            var master = AssetDatabase.LoadAssetAtPath<ShieldMaster>(assetPath);
# if UNITY_EDITOR
                            if (master == null)
                            {
                                master = CreateInstance<ShieldMaster>();
                                AssetDatabase.CreateAsset(master, assetPath);
                                EditorUtility.SetDirty(master);
                            }
                            master.ShieldList = shieldList;
                            EditorUtility.SetDirty(master);
                            AssetDatabase.SaveAssets();
#endif
                        }
                        break;
                    case "SpecialMaster":
                        var specialList = JsonHelper.ListFromJson<Special>(result);
                        if (specialList != null)
                        {
                            // すでにマスタが作成されているかを確認するために取得してみる
                            var master = AssetDatabase.LoadAssetAtPath<SpecialMaster>(assetPath);
# if UNITY_EDITOR
                            if (master == null)
                            {
                                master = CreateInstance<SpecialMaster>();
                                AssetDatabase.CreateAsset(master, assetPath);
                                EditorUtility.SetDirty(master);
                            }
                            master.SpecialList = specialList;
                            EditorUtility.SetDirty(master);
                            AssetDatabase.SaveAssets();
#endif
                        }
                        break;
                }

                Debug.Log($"{masterName} Loaded");
                return masterName;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return e.Message;
            }
        }

        /// <summary>
        /// UnityWebRequest を async/await で待ち受ける
        /// </summary>
        /// <param name="url"></param>
        /// <returns>受け取った生データ</returns>
        private static async UniTask<string> GetMasterAsync(string url)
        {
            var request = UnityWebRequest.Get(url);

            EditorUtility.DisplayCancelableProgressBar("マスタ更新中...", "", 0.0f);

            await request.SendWebRequest();

            EditorUtility.ClearProgressBar();

            if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
            {
                throw new Exception(request.error);
            }

            return request.downloadHandler.text;
        }
    }
}