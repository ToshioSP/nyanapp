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
        public bool bShitStatus;
        public string shitMemo;


        SqliteDatabase sqlDB;
        DataTable dataTable;

        public void Awake()
        {

        }
        private void Start(){
                string dbfileName = "nyanappdb.db";
                string baseFilePath = Application.streamingAssetsPath + "/" + dbfileName;
                string filePath = Application.persistentDataPath + "/" + dbfileName;


            // 初期データ登録(未登録時のみ)
            if (!PlayerPrefs.HasKey("SelectCat")) PlayerPrefs.SetString("SelectCat", "1");
                if (!PlayerPrefs.HasKey("SelectCatName")) PlayerPrefs.SetString("SelectCatName", "猫1");


                if (!File.Exists(filePath))
                {

                #if UNITY_EDITOR
                                www = new WWW("file://" + baseFilePath);
                #elif UNITY_ANDROID
                                www = new WWW(baseFilePath);
                #endif
                                //                    www = new WWW("file://" + baseFilePath);
                //                www = new WWW(baseFilePath);
                    print(baseFilePath);
                    while (!www.isDone)
                    {
                        //NOP
                    }
                    File.WriteAllBytes(filePath, www.bytes);
                }

            SqliteDatabase sqlDB = new SqliteDatabase(dbfileName);
                string query; 
                DataTable dataTable;

                
                // deleted存在確認,なかったら追加
                query = "SELECT count(*) as exist from sqlite_master where name = 'catprofile' and sql like '%deleted%'";
                dataTable = sqlDB.ExecuteQuery(query);
                string exist = null;
                foreach (DataRow dr in dataTable.Rows)
                {
                    exist = dr["exist"].ToString();
                }
                if(exist == "0")
                {
                query = "ALTER TABLE catprofile ADD COLUMN deleted[boolean] default 0";
                dataTable = sqlDB.ExecuteQuery(query);
                query = "update catprofile set birthday = ' '";
                dataTable = sqlDB.ExecuteQuery(query);

            }

            query = "select * from catprofile where catid = \"" + PlayerPrefs.GetString("SelectCat") + "\"";
                dataTable = sqlDB.ExecuteQuery(query);

            if (dataTable.Rows.Count == 0)
            {
                query = "select max(catid) as catid from catprofile";
                dataTable = sqlDB.ExecuteQuery(query);
                if (dataTable.Rows.Count == 0)
                {
                    AddCat();
                    query = "select max(catid) as catid from catprofile where deleted = 0";
                    dataTable = sqlDB.ExecuteQuery(query);
                    print(dataTable[0]["catid"].ToString());
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        PlayerPrefs.SetString("SelectCat", dr["catid"].ToString());
                    }
                }
            }
/*
                query = "select max(action_date||\" \"||action_time) as a  from cathistory where action_id = 1";
                dataTable = sqlDB.ExecuteQuery(query);
                if (dataTable.Rows.Count == 0)
                {
                }
                */
                // データ取得処理開始

                query = "select max(action_date||\" \"||action_time) as a  from cathistory where action_id = 1 and catid = " + PlayerPrefs.GetString("SelectCat");
                dataTable = sqlDB.ExecuteQuery(query);
                foreach (DataRow dr in dataTable.Rows)
                {
                    lastpiss = dr["a"].ToString();
                }

                query = "select max(action_date||\" \"||action_time) as a  from cathistory where action_id = 2 and catid = " + PlayerPrefs.GetString("SelectCat");
                dataTable = sqlDB.ExecuteQuery(query);

                foreach (DataRow dr in dataTable.Rows)
                {
                    lastshit = dr["a"].ToString();
                }
                nowtext = GameObject.Find("TextNow").GetComponent<Text>();
                string strWeekday = DateTime.Now.DayOfWeek.ToString();
                GameObject.Find("Textweekday").GetComponent<Text>().text = strWeekday.Substring(0, 3);
                GameObject.Find("Textday").GetComponent<Text>().text = DateTime.Today.Month + "/" + DateTime.Today.Day;

                query = "select catname as a  from catprofile where catid = " + PlayerPrefs.GetString("SelectCat");
                dataTable = sqlDB.ExecuteQuery(query);

                string catname = "";
                foreach (DataRow dr in dataTable.Rows)
                {
                    catname = dr["a"].ToString();
                }
                if (catname.Trim() != "")
                {

                    GameObject.Find("CatSelect").GetComponent<Text>().text = catname;
                }


        }

        private void Update()
        {
            nowtext.text = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        public void checkExec(int argActId)
        {
                Transform transPop;
                Text textpopup;
                string poptext = null;

                switch (argActId)
                {
                    case 1:
                    objPopup = Resources.Load("Prefab/InputPopupShit") as GameObject;
                    objPopup = (GameObject)GameObject.Instantiate(objPopup, new Vector3(0, 0, 0), new Quaternion());
                    objPopup.name = "PopUp";
                    transPop = objPopup.transform;
                    textpopup = transPop.FindChild("Pop/PopInfoText").GetComponent<Text>();
                    transPop = objPopup.transform;
                    transPop.SetParent(GameObject.Find("PopupArea").transform);
                    transPop.localScale = new Vector3(1, 1, 1);
                    transPop.localPosition = new Vector3(0, 0, 0);
                    poptext = "おしっこ履歴を登録します\r\n前回\r\n" + poptext + lastpiss;
                        break;
                    case 2:
                        objPopup = Resources.Load("Prefab/InputPopupShit") as GameObject;
                        objPopup = (GameObject)GameObject.Instantiate(objPopup, new Vector3(0, 0, 0), new Quaternion());
                        objPopup.name = "PopUp";
                        transPop = objPopup.transform;
                        textpopup = transPop.FindChild("Pop/PopInfoText").GetComponent<Text>();
                        transPop = objPopup.transform;
                        transPop.SetParent(GameObject.Find("PopupArea").transform);
                        transPop.localScale = new Vector3(1, 1, 1);
                        transPop.localPosition = new Vector3(0, 0, 0);

                        poptext = "うんち履歴を登録します\r\n前回\r\n" + poptext + lastshit;
                        break;
                default:
                    objPopup = Resources.Load("Prefab/Popup") as GameObject;
                    objPopup = (GameObject)GameObject.Instantiate(objPopup, new Vector3(0, 0, 0), new Quaternion());
                    objPopup.name = "PopUp";
                    transPop = objPopup.transform;
                    textpopup = transPop.FindChild("PopInfoText").GetComponent<Text>();

                    transPop.SetParent(GameObject.Find("PopupArea").transform);

                    transPop.localScale = new Vector3(1, 1, 1);
                    transPop.localPosition = new Vector3(0, 0, 0);

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

        public void ExecOk(int argActId)
        {
            try {

                    GameObject ob = GameObject.Find("PopUp/Pop");
                    InsertDbShit(argActId,ob.transform.Find("Toggle").GetComponent<Toggle>().isOn,ob.transform.Find("InputField").GetComponent<InputField>().text);
                    MoveScene(3);
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
                    SceneManager.LoadScene("CatList");

                    break;

                default:
                break;


            }
        }
    }
}
