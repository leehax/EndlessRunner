using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EndlessTileSpawner : MonoBehaviour
{

    public GameObject m_rowPrefab;
    public GameObject m_player;
    private Transform m_playerTransform;
    private ObjectPool m_tilePool;
    private GameObject[] m_objectPool;
    private Queue<GameObject> m_activeTiles;

    private float m_lastSpawnedTileZ;
    private int m_tilePoolSize = 40;
    private int m_maxTilesOnScreen = 20;
    private float m_tileSize = 1;

    private float m_lastXpos;
    private float m_allowedXDeltaBetweenTiles = 1.5f;

    void Start()
    {
        m_tilePool = new ObjectPool(m_tilePoolSize);
        m_activeTiles = new Queue<GameObject>(m_maxTilesOnScreen);

        m_playerTransform = m_player.transform;

        m_tilePool.PopulatePool(m_rowPrefab);

        
        for (int i = 0; i < m_maxTilesOnScreen; i++)
        {

            m_activeTiles.Enqueue(m_tilePool.SpawnObject(new Vector3(0, 0,
                m_lastSpawnedTileZ + m_tileSize * 4)));
            m_lastSpawnedTileZ += m_tileSize * 4;


        }
    }

    void Update()
    {

       if (m_activeTiles.Count <= 0)
       {
           Debug.LogWarning("No Active Tiles");
           return;
       }

       if (PlayerPassedTile(m_activeTiles.Peek()))
       {
          
           m_tilePool.Push(m_activeTiles.Dequeue());
      
           m_activeTiles.Enqueue(m_tilePool.SpawnObject(new Vector3(0, 0,
               m_lastSpawnedTileZ + m_tileSize * 4)));
           m_lastSpawnedTileZ += m_tileSize * 4;
        }

    }

    bool PlayerPassedTile(GameObject tile)
    {

        return m_playerTransform.position.z - tile.transform.position.z > m_tileSize * 2;
    }

    private float CalculateRandomXPosition(float min, float max)
    {

        float generatedX = Random.Range(min, max);

        ////clamp the deviance from the previous random X
        //generatedX = Mathf.Clamp(generatedX, m_lastXpos - m_allowedXDeltaBetweenTiles,
        //    m_lastXpos + m_allowedXDeltaBetweenTiles);

        m_lastXpos = generatedX;
        return generatedX;
    }

}
