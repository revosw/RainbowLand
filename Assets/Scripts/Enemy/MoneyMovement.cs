using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**
 * TODO: When implementing the boss movement in the level, make sure he doenst move too close to the ground.
 */
public class MoneyMovement : MonoBehaviour
{
    //Movement fields
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3[] positions;
    private int index;

    //Shooting fields
    private float stopAndShootCD = 3f;
    public float range;
    private float distToPlayer;
    public Transform player;

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
        if (distToPlayer <= range) //check if player is in range to be shot at.
        {
            if (Time.time > stopAndShootCD)
            {
                stopAndShootCD = Time.time + 8f;
                StartCoroutine(StopAndShoot());
            }
            transform.position = Vector2.MoveTowards(transform.position, positions[index], Time.deltaTime * speed);
            if (transform.position == positions[index])
            {
                if (index == positions.Length - 1)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
            }
        }
    }

    public GameObject bullet; //bullet prefab.
    public Transform shootPos; //Shoot from
    public Transform shootPosTop;
    public float shootPower = 3f; //shooting power
    public Transform target; // target position
    private void Shoot() {
            Vector2 shootFrom = new Vector2(shootPos.position.x, shootPos.position.y);
        if(target.position.y > 0)
        { //Make sure boss doesnt shoot itself.
            shootFrom = new Vector2(shootPosTop.position.x, shootPosTop.position.y);
        }
            GameObject newBullet = Instantiate(bullet, shootFrom, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = -1 * (shootFrom - (Vector2)target.position);
    }
}
