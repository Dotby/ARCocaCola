using UnityEngine;
using System.Collections;

public class CanBut : MonoBehaviour {

	public SceneController man;
	public GameObject part;

	// Use this for initialization
	void Start () {
		//man = GameObject.Find("Controller").GetComponent<SceneController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		//man.ShowPart(part);
		SceneController.instance.ShowPart(part);
		PlayMakerFSM.BroadcastEvent(fsmEventName: "rotateParts");
	}
}
