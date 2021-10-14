using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public enum TYPE
    {
        NONE = -1,
        FLOOR = 0,
        HOLE,
        NUM,
    };
};

public class level_control : MonoBehaviour
{
    public struct CreationInfo
    {
        public Block.TYPE block_type;
        public int max_count;
        public int height;
        public int current_count;
    };

    public CreationInfo previous_block;
    public CreationInfo current_block;
    public CreationInfo next_block;

    public int level = 0;

    private void ClearNextBlock(ref CreationInfo block)
    {
        block.block_type = Block.TYPE.FLOOR;
        block.max_count = 15;
        block.height = 0;
        block.current_count = 0;
    }

    public void Initialize()
    {
        ClearNextBlock(ref previous_block);
        ClearNextBlock(ref current_block);
        ClearNextBlock(ref next_block);
    }

    private void UpdateLevel(ref CreationInfo current, CreationInfo previous)
    {
        switch (previous.block_type)
        {
            case Block.TYPE.FLOOR: // 블록
                if (player_control.playTime / 15 >= 0.0f && player_control.playTime / 15 < 2.0f) // playtime이 0초~14초일 때
                {
                    current.block_type = Block.TYPE.HOLE;
                    current.max_count = Random.Range(1, 3);
                }
                else if (player_control.playTime / 15 >= 2.0f && player_control.playTime / 15 < 4.0f) // 15초 ~ 29초일 때
                {
                    current.block_type = Block.TYPE.HOLE;
                    current.max_count = Random.Range(2, 4);
                }
                else if (player_control.playTime / 15 >= 4.0f && player_control.playTime / 15 < 5.0f) // 60초 ~ 74초일 때
                {
                    current.block_type = Block.TYPE.HOLE;
                    current.max_count = Random.Range(1, 3);
                }

                /*
                else if (player_control.playTime / 15 >= 4.0f && player_control.playTime / 15 < 5.0f) // 60초 ~ 74초일 때
                {
                    current.max_count = Random.Range(1, 3); // 발판 길이 9~10개 중 랜덤 생성
                    current.block_type = Block.TYPE.HOLE;
                    current.height = previous.height;
                }
                */
                /* else if (player_control.playTime / 15 >= 5.0f) // 75초 ~ 일 때 처음부터 15초마다 반복.
                 {
                     player_control.playTime = 0.0f; // 플레이 시간 0초로 초기화
                 }*/
                break;
                /*
                if (player_control.playTime / 15 == 0.0f || player_control.playTime / 15 == 1.0f || player_control.playTime / 15 == 4.0f)
                {
                    current.max_count = Random.Range(1, 3); // 발판 길이 9~10개 중 랜덤 생성
                    current.block_type = Block.TYPE.HOLE;
                    current.height = previous.height;
           
                }
                if (player_control.playTime / 15 == 2.0f || player_control.playTime / 15 == 3.0f)
                {
                    current.max_count = Random.Range(2, 4); // 발판 길이 8~9개 중 랜덤 생성
                    current.block_type = Block.TYPE.HOLE;
                    current.height = previous.height;
                }
                break;
                */

            case Block.TYPE.HOLE: // 구멍
                
                if (player_control.playTime > 75.0f) // 75초 ~ 일 때 처음부터 15초마다 반복.
                {
                    player_control.playTime = 0.0f; // 플레이 시간 0초로 초기화
                    player_control.playTime += Time.deltaTime;
                }
                

                if (player_control.playTime / 15 >= 0.0f && player_control.playTime / 15 < 1.0f ) // playtime이 0초~14초일 때
                {
                    current.block_type = Block.TYPE.FLOOR;
                    current.max_count = Random.Range(9, 11);
                }
                else if (player_control.playTime / 15 >= 1.0f && player_control.playTime / 15 < 2.0f) // 15초 ~ 29초일 때
                {
                    current.block_type = Block.TYPE.FLOOR;
                    current.max_count = Random.Range(8, 10);
                }
                else if (player_control.playTime / 15 >= 2.0f && player_control.playTime / 15 < 3.0f) // 30초 ~ 44초일 때
                {
                    current.block_type = Block.TYPE.FLOOR;
                    current.max_count = Random.Range(7, 9);
                }
                else if (player_control.playTime / 15 >= 3.0f && player_control.playTime / 15 < 4.0f) // 45초 ~ 59초일 때
                {
                    current.block_type = Block.TYPE.FLOOR;
                    current.max_count = Random.Range(6, 8);
                }
                else if (player_control.playTime / 15 >= 4.0f && player_control.playTime / 15 < 5.0f) // 60초 ~ 74초일 때
                {
                    current.block_type = Block.TYPE.FLOOR;
                    current.max_count = Random.Range(5, 7);
                }

                // 시간에 따른 블럭의 높이 변경
                if (player_control.playTime / 15 >= 0.0f && player_control.playTime / 15 < 1.0f) // playtime이 0초~14초일 때
                {
                    current.height = 0;
                }
                else if (player_control.playTime / 15 >= 1.0f && player_control.playTime / 15 < 2.0f) // 15초 ~ 29초일 때
                {
                    current.height = Random.Range(-1, 2);
                }
                else if (player_control.playTime / 15 >= 2.0f && player_control.playTime / 15 < 4.0f) // 30초 ~ 44초일 때
                {
                    current.height = Random.Range(-2, 3) ;
                }
              
                else if (player_control.playTime / 15 >= 4.0f && player_control.playTime / 15 < 5.0f) // 60초 ~ 74초일 때
                {
                    current.height = Random.Range(-1, 2);
                }

                break;
        }
 
    }
    public void UpdateStatus()
    {
        current_block.current_count++;

        if(current_block.current_count >= current_block.max_count)
        {
            previous_block = current_block;
            current_block = next_block;

            ClearNextBlock(ref next_block);
            UpdateLevel(ref next_block, current_block);
        }
    }
}
