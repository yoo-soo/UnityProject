using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map_creator : MonoBehaviour
{
    public static float BLOCK_WIDTH = 1.0f; // 블럭 너비값
    public static float BLOCK_HEIGHT = 0.2f; // 블럭 높이값
    public static int BLOCK_NUM_IN_SCREEN = 24; // 한 화면에 블럭이 나타나야하는 총 개수

    private level_control lev_ctrl = null;

    private struct FloorBlock
    // 특정한 위치를 지정해놓고 그 위치에 블럭이 생성 유무를 판단할 변수만 간직함.
    {
        public bool is_created;
        public Vector3 position;
    };

    private FloorBlock last_block; // 마지막에 생성한 블록의 정보를 기록할 변수
    private player_control player = null;
    private block_creator block;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<player_control>();
        last_block.is_created = false; // 생성된 블럭 x  기본값 false.
        block = gameObject.GetComponent<block_creator>(); 

        lev_ctrl = new level_control();
        lev_ctrl.Initialize();
    }

    public void CreateFloorBlock()
    {
        Vector3 block_position;

        //false가 되는 것은 처음 게임을 실행했을 때 뿐임.
        if (!last_block.is_created) // 마지막 블럭 생성된 것이 없으면 블럭 포지션을 플레이어 위치에 생성
        {
            block_position = player.transform.position;
            block_position.x -= BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f); 
            block_position.y = 0.0f;
        }
        else // 마지막 블럭이 생성이 되어있으면 통과
        {
            block_position = last_block.position; // 최초의 블럭 위치
        }

        block_position.x += BLOCK_WIDTH;

        //block.CreateBlock(block_position);
        lev_ctrl.UpdateStatus();

        block_position.y = lev_ctrl.current_block.height * BLOCK_HEIGHT;
        level_control.CreationInfo current = lev_ctrl.current_block;

        if (current.block_type == Block.TYPE.FLOOR)
            block.CreateBlock(block_position);

        last_block.position = block_position;
        last_block.is_created = true;

    }

    public bool IsGone(GameObject block_object)
    {
        bool result = false;
        // 플레이어의 위치로부터 완쪽으로12.5개의 블럭위치만큼 .
        float left_limit = player.transform.position.x - BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);

        if (block_object.transform.position.x < left_limit)
            result = true; // left limit보다 더 왼쪽으로 위치할 시 true값을 내보냄

        return result;
    }

    // Update is called once per frame
    void Update()
    {
        float block_generate_x = player.transform.position.x; // 플레이어의 위치 값

        // 블럭 너비 * 12.5, 현재 플레이어의 위치를 중심으로 블럭 12.5개 만큼의 위치에다 블럭 생성.
        block_generate_x += BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN + 1) / 2.0f;

        while(last_block.position.x < block_generate_x) 
            // 마지막에 생성된 블럭 위치가 generate block보다 작으면 creat block.
            // 새로 생성할 블럭의 위치가 마지막으로 생성된 블럭의 위치보다 커졌으므로 
        {
            CreateFloorBlock(); // 새로운 블럭 생성.
        }
    }
}
