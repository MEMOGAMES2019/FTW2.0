using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour
{
    public Animator transicion;
    public void Change(string scene)
    {
        if (scene != "exit")
            StartCoroutine(LoadScene(scene));
        else
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    IEnumerator LoadScene(string scene)
    {
        transicion.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(scene);
    }

}