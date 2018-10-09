using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que contiene el comportamiento del coche.
/// </summary>

public class Coche : MonoBehaviour {

    bool moving = false;                                                    //Controla si el coche se está moviendo o no.
    bool paused = false;                                                    //Controla si el juego está pausado o no.
    float x=0, y=0;                                                         //Controla la posición del coche.
    float vel=0.1f;                                                         //Velocidad del coche.
    int[] incr = new int[4];                                                //Array que va a contener las direcciones al girar.
    int dir = 0;                                                            //Guarda la dirección a la que nos movemos.

    public GameObject[] flechas = new GameObject[3];                        //Array que contiene las tres flechas de dirección.
    public GameObject player;                                               //Objeto coche
    public GameObject combustible;                                          //Objeto del GUI que muestra el combustible

    private GameObject bar;                                                 //Barra de combustible dentro del objeto que muestra el combustible.
    private float totalEnergy;                                              //Cantidad total de combustible.
    private RectTransform rt;                                               //Dimensiones de la barra de combustible.

    public float consumo = 1f;                                              //Cantidad de consumo por segundo
    private float consumido;                                                //Total de lo consumido.

    private GameObject GM;                                                  //Referencia al GM.

	
	void Start () {
        int x = 1;
		for (int i = 0; i < 4; i++)
        {
            incr[i] = x;
            x -= 1;
            if (x < -1) x = 0;
        }
        for (int i = 0; i < 3; i++) flechas[i] = player.gameObject.transform.GetChild(i).gameObject;

        bar = combustible.transform.GetChild(0).transform.GetChild(0).gameObject;
        rt = bar.GetComponent<RectTransform>();                                                    //Se configura el rectángulo de la barra para poder decrementarla
        totalEnergy = rt.sizeDelta.x;

        moving = true;
        arranca();

        GM = GameObject.Find("GM");
	}
	
	
	void Update () {
        if (moving) move();
	}

    //Si se ha pulsado alguna flecha se comprueba cual es, se gira el coche y se actualiza la dirección si es necesario y se llama a arrancar.
    public void onClick(string s)
    {
        if (!paused)
        {
            if (s == "Derecha")
            {
                player.transform.position = new Vector3(player.transform.position.x + x * 8, player.transform.position.y + y * 8, player.transform.position.z);
                player.gameObject.transform.Rotate(new Vector3(0, 0, -90));
                dir = (dir + 1) % 4;
            }
            else if (s == "Izquierda")
            {
                player.transform.position = new Vector3(player.transform.position.x + x * 8, player.transform.position.y + y * 8, player.transform.position.z);
                player.gameObject.transform.Rotate(new Vector3(0, 0, 90));
                dir = (dir - 1);
                if (dir < 0) dir = 3;
            }

            arranca();
        }

    }
    //Actualiza la posición del coche según el giro y actualiza la información de las flechas.
    void arranca()
    {
        x = incr[dir] * vel;
        y = incr[(dir + 1) % 4] * vel;
        moving = true;
        foreach (GameObject go in flechas)
        {
            go.transform.position = new Vector3(go.transform.position.x + 10, go.transform.position.y + 10, go.transform.position.z + 10);
            go.GetComponent<Flecha>().estadoCoche(false);
            go.SetActive(false);
        }
    }

    float countdown = 0.25f;
    //Método que actualiza la posición del coche y suma el consumo cada segundo.
    void move()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0.0f)
        {
            countdown = 0.25f;
            consumido += consumo;
            setPercentageOfEnergy(consumido);

            //Si el consumo llega al 100% se para de contar, 
            //se manda una traza con los datos y se termina la partida.
            if (consumido >= 100)
            {
                moving = false;
                GM.GetComponent<GM>().gameOver(false);
            }
        }
        player.transform.position = new Vector3(player.transform.position.x + x *(Time.deltaTime*50), player.transform.position.y + y*(Time.deltaTime*50), player.transform.position.z);
    }

    //Este método es llamado al poner el juego en pausa
   public void onPause()
    {
        paused = !paused;
       
    }

    //Cuando el coche colisiona con un cruce o una intersección se llama a este método. 
    //Al recibir la colisión, activa las flechas y las muestra.
    private void OnCollisionEnter(Collision collision)
    {

        moving = false;
        foreach (GameObject go in flechas)
        {   
            go.transform.position = new Vector3(go.transform.position.x - 10, go.transform.position.y - 10, go.transform.position.z - 10);
            go.GetComponent<Flecha>().estadoCoche(true);
            go.GetComponent<Flecha>().doStuff();
        }
    }

    //Actualiza la barra de combustible según el consumo.
    public void setPercentageOfEnergy(float newValue)
    {
        float x = (newValue * totalEnergy) / 100;
        float y = rt.localPosition.y;
        float z = rt.localPosition.z;

        rt.localPosition = new Vector3(-x, y, z);
    }
    public int getConsumoTotal() { return (int)consumido; }
}
