using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block_creator : MonoBehaviour
{
    public GameObject[] block_prefabs; // 배열형태로 생성.
    private int block_count = 0; // 블럭을 만든 개수 저장.

    // 어느 위치에 블럭을 생성할 것인지 매개변수에 작성.
    public void CreateBlock(Vector3 block_position)
    {
        int next_block_type = block_count % block_prefabs.Length;
        // 0일 때는 0번 블록, 1일 때는 1번 블록 생성.

        GameObject game_object = GameObject.Instantiate(block_prefabs[next_block_type]) as GameObject;
        game_object.transform.position = block_position; // 생성된 블럭의 위치를 매개변수 블럭위치로 
        block_count++; 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
