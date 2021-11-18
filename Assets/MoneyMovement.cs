using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyMovement : MonoBehaviour
{
    private float RotateSpeed = 5f;
    private float Radius = 6f;

    private Vector2 _centre;
    private float _angle;

    void Start()
    {
        _centre = transform.position;
    }

    public Transform target; //where we want to shoot(player? mouse?)
    public GameObject bullet; //Your set-up prefab
    public float fireRate = 3000f; //Fire every 3 seconds
    public float shootingPower = 1f; //force of projection
    private float shootingTime; //local to store last time we shot so we can make sure its done every 3s
    private void Fire()
    {
        if (Time.time > shootingTime)
        {
            Debug.Log("Shot.");
            shootingTime = Time.time + fireRate / 1000; //set the local var. to current time of shooting
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y); //our curr position is where our muzzle points
            GameObject projectile = Instantiate(bullet, myPos, Quaternion.identity); //create our bullet
            Vector2 direction = myPos - (Vector2)target.position; //get the direction to the target
            projectile.GetComponent<Rigidbody2D>().velocity = direction * shootingPower; //shoot the bullet
        }
    }

    void Update()
    {
        Fire(); //Constantly fire

        _angle += RotateSpeed * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
        transform.position = _centre + offset;
    }
}
