using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossElevatorPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] bool moving;
    [SerializeField] public Vector2 targetPosition = new Vector2(0, 0);
    [SerializeField] double yDestroy;
    [SerializeField] GameObject Music;
    [SerializeField] GameObject BossLoop;
    public GameObject platform;
    public bool timed;
    public int timer = 3;
    [SerializeField] public Vector2 resoawnPosition = new Vector2(0, 0);




    private void OnCollisionEnter2D(Collision2D collision)
    {
     
        if (collision.gameObject.tag == "player")
        {
            moving = true;
            Music.SetActive(false);
            BossLoop.SetActive(true);
        }



    }
    private void Update()
    {
        
        
        if (moving)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), targetPosition, speed * Time.deltaTime);
            if(timed)
        {
                Invoke("TimedFunction", timer);
                Debug.Log(timer);
            }
        }
        
        if (transform.position.y < yDestroy)
        {
            Destroy(this.gameObject);
        }
    }
    private  void TimedFunction()
    {
        gameObject.transform.SetParent(null);
        Destroy(gameObject);
        Instantiate(platform, resoawnPosition, Quaternion.identity);

        return;
    }


}


