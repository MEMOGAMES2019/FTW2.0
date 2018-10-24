using UnityEngine;

/// <summary>
/// Script para las estrellas obtenidas al llegar a la meta. Se encarga de la "animación" tanto en escala como en posición.
/// </summary>
public class EstrellaPremio : MonoBehaviour
{
    /// <summary>
    /// Flag que controla si tiene que actualizarse la animación.
    /// </summary>
    bool move = false;

    /// <summary>
    /// Controlan la posición de la estrella.
    /// </summary>
    float x = 426, y = 200;

    /// <summary>
    /// Controlan el tamaño.
    /// </summary>
    float size = 0.24f;

    void Update()
    {
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