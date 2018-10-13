using UnityEngine;

/// <summary>
/// Script que mueve el objeto mediante la función seno en el eje Z.
/// </summary>
public class AnimaciónSeno : MonoBehaviour
{
    Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, _startPosition.z + (Mathf.Sin(Time.time * 3)));
    }
}