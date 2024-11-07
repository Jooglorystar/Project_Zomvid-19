using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum Size
{
    POT64,
    POT128,
    POT256,
    POT512,
    POT1024
}


public class Capture : MonoBehaviour
{
    public Camera cam;
    public RenderTexture rt;
    public Image bg;

    public Size size;

    public GameObject[] obj;
    int nowCnt = 0;

    private void Start()
    {
        cam = Camera.main;
        SettingSize();
    }

    public void Create()
    {
        StartCoroutine(CaptrueImage());
    }

    public void AllCreate()
    {
        StartCoroutine(AllCaptureImage());
    }

    IEnumerator CaptrueImage()
    {
        yield return null;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false, true);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

        yield return null;

        var data = tex.EncodeToPNG();
        string name = "Thumbnail";
        string extention = ".png";
        string path = Application.persistentDataPath + "/Thumbnail/";

        Debug.Log(path);

        if(!Directory.Exists(path)) Directory.CreateDirectory(path);

        File.WriteAllBytes(path + name + extention, data);

        yield return null;
    }

    void SettingSize()
    {
        switch (size)
        {
            case Size.POT64:
                rt.width = 64;
                rt.height = 64;
                break;
            case Size.POT128:
                rt.width = 128;
                rt.height = 128;
                break;
            case Size.POT256:
                rt.width = 256;
                rt.height = 256;
                break;
            case Size.POT512:
                rt.width = 512;
                rt.height = 512;
                break;
            case Size.POT1024:
                rt.width = 1024;
                rt.height = 1024;
                break;
        }
    }

    IEnumerator AllCaptureImage()
    {
        for(int i = 0; i < obj.Length; i++)
        {
            var nowObjt = Instantiate(obj[i].gameObject);

            yield return null;

            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false, true);
            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

            yield return null;

            var data = tex.EncodeToPNG();
            string name = $"Thumbnail_{obj[i].gameObject.name}";
            string extention = ".png";
            string path = Application.persistentDataPath + "/Thumbnail/";


            Debug.Log(path);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            File.WriteAllBytes(path + name + extention, data);

            yield return null;

            DestroyImmediate(nowObjt);

            yield return null;
        }
    }
}
