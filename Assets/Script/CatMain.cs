using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Script
{
    public class CatMain:SqliteController
    {
        public GameObject popup;
        public GameObject popupyes;
        public GameObject popupno;
        public GameObject objPopup;
        public Text nowtext;
        public string lastpiss;
        public string lastshit;
        public WWW www;
        public void Awake()
        {

        }
        private void Start(){
            try { 
                string dbfileName = "nyanappdb.db";
                string baseFilePath = Application.streamingAssetsPath + "/" + dbfileName;
                string filePath = Application.persistentDataPath + "/" + dbfileName;

                // 初期データ登録(未登録時のみ)
                if (!PlayerPrefs.HasKey("SelectCat")) PlayerPrefs.SetString("SelectCat", "1");
                if (!PlayerPrefs.HasKey("SelectCatName")) PlayerPrefs.SetString("SelectCatName", "猫1");


                if (!File.Exists(filePath))
                {
                    www = new WWW("file://" + baseFilePath);
                    while (!www.isDone)
                    {
                        //NOP
                    }
                    File.WriteAllBytes(filePath, www.bytes);
                }
                else
                {
                    www = new WWW("file://" + baseFilePath);
                    while (!www.isDone)
                    {
                        //NOP
                    }

                }

                SqliteDatabase sqlDB = new SqliteDatabase(filePath);
                string query = "select * from catprofile where catid = \"" + PlayerPrefs.GetString("SelectCat") + "\"";
                DataTable dataTable = sqlDB.ExecuteQuery(query);
                if (dataTable.Rows.Count == 0)
                {
                    query = "select max(catid) as catid from cathistory";
                    dataTable = sqlDB.ExecuteQuery(query);
                    if (dataTable.Rows.Count == 0)
                    {
                        AddCat();
                        query = "select max(catid) as catid from cathistory";
                        dataTable = sqlDB.ExecuteQuery(query);
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            PlayerPrefs.SetString("SelectCat", dr["catid"].ToString());
                        }
                    }
                }

                query = "select max(action_date||\" \"||action_time) as a  from cathistory where action_id = 1";
                dataTable = sqlDB.ExecuteQuery(query);
                if (dataTable.Rows.Count == 0)
                {
                }


                // データ取得処理開始

                query = "select max(action_date||\" \"||action_time) as a  from cathistory where action_id = 1";
                dataTable = sqlDB.ExecuteQuery(query);
                foreach (DataRow dr in dataTable.Rows)
                {
                    lastpiss = dr["a"].ToString();
                }

                query = "select max(action_date||\" \"||action_time) as a  from cathistory where action_id = 2";
                dataTable = sqlDB.ExecuteQuery(query);

                foreach (DataRow dr in dataTable.Rows)
                {
                    lastshit = dr["a"].ToString();
                }
                nowtext = GameObject.Find("TextNow").GetComponent<Text>();
                string strWeekday = DateTime.Now.DayOfWeek.ToString();
                GameObject.Find("Textweekday").GetComponent<Text>().text = strWeekday.Substring(0, 3);
                GameObject.Find("Textday").GetComponent<Text>().text = DateTime.Today.Month + "/" + DateTime.Today.Day;
            }
            catch (Exception e)
            {
                string dbfileName = "nyanappdb.db";
                string baseFilePath = Application.streamingAssetsPath + "/" + dbfileName;
                string filePath = Application.persistentDataPath + "/" + dbfileName;
                GameObject.Find("ErrorText").GetComponent<Text>().text = e.Message.ToString() + "\r\n" + e.StackTrace.ToString() + "\r\n" + baseFilePath + "\r\n" + filePath + "\r\n" + File.Exists(filePath).ToString() + "\r\n" + www.bytes.Length.ToString();
            }
        }

        private void Update()
        {
//            nowtext.text = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
        }

        public void checkExec(int argActId)
        {
            try {
                Transform transPop;
                objPopup = Resources.Load("Prefab/Popup") as GameObject;
                objPopup = (GameObject)GameObject.Instantiate(objPopup, new Vector3(0, 0, 0), new Quaternion());
                objPopup.name = "PopUp";
                transPop = objPopup.transform;
                transPop.SetParent(GameObject.Find("PopupArea").transform);
                transPop.localScale = new Vector3(1, 1, 1);
                transPop.localPosition = new Vector3(0, 0, 0);

                string poptext = null;
                Text textpopup = transPop.FindChild("PopInfoText").GetComponent<Text>();

                switch (argActId)
                {
                    case 1:
                        poptext = "おしっこ履歴を登録します\r\n前回\r\n" + poptext + lastpiss;
                        break;

                    case 2:
                        poptext = "うんち履歴を登録します\r\n前回\r\n" + poptext + lastshit;
                        break;
                }
                textpopup.text = poptext;

                GameObject okbutton = objPopup.transform.Find("Pop/btnOK").gameObject;
                Button btnOk = okbutton.GetComponent<Button>();
                btnOk.onClick.AddListener(() => ExecOk(argActId));

                GameObject ngbutton = objPopup.transform.Find("Pop/btnNG").gameObject;
                Button btnNg = ngbutton.GetComponent<Button>();
                btnNg.onClick.AddListener(() => ExecNg(argActId));
            }
            catch(Exception e)
            {
                string dbfileName = "nyanappdb.db";
                string baseFilePath = Application.streamingAssetsPath + "/" + dbfileName;
                string filePath = Application.persistentDataPath + "/" + dbfileName;
                GameObject.Find("ErrorText").GetComponent<Text>().text = e.Message.ToString() + "\r\n" + e.StackTrace.ToString() + "\r\n" + baseFilePath + "\r\n" + filePath + "\r\n" + File.Exists(filePath).ToString();
            }
        }

        public void ExecOk(int argActId)
        {
            try {
                InsertDb(argActId);
                MoveScene(3);
                Destroy(objPopup);
            }
            catch (Exception e)
            {
                string dbfileName = "nyanappdb.db";
                string baseFilePath = Application.streamingAssetsPath + "/" + dbfileName;
                string filePath = Application.persistentDataPath + "/" + dbfileName;
                GameObject.Find("ErrorText").GetComponent<Text>().text = e.Message.ToString() + "\r\n" + e.StackTrace.ToString() + "\r\n" + baseFilePath + "\r\n" + filePath + "\r\n" + File.Exists(filePath).ToString();
            }

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
                    SceneManager.LoadScene("ExtraCare");

                    break;

                case 2:
                    SceneManager.LoadScene("CareHistory");

                    break;
                case 3:
                    SceneManager.LoadScene("Main");

                    break;
                case 4:
                    SceneManager.LoadScene("CatProfile");

                    break;

                default:
                break;


            }
        }
    }
}
