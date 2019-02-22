using AStar;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script Game Manager. Se encarga de centralizar operaciones como llamar al canvas, poner el juego en pausa, controlar el estado de la partida...
/// </summary>
public class GMTutorial : GM
{

    public GameObject exitButton;
    public GameObject manoPath, manoCombustible;
    public GameObject[] cartelesTutorial;
    float timeBlocked = 25;

    /// <summary>
    /// Cartel recordatorio por si el usuario se bloquea.
    /// </summary>
    public GameObject recordatorio;
    int indTutorial;

    IEnumerator fadeOut()
    {
        while (aS.volume > 0)
        {
            aS.volume -= 0.01f;
            yield return null;
        }

    }
    
    void Awake()
    {
        aS = GameObject.Find("SoundManager").GetComponent<AudioSource>();
        StartCoroutine(fadeOut());
        manoMapa.gameObject.SetActive(false);
        manoPath.gameObject.SetActive(false);
        manoCombustible.gameObject.SetActive(false);
        foreach (GameObject go in cartelesTutorial) go.gameObject.SetActive(false);
        indTutorial = 0;

        nivel = GameObject.Find("Nivel").gameObject;

        //Se inicializan los atributos para el A*
        mapa = new int[alto, ancho];
        int it = 0;
        for (int i = 0; i < alto; i++)
            for (int j = 0; j < ancho; j++)
            {
                if (nivel.transform.GetChild(it).gameObject.layer == 8) mapa[i, j] = 20;
                else mapa[i, j] = 1;
                it++;
            }
        //Descomentar para escribir el mapa por consola.
        #region EscribirMapa
        /*
         string s = "";
         for(int i = 0; i< alto; i++)
         {
             for(int j = 0; j < ancho; j++)
             {
                 s += mapa[i, j].ToString() + " ";
             }
             //Debug.Log(s);
             s += "\n";
         }
         Debug.Log(s);
         */
        #endregion

        solver = new AStarSolver(ancho, alto);                                      //Se inicializa el solver.
        solver.ActualizaMapa(mapa);
        meta.x = Mathf.FloorToInt(metaO.transform.position.x); meta.y = Mathf.FloorToInt(-metaO.transform.position.y);

        

    }


    /// <summary>
    /// Se llama cuando se pulsa el botón de Pausa
    /// </summary>
     public override void OnMapClicked(GameObject texto)
    {
        //Debug.Log(indTutorial);
        if (indTutorial == 0 || indTutorial == 5 || indTutorial>= 8)
        {
            Debug.Log(indTutorial);
            actualizaTutorial();
        }
        else return;
        
       int num = 100;
        if (texto != null) num = int.Parse(texto.GetComponent<Text>().text);
        Debug.Log(num);
        if (num > 0)
        {
            paused = !paused;
            car.GetComponent<Car>().OnPause();
            car.transform.Find("Posicion").gameObject.SetActive(paused);
            cameraPausa.gameObject.SetActive(paused);
            cameraPrincipal.gameObject.SetActive(!paused);
            

            if (paused)
            {
                ImageConsumo.SetActive(false);
                metaO.GetComponent<MeshRenderer>().enabled = true;
                x = Mathf.FloorToInt(car.gameObject.transform.position.x);
                y = Mathf.FloorToInt(-car.gameObject.transform.position.y);
                Posicion pos = car.GetComponentInChildren<Car>().UltimaCasilla();

                mapa[pos.y, pos.x] = 100000;
                Find(x, y, true);
                //contexto.SetActive(true);

            }
           else
            {
                manoMapa.SetActive(false);
                ImageConsumo.SetActive(true);
                Find(x, y, false);
                Posicion pos = car.GetComponentInChildren<Car>().UltimaCasilla();
                mapa[pos.y, pos.x] = 1;
                num--;
                if (texto != null) texto.GetComponent<Text>().text = num.ToString();
                contexto.SetActive(false);
                metaO.GetComponent<MeshRenderer>().enabled = false;

            }

        }

    }

    
    void actualizaTutorial()
    {
        switch (indTutorial)
        {
            case 0:
                car.transform.Find("Coche").gameObject.GetComponent<Car>().OnPause();
                cartelesTutorial[indTutorial].gameObject.SetActive(true);
                break;
            case 1:
                cartelesTutorial[indTutorial - 1].gameObject.SetActive(false);
                cartelesTutorial[indTutorial].gameObject.SetActive(true);
                break;
            case 2:
                manoMapa.SetActive(false);
                cartelesTutorial[indTutorial - 1].gameObject.SetActive(false);
                manoPath.SetActive(true);
                cartelesTutorial[indTutorial].gameObject.SetActive(true);
                break;
            case 3:
                cartelesTutorial[indTutorial - 1].gameObject.SetActive(false);
                manoPath.SetActive(false);
                cartelesTutorial[indTutorial].gameObject.SetActive(true);
                break;
            case 4:
                cartelesTutorial[indTutorial - 1].gameObject.SetActive(false);
                manoPath.SetActive(false);
                manoMapa.SetActive(true);
                cartelesTutorial[indTutorial].gameObject.SetActive(true);
                break;
            case 5:
                cartelesTutorial[indTutorial - 1].gameObject.SetActive(false);
                cartelesTutorial[indTutorial].gameObject.SetActive(true);
                manoMapa.SetActive(false);
                car.transform.Find("Coche").gameObject.GetComponent<Car>().OnPause();
                break;

            case 6:
                cartelesTutorial[indTutorial - 1].gameObject.SetActive(false);
                cartelesTutorial[indTutorial].gameObject.SetActive(true);
                manoCombustible.gameObject.SetActive(true);
                break;
            case 7:
                cartelesTutorial[indTutorial - 1].gameObject.SetActive(false);
                cartelesTutorial[indTutorial].gameObject.SetActive(true);
                manoCombustible.gameObject.SetActive(false);
                break;
            case 8:
                cartelesTutorial[indTutorial - 1].gameObject.SetActive(false);
                exitButton.SetActive(true);
                break;

        }
        indTutorial++;
    }
    private void Update()
    {
        if (!car.transform.Find("Coche").GetComponent<Car>().IsMoving() && indTutorial == 0) actualizaTutorial();
        if (indTutorial <= 8&& indTutorial != 5 && Input.GetMouseButtonDown(0)) actualizaTutorial();
        if (Input.GetMouseButtonDown(0))
        {
            timeBlocked = 10;
            recordatorio.SetActive(false);
        }
        if(timeBlocked <= 0)
        {
            recordatorio.SetActive(true);
            timeBlocked = 10;
        }
        else
        {
            timeBlocked -= Time.deltaTime;
        }
    }
}
