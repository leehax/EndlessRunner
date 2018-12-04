using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class GameSettings : MonoBehaviour
{

//todo: make a generic singleton class from which GameSettings derives from
#region singleton
    private static GameSettings m_instance;

    public static GameSettings Instance()
    {
        if (m_instance == null)
        {
            if ((m_instance = FindObjectOfType<GameSettings>()) == null)
            {
                GameObject obj = new GameObject();
                obj.name = "GameSettings";
                m_instance = obj.AddComponent<GameSettings>();
                DontDestroyOnLoad(obj);
            }
            
        }
        return m_instance;
    }

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        m_instance = this;
        DontDestroyOnLoad(gameObject);
    }
#endregion



    private float m_distanceBetweenPlatforms = 32f;

    public float DistanceBetweenPlatforms()
    {
        return m_distanceBetweenPlatforms;
    }

    private int m_score = 0;


    public int Score()
    {
        return m_score;
    }

    public void AddScore(int amount)
    {
        m_score += amount;
    }








}