using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Arrow : MonoBehaviour {

    public int dir;
    public Car car;
    GM gm;
    private void Start()
    {
        gm = GameObject.Find("GM").GetComponent<GM>();
    }
    /// <summary>
    /// Comprueba si se ha hecho click sobre ella y avisa al coche.
    /// </summary>
    private void OnMouseUp()
    {
        if (!gm.Paused)
        {
            switch (dir)
            {
                case 0: car.MoveToRight(); break;
                case 1: car.MoveDown(); break;
                case 2: car.MoveToLeft(); break;
                case 3: car.MoveUp(); break;

            }
        }
       
    }
}
