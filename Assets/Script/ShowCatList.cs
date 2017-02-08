using UnityEngine;
using System.Collections;
using System;
using Assets.Script;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ShowCatList : SqliteController
{

    // Use this for initialization
    [SerializeField]
    RectTransform prefab = null;
    GameObject objPopupSelect;
    GameObject objPopup;
    GameObject objCatprofile;
    GameObject objCanvas;
    private string catName = "あああ";
    private string ChangeCatId = null;

    void Start()
    {
        string dbfileName = "nyanappdb.db";
        string filePath = Application.persistentDataPath + "/" + dbfileName;
        objCanvas = GameObject.Find("Canvas");
        GameObject.Find("btnBack").GetComponent<Button>().onClick.AddListener(()=> SceneManager.LoadScene("Main"));
        try
        {
            SqliteDatabase sqlDB = new SqliteDatabase(filePath);
            string query = "select * from catprofile where deleted = 0 order by 1 desc";
            DataTable dataTable = sqlDB.ExecuteQuery(query);
            if(dataTable.Rows.Count == 0)
            {
                AddCat();
                query = "select * from catprofile where deleted = 0 order by 1 desc";
                dataTable = sqlDB.ExecuteQuery(query);
                PlayerPrefs.SetString("SelectCat", dataTable[0]["catid"].ToString());
            }
            else{
                Text z = GameObject.Find("TextHistory").GetComponent<Text>();
                z.text = z.text + dataTable[0]["catname"].ToString();
            }

            foreach (DataRow dr in dataTable.Rows)
            {
                var item = GameObject.Instantiate(prefab) as RectTransform;
                item.SetParent(transform, false);

                var text = item.GetComponentInChildren<Text>();

                GameObject objItem = (GameObject)item.gameObject;
                GameObject objButton = objItem.transform.GetChild(0).gameObject;
                Button button = objButton.GetComponent<Button>();
                string catId = dr["catid"].ToString();
                string memo = "";
                if (dr["catname"].ToString().Trim() != "")
                {
                    catName = dr["catname"].ToString();
                    memo = dr["birthday"].ToString();
                }
                else
                {
                    catName = "名称未登録";
                }
//                string birthday = dr["birthday"].ToString();
                SetListener(button,catId );
                string strText = catName ;
                strText = strText + " " + memo;
                  text.text = strText;
                
            }
        }
        catch (Exception e)
        {
            GameObject.Find("TextHistory").GetComponent<Text>().text = e.Message.ToString() + e.StackTrace.ToString();
        }
    }

    private void SetListener(Button btnButton,string argCatId)
    {
        btnButton.onClick.AddListener(() => SelectPopup(argCatId));
    }

    private void SelectPopup(string argCatId)
    {
        Transform transPop;
        objPopupSelect = Resources.Load("Prefab/PopupSelect") as GameObject;
        objPopupSelect = (GameObject)GameObject.Instantiate(objPopupSelect, new Vector3(0, 0, 0), new Quaternion());
        objPopupSelect.name = "PopUpSelect";
        transPop = objPopupSelect.transform;
        transPop.SetParent(GameObject.Find("PopupArea").transform);

        transPop.localScale = new Vector3(1, 1, 1);
        transPop.localPosition = new Vector3(0, 0, 0);

        GameObject updatebutton = objPopupSelect.transform.Find("Pop/btnUpdate").gameObject;
        Button btnUpdate = updatebutton.GetComponent<Button>();
        btnUpdate.onClick.AddListener(() => checkExec(argCatId,2));

        GameObject selectbutton = objPopupSelect.transform.Find("Pop/btnSelect").gameObject;
        Button btnSelect = selectbutton.GetComponent<Button>();
        btnSelect.onClick.AddListener(() => checkExec(argCatId,1));

        GameObject deletebutton = objPopupSelect.transform.Find("Pop/btnDelete").gameObject;
        Button btnDelete = deletebutton.GetComponent<Button>();
        btnDelete.onClick.AddListener(() => checkExec(argCatId,3));

        GameObject cancelbutton = objPopupSelect.transform.Find("Pop/btnBack").gameObject;
        Button btnCancel = cancelbutton.GetComponent<Button>();
        btnCancel.onClick.AddListener(() => DestroyObject(objPopupSelect));

    }

    public void checkExec(string argCatId,int argSelectId)
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
        btnOk.onClick.AddListener(() => ExecOk(argCatId,argSelectId));

        GameObject ngbutton = objPopup.transform.Find("Pop/btnNG").gameObject;
        Button btnNg = ngbutton.GetComponent<Button>();
        btnNg.onClick.AddListener(() => Destroy(objPopup));
        Text infoText = transPop.FindChild("PopInfoText").GetComponent<Text>();
        switch (argSelectId)
        {
            case 1:
                infoText.text = "お世話する対象を変更しますか？";
                break;
            case 2:
                infoText.text = "情報を更新しますか？";
                break;
            case 3:
                infoText.text = "本当に削除しますか？\r\nこの操作は元に戻せません。";
                break;
            default:
                break;
        }
    }

    void PopCatProfile()
    {
        Transform transPop;
        GameObject objProfPopup;
        objProfPopup = Resources.Load("Prefab/CatProfilePop") as GameObject;
        objProfPopup = (GameObject)GameObject.Instantiate(objProfPopup, new Vector3(0, 0, 0), new Quaternion());
        objProfPopup.name = "CatProfilePop";
        transPop = objProfPopup.transform;
        transPop.SetParent(objCanvas.transform);

        transPop.localScale = new Vector3(1, 1, 1);
        transPop.localPosition = new Vector3(0, 0, 0);
        GameObject.Find("btnProfOk").GetComponent<Button>().onClick.AddListener(PopCatProfileCheckExec);
        GameObject.Find("btnProfNg").GetComponent<Button>().onClick.AddListener(() => Destroy(objProfPopup));

    }

    void PopCatProfileCheckExec()
    {
        Transform transPop;

        objPopup = Resources.Load("Prefab/Popup") as GameObject;
        objPopup = (GameObject)GameObject.Instantiate(objPopup, new Vector3(0, 0, 0), new Quaternion());
        objPopup.name = "PopUp";
        transPop = objPopup.transform;
        transPop.SetParent(objCanvas.transform);

        transPop.localScale = new Vector3(1, 1, 1);
        transPop.localPosition = new Vector3(0, 0, 0);

        GameObject okbutton = objPopup.transform.Find("Pop/btnOK").gameObject;
        Button btnOk = okbutton.GetComponent<Button>();
        btnOk.onClick.AddListener(() => ChangeCatProfile());

        GameObject ngbutton = objPopup.transform.Find("Pop/btnNG").gameObject;
        Button btnNg = ngbutton.GetComponent<Button>();
        btnNg.onClick.AddListener(() => Destroy(objPopup));
        Text infoText = transPop.FindChild("PopInfoText").GetComponent<Text>();
    }

    void ChangeCatProfile()
    {
        string dbfileName = "nyanappdb.db";
        string filePath = Application.persistentDataPath + "/" + dbfileName;
        objCanvas = GameObject.Find("Canvas");
        try
        {
            SqliteDatabase sqlDB = new SqliteDatabase(filePath);
            string query = "update catprofile set catname = \"";
            query = query + GameObject.Find("inputnametext").GetComponent<Text>().text + "\", ";
            query = query + "birthday = \"";
            query = query + GameObject.Find("inputmemotext").GetComponent<Text>().text + "\" ";
            query = query + " where catid = \"" + ChangeCatId + "\"";

            DataTable dataTable = sqlDB.ExecuteQuery(query);
            dataTable = sqlDB.ExecuteQuery(query);

            SceneManager.LoadScene("CatList");


        }
        catch (Exception e)
        {
            GameObject.Find("TextHistory").GetComponent<Text>().text = e.Message.ToString() + e.StackTrace.ToString();

        }
    }


    public void ExecOk(string argCatId, int argSelectId)
    {
        switch (argSelectId)
        {
            case 1:
                Destroy(objPopup);
                Destroy(objPopupSelect);
                PlayerPrefs.SetString("SelectCat", argCatId);
                break;
            case 2:
                ChangeCatId = argCatId;
                Destroy(objPopup);
                Destroy(objPopupSelect);
                PopCatProfile();
                break;
            case 3:
                DeleteCat(argCatId);
                Destroy(objPopup);
                Destroy(objPopupSelect);
                break;
            default:
                break;
        }
    }
    private void DeleteCat(string argCatID)
    {
        string dbfileName = "nyanappdb.db";
        string filePath = Application.persistentDataPath + "/" + dbfileName;
        try
        {
            SqliteDatabase sqlDB = new SqliteDatabase(filePath);
            string query = "update catprofile set deleted = 1 where catid = ";
            query = query + argCatID;
            DataTable dataTable = sqlDB.ExecuteQuery(query);

            if (argCatID == PlayerPrefs.GetString("SelectCat")) {
                sqlDB = new SqliteDatabase(filePath);
                query = "select max(catid) id from catprofile where deleted = 0";
                dataTable = sqlDB.ExecuteQuery(query);
                if (dataTable.Rows.Count <= 0)
                {
                    PlayerPrefs.SetString("SelectCat","0");
                }
                else
                {
                    PlayerPrefs.SetString("SelectCat", dataTable[0]["id"].ToString());
                }

            }
            SceneManager.LoadScene("CatList");

        }
        catch (Exception e)
        {
            GameObject.Find("TextHistory").GetComponent<Text>().text = e.Message.ToString() + e.StackTrace.ToString();
        }

    }



    // Update is called once per frame
    void Update () {
	
	}
}
