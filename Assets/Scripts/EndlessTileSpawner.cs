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
    private Queue<GameObject> m_activeRows;

    private float m_lastSpawnedTileZ;
    private int m_tilePoolSize = 40;
    private int m_maxTilesOnScreen = 20;
    private float m_tileSize = 1f;
    private TileRow.RowTypes m_lastRowType;

    public AnimationCurve m_probabilityCurve;

    void Start()
    {
        m_tilePool = new ObjectPool(m_tilePoolSize);
        m_activeRows = new Queue<GameObject>(m_maxTilesOnScreen);

        m_playerTransform = m_player.transform;

        m_tilePool.PopulatePool(m_rowPrefab);

        
        for (int i = 0; i < m_maxTilesOnScreen; i++)
        {


            GameObject row = m_tilePool.SpawnObject(new Vector3(0, 0,
                m_lastSpawnedTileZ + m_tileSize * 4));


            row.GetComponent<TileRow>().m_prevType = m_lastRowType.ToString();
            if (m_lastRowType == TileRow.RowTypes.EdgeTilesWithWall)
            {
                //Force left or right single tile after a wall
                m_lastRowType = row.GetComponent<TileRow>().ActivateLeftOrRight();
            }
            else
            {
                m_lastRowType = row.GetComponent<TileRow>().ActivateRandomType(m_probabilityCurve);
            }

            m_activeRows.Enqueue(row);
            m_lastSpawnedTileZ += m_tileSize * 4;


        }
    }

    void Update()
    {

        if (m_activeRows.Count <= 0)
        {
            Debug.LogWarning("No Active Tiles");
            return;
        }

        if (PlayerPassedTile(m_activeRows.Peek()))
        {

            m_tilePool.Push(m_activeRows.Dequeue());

            GameObject row = m_tilePool.SpawnObject(new Vector3(0, 0,
                m_lastSpawnedTileZ + m_tileSize * 4));

            row.GetComponent<TileRow>().m_prevType = m_lastRowType.ToString();
            if (m_lastRowType == TileRow.RowTypes.EdgeTilesWithWall)
            {
                //Force left or right single tile after a wall
                m_lastRowType = row.GetComponent<TileRow>().ActivateLeftOrRight();
            }
            else
            {
                m_lastRowType = row.GetComponent<TileRow>().ActivateRandomType(m_probabilityCurve);
            }

            m_activeRows.Enqueue(row);
            m_lastSpawnedTileZ += m_tileSize * 4;

        }
    }

    bool PlayerPassedTile(GameObject tile)
    {

        return m_playerTransform.position.z - tile.transform.position.z > m_tileSize * 2;
    }


}
