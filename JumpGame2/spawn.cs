using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    public GameObject pf_wall; // wall prefab
    //public float interval; // 얼마 주기로 벽이 생성되도록 할 것인지

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (true) // 무한실행, Start 함수 (스레드) 별개로 다른함수 유지, 실행
        {
            float interval = Random.Range(1.0f, 2.0f); // 벽이 생성되는 주기 랜덤 1~2초
            float width = Random.Range(-3.0f, 4.0f);  // 생성되는 벽의 높이 랜덤 -3~4
            Instantiate(pf_wall, new Vector3(0.0f, width, 0.0f), transform.rotation); // wall 프리팹, 
            yield return new WaitForSeconds(interval); // 얼마마다 호출할 것인지 정의해주는 타입 yield return
            // 1.5초마다 wall prefab을 생성하는 코드
        }
    }
}
