using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {
	
	public Animator _markerTip = null;
	public string _tipAnimType = "";
	
	void Start () {
		if (_markerTip == null){
			Debug.Log("MarkerTip no found.");
		}
	}

	void Update () {
	
	}

	public void ShowTip(){
		if (_markerTip.GetCurrentAnimatorStateInfo(0).IsName("MarkerTipOff")){
			_markerTip.Play("MarkerTipShow" + _tipAnimType);
		}
	}

	public void HideTip(){
		if (_markerTip.GetCurrentAnimatorStateInfo(0).IsName("MarkerTipOn")){
			_markerTip.Play("MarkerTipHide" + _tipAnimType);
		}
	}
}
