using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    public void LoadByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadByName(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
