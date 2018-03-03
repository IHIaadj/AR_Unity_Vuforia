using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageRender : MonoBehaviour {

    public static string path = "" ; 
    public GameObject Image;
    public static ImageRender instance;

    // Use this for initialization
    void Start()
    {
        instance = this;
        if (path != "")
        {
            Debug.Log(path);
            Image = GameObject.Find("Image");
            GetPhoto(Image, path);
        }
    

        }

    // Update is called once per frame
    void Update()
    {
 
        if (Input.GetKey(KeyCode.Escape))
        { 
            SceneManager.LoadScene("mainScene");
        }
    }    

    // Get the Viewer to display the right image
    public void GetPhoto(GameObject screenShotViewer, string path )
    {
        string url = path;
        var bytes = File.ReadAllBytes(url);
        Texture2D texture = new Texture2D(2, 2);
        bool imageLoadSuccess = texture.LoadImage(bytes);
        while (!imageLoadSuccess)
        {
            print("image load failed");
            bytes = File.ReadAllBytes(url);
            imageLoadSuccess = texture.LoadImage(bytes);
        }
        print("Image load success: " + imageLoadSuccess);
        screenShotViewer.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0f, 0f), 100f);


    }
}
