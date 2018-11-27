using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileManager : MonoBehaviour
{

    public GameObject[] m_tilePrefabs;
    public GameObject m_player;
    private Transform m_playerTransform;
    private Queue<GameObject> m_tilePool;
    private Queue<GameObject> m_activeTiles;
    private float m_lastSpawnedTileZ = 0;
    private int m_tilePoolSize = 100;
    private int m_tilesOnScreen = 20;
    private float m_tileSize= 1;

	// Use this for initialization
	void Start ()
	{
        m_tilePool = new Queue<GameObject>();
        m_activeTiles = new Queue<GameObject>();

	    m_playerTransform = m_player.transform;

	    for (int i = 0; i < m_tilePoolSize; i++)
	    {

	        GameObject tile = Instantiate(m_tilePrefabs[i%2]);
	        tile.SetActive(false);
	        m_tilePool.Enqueue(tile);
	    }

	    for (int i = 0; i < m_tilesOnScreen; i++)
	    {
           m_activeTiles.Enqueue( SpawnTileFromPool( m_lastSpawnedTileZ + m_tileSize));
	    }
	}
	
	// Update is called once per frame
	void Update () {

	    if (m_activeTiles.Count <= 0)
	    {
	        Debug.LogWarning("No Active Tiles");
	        return;
	    }
        if (PlayerPassedTile(m_activeTiles.Peek()))
	    {
	        GameObject tile = m_activeTiles.Dequeue();
            DespawnTileToPool(tile);

	        m_activeTiles.Enqueue(SpawnTileFromPool(m_lastSpawnedTileZ + m_tileSize));
	    }

	}

    GameObject SpawnTileFromPool(float zPosition)
    {
        if (m_tilePool.Count <= 0)
        {
            Debug.LogWarning("TilePool is Empty");
            return null;
        }
        GameObject tile = m_tilePool.Dequeue();
        Vector3 tilePos = new Vector3(0f, 0f, zPosition);
        tile.transform.position = tilePos;
        tile.SetActive(true);
        m_lastSpawnedTileZ = zPosition;
        return tile;
    }

    bool PlayerPassedTile(GameObject tile)
    {
        //return player pos - tile pos > despawnbuffer
        return m_playerTransform.position.z - tile.transform.position.z > m_tileSize*2;
    }

    void DespawnTileToPool(GameObject tile)
    {
        tile.SetActive(false);
        m_tilePool.Enqueue(tile);
    }
}
