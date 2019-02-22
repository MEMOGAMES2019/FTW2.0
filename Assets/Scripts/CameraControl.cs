using UnityEngine;

/// <summary>
/// Script que se encarga del movimiento de la cámara detrás de un target.
/// </summary>
public class CameraControl : MonoBehaviour
{
    public GameObject target;
    Vector3 offset;

    public float posXmax;
    public float posXmin;
    public float posYmax;
    public float posYmin;
    public float offsetXmax = 6;
    public float offsetXmin = 6;
    public float offsetYmax = 15;
    public float vel;

    // ==============================
    void Start()
    {
        posXmax = 261 - offsetXmax;
        posXmin = 0 + offsetXmin;
        posYmax = 50;// 0 - offsetYmax;
        posYmin = -57;

        offset = target.transform.position - transform.position;
    }

    // ==========================
    void LateUpdate()
    {
        // Limite del offset vertical del jugador y la cámara
        if (offset.y > 1) offset.y = 1;
        if (offset.y < -2) offset.y = -2;

        Vector3 orig = transform.position;
        Vector3 destino = target.transform.position - offset;

        // Limites de movimiento de la cámara
        if (destino.x < posXmin) destino.x = posXmin;
        if (destino.x > posXmax) destino.x = posXmax;
        if (destino.y < posYmin) destino.y = posYmin;
        if (destino.y > posYmax) destino.y = posYmax;

        Vector3.Lerp(orig, destino, 1 / 20f);
        Vector3 despl = Vector3.Lerp(orig, destino, Time.deltaTime*vel);
        transform.position = despl;
    }
}