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
    public static float playTime = 0.0f;
    public static float playtime = 0.0f;



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

    private void CheckLanded() // 착지를 판단하는 함수
    {
        is_landed = false;

        do
        {
            Vector3 current_position = transform.position;
            Vector3 down_position = current_position + Vector3.down * 1.0f;

            RaycastHit hit; 
            // 두 개의 위치가 주어지면 두 개의 위치 사이를 선으로 연결, 연결한 선 사이에 특정한 물체가 있을 경우 true반환
            if (!Physics.Linecast(current_position, down_position, out hit))
                break;

            if (step == STEP.JUMP)  // 점프한 상태일 경우
            {
                if (step_timer < Time.deltaTime * 3.0f) // 점프한지 시간이 얼마나 되었는지 기록. 세 프레임 정도 시간이 지나면 지나감.
                    break; // 현재 상태 점프x, steptimmer가 timedelta * 3.0f 보다 작았다는 것.
            }

            is_landed = true;
        } while (false);

    }
    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        CheckLanded();
        playTime += Time.deltaTime;
        playtime += Time.deltaTime;



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
                case STEP.RUN: // 달리는 중일경우
                    if (!is_landed) // 착지x
                    {
                    }
                    else if (Input.GetMouseButtonDown(0)) // 착지했을 때 마우스 왼쪽 버튼으로
                        next_step = STEP.JUMP; // 점프
                    break;

                case STEP.JUMP: // 점프 중인 경우
                    if (is_landed) // 착지
                        next_step = STEP.RUN; // 달려가게 상태 바꿔줌.
                    break;
            }
        }

        while(next_step != STEP.NONE) 
        {
            step = next_step;
            next_step = STEP.NONE;

            switch (step)
            {
                case STEP.JUMP: // 점프일 경우
                    velocity.y = Mathf.Sqrt(2.0f * 9.8f * JUMP_HEIGHT_MAX); // 지정한 수치만큼 뛰어오를 수 있게 수식을 강제로 넣어줌
                    is_key_released = false;
                    break;
            }

            step_timer = 0.0f; // 0.0으로 초기화
        }

        switch (step)
        {
            case STEP.RUN: // 달리는 중인 경우
                velocity.x += ACCELERATION * Time.deltaTime; // 가속시켜줌
                if (Mathf.Abs(velocity.x) > SPEED_MAX) // 지정한 최고속도보다 커지지 않도록
                    velocity.x = SPEED_MAX;
                break;

            case STEP.JUMP: // 점프 중일 경우
                do
                {
                    if (!Input.GetMouseButtonUp(0)) // 버튼을 눌렀다 떼는 순간
                        break; 
                    if (is_key_released) // 누르고 있지 않을 경우
                        break;
                    if (velocity.y <= 0.0f) // 하강하고 있는 경우
                        break;

                    velocity.y *= JUMP_POWER_REDUCE; // 속도 줄어줌
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


        if (player_control.playTime / 15 >= 0.0f && player_control.playTime / 15 < 1.0f) // playtime이 0초~14초일 때
        {
            SPEED_MAX = 5;
        }
        else if (player_control.playTime / 15 >= 1.0f && player_control.playTime / 15 < 2.0f) // 15초 ~ 29초일 때
        {
            SPEED_MAX = 6;
        }
        else if (player_control.playTime / 15 >= 2.0f && player_control.playTime / 15 < 3.0f) // 30초 ~ 44초일 때
        {
            SPEED_MAX = 7;
        }
        else if (player_control.playTime / 15 >= 3.0f && player_control.playTime / 15 < 4.0f) // 45초 ~ 59초일 때
        {
            SPEED_MAX = 8;

        }
        else if (player_control.playTime / 15 >= 4.0f && player_control.playTime / 15 < 5.0f) // 60초 ~ 74초일 때
        {
            SPEED_MAX = 9;
        }
       

        GetComponent<Rigidbody>().velocity = velocity;
    }
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 60, 60, 100, 20), playtime.ToString());
        //GUI.Label(new Rect(Screen.width / 2 - 60, 20, 100, 20), SPEED_MAX.ToString());
    }
}
