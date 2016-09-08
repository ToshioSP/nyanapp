using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Assets.Script;
public class HistoryViewBehaviourDetail : MonoBehaviour
{

    // Use this for initialization
    [SerializeField]
    RectTransform prefab = null;

    //    int i = 0;
    GameObject objPopup;
    void Start()
    {
        string dbfileName = "nyanappdb.db";
        string filePath = Application.persistentDataPath + "/" + dbfileName;
        try
        {
            SqliteDatabase sqlDB = new SqliteDatabase(filePath);
            string strOrder = " order by action_id asc,action_time desc";
            //                    string query = "insert into cathistory (action_date,action_id) values ( datetime('now', 'localtime') , ";
            //                    query = query + argActionId.ToString() + ")";
            string query = "select * from cathistory ";
            query = query + "where action_date =  ";
            query = query + "\"" + CareHistory.strData + "\" ";
            query = query + strOrder;
            //            string query = "delete from cathistory ";
            print(query);

            DataTable dataTable = sqlDB.ExecuteQuery(query);
            foreach (DataRow dr in dataTable.Rows)
            {
                var item = GameObject.Instantiate(prefab) as RectTransform;
                item.SetParent(transform, false);

                var text = item.GetComponentInChildren<Text>();

                GameObject objItem = (GameObject)item.gameObject;
                GameObject objButton = objItem.transform.FindChild("Button").gameObject;
                Button button = objButton.GetComponent<Button>();
                string seqno = dr["seqno"].ToString();
                button.onClick.AddListener(() => checkExec(seqno));
                
                string strText = (string)dr["action_time"] + " " ;

                switch ((int)dr["action_id"])
                {
                    case 1:
                        strText = strText + "尿";
                        break;
                    case 2:
                        strText = strText + "糞";
                        break;
                    case 3:
                        strText = strText + "水" + dr["amount"].ToString() + "ml";
                        break;
                    case 4:
                        strText = strText + "ブラッシング";
                        break;
                    case 5:
                        strText = strText + "シャンプー";
                        break;

                    case 6:
                        strText = strText + "病院";
                        break;
                    case 7:
                        strText =  strText + "メモ：" + (string)dr["memo"];
                        break;
                    default:
                        break;
                }
                text.text = strText;
            }
        }
        catch (Exception e)
        {
            GameObject.Find("errorText").GetComponent<Text>().text = e.Message.ToString();
            print(e.Message.ToString());
        }
    }

    public static void DestroyImmediateChildObject(Transform parent_trans)
    {
        for (int i = parent_trans.childCount - 1; i >= 0; --i)
        {
            GameObject.DestroyImmediate(parent_trans.GetChild(i).gameObject);
        }
    }

    public void checkExec(string argActId)
    {
        Transform transPop;
        objPopup = Resources.Load("Prefab/Popup") as GameObject;
        objPopup = (GameObject)GameObject.Instantiate(objPopup, new Vector3(0, 0, 0), new Quaternion());
        objPopup.name = "PopUp";
        transPop = objPopup.transform;
        transPop.SetParent(GameObject.Find("PopupArea").transform);
        objPopup.transform.Find("PopInfoText").GetComponent<Text>().text = "削除する？";

        transPop.localScale = new Vector3(1, 1, 1);
        transPop.localPosition = new Vector3(0, 0, 0);

        GameObject okbutton = objPopup.transform.Find("Pop/btnOK").gameObject;
        Button btnOk = okbutton.GetComponent<Button>();
        btnOk.onClick.AddListener(() => ExecOk(argActId));

        GameObject ngbutton = objPopup.transform.Find("Pop/btnNG").gameObject;
        Button btnNg = ngbutton.GetComponent<Button>();
        btnNg.onClick.AddListener(() => ExecNg());

    }

    public void ExecOk(string argActId)
    {
        DeleteDetail(argActId);
        Destroy(objPopup);
    }

    public void ExecNg()
    {
        Destroy(objPopup);
    }


    public void DeleteDetail(string argSeq)
    {

        DestroyImmediateChildObject(gameObject.transform);
        string dbfileName = "nyanappdb.db";
        string filePath = Application.persistentDataPath + "/" + dbfileName;
        try
        {
            SqliteDatabase sqlDB = new SqliteDatabase(filePath);
            string query = "delete from cathistory where seqno = \"" + argSeq + "\"";

            //            DataTable dataTable = sqlDB.ExecuteQuery(query);
            sqlDB.ExecuteQuery(query);
            this.Start();
        }
        catch (Exception e)
        {
            GameObject.Find("errorText").GetComponent<Text>().text = e.Message.ToString();
            print(e.Message.ToString());
        }
    }

    public void ViewDetail()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }
}
