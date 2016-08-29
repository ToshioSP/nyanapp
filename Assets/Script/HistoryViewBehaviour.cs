using UnityEngine;
using System.Collections;
using System;
using Assets.Script;
using UnityEngine.UI;

public class HistoryViewBehaviour : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    RectTransform prefab = null;


    void Start()
    {
        string dbfileName = "nyanappdb.db";
        string filePath = Application.persistentDataPath + "/" + dbfileName;
        try
        {
            SqliteDatabase sqlDB = new SqliteDatabase(filePath);
            //                    string query = "insert into cathistory (action_date,action_id) values ( datetime('now', 'localtime') , ";
            //                    query = query + argActionId.ToString() + ")";
            string query = "select main.action_date,coalesce(b.pisscount,0) as pisscount,coalesce(c.shitcount,0) as shitcount,coalesce(d.amount,0) as amount,coalesce(e.brash,0) as brash,coalesce(f.shampoo,0) as shampoo,coalesce(g.hosp,0) as hosp,coalesce(g.hosp,0) as hosp,coalesce(h.memo,0) as memo from ( select action_date from cathistory group by action_date ) main left outer join ( select action_date,count(*) as pisscount from cathistory where action_id = 1 group by action_date ) b on main.action_date = b.action_date left outer join ( select action_date,count(*) as shitcount from cathistory where action_id = 2 group by action_date ) c on main.action_date = c.action_date left outer join ( select action_date,sum(amount) as amount from cathistory where action_id = 3 group by action_date ) d on main.action_date = d.action_date  left outer join ( select action_date,count(*) as brash from cathistory where action_id = 4 group by action_date ) e on main.action_date = e.action_date left outer join ( select action_date,count(*) as shampoo from cathistory where action_id = 5 group by action_date ) f on main.action_date = f.action_date left outer join ( select action_date,count(*) as hosp from cathistory where action_id = 6 group by action_date ) g on main.action_date = g.action_date left outer join ( select action_date,count(*) as memo from cathistory where action_id = 7 group by action_date ) h on main.action_date = h.action_date  order by main.action_date desc";
            //            string query = "delete from cathistory ";
            DataTable dataTable = sqlDB.ExecuteQuery(query);
            foreach (DataRow dr in dataTable.Rows)
            {

                var item = GameObject.Instantiate(prefab) as RectTransform;
                item.SetParent(transform, false);

                var text = item.GetComponentInChildren<Text>();

                GameObject objItem = (GameObject)item.gameObject;
                GameObject objButton = objItem.transform.GetChild(0).gameObject;
                Button button = objButton.GetComponent<Button>();
                //                button.onClick.AddListener(ViewDetail);
                //                button.onClick.AddListener(() => ViewDetail((string)dr["action_date"]));

                string resDate = dr["action_date"].ToString();
                string resPiss = dr["pisscount"].ToString();
                string resShit = dr["shitcount"].ToString();
                string resAmount = dr["amount"].ToString();

                SetListener(button,resDate );
                                string strText = resDate + "　尿:" + resPiss + "　";
                                strText = strText + "糞:" + resShit + "　";
                                strText = strText + "水:" + resAmount + "ml";
                if (dr["hosp"].ToString() != "0")
                {
                    strText = strText + "病";
                }
                if (dr["memo"].ToString() != "0")
                {
                    strText = strText + "メ";

                }
                if (dr["shampoo"].ToString() != "0")
                {
                    strText = strText + "シ";

                }

                if (dr["brash"].ToString() != "0")
                {
                    strText = strText + "ブ";

                }

                text.text = strText;

                //                Debug.Log((string)dr["action_date"]);
                //                i++;
                
            }
        }
        catch (Exception e)
        {
            GameObject.Find("TextHistory").GetComponent<Text>().text = e.Message.ToString() + e.StackTrace.ToString();

        }
    }

    private void SetListener(Button btnButton,string argDate)
    {
        btnButton.onClick.AddListener(() => ViewDetail(argDate));
    }

    public void ViewDetail(string argDate)
    {
        CareHistory.strData = argDate;
        UnityEngine.Object detailPrefab = Resources.Load("Prefab/ScrollViewDetail");        // プレハブ取得
        GameObject detailContent = (detailPrefab != null) ? GameObject.Instantiate(detailPrefab) as GameObject : null;     // Instantiate
        Transform detailTrans = detailContent.transform;
        detailTrans.SetParent(GameObject.Find("AreaScrollView").transform);
        detailTrans.localScale = new Vector3(1, 1, 1);
        detailTrans.localPosition = new Vector3(0, 0, 0);
        CareHistory.strMoveSceneName = "CareHistory";
    }

    // Update is called once per frame
    void Update () {
	
	}
}
