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
    //public bool timed = true;
    public  float timer = 0f;
    private float _timer;
    [SerializeField] public Vector2 resoawnPosition = new Vector2(0, 0);


   

    private void OnCollisionEnter2D(Collision2D collision)
    {
     
        if (collision.gameObject.tag == "player")
        {
             timed = true;
            _timer = timer;
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

            if (timed) {
                
                _timer -= Time.deltaTime;
                Debug.Log("Timer: " + _timer);
                if (_timer < 0)
                {
                    timed = false;
                    TimedFunction();
                }
            }

      
            
   
        }
        
        if (transform.position.y < yDestroy)
        {
            Destroy(this.gameObject);
        }
    }
    private  void TimedFunction()
    {

        Debug.Log("TimedFunction");
        gameObject.transform.SetParent(null);
        
        moving = false;
        timed = false;
        Instantiate(platform, resoawnPosition, Quaternion.identity);
        
        Destroy(gameObject);
       

        return;
    }


}


