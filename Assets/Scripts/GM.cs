using AStar;
using RAGE.Analytics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script Game Manager. Se encarga de centralizar operaciones como llamar al canvas, poner el juego en pausa, controlar el estado de la partida...
/// </summary>
public class GM : MonoBehaviour
{

    //Atributos para el algoritmo de pathfinding A*
    AStarSolver solver;
    LinkedList<Posicion> sol;
    Posicion meta;
    GameObject nivel;
    public int ancho = 0, alto = 0;

    int[,] mapa;

    /// <summary>
    /// Controla si el juego está pausado o no.
    /// </summary>
    bool paused = false;

    /// <summary>
    /// Controlan la posición X del player en el mapa.
    /// </summary>
    private int x;

    /// <summary>
    /// Controlan la posición Y del player en el mapa.
    /// </summary>
    private int y;

    /// <summary>
    /// Referencia al objeto coche.
    /// </summary>
    public GameObject coche;

    /// <summary>
    /// Referencia a la meta.
    /// </summary>
    public GameObject metaO;

    /// <summary>
    /// Cámara principal del juego. Se usa cuando el juego no está pausado.
    /// </summary>
    public GameObject cameraPrincipal;

    /// <summary>
    /// Cámara global del mapa. Se usa en pausa.
    /// </summary>
    public GameObject cameraPausa;

    /// <summary>
    /// Panel de victoria.
    /// </summary>
    public GameObject panelWin;

    /// <summary>
    /// Variable que contiene el mejor consumo para recorrer el mapa hasta la meta.
    /// </summary>
    int consumoIdeal;

    /// <summary>
    /// Nivel del mapa.
    /// </summary>
    public int numNivel;

    /// <summary>
    /// Número de mapa.
    /// </summary>
    public int numMapa;


    /// <summary>
    /// Contexto que indica donde debe llegar el jugador.
    /// </summary>
    public GameObject contexto;


    /// <summary>
    /// Mano de ayuda que señala el mapa.
    /// </summary>
    public GameObject manoMapa;

    /// <summary>
    /// Imagen con el consumo
    /// </summary>
    public GameObject ImageConsumo;

    void Start()
    {
        consumoIdeal = -1;
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

        OnMapClicked(null);

    }
  

    /// <summary>
    /// Este método se encarga de llamar al solver A* y recibe una lista con el camino óptimo desde la posición (x,y) hasta la meta.
    /// Si el parámetro mostrar es cierto, resalta el camino encontrado. Si es falso, quita el resaltado.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="mostrar"></param>
    public void Find(int x, int y, bool mostrar)
    {
        sol = solver.Solve(x, y, meta);
        sol.RemoveFirst();
        if(consumoIdeal == -1)
        {
            consumoIdeal = sol.Count;
        }
        while (sol.Count > 0)
        {
            int ind;
            ind = sol.First.Value.y * (ancho - 1) + sol.First.Value.y + sol.First.Value.x;
            nivel.transform.GetChild(ind).gameObject.transform.Find("Resaltar").gameObject.SetActive(mostrar);
            sol.RemoveFirst();
        }

    }

    /// <summary>
    /// Se llama cuando se pulsa el botón de Pausa
    /// </summary>
    public void OnMapClicked(GameObject texto)
    {
        int num = 100;
        if (texto != null) num = int.Parse(texto.GetComponent<Text>().text);
        if (num > 0)
        {
            paused = !paused;
            coche.transform.Find("Coche").gameObject.GetComponent<Coche>().OnPause();
            coche.transform.Find("Posicion").gameObject.SetActive(paused);
            cameraPausa.gameObject.SetActive(paused);
           

            if (paused)
            {
                ImageConsumo.SetActive(false);
                metaO.GetComponent<MeshRenderer>().enabled = true;
                x = Mathf.FloorToInt(coche.gameObject.transform.position.x);
                y = Mathf.FloorToInt(-coche.gameObject.transform.position.y);
                Posicion pos = coche.GetComponentInChildren<Coche>().UltimaCasilla();
                mapa[pos.y, pos.x] = 1000;
                Find(x, y, true);
                contexto.SetActive(true);

            }
            else
            {
                manoMapa.SetActive(false);
                ImageConsumo.SetActive(true);
                Find(x, y, false);
                Posicion pos = coche.GetComponentInChildren<Coche>().UltimaCasilla();
                mapa[pos.y, pos.x] = 1;
                num--;
                if (texto != null) texto.GetComponent<Text>().text = num.ToString();
                contexto.SetActive(false);
                metaO.GetComponent<MeshRenderer>().enabled = false;

            }

        }

    }

    /// <summary>
    /// Se llama cuando acaba la partida. El parametro win contiene si se ha ganado o no.
    /// </summary>
    /// <param name="win"></param>
    public void GameOver(bool win)
    {
        string nivelMapa = string.Concat("N", this.numNivel, "mapa", this.numMapa);
        coche.transform.Find("Coche").gameObject.GetComponent<Coche>().OnPause();

        if (win) //Si se ha ganado se activa el panel de victoria y se dan las estrellas correspondientes según el consumo.
        {
            panelWin.gameObject.SetActive(true);
            int consumo = coche.transform.GetChild(3).GetComponent<Coche>().GetConsumoTotal();
            int numEstr = 0;
            Debug.Log(consumoIdeal);
            if (consumo <= consumoIdeal) numEstr = 3;
            else if (consumo <= consumoIdeal + 15) numEstr = 2;
            else if (consumo <= consumoIdeal + 20) numEstr = 1;
            else  numEstr = 0;

            /* Guardamos el número de estrellas que hemos conseguido si el número de estrellas 
             * es mayor al que teníamos anteriormente */
            Tracker.T.setVar("Estrellas " + nivelMapa, numEstr);

            Tracker.T.Completable.Completed(nivelMapa, CompletableTracker.Completable.Level, true);

            int estrellasActuales = PlayerPrefs.HasKey(nivelMapa) ? PlayerPrefs.GetInt(nivelMapa) : 0;

            if (numEstr > estrellasActuales)
            {
                PlayerPrefs.SetInt(nivelMapa, numEstr);
            }

            for (int i = 0; i < numEstr; i++)
                panelWin.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            Tracker.T.Completable.Completed(nivelMapa, CompletableTracker.Completable.Level, false);
        }
    }
}
