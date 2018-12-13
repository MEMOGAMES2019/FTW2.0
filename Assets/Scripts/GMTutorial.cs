using AStar;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script Game Manager. Se encarga de centralizar operaciones como llamar al canvas, poner el juego en pausa, controlar el estado de la partida...
/// </summary>
public class GMTutorial : MonoBehaviour
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
    public int consumoIdeal;


    public GameObject manoMapa, manoPath, manoCombustible;
    public GameObject[] cartelesTutorial;
    int indTutorial;

    IEnumerator fadeOut()
    {
        while (aS.volume > 0)
        {
            aS.volume -= 0.01f;
            yield return null;
        }

    }
    AudioSource aS;
    void Start()
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
        if (indTutorial == 0 || indTutorial == 5)
        {
            actualizaTutorial();
        }
        else return;
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
                x = Mathf.FloorToInt(coche.gameObject.transform.position.x);
                y = Mathf.FloorToInt(-coche.gameObject.transform.position.y);
                Debug.Log(x + " " + y);
                Find(x, y, true);
            }
            else
            {

                Debug.Log(x + " " + y);
                Find(x, y, false);
                num--;
                if (texto != null) texto.GetComponent<Text>().text = num.ToString();
            }

        }

    }

    /// <summary>
    /// Se llama cuando acaba la partida. El parametro win contiene si se ha ganado o no.
    /// </summary>
    /// <param name="win"></param>
    public void GameOver(bool win)
    {
        coche.transform.Find("Coche").gameObject.GetComponent<Coche>().OnPause();

        if (win) //Si se ha ganado se activa el panel de victoria y se dan las estrellas correspondientes según el consumo.
        {
            panelWin.gameObject.SetActive(true);
            int consumo = coche.transform.GetChild(3).GetComponent<Coche>().GetConsumoTotal();
            int numEstr = 0;
            if (consumo <= consumoIdeal + 10) numEstr = 3;
            else if (consumo <= consumoIdeal + 30) numEstr = 2;
            else if (consumo <= consumoIdeal + 50) numEstr = 1;

            for (int i = 0; i < numEstr; i++)
                panelWin.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    void actualizaTutorial()
    {
        switch (indTutorial)
        {
            case 0:
                coche.transform.Find("Coche").gameObject.GetComponent<Coche>().SwitchOff();
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
                coche.transform.Find("Coche").gameObject.GetComponent<Coche>().SwitchOff();
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
                break;

        }
        indTutorial++;
    }
    private void Update()
    {
        if (!coche.transform.Find("Coche").GetComponent<Coche>().IsMoving() && indTutorial == 0) actualizaTutorial();
        if (indTutorial != 5 && Input.GetMouseButtonDown(0)) actualizaTutorial();
        //if ((indTutorial == 1 || indTutorial >= 4) && Input.GetMouseButtonDown(0)) actualizaTutorial();
    }
}
