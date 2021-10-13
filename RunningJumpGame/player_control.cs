using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_control : MonoBehaviour
{
    public static float ACCELERATION = 10.0f;
    public static float SPEED_MIN = 4.0f;
    public static float SPEED_MAX = 8.0f;
    public static float JUMP_HEIGHT_MAX = 3.0f;
    public static float JUMP_POWER_REDUCE = 0.5f;
    public static float FAIL_LIMIT = -5.0f;

    public enum STEP
    {
        NONE = -1,
        RUN = 0,
        JUMP,
        MISS,
        NUM, // 열겨형 함수에 몇 개의 상태가 있는 지 알 수 있다.
    };

    public STEP step = STEP.NONE;
    public STEP next_step = STEP.NONE;

    public float step_timer = 0.0f;
    private bool is_landed = false; // 착륙을 했는지
    private bool is_collided = false; // 충돌했는지
    private bool is_key_released = false; // 점프키가 손에서 떼어졌는지 이중점프 제어

    // Start is called before the first frame update
    void Start()
    {
        next_step = STEP.RUN; // 게임 시작 시 바로 달릴 수 있게 함.
    }

    private void CheckLanded()
    {
        is_landed = false;

        do
        {
            Vector3 current_position = transform.position;
            Vector3 down_position = current_position + Vector3.down * 1.0f;

            RaycastHit hit;
            if (!Physics.Linecast(current_position, down_position, out hit))
                break;

            if (step == STEP.JUMP)
            {
                if (step_timer < Time.deltaTime * 3.0f)
                    break;
            }

            is_landed = true;
        } while (false);

    }
    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        CheckLanded();

        switch (step)
        {
            case STEP.RUN:
            case STEP.JUMP:
                if (transform.position.y < FAIL_LIMIT)
                    next_step = STEP.MISS;
                break;
        }

        step_timer += Time.deltaTime;

        if(next_step == STEP.NONE)
        {
            switch (step)
            {
                case STEP.RUN:
                    if (!is_landed)
                    {
                    }
                    else if (Input.GetMouseButtonDown(0))
                        next_step = STEP.JUMP;
                    break;

                case STEP.JUMP:
                    if (is_landed)
                        next_step = STEP.RUN;
                    break;
            }
        }

        while(next_step != STEP.NONE)
        {
            step = next_step;
            next_step = STEP.NONE;

            switch (step)
            {
                case STEP.JUMP:
                    velocity.y = Mathf.Sqrt(2.0f * 9.8f * JUMP_HEIGHT_MAX);
                    is_key_released = false;
                    break;
            }

            step_timer = 0.0f;
        }

        switch (step)
        {
            case STEP.RUN:
                velocity.x += ACCELERATION * Time.deltaTime;
                if (Mathf.Abs(velocity.x) > SPEED_MAX)
                    velocity.x = SPEED_MAX;
                break;

            case STEP.JUMP:
                do
                {
                    if (!Input.GetMouseButtonUp(0))
                        break;
                    if (is_key_released)
                        break;
                    if (velocity.y <= 0.0f)
                        break;

                    velocity.y *= JUMP_POWER_REDUCE;
                    is_key_released = true;
                } while(false);
                break;
        }

        switch(step)
        {
            case STEP.RUN:
                velocity.x += player_control.ACCELERATION * Time.deltaTime;
                if (Mathf.Abs(velocity.x) > player_control.SPEED_MAX)
                    velocity.x = player_control.SPEED_MAX;
                    break;

            case STEP.JUMP:
                do
                {
                    if (!Input.GetMouseButtonUp(0))
                        break;

                    if (is_key_released)
                        break;

                    if (velocity.y <= 0.0f)
                        break;

                    velocity.y *= JUMP_POWER_REDUCE;
                    is_key_released = true;
                } while (false);
                break;

            case STEP.MISS:
                velocity.x -= player_control.ACCELERATION * Time.deltaTime;
                if(velocity.x < 0.0f)
                {
                    velocity.x = 0.0f;
                    Application.Quit();
                }
                break;
        }

        GetComponent<Rigidbody>().velocity = velocity;
    }
}
