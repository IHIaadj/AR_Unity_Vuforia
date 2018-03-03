using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI; 


public class Screenshot : MonoBehaviour
{

    private string path = ""; 
    public static int i = 0;
    Camera mainCamera;
    RenderTexture renderTex;
    Texture2D screenshot;
    Texture2D LoadScreenshot;
    int width = Screen.width;   // for Taking Picture
    int height = Screen.height; // for Taking Picture
    string fileName;
    string screenShotName = "ARPicture";

    public void Start()
    {

        Debug.Log("Start screenshot");
       
    }


    public void TakeScreenShotAndShare()
    {
        Debug.Log("Starting capture "); 
        // Coroutine : Because we're going to pause the function 
        StartCoroutine(takeScreenshot());
    }

    private IEnumerator takeScreenshot()
    {

    
        yield return null; // Wait till the last possible moment before screen rendering to hide the UI

        yield return new WaitForEndOfFrame(); // Wait for screen rendering to complete
        // Deviding in two possible orientations to choose the right width and height 
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            mainCamera = Camera.main.GetComponent<Camera>(); 
            renderTex = new RenderTexture(width, height, 24);
            mainCamera.targetTexture = renderTex;
            RenderTexture.active = renderTex;
            mainCamera.Render();
            screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenshot.Apply(); //false
            RenderTexture.active = null;
            mainCamera.targetTexture = null;
        }
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            mainCamera = Camera.main.GetComponent<Camera>();
            renderTex = new RenderTexture(height, width, 24);
            mainCamera.targetTexture = renderTex;
            RenderTexture.active = renderTex;
            mainCamera.Render();
            screenshot = new Texture2D(height, width, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(0, 0, height, width), 0, 0);
            screenshot.Apply(); //false
            RenderTexture.active = null;
            mainCamera.targetTexture = null;
        }
        // on Windows 8 - C:/Users/Username/AppData/LocalLow/CompanyName/GameName
        // on Android - /Data/Data/com.companyname.gamename/File

        path = Application.persistentDataPath + "/" + screenShotName  + i + ".png";
        File.WriteAllBytes(path , screenshot.EncodeToPNG());

        // Loading next scene 
        SceneManager.LoadScene("PhotoScene");
        ImageRender.path = path;
        
    }

    public void sharePicture()
    {
        StartCoroutine(shareScreenshot(Application.persistentDataPath + "/" + screenShotName + i +".png"));
    }

    private IEnumerator shareScreenshot(string path)
    {
        string ShareSubject = "Picture Share";
        string shareLink = "GDD Extended Algiers 2018 !!! " + "\nhttps://gdd-ext-algiers.firebaseapp.com";
        string textToShare = "";

        Debug.Log("share : " +path);
        i++; 

        if (!Application.isEditor)
        {

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + path);

            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), textToShare + shareLink);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), ShareSubject);
            intentObject.Call<AndroidJavaObject>("setType", "image/png");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("startActivity", intentObject);
        }
        yield return null;
    }
 

}
