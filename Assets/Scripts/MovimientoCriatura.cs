using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoCriatura : MonoBehaviour
{
    public float maxSpeed = 3f; // Velocidad máxima que la criatura puede alcanzar
    public float accelerationTime = 2f;

    private Rigidbody2D rigidBody;
    private float timeBetweenChangeDirection; //tiempo entre cambio de dirección
    private Vector2 movement;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Setea una nueva dirección para que la criatura se mueva en cada frame
        timeBetweenChangeDirection -= Time.deltaTime;
        if (timeBetweenChangeDirection <= 0)
        {
            movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            timeBetweenChangeDirection += accelerationTime;
        }
    }

    //Si colisiona con algo, entonces camina en la dirección opuesta
    void OnCollissionEnter2D(Collision2D col)
    {
        movement = -movement;
    }

    //Hacer que la criatura camine
    void FixedUpdate()
    {
        rigidBody.AddForce(movement * maxSpeed);
    }
}
