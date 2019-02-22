using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Car : MonoBehaviour {

    int [,] map;
    public GameObject level;
    public GameObject car;
    public GM gm;
    public GameObject[] arrows; //0 --> derecha, 1 --> abajo, 2 --> izquierda, 3 --> arriba.
    public int width = 0, high = 0;
    public int dir = 0; //0 --> derecha, 1 --> abajo, 2 --> izquierda, 3 --> arriba.
    public int posX=0, posY= 0;
    bool pause = false;
    bool moving = true;
    bool OnMove = false;

    /// <summary>
    /// Objeto del GUI que muestra el combustible.
    /// </summary>
    public GameObject combustible;

    /// <summary>
    /// Barra de combustible dentro del objeto que muestra el combustible.
    /// </summary>
    private GameObject bar;

    /// <summary>
    /// Cantidad total de combustible.
    /// </summary>
    private float totalEnergy;

    /// <summary>
    /// Dimensiones de la barra de combustible.
    /// </summary>
    private RectTransform rt;

    /// <summary>
    /// Cantidad de consumo por segundo.
    /// </summary>
    float consumo = 1f;

    /// <summary>
    /// Total de lo consumido.
    /// </summary>
    private float consumido;


    void Start () {
        map = new int[high, width];
        int it = 0;
        for (int i = 0; i < high; i++)
            for (int j = 0; j < width; j++)
            {
                if (level.transform.GetChild(it).gameObject.layer == 8) map[i, j] = 20;
                else if (level.transform.GetChild(it).gameObject.layer == 10) map[i, j] = 2;
                else map[i, j] = 1;
                it++;
            }


        bar = combustible.transform.GetChild(0).transform.GetChild(0).gameObject;
        rt = bar.GetComponent<RectTransform>();                                                    //Se configura el rectángulo de la barra para poder decrementarla
        totalEnergy = rt.sizeDelta.x;
        gm.OnMapClicked(null);

        StartToMove();

    }
   
    void StartToMove()
    {
        switch (dir)
        {
            case 0: MoveToRight(); break;
            case 1: MoveDown(); break;
            case 2: MoveToLeft(); break;
            case 4: MoveUp(); break;
        }
    }

    public bool MoveToRight()
    {
        
        if (map[posY, posX + 1] == 1 && moving) posX++;
        else return false;
        dir = 0;

        while (map[posY, posX] != 2)
        {
            posX++;
        }


        int ind = posY * (width - 1) + posY + (posX);
        float x  = level.transform.GetChild(ind).gameObject.transform.position.x;
        
        //transform.position = new Vector3(x, transform.position.y, transform.position.z);
        StartCoroutine(move(new Vector3(x, transform.position.y, transform.position.z), 20.0f));
        return true;

    }

   
    public bool MoveToLeft()
    {
        if (map[posY, posX - 1] == 1 && moving) posX--;
        else return false;
        dir = 2;
       
        while (map[posY, posX] != 2)
        {
            posX--;
        }
        int ind = posY * (width - 1) + posY + (posX);
        float x = level.transform.GetChild(ind).gameObject.transform.position.x;

        StartCoroutine(move(new Vector3(x, transform.position.y, transform.position.z), 20.0f));

        return true;
    }
    public bool MoveUp()
    {
        if (map[posY-1, posX] == 1 && moving) posY--;
        else return false;
        dir = 3;
        while (map[posY, posX] != 2)
        {
            posY--;
        }
        int ind = posY * (width - 1) + posY + posX;
        float y = level.transform.GetChild(ind).gameObject.transform.position.y;
        StartCoroutine(move(new Vector3(transform.position.x, y, transform.position.z), 20.0f));

        return true;
    }
    public bool MoveDown()
    {
        if (map[posY + 1, posX] == 1 && moving) posY++; 
        else return false;
        dir = 1;

       
        while (map[posY, posX] != 2)
        {
            posY++;
        }

        int ind = posY * (width - 1) + posY + posX;
        float y = level.transform.GetChild(ind).gameObject.transform.position.y;
        StartCoroutine(move(new Vector3(transform.position.x, y, transform.position.z), 20.0f));

        return true;
    }

    void showArrows()
    {
        if (map[posY, posX + 1] != 20) arrows[0].gameObject.SetActive(true);
        if (map[posY + 1, posX] != 20) arrows[1].gameObject.SetActive(true);
        if (map[posY, posX-1] != 20) arrows[2].gameObject.SetActive(true);
        if (map[posY -1, posX] != 20) arrows[3].gameObject.SetActive(true);

        switch (dir)
        {
            case 0: arrows[2].gameObject.SetActive(false); break;
            case 1: arrows[3].gameObject.SetActive(false); break;
            case 2: arrows[0].gameObject.SetActive(false); break;
            case 3: arrows[1].gameObject.SetActive(false); break;
        }

    }

    IEnumerator move(Vector3 v, float t)
    {
        OnMove = true;
        foreach (GameObject go in arrows) go.SetActive(false);
       Vector3 vAux = new Vector3(v.x+0.5f, v.y, v.z);
        car.transform.LookAt(vAux, car.transform.up);
        while (transform.position != v)
        {

            if (!pause)
            {
                consumido += 8 * Time.deltaTime;
                SetPercentageOfEnergy(consumido);
                if (consumido >= 100)
                {
                    moving = false;
                    gm.GameOver(false);
                }

                transform.position = Vector3.MoveTowards(transform.position, v, (10 * Time.deltaTime));
            }
            yield return null;
        }
        OnMove = false;
        showArrows();

    }
    // Update is called once per frame
    void Update () {
		if (Input.GetKeyUp(KeyCode.D))
        {
            MoveToRight();
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            MoveToLeft();
        }
        else if(Input.GetKeyUp(KeyCode.W))
        {
            MoveUp();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            MoveDown();
        }
	}
    public void SetPercentageOfEnergy(float newValue)
    {
        float x = (newValue * totalEnergy) / 100;
        float y = rt.localPosition.y;
        float z = rt.localPosition.z;

        rt.localPosition = new Vector3(-x, y, z);
    }
    public int GetConsumoTotal() { return (int)consumido; }
    public Posicion UltimaCasilla()
    {
        Posicion pos; pos.x = posX; pos.y = posY;
        switch (dir)
        {
            case 0: pos.x-=2; break;
            case 1: pos.y-= 2; break;
            case 2: pos.x+=2; break;
            case 3: pos.y+=2; break;
        }

        return pos;
    }

    /// <summary>
    /// Este método es llamado al poner el juego en pausa
    /// </summary>
    public void OnPause()
    {
        pause = !pause;
    }
    
    public bool IsMoving() { return OnMove; }
}
