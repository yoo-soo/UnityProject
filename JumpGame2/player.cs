using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    public float jump_power;
    public float time;

    // Start is called before the first frame update]
    void Start()
    {
        jump_power = Random.Range(5.0f, 8.0f);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, jump_power, 0);
    }

    private void OnCollisionEnter(Collision other)
    {
        SceneManager.LoadScene("scene2");
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 60, 20, 100, 20), jump_power.ToString());
        GUI.Label(new Rect(Screen.width / 2 - 60, 40, 100, 20), time.ToString());
    }
}
