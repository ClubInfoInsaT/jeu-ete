using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnTrigger : MonoBehaviour
{
    public LayerMask groundMask;
    public GameObject pre, curr;
    private void Update()
    {
        Collider2D overGround = Physics2D.OverlapBox(gameObject.transform.position, new Vector2(1f, 14f), 0f, groundMask);
        if(overGround != null)
        {
            curr = overGround.gameObject;
            if(curr != pre)
            {
                Debug.Log("UPDATE CHUNK");
                FindObjectOfType<Generation>().SpawnRandomChunk();
            }
            else
            {
                pre = curr;
            }
        }
    }
}
