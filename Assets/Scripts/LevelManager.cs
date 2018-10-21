using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Niveles del juego.
    /// </summary>
    public List<GameObject> niveles;

    /// <summary>
    /// Conjuntos de mapas de los niveles
    /// </summary>
    public List<GameObject> mapas;

    /// <summary>
    /// Nivel.
    /// </summary>
    public int level;

    void Start()
    {
        int index = 0;
        foreach (GameObject nivel in niveles)
        {
            if (index == 0)
                nivel.gameObject.GetComponent<Button>().interactable = true;
            else
            {
                /* Cogemos los mapas del nivel anterior donde se han superado con al menos 2 estrellas. Ejemplo: Nivel1 */
                string mapa2Star = string.Concat("Nivel", index);

                /* Comprobamos las condiciones para desbloquear el nivel */
                bool desbloqueo = PlayerPrefs.HasKey(mapa2Star) && PlayerPrefs.GetInt(mapa2Star) >= 2;

                nivel.gameObject.GetComponent<Button>().interactable = desbloqueo;
                nivel.transform.Find("Block").gameObject.SetActive(!desbloqueo);
            }
            ++index;
        }

        /* Recorremos los conjuntos de mapas de los diferentes niveles */
        foreach (GameObject cjtoMapa in mapas)
        {
            /* Recorremos cada mapa (cada botón) */
            int numMapa = 1;
            string nivel = string.Concat("Nivel", level);
            int numNivelesPasados = 0;

            foreach (Button mapa in cjtoMapa.transform.GetComponentsInChildren<Button>())
            {
                /* Nombre del mapa. Ejemplo: N1mapa1 */
                string s = string.Concat("N", level, "mapa", numMapa);
                int m = PlayerPrefs.HasKey(s) ? PlayerPrefs.GetInt(s) : 0;
                if (m >= 2)
                {
                    ++numNivelesPasados;
                }

                /* Recorremos todas las estrellas conseguidas en ese mapa.
                 * Empezamos por 1 ya que el primer hijo es el texto */
                for (int j = 1; j <= m; ++j)
                {
                    mapa.transform.GetChild(j).transform.GetChild(0).gameObject.SetActive(false);
                }
                ++numMapa;
            }

            PlayerPrefs.SetInt(nivel, numNivelesPasados);
        }
    }
}
