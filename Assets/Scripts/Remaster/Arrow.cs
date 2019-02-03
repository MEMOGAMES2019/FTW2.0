﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Arrow : MonoBehaviour {

    public int dir;
    public Car car;
	
    /// <summary>
    /// Comprueba si se ha hecho click sobre ella y avisa al coche.
    /// </summary>
    private void OnMouseUp()
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
