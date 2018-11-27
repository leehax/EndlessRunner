using System.Collections.Generic;
using UnityEngine;

public class EndlessTileSpawner : MonoBehaviour
{

    public GameObject[] m_tilePrefabs;
    public GameObject m_player;
    private Transform m_playerTransform;
    private ObjectPool m_tilePool;
    private Queue<GameObject> m_activeTiles;

    private float m_lastSpawnedTileZ = -1f;
    private int m_tilePoolSize = 40;
    private int m_maxTilesOnScreen = 20;
    private float m_tileSize = 1;

    void Start()
    {
        m_tilePool = new ObjectPool(m_tilePoolSize);
        m_activeTiles = new Queue<GameObject>(m_maxTilesOnScreen);

        m_playerTransform = m_player.transform;

        m_tilePool.PopulatePool(m_tilePrefabs);

        for (int i = 0; i < m_maxTilesOnScreen; i++)
        {

            m_activeTiles.Enqueue(m_tilePool.SpawnObject(new Vector3(0, 0, m_lastSpawnedTileZ + m_tileSize)));
            m_lastSpawnedTileZ += m_tileSize;
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
            GameObject tile = m_activeTiles.Dequeue();
            m_tilePool.Push(tile);

            m_activeTiles.Enqueue(m_tilePool.SpawnObject(new Vector3(0, 0, m_lastSpawnedTileZ + m_tileSize)));
            m_lastSpawnedTileZ += m_tileSize;
        }

    }

    bool PlayerPassedTile(GameObject tile)
    {

        return m_playerTransform.position.z - tile.transform.position.z > m_tileSize * 2;
    }

}
