using System;
using System.Collections;
using System.Collections.Generic;
using Gs2.Core;
using Gs2.Unity.Gs2Account.Model;
using Gs2.Unity.Gs2Account.Result;
using Gs2.Unity.Util;
using Gs2.Unity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Gs2.Unity.Gs2Version.Result;
using Gs2.Unity.Gs2JobQueue.Result;

public class LoginInitial : MonoBehaviour
{
    Profile profile;
    public string clientId;
    public string clientSecret;
    public string accountNamespaceName;
    public string accountEncryptionKeyId;
    public GameObject startButtun;
    public GameObject randomChara;
    Client gs2 = null;
    GameSession session = null;

    public Text ConnectionText;
    public Text IdText;
    public bool test = false;
    public GameObject versionWindow;
    public Text versionMessage;
    public CardMaster cardMaster;
    public ShieldMaster shieldMaster;

    void Start()
    {
        //Gs2Json();
        DontDestroyOnLoad(this);
        StartCoroutine(GameObject.Find("Loading").GetComponent<Loading>().LoadingRamdom());
        GameObject.Find("AudioBGM").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("BGMVolume", 3));
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("SEVolume", 4));
        GameObject.Find("AudioAnimeEffect").GetComponent<AudioController>().VolumeChange(PlayerPrefs.GetInt("SEVolume", 4));
        StartCoroutine(CreateAndLoginAction());
    }

    public IEnumerator CreateAndLoginAction()
    {
        // GS2 SDK のクライアントを初期化

        //Debug.Log("GS2 SDK のクライアントを初期化");

        profile = new Gs2.Unity.Util.Profile(
            clientId: clientId,
            clientSecret: clientSecret,
            reopener: new Gs2BasicReopener()
        );

        {
            AsyncResult<object> asyncResult = null;

            var current = profile.Initialize(
                r => { asyncResult = r; }
            );

            yield return current;

            // コルーチンの実行が終了した時点で、コールバックは必ず呼ばれています

            // クライアントの初期化に失敗した場合は終了
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
        }

        gs2 = new Gs2.Unity.Client(profile);

        EzAccount account = null;
        if (PlayerPrefs.HasKey("ID"))
        {
            //Debug.Log("アカウント作成済み");
            IdText.text = PlayerPrefs.GetString("ID");
        }
        else
        {

            // アカウントを新規作成
            //Debug.Log("アカウントを新規作成");

            {
                AsyncResult<EzCreateResult> asyncResult = null;

                var current = gs2.Account.Create(
                    r => { asyncResult = r; },
                    accountNamespaceName
                );

                yield return current;

                // コルーチンの実行が終了した時点で、コールバックは必ず呼ばれています

                // アカウントが作成できなかった場合は終了
                if (asyncResult.Error != null)
                {
                    OnError(asyncResult.Error);
                    yield break;
                }

                // 作成したアカウント情報を取得
                account = asyncResult.Result.Item;
                PlayerPrefs.SetString("ID", account.UserId);
                PlayerPrefs.SetString("PASS", account.Password);
                PlayerPrefs.SetInt("SEVolume", 4);
                PlayerPrefs.SetInt("BGMVolume", 4);
            }

        }
        // ログイン

        //Debug.Log("ログイン");


        {
            AsyncResult<GameSession> asyncResult = null;
            var current = profile.Login(
               authenticator: new Gs2AccountAuthenticator(
                   session: profile.Gs2Session,
                   accountNamespaceName: accountNamespaceName,
                   keyId: accountEncryptionKeyId,
                   userId: PlayerPrefs.GetString("ID"),
                   password: PlayerPrefs.GetString("PASS")
               ),
               r => { asyncResult = r; }
           );

            yield return current;

            // コルーチンの実行が終了した時点で、コールバックは必ず呼ばれています

            // ゲームセッションオブジェクトが作成できなかった場合は終了
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                ConnectionText.text = "ログインエラー";
                yield break;
            }

            // ログイン状態を表すゲームセッションオブジェクトを取得
            session = asyncResult.Result;
        }
        //job
        StartCoroutine(JobQueue());

        //バージョンチェック
        {
            AsyncResult<EzGetVersionModelResult> asyncResult = null;

            var current = gs2.Version.GetVersionModel(
                r => { asyncResult = r; },
                "version",
                "1.0.0"
            );
            yield return current;


            if (asyncResult.Error != null)
            {
                //アプリの更新が必要
                versionWindow.SetActive(true);
                versionMessage.text = "アプリが更新されています";
                OnError(asyncResult.Error);
                yield break;
            }
            if (asyncResult.Result.Item.Metadata != "OK")
            {
                versionWindow.SetActive(true);
                versionMessage.text = asyncResult.Result.Item.Metadata;
                //メンテナンス中
                yield break;
            }
        }

        StartCoroutine(Session30());
        ConnectionText.text = "";
        startButtun.SetActive(true);
    }

    public Client GetClient()
    {
        return this.gs2;
    }
    public GameSession GetSession()
    {
        return this.session;
    }
    public void Messege(String messege)
    {
        Debug.Log(messege);
    }


    private void OnError(Exception e)
    {
        Debug.Log(e.ToString());
    }
    public IEnumerator JobQueue()
    {
        while (true) {
            AsyncResult<EzRunResult> asyncResult = null;

            var current = gs2.JobQueue.Run(
                        r => asyncResult = r,
                        session,
                        "queue"
                    );
            yield return current;
            if (asyncResult.Error != null)
            {
                OnError(asyncResult.Error);
                yield break;
            }
            if (asyncResult.Result.IsLastJob) break;
        }
    }

    private void Gs2Json()
    {
        /*
        for (int i = 1; i <= 50; i++)
        {
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "    {" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      \"name\": \"b" + i + "\"," + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      \"resetType\": \"notReset\"" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "    }," + Environment.NewLine);
        }
        File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "" + Environment.NewLine);
        for (int i = 51; i <= 100; i++)
        {
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "    {" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      \"name\": \"b" + i + "\"," + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      \"resetType\": \"notReset\"" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "    }," + Environment.NewLine);
        }
        File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "" + Environment.NewLine);
        for (int i = 1; i <= 50; i++)
        {
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "    {" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      \"name\": \"m" + i + "\"," + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      \"resetType\": \"notReset\"" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "    }," + Environment.NewLine);
        }
        File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "" + Environment.NewLine);
        for (int i = 51; i <= 100; i++)
        {
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "    {" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      \"name\": \"m" + i + "\"," + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      \"resetType\": \"notReset\"" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "    }," + Environment.NewLine);
        }
        File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "" + Environment.NewLine); for (int i = 1; i <= 50; i++)
        {
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "    {" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      \"name\": \"o" + i + "\"," + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      \"resetType\": \"notReset\"" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "    }," + Environment.NewLine);
        }
        File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "" + Environment.NewLine);
        for (int i = 51; i <= 100; i++)
        {
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "    {" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      \"name\": \"o" + i + "\"," + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      \"resetType\": \"notReset\"" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "    }," + Environment.NewLine);
        }
        */

        /*
        for (int i = 1; i < shieldMaster.ShieldList.Count; i++)
        {
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "    {" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      \"name\": \"" + shieldMaster.ShieldList[i].itemId + "\"," + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      \"consumeActions\": [" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "        {" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "          \"action\": \"Gs2Money:WithdrawByUserId\"," + Environment.NewLine);
            switch (shieldMaster.ShieldList[i].rare)
            {
                case "LE":
                        File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "          \"request\": \"{\\\"namespaceName\\\":\\\"money\\\",\\\"userId\\\":\\\"#{userId}\\\",\\\"slot\\\":\\\"0\\\",\\\"count\\\":\\\"400\\\"}\"" + Environment.NewLine);
                        break;
                case "SR":
                    File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "          \"request\": \"{\\\"namespaceName\\\":\\\"money\\\",\\\"userId\\\":\\\"#{userId}\\\",\\\"slot\\\":\\\"0\\\",\\\"count\\\":\\\"300\\\"}\"" + Environment.NewLine);
                    break;
                case "R":
                    File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "          \"request\": \"{\\\"namespaceName\\\":\\\"money\\\",\\\"userId\\\":\\\"#{userId}\\\",\\\"slot\\\":\\\"0\\\",\\\"count\\\":\\\"200\\\"}\"" + Environment.NewLine);
                    break;
                case "N":
                    File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "          \"request\": \"{\\\"namespaceName\\\":\\\"money\\\",\\\"userId\\\":\\\"#{userId}\\\",\\\"slot\\\":\\\"0\\\",\\\"count\\\":\\\"100\\\"}\"" + Environment.NewLine);
                    break;
                default:
                        break;
            }

            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "        }" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      ]," + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      \"acquireActions\": [" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "        {" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "          \"action\": \"Gs2Inventory:AcquireItemSetByUserId\"," + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "          \"request\": \"{\\\"namespaceName\\\":\\\""+shieldMaster.ShieldList[i].inventory+ "\\\",\\\"inventoryName\\\":\\\"" + shieldMaster.ShieldList[i].inventory + "\\\",\\\"itemName\\\":\\\"" + shieldMaster.ShieldList[i].itemId + "\\\",\\\"userId\\\":\\\"#{userId}\\\",\\\"acquireCount\\\":\\\"1\\\"}\"" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "        }" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "      ]" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "    }," + Environment.NewLine);
        }
        */


        /*
        for (int i = 1; i < 51; i++)
        {
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "        {" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "          \"type\":\"action\"," + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "          \"acquireActions\":[" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "            {" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "              \"action\":\"Gs2Inventory:AcquireItemSetByUserId\"," + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "              \"request\":\"{\\\"namespaceName\\\":\\\"dan1\\\",\\\"inventoryName\\\":\\\"dan1\\\",\\\"itemName\\\":\\\"m"+i+"\\\",\\\"acquireCount\\\":\\\"1\\\",\\\"userId\\\":\\\"#{userId}\\\"}\"" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "            }" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "          ]," + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "          \"weight\": 1" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt", "        }," + Environment.NewLine);
        }
        */
        /*
        for (int i = 1; i < 51; i++)
        {
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt","        {" + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt","          \"name\": \"BGM" + i + "\"," + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt","          \"stackingLimit\": 1," + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt","          \"allowMultipleStacks\": false," + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt","          \"sortValue\": " + i + Environment.NewLine);
            File.AppendAllText(@"C:\Users\81803\Desktop\gs2.txt","        }," + Environment.NewLine);
        }
        */

    }

    public void TestButtonDown()
    {
        GameObject.Find("AudioTapEffect").GetComponent<AudioController>().TapEffect(0);
        StartCoroutine(TestPlayer());
    }
    private IEnumerator TestPlayer()
    {
        test = true;
        AsyncResult<GameSession> asyncResult = null;
        var current = profile.Login(
           authenticator: new Gs2AccountAuthenticator(
               session: profile.Gs2Session,
               accountNamespaceName: accountNamespaceName,
               keyId: accountEncryptionKeyId,
               userId: "cfd30423-79c0-4e09-bdc3-b5cc0b4fd6cc",
               password: "Gh6tiT3o9RRU13HLWQSc8kJLoVA0iozP"
           ),
           r => { asyncResult = r; }
       );

        yield return current;

        // コルーチンの実行が終了した時点で、コールバックは必ず呼ばれています

        // ゲームセッションオブジェクトが作成できなかった場合は終了
        if (asyncResult.Error != null)
        {
            OnError(asyncResult.Error);
            ConnectionText.text = "ログインエラー";
            yield break;
        }

        // ログイン状態を表すゲームセッションオブジェクトを取得
        session = asyncResult.Result;
        PlayerPrefs.SetString("menu", "home");
        SceneManager.LoadScene("MainMenu");
    }
    private IEnumerator Session30()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1800);
            Debug.Log("session");
            profile = new Gs2.Unity.Util.Profile(
                    clientId: clientId,
                    clientSecret: clientSecret,
                    reopener: new Gs2BasicReopener()
                );

            {
                AsyncResult<object> asyncResult = null;

                var current = profile.Initialize(
                    r => { asyncResult = r; }
                );

                yield return current;

                // コルーチンの実行が終了した時点で、コールバックは必ず呼ばれています

                // クライアントの初期化に失敗した場合は終了
                if (asyncResult.Error != null)
                {
                    OnError(asyncResult.Error);
                    yield break;
                }
            }
            gs2 = new Gs2.Unity.Client(profile);
            {
                AsyncResult<GameSession> asyncResult = null;
                var current = profile.Login(
                   authenticator: new Gs2AccountAuthenticator(
                       session: profile.Gs2Session,
                       accountNamespaceName: accountNamespaceName,
                       keyId: accountEncryptionKeyId,
                       userId: PlayerPrefs.GetString("ID"),
                       password: PlayerPrefs.GetString("PASS")
                   ),
                   r => { asyncResult = r; }
               );

                yield return current;

                // コルーチンの実行が終了した時点で、コールバックは必ず呼ばれています

                // ゲームセッションオブジェクトが作成できなかった場合は終了
                if (asyncResult.Error != null)
                {
                    OnError(asyncResult.Error);
                    ConnectionText.text = "ログインエラー";
                    yield break;
                }

                // ログイン状態を表すゲームセッションオブジェクトを取得
                session = asyncResult.Result;
            }
        }
    }
}
