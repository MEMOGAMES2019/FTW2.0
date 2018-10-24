using UnityEngine;
using RAGE.Analytics;

/// <summary>
/// Script que contiene el comportamiento del coche.
/// </summary>
public class Coche : MonoBehaviour
{
    #region ATRIBUTOS

    /// <summary>
    /// Controla si el coche se está moviendo.
    /// </summary>
    bool moving = false;

    /// <summary>
    /// Controla si el coche se está activo
    /// </summary>
    bool sleep = false;

    /// <summary>
    /// Controla si el juego está pausado o no.
    /// </summary>
    bool paused = false;

    /// <summary>
    /// Controla la posición X del coche.
    /// </summary>
    float x = 0;

    /// <summary>
    /// Controla la posición Y del coche.
    /// </summary>
    float y = 0;

    /// <summary>
    /// Velocidad del coche.
    /// </summary>
    readonly float vel = 0.1f;

    /// <summary>
    /// Array que va a contener las direcciones al girar.
    /// </summary>
    readonly int[] incr = new int[4];

    /// <summary>
    /// Guarda la dirección a la que se mueve el coche.
    /// </summary>
    int dir = 0;

    /// <summary>
    /// Array que contiene las tres flechas de dirección.
    /// </summary>
    public GameObject[] flechas = new GameObject[3];

    /// <summary>
    /// Objeto coche.
    /// </summary>
    public GameObject player;

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
    public float consumo = 1f;

    /// <summary>
    /// Total de lo consumido.
    /// </summary>
    private float consumido;

    /// <summary>
    /// Referencia al GM.
    /// </summary>
    private GameObject GM;

    private int derecha = 0;
    private int izquierda = 0;
    private int recto = 0;

    #endregion

    void Start()
    {
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
        Arranca();

        GM = GameObject.Find("GM");
    }

    void Update()
    {
        if (moving && !sleep) Move();
    }

    /// <summary>
    /// Si se ha pulsado alguna flecha se comprueba cual es, se gira el coche 
    /// y se actualiza la dirección si es necesario y se llama a arrancar.
    /// </summary>
    public void OnClick(string s)
    {
        if (!paused && !sleep)
        {
            if (s == "Derecha")
            {
                player.transform.position = new Vector3(player.transform.position.x + x * 8, player.transform.position.y + y * 8, player.transform.position.z);
                player.gameObject.transform.Rotate(new Vector3(0, 0, -90));
                dir = (dir + 1) % 4;
                ++derecha;
                Tracker.T.setVar("Derecha", derecha);
            }
            else if (s == "Izquierda")
            {
                player.transform.position = new Vector3(player.transform.position.x + x * 8, player.transform.position.y + y * 8, player.transform.position.z);
                player.gameObject.transform.Rotate(new Vector3(0, 0, 90));
                dir = (dir - 1);
                if (dir < 0) dir = 3;
                ++izquierda;
                Tracker.T.setVar("Izquierda", izquierda);
            }
            else
            {
                ++recto;
                Tracker.T.setVar("Recto", recto);
            }
            Arranca();
        }
    }

    /// <summary>
    /// Actualiza la posición del coche según el giro y actualiza la información de las flechas.
    /// </summary>
    void Arranca()
    {
        x = incr[dir] * vel;
        y = incr[(dir + 1) % 4] * vel;
        moving = true;
        foreach (GameObject go in flechas)
        {
            go.transform.position = new Vector3(go.transform.position.x + 10, go.transform.position.y + 10, go.transform.position.z + 10);
            go.GetComponent<Flecha>().EstadoCoche(false);
            go.SetActive(false);
        }
    }

    float countdown = 0.25f;
    /// <summary>
    /// Método que actualiza la posición del coche y suma el consumo cada segundo.
    /// </summary>
    void Move()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0.0f)
        {
            countdown = 0.25f;
            consumido += consumo;
            SetPercentageOfEnergy(consumido);

            //Si el consumo llega al 100% se para de contar, 
            //se manda una traza con los datos y se termina la partida.
            if (consumido >= 100)
            {
                moving = false;
                GM.GetComponent<GM>().GameOver(false);
            }
        }
        player.transform.position = new Vector3(player.transform.position.x + x * (Time.deltaTime * 50), player.transform.position.y + y * (Time.deltaTime * 50), player.transform.position.z);
    }

    /// <summary>
    /// Este método es llamado al poner el juego en pausa
    /// </summary>
    public void OnPause()
    {
        paused = !paused;
    }

    /// <summary>
    /// Cuando el coche colisiona con un cruce o una intersección se llama a este método. 
    /// Al recibir la colisión, activa las flechas y las muestra.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        moving = false;
        foreach (GameObject go in flechas)
        {
            go.transform.position = new Vector3(go.transform.position.x - 10, go.transform.position.y - 10, go.transform.position.z - 10);
            go.GetComponent<Flecha>().EstadoCoche(true);
            go.GetComponent<Flecha>().DoStuff();
        }


    }
    public Posicion UltimaCasilla()
    {
        Posicion pos; pos.x = (int)transform.position.x - incr[dir];
        int resta = dir - 1;
        if (resta < 0) resta = 3;
        pos.y = (int)(transform.position.y * -1) - incr[resta];
        return pos;
    }

    /// <summary>
    /// Actualiza la barra de combustible según el consumo.
    /// </summary>
    public void SetPercentageOfEnergy(float newValue)
    {
        float x = (newValue * totalEnergy) / 100;
        float y = rt.localPosition.y;
        float z = rt.localPosition.z;

        rt.localPosition = new Vector3(-x, y, z);
    }
    public int GetConsumoTotal() { return (int)consumido; }
    public bool IsMoving() { return moving; }
    public void SwitchOff() { sleep = !sleep; }
}