using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_control : MonoBehaviour
{
    private GameObject player = null; 
    private Vector3 position_offset = Vector3.zero; // 플레이어와의 간격

    // Start is called before the first frame update
    void Start()
    {
        // 매개변수에 지정한 태그랑 똑같은 태그를 가진 물체를 찾아서 리턴시켜줌.
        player = GameObject.FindGameObjectWithTag("Player"); // Find : 매개변수 이름을 가진 물체 찾음.
        // 실행됐을 때 플레이어와 간격을 유지. 카메라의 위치에서부터 플레이어로부터 상대적인 위치값
        position_offset = transform.position - player.transform.position; 
    }

    // 플레이어가 이동한 만큼 카메라도 따라서 움직이게 함.
    // LateUpdate : 모든 게임 오브젝트의 Update함수 처리가 끝난 뒤 자동으로 실행되는 함수.
    private void LateUpdate()
    {
        Vector3 new_position = transform.position; // 현재 카메라의 위치 값 저장.
        // x값만 변경, 플레이어가 움직일 위치값에 플레이어와 카메라 사이의 거리에 해당하는 만큼만 이동. y,z값 유지. 
        new_position.x = player.transform.position.x + position_offset.x; 
        transform.position = new_position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
