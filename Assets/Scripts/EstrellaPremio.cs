using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script para las estrellas obtenidas al llegar a la meta. Se encarga de la "animación" tanto en escala como en posición.
/// </summary>

public class EstrellaPremio : MonoBehaviour {

    bool move = false;                                                  //Flag que controla si tiene que actualizarse la animación.
    float x = 426, y = 200;                                             //Controlan la posición de la estrella.
    float size = 0.24f;                                                 //Controlan el tamaño.

	void Update () {
        if (move)
        {
            float incrX = 300 * Time.deltaTime;
            float incrY = 140 * Time.deltaTime;
            if (x - incrX >= 0) x -= incrX;
            else x = 0;
            if (y - incrY >= 0) y -= incrY;
            else y = 0;

            if (size + 0.02 <= 1) size += 0.02f;
            else size = 1;

            transform.localPosition = new Vector3(x, y, transform.localPosition.z);
            transform.localScale = new Vector3(size, size, size);
            if (x + y == 0) move = false;
        }
	}
    private void OnEnable()
    {
        move = true;
    }
}
