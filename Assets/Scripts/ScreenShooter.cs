using UnityEngine;
using System.Collections;

public class ScreenShooter : MonoBehaviour {
	
	public string albumName = "";
	public string fileName = "";
	bool isScreenShotSave = false;
	public bool isScreenShotWithDateTime;
	public GameObject button;
	
	public Texture2D texture;
	bool saved = false;
	bool saved2 = false;
	
	
	void Start () {
		ScreenshotManager.ScreenshotFinishedSaving += ScreenshotSaved;
		ScreenshotManager.ImageFinishedSaving += ImageSaved;
	}
	
	void Update () {
		
	}
	
	void ScreenshotSaved()
	{
		Debug.Log ("screenshot finished saving");
		saved = true;
	}
	
	void ImageSaved()
	{
		Debug.Log (texture.name + " finished saving");
		saved2 = true;
	}
	
	public void MakeScreenshot(){
		
		SoundController.instance.Play("camshot", .05f, 1f);
		StartCoroutine(ScreenshotManager.Save("SafePhone", "SafePhone", true));
		
	}
	
	void ScreenShotStatus(bool status)
	{
		isScreenShotSave = status;
	}
}