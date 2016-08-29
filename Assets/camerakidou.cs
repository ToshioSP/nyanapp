using UnityEngine;
using System.Collections;

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
