using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileWorld;
using TileWorld.Events;
using System;

public class TestTileCreator : MonoBehaviour
{
    [Range(5, 15)]
    public int BirthCount = 5;
    public string names = "";
    public GameObject m_SpawnPrefab;
    public TileWorldObjectScatter m_TilerWorldObjectsSca;
    public TileWorldCreator m_TileWorld;
    private List<GameObject> m_NoOccupyList = new List<GameObject>();
    private List<GameObject> m_OccupyList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        m_TileWorld.GenerateAndBuild(false);
        m_TilerWorldObjectsSca.ScatterPositionBasedObjects();
        m_TilerWorldObjectsSca.ScatterProceduralObjects();
    }
    private void OnEnable()
    {
        TileWorldEvents.OnBuildComplete += MapOnBuildComplete;

    }

      private void OnDisable()
    {
        TileWorldEvents.OnBuildComplete -= MapOnBuildComplete;
    }
    private void MapOnBuildComplete()
    {
        for (int i = 0; i < m_TileWorld.configuration.worldMap[0].tileObjects.GetLength(0); i++)
        {
            for (int j = 0; j < m_TileWorld.configuration.worldMap[0].tileObjects.GetLength(1); j++)
            {
                // Debug.LogError(
                //  m_TileWorld.configuration.worldMap[0].tileObjects[i, j].gameObject.name);
                if (m_TileWorld.configuration.worldMap[0].tileObjects[i, j] != null &&
                    m_TileWorld.configuration.worldMap[0].tileTypes[i, j] ==
                    TileWorldConfiguration.TileInformation.TileTypes.block &&
                    !m_TilerWorldObjectsSca.OccupyMap[i, j])
                {
                    m_NoOccupyList.Add(
                        m_TileWorld.configuration.worldMap[0].tileObjects[i, j].gameObject);
                }
            }
        }
        GenerateRandomObjects(BirthCount,names,m_SpawnPrefab);

    }

    private void GenerateRandomObjects(int BirthCount,string name,GameObject m_SpawnPrefab)
    {
        for (int i = 0; i < BirthCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, m_NoOccupyList.Count);
            GameObject obj = m_NoOccupyList[randomIndex];
            GameObject g = Instantiate(m_SpawnPrefab);
            g.name = name+ i;
            g.transform.position = new Vector3(obj.transform.position.x,
                obj.transform.position.y+0.5f, obj.transform.position.z);
            g.transform.localRotation = Quaternion.identity;
            g.transform.localScale = Vector3.one;
            m_NoOccupyList.Remove(obj);
            m_OccupyList.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
