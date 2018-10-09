using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script de las flechas de dirección del coche. Detectan el click y avisan al coche.
/// </summary>

public class Flecha : MonoBehaviour {

    public GameObject coche;
    public AudioClip flecha;
    AudioSource audioSource;

    bool cocheParado = false;

    void Start () {
        audioSource = Camera.main.GetComponent<AudioSource>();
	}
	
	//Comprueba si se ha hecho click sobre ella y avisa al coche.
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.clip = flecha;
            audioSource.Play();
            coche.GetComponent<Coche>().onClick(this.gameObject.name);
        }

    }
    //Activa la flecha pero no la dibuja.
    public void doStuff()
    {
        this.gameObject.SetActive(true);
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
   
    //Método que recibe el estado del coche.(Parado o moviendose)
    public void estadoCoche(bool estado)
    {
        cocheParado = estado;
        layer = 0;
    }

    int layer;
    //Método que comprueba si la flecha está sobre la carretera o 
    //sobre el césped basándose en la capa de física con la que colisiona.
    private void OnTriggerStay(Collider other)
    {
        if (cocheParado)
        {
            layer += other.gameObject.layer;
            if(layer == 16)this.gameObject.SetActive(false);
        }
    }

    //Método que se ejecuta al final de la actualización y muestra la flechas de nuevo.
    float time = 0.1f;
    private void LateUpdate()
    {
        time -= Time.deltaTime;
        
        if (cocheParado && time <=0)
        {
            time = 0.1f;
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
