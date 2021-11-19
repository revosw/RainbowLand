using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MoneyMovement : MonoBehaviour
{
    //Movement fields
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3[] positions;
    private int index;
    private float stopAndShootCD = 3f;


    //Stuff?
    public float range;
    private float distToPlayer;
    public Transform player;

    void Start()
    {
    }


    IEnumerator StopAndShoot() {
        speed = 0;
        Shoot();
        yield return new WaitForSeconds(1);
        Shoot();
        yield return new WaitForSeconds(1);
        Shoot();
        yield return new WaitForSeconds(1);
        speed = 10f;
    }

    void Update() {
        if (Time.time > stopAndShootCD) {
            stopAndShootCD = Time.time + 8f;
            StartCoroutine(StopAndShoot());
        }

        transform.position = Vector2.MoveTowards(transform.position, positions[index], Time.deltaTime * speed);
        if (transform.position == positions[index]) {
            if (index == positions.Length - 1) {
                index = 0;
            } else {
                index++;
            }
        }
        // Fire(); //Constantly fire
        /*
        _angle += RotateSpeed * Time.deltaTime;
        var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
        transform.position = _centre + offset; 
        */
    }

    private float shootingCD; //local to store last time we shot so we can make sure its done every 3s
    public float fireRate = 3f;
    public GameObject bullet; //bullet prefab.
    public Transform shootPos; //Shoot from
    public float shootPower = 3f; //shooting power
    public Transform target; // target position
    private void Shoot() {
            Vector2 shootFrom = new Vector2(shootPos.position.x, shootPos.position.y);
            GameObject newBullet = Instantiate(bullet, shootFrom, Quaternion.identity);
            Vector2 direction = shootFrom - (Vector2)target.position; //get the direction to the target
            //newBullet.GetComponent<Rigidbody2D>().velocity = direction * shootPower;
            
        newBullet.GetComponent<Rigidbody2D>().velocity = -1 * (shootFrom - (Vector2)target.position) ;
    }
}
