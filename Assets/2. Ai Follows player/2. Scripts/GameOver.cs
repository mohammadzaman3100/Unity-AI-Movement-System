using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    GameObject player;
    Path_Enemy_Controller_Version_2 enemyContr;
    private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyContr = this.gameObject.GetComponent<Path_Enemy_Controller_Version_2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyContr.canRotateToPlayer)
        {
            Debug.Log("dgrtrfeth");
            if (Vector3.Distance(this.transform.position, player.transform.position) < 3f)
            {
                Debug.Log(time);
                time += Time.deltaTime;

                if (time > 1.5f)
                {
                    SceneManager.LoadScene(2);
                }
            }
        }
        else
        {
            time = 0f;
        }
    }
}