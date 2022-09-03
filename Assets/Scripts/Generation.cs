using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generation : MonoBehaviour
{
    public GameObject[] chunks;
    public int TerrainWindowSize; 
 
    //[SerializeField]
    private GameObject[] Terrain; //Sliding window 
    //[SerializeField]
    private int CurrentChunkIndex = 0; // between 0 and TerrainWindowSize-1
	private Transform gridTrans;
    private GameObject newChunk;
    // Start is called before the first frame update
    void Start()
    {
        gridTrans = GameObject.FindGameObjectWithTag("Grid").transform;
        Terrain = new GameObject[TerrainWindowSize];
        //Generating initial chunks
        for (int i = 0; i < TerrainWindowSize; i++)
        {
            Invoke("SpawnChunk", 0.1f);
        }

    }

    // Instantiate a new chunk to replace the useless one
    public void SpawnChunk() 
    {
        // randomly selecting a chunk in the chunk list
        // (no proximity constraint for now)
        newChunk = chunks[Random.Range(0, chunks.Length)];
        // chunk instance position is next to the previous one (or 0 if first)
        //Debug.Log("prev index: "+(CurrentChunkIndex + TerrainWindowSize - 1) % 10);
        float newPos = Terrain[(CurrentChunkIndex + TerrainWindowSize - 1) % 10] == null ? 0 : ( Terrain[ (CurrentChunkIndex + TerrainWindowSize - 1) % 10 ].transform.position.x + Terrain[ (CurrentChunkIndex + TerrainWindowSize - 1) % 10 ].transform.GetChild(0).GetComponent<UnityEngine.Tilemaps.Tilemap>().size.x );
        //deleting previous chunk
        if(Terrain[CurrentChunkIndex] != null)
            Destroy(Terrain[CurrentChunkIndex]);
        // adding chunk to terrain window
        Terrain[CurrentChunkIndex] = Instantiate(newChunk, new Vector3(newPos,0,0) , Quaternion.identity, gridTrans);
        //Debug.Log("newChunk = "+ newChunk+" newPos = "+newPos+" CurrentChunkIndex = "+CurrentChunkIndex);

        //updating chunk index in the terrain window
        CurrentChunkIndex = (CurrentChunkIndex + 1) % 10; 
        
    }
}
