using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;

public class ShareHandler : MonoBehaviour
{
    public void Share()
    {
        ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "capture.png");
        StartCoroutine(Share("share text", "url", Application.streamingAssetsPath + "capture.png"));
    }

    public void OnShareClick() { }

    IEnumerator Share(string text, string url, string streamAssetsImagePath)
    {
#if UNITY_ANDROID
        UnityWebRequest request = UnityWebRequest.Get(streamAssetsImagePath); // ローカルファイルを読む
        yield return request.SendWebRequest();

        string imagePath = Application.temporaryCachePath + "/shareimage.png";
        System.IO.File.WriteAllBytes(imagePath, request.downloadHandler.data);
        SocialConnector.SocialConnector.Share(text, url, imagePath);
#else
        SocialConnector.SocialConnector.Share(text, url, streamAssetsImagePath);
        yield break;
#endif
    }
}