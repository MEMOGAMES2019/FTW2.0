using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfo : MonoBehaviour {

    public GameObject info;
    public string name;
	// Use this for initialization
	void Start () {
        info.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnMouseEnter()
    {
        info.SetActive(true);
        info.GetComponentInChildren<Text>().text = name;
    }
    private void OnMouseExit()
    {
        info.SetActive(false);
    }
}
