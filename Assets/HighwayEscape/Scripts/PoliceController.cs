using UnityEngine;

public class PoliceController : MonoBehaviour
{
    public GameObject player;   // Referencia al objeto del carro
    
    // Velocidad del policía

    private Rigidbody rigid;


    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Obtener la dirección hacia el carro
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        float currentSpeed = player.GetComponent<PlayerController>().currentSpeed;
        rigid.velocity = direction.normalized * currentSpeed * 0.9f;

        // Mover al policía hacia la posición del carro a una velocidad gradual despues de un tiempo

        
    }
}
