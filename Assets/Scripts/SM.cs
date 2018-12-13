using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM : MonoBehaviour {

    // Use this for initialization

    public static SM soundManager;

    private void Awake()
    {
        if(soundManager == null)
        {
            soundManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (soundManager != this)
        {
            Destroy(gameObject);
        }
    }

}
