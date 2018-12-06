using System.Collections;
using UnityEngine;


public class LoseScreen : MonoBehaviour
{
    [SerializeField] private GameObject m_loseScreen;

 

    private void Awake()
    {
        GameSettings.OnGameEnd += Activate;
        m_loseScreen.SetActive(false);
    }

    private void OnDestroy()
    {
        GameSettings.OnGameEnd -= Activate; 
    }


    private void Activate()
    {
        StartCoroutine(ActivateWithDelay());
    }

    private IEnumerator ActivateWithDelay()
    {
        yield return new WaitForSeconds(1);
        m_loseScreen.SetActive(true);
    }
  
}
