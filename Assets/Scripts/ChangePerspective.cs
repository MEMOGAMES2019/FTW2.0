using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChangePerspective : MonoBehaviour {

    public Sprite isoSprite, cenitSprite;
    public Camera cameraPauseIso, cameraPauseCenit, cameraGameIso, cameraGameCenit;
    bool iso = false;
    public Button button;
	// Use this for initialization
	void Start () {
        Change_Perspective();
	}

    public void Change_Perspective() {
        iso = !iso;
        cameraPauseCenit.gameObject.SetActive(!iso);
        cameraGameCenit.gameObject.SetActive(!iso);
        cameraPauseIso.gameObject.SetActive(iso);
        cameraGameIso.gameObject.SetActive(iso);
        if (!iso) button.GetComponent<Image>().sprite = isoSprite;
        else button.GetComponent<Image>().sprite = cenitSprite;
        
    }
}
