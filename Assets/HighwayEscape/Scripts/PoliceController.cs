using UnityEngine;

public class PoliceController : MonoBehaviour
{
    public GameObject player;  // Referencia al GameObject del jugador
    public float speed = 5f;   // Velocidad del policía

    private Vector3 targetPos; // Posición objetivo del policía

    void Start()
    {
        // La posición inicial del policía será detrás del jugador
        targetPos = player.transform.position - player.transform.forward * 5f;
    }

    void Update()
    {
        // Mover el policía hacia la posición objetivo del jugador
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Actualizar la posición objetivo del policía para que siempre esté detrás del jugador
        targetPos = player.transform.position - player.transform.forward * 5f;
    }
}
