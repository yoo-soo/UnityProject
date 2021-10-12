using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(-6.0f, -4.0f); // 벽이 다가오는 속도 랜덤
        Destroy(gameObject, 5.0f); // wall prefab이 메모리에서 5초 뒤에 제거되도록 함.
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0); // 벽 x좌표 speed만큼 다가가기.
    }
}
