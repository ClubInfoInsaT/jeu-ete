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
	
		
	[Serializable]
	public struct BiomeChunkList
	{
		public List<GameObject> chunks;
	}
	public List<BiomeChunkList> BiomeChunkLists = new() ;
	private Dictionary<GameObject, List<KeyValuePair<GameObject, float>>>[] BiomeAdjacencyMatrices ; 
	
	[Header("Tutorial intro chunks\n(a flat terrain for example)")] 
	public List<GameObject> tutorialChunks = new();
	[Header("Size of the sliding window (terrain lenght)")]
	public int terrainWindowSize; 
 
	//[SerializeField]
	private GameObject[] terrain; //Sliding window 
	//[SerializeField]
	private int currentChunkIndex = 0; // between 0 and TerrainWindowSize-1
	private Transform gridTrans;
	private GameObject newChunk;

	private List<GameObject> currentChunkList = new(); 
	private Dictionary<GameObject, List<KeyValuePair<GameObject, float>>> currentAdjacencyMatrix = new();

	private int currentBiomeIndex = 0;

	private bool isNextBiome = false;
	// Start is called before the first frame update
	void Start()
	{
		//Subscribing to nextBiomeEvent 
		FindObjectOfType<ScoreManager>().nextBiomeEvent += NextBiome;  
		gridTrans = GameObject.FindGameObjectWithTag("Grid").transform;
		terrain = new GameObject[terrainWindowSize];
		BiomeAdjacencyMatrices =
			new Dictionary<GameObject, List<KeyValuePair<GameObject, float>>>[BiomeChunkLists.Count]; 
		// Iterating over each biome chunk lists to set up each adjacency matrices
		for (int biomeIndex = 0 ; biomeIndex < BiomeChunkLists.Count ; biomeIndex++)
		{
			BiomeAdjacencyMatrices[biomeIndex] = new(); 
			//Setting up adjacency matrix
			// Iterating over each chunk to properly set up its probability vector
			foreach (GameObject chunkX in BiomeChunkLists[biomeIndex].chunks)
			{
				//Debug.Log(chunkX.name);
				// Part 1: Setting default coefficient to 1f (that will become 1/<number of chunks> right after) 
				List<KeyValuePair<GameObject, float>> adjacencyVector =
					new List<KeyValuePair<GameObject, float>>(BiomeChunkLists[biomeIndex].chunks.Count);
				
				foreach (GameObject chunkY in BiomeChunkLists[biomeIndex].chunks)
				{
					adjacencyVector.Add(new KeyValuePair<GameObject, float>(chunkY, 1f));
				}

				// Part 2: Replacing exceptions to the default setting in the vector 
				float accumulatedConstraints = 0;
				int nbConstraints = chunkX.GetComponent<AdjacencyConstraint>().constraints.Count;
				foreach (Constraint constraint in chunkX.GetComponent<AdjacencyConstraint>().constraints)
				{
					int index = adjacencyVector.FindIndex(pair => pair.Key.Equals(constraint.chunk));
					if (index != -1)
					{
						float proba = constraint.constraintCoefficient / BiomeChunkLists[biomeIndex].chunks.Count;
						//Debug.Log(index + "   " + adjacencyVector.Count);
						adjacencyVector[index] = new KeyValuePair<GameObject, float>(constraint.chunk, proba);
						//Debug.Log("new prob (constraint) for " + adjacencyVector[index].Key + " : " + proba);
						accumulatedConstraints += proba;
					}
					else
					{
					//	Debug.Log("Couldn't find "+constraint.chunk.name+" in biome chunk list. Replaced with default probability.");
					}
				}

				//Part 3: Computing actual probabilities completed by their accumulated predecessors (to deal with random chunk selection later)
				if (BiomeChunkLists[biomeIndex].chunks.Count != nbConstraints)
				{
					float accumulator = 0f;
					for (int index = 0; index < BiomeChunkLists[biomeIndex].chunks.Count; index++)
					{
						if (!chunkX.GetComponent<AdjacencyConstraint>().constraints //Default
							    .Exists(constraint => constraint.chunk.Equals(adjacencyVector[index].Key)))
						{
							accumulator += (1 - accumulatedConstraints) / (BiomeChunkLists[biomeIndex].chunks.Count - nbConstraints);
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

			//	Debug.Log(chunkX + "    " + adjacencyVector); 
				BiomeAdjacencyMatrices[biomeIndex].Add(chunkX, adjacencyVector);
			}
			
		}
		//Initiating first biome
		currentChunkList = BiomeChunkLists[currentBiomeIndex].chunks ;
		currentAdjacencyMatrix = BiomeAdjacencyMatrices[currentBiomeIndex] ;
		
		//Generating initial chunks
		foreach (GameObject chunk in tutorialChunks)
		{
			SpawnChunk(chunk);
		}
		newChunk = currentChunkList[Random.Range(0, currentChunkList.Count)];
		for (int i = 0; i < terrainWindowSize - tutorialChunks.Count; i++)
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
		int newChunkIndex = currentAdjacencyMatrix[newChunk].FindIndex(pair => randomFloat <= pair.Value);
		newChunk = currentAdjacencyMatrix[newChunk][newChunkIndex].Key;
		if (isNextBiome)
		{
			isNextBiome = false;
			//randomize biome sequence in a future version?
			currentBiomeIndex = (currentBiomeIndex == BiomeChunkLists.Count - 1 ? 0 : currentBiomeIndex + 1);
			//Next biome
			currentChunkList = BiomeChunkLists[currentBiomeIndex].chunks;
			currentAdjacencyMatrix = BiomeAdjacencyMatrices[currentBiomeIndex];
			newChunk = currentChunkList[Random.Range(0, currentChunkList.Count)];
			
		}

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

	public void NextBiome(ScoreManager t)
	{
		isNextBiome = true;
	}

	public GameObject[] getTerrain()
	{
		return terrain; 
	}
	
	
}
