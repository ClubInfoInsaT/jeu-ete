using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{


	private ChunkTemplate templates;
	private int rand;

    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Chunks").GetComponent<ChunkTemplate>();
		Invoke("Spawn", 0.1f);
    }

    void Spawn()
    {

        rand = Random.Range(0, templates.chunks.Length);
        Instantiate(templates.chunks[rand], transform.position, Quaternion.identity);

    }
}
