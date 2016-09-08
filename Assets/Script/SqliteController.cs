using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class SqliteController : MonoBehaviour {

    SqliteDatabase sqlDB;
    DataTable dataTable;
    // Use this for initialization
    void Start () {
        string dbfileName = "nyanappdb.db";
        string filePath = Application.persistentDataPath + "/" + dbfileName;
        sqlDB = new SqliteDatabase(filePath);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddCat(string catName = null, string sex = null, string birthday = null)
    {

        catName = catName ?? "1";
        sex = sex ?? "3";

        string dbfileName = "nyanappdb.db";
        string filePath = Application.persistentDataPath + "/" + dbfileName;
        try
        {
            SqliteDatabase sqlDB = new SqliteDatabase(filePath);
            string query = "insert into catprofile (catname,birthday,sex) values(\" ";
            query = query + catName + "\",\"" + birthday + "\",\"" + sex + "\")";
            print(query);
//            DataTable 
                dataTable = sqlDB.ExecuteQuery(query);
        }
        catch (Exception e)
        {
            GameObject.Find("TextHistory").GetComponent<Text>().text = e.Message.ToString() + e.StackTrace.ToString();
        }
    }


    public void InsertDb(int argActionId)
    {
        string dbfileName = "nyanappdb.db";
        string filePath = Application.persistentDataPath + "/" + dbfileName;
        sqlDB = new SqliteDatabase(filePath);

        try
        {
            string query = "insert into cathistory (catid,action_date,action_time,action_id) values ('" + PlayerPrefs.GetString("SelectCat") + "', date('now', 'localtime') ,time('now', 'localtime'), ";
            query = query + argActionId.ToString() + ")";
            dataTable = sqlDB.ExecuteQuery(query);
        }
        catch (Exception e)
        {
            print(e.Message.ToString());
            print(e.StackTrace);
//            string baseFilePath = Application.streamingAssetsPath + "/" + dbfileName;
            GameObject.Find("ErrorText").GetComponent<Text>().text = e.Message.ToString() + "\r\n" + e.StackTrace.ToString() ;
        }
    }

    public void InsertDb(int argActionId,string memo)
    {
        try
        {
            SqliteDatabase sqlDB = new SqliteDatabase("nyanappdb.db");
            //                    string query = "insert into cathistory (action_date,action_id) values ( datetime('now', 'localtime') , ";
            //                    query = query + argActionId.ToString() + ")";
            string query = "select * from cathistory ";
            //            string query = "delete from cathistory ";
            //DataTable 
                dataTable = sqlDB.ExecuteQuery(query);
            foreach (DataRow dr in dataTable.Rows)
            {

                Debug.Log((string)dr["action_date"]);
            }
        }
        catch (Exception e)
        {
            print(e.Message.ToString());
        }
    }
}
