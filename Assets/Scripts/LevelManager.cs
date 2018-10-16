using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    // Use this for initialization
    public GameObject[] niveles;
    public GameObject[] mapas;
	void Start () {
        for (int i = 1; i < niveles.Length; i++)
            niveles[i].gameObject.GetComponent<Button>().interactable = false;

        
        if(PlayerPrefs.GetInt("Nivel1") == 2)
        {
            Debug.Log("IN");
            niveles[1].gameObject.GetComponent<Button>().interactable = true;
            niveles[1].transform.Find("Block").gameObject.SetActive(false);
        }

        for(int i = 0; i < mapas.Length; i++)
        {
            for(int mapa = 0; mapa < 3; mapa++)
            {
                string s = "N" + (i + 1).ToString() + "mapa" + (mapa+1).ToString();
                int m = PlayerPrefs.GetInt(s);
                //int m1 = PlayerPrefs.GetInt("N1mapa1");
                for (int j = 1; j <= m; j++)
                {
                    mapas[i].transform.GetChild(mapa).transform.GetChild(j).transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            
        }
	}
}
