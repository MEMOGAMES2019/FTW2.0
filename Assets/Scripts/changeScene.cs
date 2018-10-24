using RAGE.Analytics;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
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
        Tracker.T.Accessible.Accessed(scene, AccessibleTracker.Accessible.Screen);
        transicion.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(scene);
    }

}