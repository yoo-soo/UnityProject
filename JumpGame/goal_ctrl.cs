using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal_ctrl : MonoBehaviour
{
    private bool is_collided = false;
    public GameObject prefab = null;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(prefab);
        //GameObject game_object = GameObject.Instantiate(this.prefab) as GameObject;
        prefab.transform.position = new Vector3(Random.Range(5.0f, 10.0f), Random.Range(-3.0f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
   
    }
    void OnCollisionStay(Collision other)  // goal에 충돌하면
    {
        this.is_collided = true; // 참
        other.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    void OnGUI()
    {
        if (is_collided) // is_collided 변수가 참일때
            GUI.Label(new Rect(Screen.width / 2 - 30, 80, 100, 20), "Success!!!"); // 성공 텍스트 띄우기
    }
}

