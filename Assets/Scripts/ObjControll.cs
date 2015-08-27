using UnityEngine;
using System.Collections;

public class ObjControll : MonoBehaviour {

	public GameObject _info;

	// Use this for initialization
	void Start () {
		_info = gameObject.transform.FindChild("Info").gameObject;
		_info.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		SceneController.instance.ShowPart(gameObject);
	}
}
