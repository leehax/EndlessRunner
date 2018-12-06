using System.Collections.Generic;
using UnityEngine;


public class EndlessTileSpawner : MonoBehaviour
{

    public GameObject RowPrefab;
    public GameObject Player;

    private Transform m_playerTransform;
    private ObjectPool m_tilePool;
    private GameObject[] m_objectPool;
    private Queue<GameObject> m_activeRows;

    private float m_lastSpawnedTileZ;
    private int m_tilePoolSize = 40;
    private int m_maxTilesOnScreen = 20;
    private TileRow.RowTypes m_lastRowType;

    public AnimationCurve m_probabilityCurve;

    void Awake()
    {
        m_tilePool = new ObjectPool(m_tilePoolSize);
        m_activeRows = new Queue<GameObject>(m_maxTilesOnScreen);

        m_playerTransform = Player.transform;

        m_tilePool.PopulatePool(RowPrefab);

        
        for (int i = 0; i < m_maxTilesOnScreen; i++)
        {


            GameObject row = m_tilePool.SpawnObject(new Vector3(0, 0,
                m_lastSpawnedTileZ + GameSettings.Instance().DistanceBetweenPlatforms()));

            //force left or right after a wall, otherwise random
            m_lastRowType = m_lastRowType == TileRow.RowTypes.EdgeTilesWithWall ? row.GetComponent<TileRow>().ActivateLeftOrRight() : row.GetComponent<TileRow>().ActivateRandomType(m_probabilityCurve);

            m_activeRows.Enqueue(row);
            m_lastSpawnedTileZ += GameSettings.Instance().DistanceBetweenPlatforms();


        }
    }

    void Update()
    {
        if (PlayerPassedTile(m_activeRows.Peek()))
        {

            m_tilePool.Push(m_activeRows.Dequeue());

            GameObject row = m_tilePool.SpawnObject(new Vector3(0, 0,
                m_lastSpawnedTileZ + GameSettings.Instance().DistanceBetweenPlatforms()));

            m_lastRowType = m_lastRowType == TileRow.RowTypes.EdgeTilesWithWall ? row.GetComponent<TileRow>().ActivateLeftOrRight() : row.GetComponent<TileRow>().ActivateRandomType(m_probabilityCurve);

            m_activeRows.Enqueue(row);
            m_lastSpawnedTileZ += GameSettings.Instance().DistanceBetweenPlatforms();

        }
    }

    bool PlayerPassedTile(GameObject tile)
    {

        return m_playerTransform.position.z - tile.transform.position.z > GameSettings.Instance().DistanceBetweenPlatforms();
    }


}
