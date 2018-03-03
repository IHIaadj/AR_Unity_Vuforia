using UnityEngine;
using UnityEngine.SceneManagement;
public class ScenesManip : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("mainScene");
        }
    }
}
