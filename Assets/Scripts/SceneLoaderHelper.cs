using UnityEngine;

public class SceneLoaderHelper : MonoBehaviour {

    public void LoadByIndex(int index)
    {
        SceneLoader.Instance().LoadByIndex(index);
    }

    public void LoadByName(string name)
    {
        SceneLoader.Instance().LoadByName(name);
    }

    public void Quit()
    {
        SceneLoader.Instance().Quit();
    }
}
