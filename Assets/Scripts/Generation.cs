/* Author: Valentin */
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generation : MonoBehaviour
{
	[Serializable]
	public struct Constraint
	{
		public GameObject chunk;
		// Ici on ne peut pas imposer une proba car le nombre de chunk peut changer. (Proba par defaut = 1/chunks.size())
		// On indique donc combien de fois il est plus ou moins probable d'apparaitre (2 fois plus, 1.5 fois plus, 0 fois plus ...)
		public float constraintCoefficient ;
	}
	private Dictionary<GameObject,List<KeyValuePair<GameObject,float>>> adjacencyMatrix = new();

	public List<GameObject> chunks = new();
	[Header("Chunks d'intro pour apprendre à jouer\n(par exemple un terrain plat)")] 
	public List<GameObject> tutorialChunks = new();
	public int terrainWindowSize; 
 
	//[SerializeField]
	private GameObject[] terrain; //Sliding window 
	//[SerializeField]
	private int currentChunkIndex = 0; // between 0 and TerrainWindowSize-1
	private Transform gridTrans;
	private GameObject newChunk;
	// Start is called before the first frame update
	void Start()
	{
		gridTrans = GameObject.FindGameObjectWithTag("Grid").transform;
		terrain = new GameObject[terrainWindowSize];

		
		//Setting up adjacency matrix
		// Iterating over each chunk to properly set up its probability vector

		foreach (GameObject chunkX in chunks)
		{
			Debug.Log(chunkX.name);
			// Part 1: Setting default coefficient to 1f (that will become 1/<number of chunks> right after) 
			List<KeyValuePair<GameObject, float>> adjacencyVector = new List<KeyValuePair<GameObject, float>>(chunks.Count);
			foreach (GameObject chunkY in chunks)
			{
				adjacencyVector.Add(new KeyValuePair<GameObject,float>(chunkY,1f));
			}
			
			// Part 2: Replacing exceptions to the default setting in the vector 
			float accumulatedConstraints = 0;
			int nbConstraints = chunkX.GetComponent<AdjacencyConstraint>().constraints.Count; 
			foreach (Constraint constraint in chunkX.GetComponent<AdjacencyConstraint>().constraints)
			{
				Debug.Log("computing excp prob for " + constraint.chunk.name); 
				int index = adjacencyVector.FindIndex(pair => pair.Key.Equals(constraint.chunk));
				float proba = constraint.constraintCoefficient / chunks.Count; 
				adjacencyVector[index] = new KeyValuePair<GameObject, float>(constraint.chunk,proba);
				Debug.Log("new prob (constraint) for "+adjacencyVector[index].Key+" : "+proba);
				accumulatedConstraints += proba; 
			}

			//Part 3: Computing actual probabilities completed by their accumulated predecessors (to deal with random chunk selection later)
			if (chunks.Count != nbConstraints)
			{
				float accumulator = 0f;
				for (int index = 0; index < chunks.Count; index++)
				{
					if (!chunkX.GetComponent<AdjacencyConstraint>().constraints //Default
						    .Exists(constraint => constraint.chunk.Equals(adjacencyVector[index].Key)))
					{
						accumulator += (1 - accumulatedConstraints) / (chunks.Count - nbConstraints);
						adjacencyVector[index] = new KeyValuePair<GameObject, float>
							(adjacencyVector[index].Key, accumulator);
					}
					else
					{
						accumulator += adjacencyVector[index].Value;
						adjacencyVector[index] = new KeyValuePair<GameObject, float>
							(adjacencyVector[index].Key, accumulator);
					}
						
				}
			}
			adjacencyMatrix.Add(chunkX,adjacencyVector);
		}
		
		//Generating initial chunks
		foreach (GameObject chunk in tutorialChunks)
		{
			SpawnChunk(chunk);
		}
		newChunk = chunks[Random.Range(0, chunks.Count)];
		for (int i = 0; i < terrainWindowSize-tutorialChunks.Count; i++)
		{
			Invoke("SpawnRandomChunk", 0.1f);
		}
	}

	// Instantiate a new random chunk to replace the useless one
	public void SpawnRandomChunk() 
	{
		float randomFloat = Random.Range(0f, 1f);
		// Iterating over the current chunk's probability vector (with accumulation of each predecessor)
		// The first correct threshold gives us the new chunk
		
		int newChunkIndex = adjacencyMatrix[newChunk].FindIndex(pair => randomFloat <= pair.Value);
		newChunk = adjacencyMatrix[newChunk][newChunkIndex].Key;
		SpawnChunk(newChunk);
		
	}

	// Instantiate a new chunk to replace the useless one
	private void SpawnChunk(GameObject newChunk)
	{
		// chunk instance position is next to the previous one (or 0 if first)
		float newPos = terrain[(currentChunkIndex + terrainWindowSize - 1) % terrainWindowSize] == null ? 
			0 : ( terrain[ (currentChunkIndex + terrainWindowSize - 1) % terrainWindowSize ].transform.position.x 
			      + terrain[ (currentChunkIndex + terrainWindowSize - 1) % terrainWindowSize ].transform.GetChild(0).
				      GetComponent<UnityEngine.Tilemaps.Tilemap>().size.x );
		
		//deleting previous chunk
		if(terrain[currentChunkIndex] != null)
			Destroy(terrain[currentChunkIndex]);
		// adding chunk to terrain window
		terrain[currentChunkIndex] = Instantiate(newChunk, new Vector3(newPos,0,0) , Quaternion.identity, gridTrans);
		
		//updating chunk index in the terrain window
		currentChunkIndex = (currentChunkIndex + 1) % terrainWindowSize; 

	}
	
}
