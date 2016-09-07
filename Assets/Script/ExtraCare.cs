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
    public class ExtraCare:MonoBehaviour
    {
        void Start()
        {

//            GameObject.Find("memo").GetComponent<Button>()

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
                        query = "insert into cathistory (action_date,action_time,action_id,amount) values ( date('now', 'localtime') ,time('now', 'localtime'), ";
                        query = query + argActionId.ToString() + "," + amount + ")";

                        amount = int.Parse(textmemo.text);
                        textmemo.text = "0";
                        break;
                    case 7:
                        query = "insert into cathistory (action_date,action_time,action_id,memo) values ( date('now', 'localtime') ,time('now', 'localtime'), ";
                        query = query + argActionId.ToString() + ",\"" + memo + "\")";

                        break;
                    default:
                        query = "insert into cathistory (action_date,action_time,action_id) values ( date('now', 'localtime') ,time('now', 'localtime'), ";
                        query = query + argActionId.ToString() + ")";

                        break;
                }
                print(query);
                DataTable dataTable = sqlDB.ExecuteQuery(query);
            }
            catch (Exception e)
            {
                print(e.Message.ToString());
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
                case 3:
                    textPop.text = "水" + GameObject.Find("InputField").transform.FindChild("Text").GetComponent<Text>() + "ml\r\n登録する？";
                    break;
                case 4:
                    textPop.text = "ブラッシング\r\n登録する？";
                    break;
                case 5:
                    textPop.text = "シャンプー\r\n登録する？";
                    break;
                case 6:
                    textPop.text = "病院\r\n登録する？";
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

                default:

                    break;

            }
        }
    }
}
