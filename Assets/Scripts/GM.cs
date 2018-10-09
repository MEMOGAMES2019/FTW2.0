using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStar;

/// <summary>
/// Script Game Manager. Se encarga de centralizar operaciones como llamar al canvas, poner el juego en pausa, controlar el estado de la partida...
/// </summary>

public class GM : MonoBehaviour {

    //Atributos para el algoritmo de pathfinding A*
    AStarSolver solver;
    LinkedList<Posicion> sol;
    Posicion meta;
    GameObject nivel;
    public int ancho = 0, alto = 0;

    int[,] mapa;
    bool paused = false;                                            //Controla si el juego está pausado o no.
    private int x, y;                                               //Controlan la posición del player en el mapa.

    public GameObject coche;                                        //Referencia al objeto coche.
    public GameObject metaO;                                        //Referencia a la meta.
    public GameObject cameraPrincipal;                              //Cámara principal del juego. Se usa cuando el juego no está pausado.
    public GameObject cameraPausa;                                  //Cámara global del mapa. Se usa en pausa.
    public GameObject panelWin;                                     //Panel de victoria
    public int consumoIdeal;                                        //Variable que contiene el mejor consumo para recorrer el mapa hasta la meta.


    void Start () {
        nivel = GameObject.Find("Nivel").gameObject;

        //Se inicializan los atributos para el A*
        mapa = new int[alto, ancho];
       int it = 0;
       for(int i = 0; i < alto; i++)
            for(int j = 0; j < ancho; j++)
            {
                if (nivel.transform.GetChild(it).gameObject.layer == 8) mapa[i, j] = 20;
                else mapa[i,j] = 1;
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
        solver.actualizaMapa(mapa);
        meta.x = Mathf.FloorToInt(metaO.transform.position.x); meta.y = Mathf.FloorToInt(-metaO.transform.position.y);
        
    }

    /// <summary>
    /// Este método se encarga de llamar al solver A* y recibe una lista con el camino óptimo desde la posición (x,y) hasta la meta.
    /// Si el parámetro mostrar es cierto, resalta el camino encontrado. Si es falso, quita el resaltado.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="mostrar"></param>

	public void find(int x, int y, bool mostrar)
    {
        sol = solver.solve(x, y, meta);
        sol.RemoveFirst();
        while ( sol.Count > 0)
        {
            int ind;
            ind = sol.First.Value.y * (ancho-1) + sol.First.Value.y + sol.First.Value.x;
            nivel.transform.GetChild(ind).gameObject.transform.Find("Resaltar").gameObject.SetActive(mostrar);
            sol.RemoveFirst();
        }
       
    }
	
    /// <summary>
    /// Se llama cuando se pulsa el botón de Pausa
    /// </summary>
    public void OnMapClicked()
    {
        paused = !paused;
        coche.transform.Find("Coche").gameObject.GetComponent<Coche>().onPause();
        coche.transform.Find("Posicion").gameObject.SetActive(paused);
        cameraPausa.gameObject.SetActive(paused);

        if (paused)
        {
            x = Mathf.FloorToInt(coche.gameObject.transform.position.x);
            y = Mathf.FloorToInt(-coche.gameObject.transform.position.y);
            Debug.Log(x + " " + y);
            find(x, y, true);
        }
        else
        {

            Debug.Log(x + " " + y);
            find(x, y, false);
        }
    }

    /// <summary>
    /// Se llama cuando acaba la partida. El parametro win contiene si se ha ganado o no.
    /// </summary>
    /// <param name="win"></param>
    public void gameOver(bool win)
    {
        coche.transform.Find("Coche").gameObject.GetComponent<Coche>().onPause();

        if (win) //Si se ha ganado se activa el panel de victoria y se dan las estrellas correspondientes según el consumo.
        {
            panelWin.gameObject.SetActive(true);
            int consumo = coche.transform.GetChild(3).GetComponent<Coche>().getConsumoTotal();
            int numEstr = 0;
            if (consumo <= consumoIdeal +10) numEstr = 3;
            else if (consumo  <= consumoIdeal+30) numEstr = 2;
            else if (consumo  <= consumoIdeal+50) numEstr = 1;

            for (int i = 0; i < numEstr; i++)
                    panelWin.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
