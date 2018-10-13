using UnityEngine;

/// <summary>
/// Script de las flechas de dirección del coche. Detectan el click y avisan al coche.
/// </summary>
public class Flecha : MonoBehaviour
{

    public GameObject coche;
    public AudioClip flecha;
    AudioSource audioSource;

    bool cocheParado = false;

    void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Comprueba si se ha hecho click sobre ella y avisa al coche.
    /// </summary>
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.clip = flecha;
            audioSource.Play();
            coche.GetComponent<Coche>().OnClick(this.gameObject.name);
        }

    }

    /// <summary>
    /// Activa la flecha pero no la dibuja.
    /// </summary>
    public void DoStuff()
    {
        this.gameObject.SetActive(true);
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    /// <summary>
    /// Método que recibe el estado del coche.
    /// </summary>
    /// <param name="estado">Estado del coche: Parado o moviendose </param>
    public void EstadoCoche(bool estado)
    {
        cocheParado = estado;
        layer = 0;
    }

    int layer;
    /// <summary>
    /// Método que comprueba si la flecha está sobre la carretera o 
    /// sobre el césped basándose en la capa de física con la que colisiona.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (cocheParado)
        {
            layer += other.gameObject.layer;
            if (layer == 16) this.gameObject.SetActive(false);
        }
    }

    float time = 0.1f;
    /// <summary>
    /// Método que se ejecuta al final de la actualización y muestra la flechas de nuevo.
    /// </summary>
    private void LateUpdate()
    {
        time -= Time.deltaTime;

        if (cocheParado && time <= 0)
        {
            time = 0.1f;
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}