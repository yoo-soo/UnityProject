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
            case Block.TYPE.FLOOR:
                current.block_type = Block.TYPE.HOLE;
                current.max_count = 5;
                current.height = previous.height;
                break;

            case Block.TYPE.HOLE:
                current.block_type = Block.TYPE.FLOOR;
                current.max_count = 10;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
