using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class changeScene : MonoBehaviour
{
    public Animator transicion;
    public void ChangeScene(string scene)
    {
        if (scene != "exit")
            StartCoroutine(loadScene(scene));
        else
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    IEnumerator loadScene(string scene)
    {
        transicion.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(scene);
    }

}