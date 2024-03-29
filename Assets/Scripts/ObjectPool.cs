﻿using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{

    private int m_size;
    private Queue<GameObject> m_pool;

    public ObjectPool(int size)
    {
        m_pool = new Queue<GameObject>(size);
        m_size = size;
    }

    public void PopulatePool(GameObject prefab)
    {
        for (int i = 0; i < m_size; i++)
        {

            GameObject tile = Object.Instantiate(prefab); 
            tile.SetActive(false);
            m_pool.Enqueue(tile);
        }

    }

    public void Push(GameObject gameObjectToPush)
    {
        gameObjectToPush.SetActive(false);
        m_pool.Enqueue(gameObjectToPush);
    }

    public GameObject SpawnObject(Vector3 spawnPosition, Quaternion spawnRotation = default(Quaternion))
    {
        if (m_pool.Count <= 0)
        {
            Debug.LogWarning("TilePool is Empty");
            return null;
        }
        GameObject obj = m_pool.Dequeue();
        obj.transform.position = spawnPosition;
        obj.transform.rotation = spawnRotation;
        obj.SetActive(true);
 
        return obj;
    }


}
