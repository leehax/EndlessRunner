using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : GenericSingleton<SceneLoader> {

    protected override void OnAwake()
    {
        m_persistent = true;
       
    }
    
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
        PlayerPrefs.Save();
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
   

    
}
