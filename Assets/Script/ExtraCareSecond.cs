using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace Assets.Script
{
    public class ExtraCareSecond:MonoBehaviour
    {
        InputField inputYear;
        InputField inputDay;
        InputField inputTime;
        void Start()
        {
            GameObject.Find("OmakeButton").GetComponent<Button>().onClick.AddListener(PlayAdMovie);
            inputYear = GameObject.Find("YEAR").GetComponent<InputField>();
            inputDay = GameObject.Find("DAY").GetComponent<InputField>();
            inputTime = GameObject.Find("TIME").GetComponent<InputField>();

            string dbfileName = "nyanappdb.db";
//            string baseFilePath = Application.streamingAssetsPath + "/" + dbfileName;
            string filePath = Application.persistentDataPath + "/" + dbfileName;

            SqliteDatabase sqlDB = new SqliteDatabase(filePath);
            string query = "select date('now', 'localtime') as date ,time('now', 'localtime') as time";
            DataTable dataTable = sqlDB.ExecuteQuery(query);

            GameObject.Find("InputField").GetComponent<InputField>().text = "0";
            dataTable = sqlDB.ExecuteQuery(query);
            foreach (DataRow dr in dataTable.Rows)
            {
                inputYear.text = dr["date"].ToString().Substring(0, 4);
                inputDay.text = dr["date"].ToString().Substring(5, 2) + dr["date"].ToString().Substring(8, 2);
                inputTime.text = dr["time"].ToString().Substring(0, 2) + dr["time"].ToString().Substring(3, 2) + dr["time"].ToString().Substring(6, 2);
            }

        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MoveScene(1);
            }
        }

        //動画再生開始
        public void PlayAdMovie()
        {
            //再生できるか確認
            if (MovieAdManager.Instance.CanPlay())
            {
                //コールバックメソッドを指定して動画再生
                MovieAdManager.Instance.Play(OnFinished, OnFailed, OnFailed);
            }
        }

        //動画視聴完了時
        private void OnFinished()
        {
            ExecUranai();
            Debug.Log("完了");
        }

        //動画視聴失敗時
        private void OnFailed()
        {
            Debug.Log("失敗");
        }

        //動画視聴キャンセル時
        private void OnSkipped()
        {
            Debug.Log("キャンセル");
        }

        public void ExecUranai()
        {
            Transform transPop;

            objPopup = Resources.Load("Prefab/PopupUranai") as GameObject;
            objPopup = (GameObject)GameObject.Instantiate(objPopup, new Vector3(0, 0, 0), new Quaternion());
            objPopup.name = "PopUp";
            transPop = objPopup.transform;
            transPop.SetParent(GameObject.Find("PopupArea").transform);

            transPop.localScale = new Vector3(1, 1, 1);
            transPop.localPosition = new Vector3(0, 0, 0);
            Text textPop = GameObject.Find("PopInfoText").GetComponent<Text>();
            Text textPop2 = GameObject.Find("PopInfoText2").GetComponent<Text>();

            int kekka = UnityEngine.Random.Range(1,8);

            switch (kekka)
            {
                case 1:
                    textPop.text = "大吉！！！！";
                    textPop2.text = "今日はいいことありそう";
                    break;
                case 2:
                    textPop.text = "吉！！！";
                    textPop2.text = "実は大吉の次に良いんだよ";
                    break;
                case 3:
                    textPop.text = "中吉！！";
                    textPop2.text = "まあまあいい感じ";
                    break;
                case 4:
                    textPop.text = "小吉！";
                    textPop2.text = "ささやかな幸せ";
                    break;
                case 5:
                    textPop.text = "末吉";
                    textPop2.text = "セーフ！";
                    break;
                case 6:
                    textPop.text = "凶";
                    textPop2.text = "まあ、まあね…！";
                    break;
                case 7:
                    textPop.text = "大凶";
                    textPop2.text = "……。";
                    break;
                case 8:
                    textPop.text = "狂";
                    textPop2.text = "バグがなきゃ出ないはず";
                    break;
                default:
                    break;
            }
            GameObject ngbutton = objPopup.transform.Find("Pop/btnNG").gameObject;
            Button btnNg = ngbutton.GetComponent<Button>();
            btnNg.onClick.AddListener(() => Destroy(objPopup));
        }


        GameObject objPopup;
        public void InsertDb(int argActionId, string memo = null)
        {
            string dbfileName = "nyanappdb.db";
            string filePath = Application.persistentDataPath + "/" + dbfileName;

            try
            {

                SqliteDatabase sqlDB = new SqliteDatabase(filePath);
                
                int amount = 0;
                string query;
                switch (argActionId)
                {
                    case 1:
                        query = "insert into cathistory (catid,action_date,action_time,action_id,memo) values ('" + PlayerPrefs.GetString("SelectCat") + "', '" + inputYear.text + "-" + inputDay.text.Substring(0, 2) + "-" + inputDay.text.Substring(2, 2) + "','" + inputTime.text.Substring(0, 2) + ":" + inputTime.text.Substring(2, 2) + ":" + inputTime.text.Substring(4, 2) + "',";
                        query = query + argActionId.ToString() + ",\"" + memo + "\")";
                        break;

                    case 2:
                        query = "insert into cathistory (catid,action_date,action_time,action_id,memo) values ('" + PlayerPrefs.GetString("SelectCat") + "', '" + inputYear.text + "-" + inputDay.text.Substring(0, 2) + "-" + inputDay.text.Substring(2, 2) + "','" + inputTime.text.Substring(0, 2) + ":" + inputTime.text.Substring(2, 2) + ":" + inputTime.text.Substring(4, 2) + "',";
                        query = query + argActionId.ToString() + ",\"" + memo + "\")";
                        break;

                    case 3:
                        string textmemo = GameObject.Find("InputField").GetComponent<InputField>().text;
                        amount = int.Parse(textmemo);

                        query = "insert into cathistory (catid,action_date,action_time,action_id,amount) values ('" + PlayerPrefs.GetString("SelectCat") + "', date('now', 'localtime') ,time('now', 'localtime'), ";
                        query = query + argActionId.ToString() + "," + amount + ")";
                        Text texta = GameObject.Find("InputField").transform.FindChild("Text").GetComponent<Text>();
                        texta.text = "0";
                        break;
                    case 7:
                        query = "insert into cathistory (catid,action_date,action_time,action_id,memo) values ('" + PlayerPrefs.GetString("SelectCat") + "', '" + inputYear.text + "-" + inputDay.text.Substring(0, 2) + "-" + inputDay.text.Substring(2, 2) + "','" + inputTime.text.Substring(0, 2) + ":" + inputTime.text.Substring(2, 2) + ":" + inputTime.text.Substring(4, 2) + "',";
                        query = query + argActionId.ToString() + ",\"" + memo + "\")";

                        break;
                    default:
                        query = "insert into cathistory (catid,action_date,action_time,action_id) values ('" + PlayerPrefs.GetString("SelectCat") + "','" + inputYear.text + "-" + inputDay.text.Substring(0, 2) + "-" + inputDay.text.Substring(2, 2) + "','" + inputTime.text.Substring(0, 2) + ":" + inputTime.text.Substring(2, 2) + ":" + inputTime.text.Substring(4, 2) + "',";
                        query = query + argActionId.ToString() + ")";

                        break;
                }
                //DataTable dataTable = 
                    sqlDB.ExecuteQuery(query);
            }
            catch (Exception e)
            {
                print(e.Message.ToString());
                print(e.StackTrace.ToString());
            }
        }

        public void checkExec(int argActId)
        {
            Transform transPop;

            objPopup = Resources.Load("Prefab/Popup") as GameObject;
            objPopup = (GameObject)GameObject.Instantiate(objPopup, new Vector3(0, 0, 0), new Quaternion());
            objPopup.name = "PopUp";
            transPop = objPopup.transform;
            transPop.SetParent(GameObject.Find("PopupArea").transform);

            transPop.localScale = new Vector3(1, 1, 1);
            transPop.localPosition = new Vector3(0, 0, 0);
            Text textPop = GameObject.Find("PopInfoText").GetComponent<Text>();

            switch (argActId)
            {
                case 1:
                    textPop.text = inputYear.text + "-" + inputDay.text.Substring(0, 2) + "-" + inputDay.text.Substring(2, 2) + " " + inputTime.text.Substring(0, 2) + ":" + inputTime.text.Substring(2, 2) + ":" + inputTime.text.Substring(4, 2) + "\r\nおしっこ\r\n登録する？";
                    break;
                case 2:
                    textPop.text = inputYear.text + "-" + inputDay.text.Substring(0, 2) + "-" + inputDay.text.Substring(2, 2) + " " + inputTime.text.Substring(0, 2) + ":" + inputTime.text.Substring(2, 2) + ":" + inputTime.text.Substring(4, 2) + "\r\nうんち\r\n登録する？";
                    break;
                case 3:
                    textPop.text = inputYear.text + "-" + inputDay.text.Substring(0, 2) + "-" + inputDay.text.Substring(2, 2) + " " + inputTime.text.Substring(0, 2) + ":" + inputTime.text.Substring(2, 2) + ":" + inputTime.text.Substring(4, 2) + "\r\n飲水量" + GameObject.Find("InputField").GetComponent<InputField>().text + "ml\r\n登録する？";
                    break;
                case 4:
                    textPop.text = inputYear.text + "-" + inputDay.text.Substring(0, 2) + "-" + inputDay.text.Substring(2, 2) + " " + inputTime.text.Substring(0, 2) + ":" + inputTime.text.Substring(2, 2) + ":" + inputTime.text.Substring(4, 2) + "\r\nブラッシング\r\n登録する？";
                    break;
                case 5:
                    textPop.text = inputYear.text + "-" + inputDay.text.Substring(0, 2) + "-" + inputDay.text.Substring(2, 2) + " " + inputTime.text.Substring(0, 2) + ":" + inputTime.text.Substring(2, 2) + ":" + inputTime.text.Substring(4, 2) + "\r\nシャンプー\r\n登録する？";
                    break;
                case 6:
                    textPop.text = inputYear.text + "-" + inputDay.text.Substring(0, 2) + "-" + inputDay.text.Substring(2, 2) + " " + inputTime.text.Substring(0, 2) + ":" + inputTime.text.Substring(2, 2) + ":" + inputTime.text.Substring(4, 2) + "\r\n病院\r\n登録する？";
                    break;
                default:
                    break;
            }

            GameObject okbutton = objPopup.transform.Find("Pop/btnOK").gameObject;
            Button btnOk = okbutton.GetComponent<Button>();
            btnOk.onClick.AddListener(() => ExecOk(argActId));

            GameObject ngbutton = objPopup.transform.Find("Pop/btnNG").gameObject;
            Button btnNg = ngbutton.GetComponent<Button>();
            btnNg.onClick.AddListener(() => ExecNg(argActId));
            
        }

        public void popInput(int argActId )
        {
            
            Transform transPop;
            objPopup = Resources.Load("Prefab/InputPopup") as GameObject;
            objPopup = (GameObject)GameObject.Instantiate(objPopup, new Vector3(0, 0, 0), new Quaternion());
            objPopup.name = "PopUp";
            transPop = objPopup.transform;
            transPop.SetParent(GameObject.Find("PopupArea").transform);

            transPop.localScale = new Vector3(1, 1, 1);
            transPop.localPosition = new Vector3(0, 0, 0);

            GameObject okbutton = objPopup.transform.Find("Pop/btnOK").gameObject;
            Button btnOk = okbutton.GetComponent<Button>();
            btnOk.onClick.AddListener(() => ExecOk(argActId));

            GameObject ngbutton = objPopup.transform.Find("Pop/btnNG").gameObject;
            Button btnNg = ngbutton.GetComponent<Button>();
            btnNg.onClick.AddListener(() => ExecNg(argActId));
        }

        public void popShit(int argActId)
        {
            Transform transPop;

            objPopup = Resources.Load("Prefab/InputPopupShit") as GameObject;
            objPopup = (GameObject)GameObject.Instantiate(objPopup, new Vector3(0, 0, 0), new Quaternion());
            objPopup.name = "PopUp";
            transPop = objPopup.transform;
            transPop.SetParent(GameObject.Find("PopupArea").transform);

            transPop.localScale = new Vector3(1, 1, 1);
            transPop.localPosition = new Vector3(0, 0, 0);

            GameObject okbutton = objPopup.transform.Find("Pop/btnOK").gameObject;
            Button btnOk = okbutton.GetComponent<Button>();
            btnOk.onClick.AddListener(() => ExecOk(argActId));

            GameObject ngbutton = objPopup.transform.Find("Pop/btnNG").gameObject;
            Button btnNg = ngbutton.GetComponent<Button>();
            btnNg.onClick.AddListener(() => ExecNg(argActId));

        }

        public void checkExec(int argActId,string memo = "0")
        {
            Transform transPop;
            objPopup = Resources.Load("Prefab/Popup") as GameObject;
            objPopup = (GameObject)GameObject.Instantiate(objPopup, new Vector3(0, 0, 0), new Quaternion());
            objPopup.name = "PopUp";
            transPop = objPopup.transform;
            transPop.SetParent(GameObject.Find("PopupArea").transform);
            transPop.localScale = new Vector3(1, 1, 1);
            transPop.localPosition = new Vector3(0, 0, 0);

            GameObject okbutton = objPopup.transform.Find("Pop/btnOK").gameObject;
            Button btnOk = okbutton.GetComponent<Button>();
            btnOk.onClick.AddListener(() => ExecOk(argActId));

            GameObject ngbutton = objPopup.transform.Find("Pop/btnNG").gameObject;
            Button btnNg = ngbutton.GetComponent<Button>();
            btnNg.onClick.AddListener(() => ExecNg(argActId));
        }

        public void ExecOk(int argActId)
        {
            string memo = "";
                GameObject obj;
                switch (argActId)
                {
                case 1:
                    obj = GameObject.Find("PopUp/Pop/");
                    if (obj.transform.Find("Toggle").GetComponent<Toggle>().isOn)
                    {
                        memo = "【異】";
                    }
                    //                        InputField inputfield = 
                    memo = memo + obj.transform.Find("InputField").GetComponent<InputField>().text;
                    print(memo);
                    InsertDb(argActId, memo);

                    break;

                case 2:
                    obj = GameObject.Find("PopUp/Pop/");
                    if (obj.transform.Find("Toggle").GetComponent<Toggle>().isOn)
                    {
                        memo = "【異】";
                    }
                    //                        InputField inputfield = 
                    memo = memo + obj.transform.Find("InputField").GetComponent<InputField>().text;
                    print(memo);
                        InsertDb(argActId, memo);

                        break;
                    case 7:
                        memo = GameObject.Find("PopUp").transform.Find("Pop/InputField").GetComponent<InputField>().text;
                        InsertDb(argActId,memo);
                        break;

                    default:
                        InsertDb(argActId);
                        break;
                }
                Destroy(objPopup);
                print(argActId);
        }

        public void ExecNg(int argActId)
        {
            Destroy(objPopup);
        }


        public void MoveScene(int id)
        {
            switch (id)
            {
                case 1:
                    SceneManager.LoadScene("Main");
                    break;
                case 2:
                    SceneManager.LoadScene("ExtraCare");
                    break;

                default:

                    break;

            }
        }
    }
}
