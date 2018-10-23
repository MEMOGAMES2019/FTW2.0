using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Este script controla cuando el coche atraviesa la meta y avisa al GM.
/// </summary>
public class TriggerMeta : MonoBehaviour
{

    GameObject GM;
    void Start()
    {
        GM = GameObject.Find("GM");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (SceneManager.GetActiveScene().name != "Tutorial")
            GM.GetComponent<GM>().GameOver(true);
        else
            GM.GetComponent<GMTutorial>().GameOver(true);
    }
}