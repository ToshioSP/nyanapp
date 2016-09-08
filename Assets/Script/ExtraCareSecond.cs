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
                    case 3:
                        Text textmemo = GameObject.Find("InputField").transform.FindChild("Text").GetComponent<Text>();
                        query = "insert into cathistory (catid,action_date,action_time,action_id,amount) values ( '" + PlayerPrefs.GetString("SelectCat") + "','" + inputYear.text + "-" + inputDay.text.Substring(0, 2) + "-" + inputDay.text.Substring(2, 2) + "','" + inputTime.text.Substring(0, 2) + ":" + inputTime.text.Substring(2, 2) + ":" + inputTime.text.Substring(4, 2) + "',";
                        
//                        query = "insert into cathistory (action_date,action_time,action_id,amount) values ( date('now', 'localtime') ,time('now', 'localtime'), ";
                        query = query + argActionId.ToString() + "," + amount + ")";

                        amount = int.Parse(textmemo.text);
                        textmemo.text = "0";
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
                    textPop.text = inputYear.text + "-" + inputDay.text.Substring(0, 2) + "-" + inputDay.text.Substring(2, 2) + " " + inputTime.text.Substring(0, 2) + ":" + inputTime.text.Substring(2, 2) + ":" + inputTime.text.Substring(4, 2) + "\r\n水" + GameObject.Find("InputField").GetComponent<InputField>().text + "mml\r\n登録する？";
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
            try {
                switch (argActId)
                {
                    case 7:
                        string memo = GameObject.Find("PopUp").transform.Find("Pop/InputField/Text").GetComponent<Text>().text;
                        InsertDb(argActId,memo);
                        break;

                    default:
                        InsertDb(argActId);
                        break;
                }
                Destroy(objPopup);
                print(argActId);
            }
            catch(Exception e)
            {
                print(e.Message.ToString());
                print(e.StackTrace.ToString());
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
