using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {

	public static SceneController instance;

	private GUIController _GUI;
	private SoundController _SOUND;
	public	GameObject _marker; 
	GameObject[] _parts;
	public Animator logoGui;

	void Awake () {
		if (instance){
			Destroy (gameObject);
		}else{
			instance = this;
			DontDestroyOnLoad (gameObject);
		}

		DontDestroyOnLoad (transform.gameObject);

		_SOUND = GetComponent<SoundController>();
		_GUI = GetComponent<GUIController>();
	}

	void Start(){
		AddControllersToObjs();
	}

	public void OpenScene(){
		logoGui.Play("action");
		_SOUND.Play("capOpen", 0.05f, 1f);
		_parts[0].GetComponent<ObjControll>()._info.SetActive(true);
	}

	public void ShowPart(GameObject _pt){
		foreach(GameObject _obj in _parts){
			_obj.GetComponent<ObjControll>()._info.SetActive(false);
		}

		_pt.GetComponent<ObjControll>()._info.SetActive(true);

		if (_pt.GetComponent<Animator>() != null){
			_pt.GetComponent<Animator>().Play("action");
		}

		_SOUND.StopMusic(true);
		//string sndName = "whisp1";
		switch(_pt.name){
			case "Obj1": _SOUND.Play("whisp1", 0.4f, 1f); break;
			case "Obj2": _SOUND.Play("whisp1", 0.4f, 1f); break;
			case "Obj3": _SOUND.Play("whisp1", 0.4f, 1f); break;
			case "Obj4": _SOUND.Play("swish", 0.4f, 1f); break;
			case "Obj5": _SOUND.Play("water", 0.4f, 1f); break;
			case "Obj6": _SOUND.PlayMusic("bubbles", 0.35f, 1f, true); break;
			case "Obj7": _SOUND.Play("swish", 0.4f, 1f); break;
			
		default: break;
		}


	}

	void AddControllersToObjs(){
		int chNum = _marker.transform.childCount;
		_parts = new GameObject[chNum];

		for (int i = 0; i < chNum; i++){
			_marker.transform.GetChild(i).gameObject.AddComponent<ObjControll>();
			_parts[i] = _marker.transform.GetChild(i).gameObject;
		}
	}

	void Update () {
	
	}

	public void PlaySnd(){
		SoundController.instance.Play("test 0", 1f, 1f);
	}

	public void FoundMarker(){
		_GUI.HideTip();
	}

	public void LostMarker(){
		_GUI.ShowTip();
	}
	
}

//TODO: сделать два режима нормализации в SmoothCamera
