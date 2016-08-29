using UnityEngine;
using System.Collections;
using System.IO;
public class camerakidou : MonoBehaviour {

    // Use this for initialization
    public int Width = 1920;
    public int Height = 1080;
    public int FPS = 30;
    public WebCamTexture webcamTexture;
    public Color32[] color32;
    public bool flaga = false;
    public void Camerakidou()
    {
        GameObject.Find("NonActObj").SetActive(false);

        flaga = true;
        WebCamDevice[] devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(devices[0].name, Width, Height, FPS);

        // display all cameras
        for (var i = 0; i < devices.Length; i++)
        {
            Debug.Log(devices[i].name);
        }

        GetComponent<Renderer>().material.mainTexture = webcamTexture;
        //        color32 = webcamTexture.GetPixels32();
        webcamTexture.Play();

    }

    public void Shot()
    {

        string format = "yyyy-MM-dd-HH-mm-ss";
        string fileName = System.DateTime.Now.ToString(format) + ".png";

        //スクリーンショット撮影(保存先はApplication.persistentDataPath直下)
        Application.CaptureScreenshot(fileName);


        string filePath = "";

        // select platform
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                filePath = Application.persistentDataPath + "/" + fileName;
                break;
            case RuntimePlatform.Android:
                filePath = Application.persistentDataPath + "/" + fileName;
                break;
            default:
                filePath = Application.persistentDataPath + "/" + fileName;
//                filePath = fileName;
                break;
        }
        //ファイルの保存を待つ処理
        StartCoroutine(ImageCheck(filePath));
        print("撮影完了");

    }

    IEnumerator ImageCheck(string _filePath){
            while (File.Exists(_filePath) == false)
            {
                yield return new WaitForSeconds(0.2f); //
            }
            Debug.Log("OK"); ;
    }
        // Update is called once per frame
        void Update () {
        /*
                if (flaga)
                {
                    Texture2D texture = new Texture2D(webcamTexture.width, webcamTexture.height);
                    GameObject.Find("Panel").GetComponent<Renderer>().material.mainTexture = texture;

                    texture.SetPixels32(color32);
                    texture.Apply();

                }
            */
    }
}
