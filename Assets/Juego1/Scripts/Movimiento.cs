using System;
using System.Threading;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// OJO
// con  esta directiva obligamos la presencia de un componente en el gameobject
// (todos tienen transforms asi que este ejemplo es redundante) se usa para evitar el nulo
//[RequireComponent(typeof(Transform))];

public class Movimiento : MonoBehaviour
{
    //ciclo de vida / lifecycle
    //existe metodos que se invocan en momento especificos de la vida del script

    private Transform _transform;

    [SerializeField]

    private float _speed = 10;

    [SerializeField]
    private Proyectil _disparoOriginal;

    // Start is called before the first frame update
    void Awake()
    {
        print("AWAKE");

    }

    // Se invoca una vez después de que fueron invocados todos los awakes
    void Start()
    {
        Debug.Log("START"); 

        // como obtener una referencia a otro componente
        // busca el objeto que tengamos de tipo transform y me regresas una referencias

        // NOTAS:
        // GetComponent es lento, hazlo la menor cantidad de veces posibles
        // Con trasnform ya tenemos referencia
        // Esta operación puede regresar nulo
        _transform = GetComponent<Transform>();

        // Si tiene requiere esto ya no es necesario
        // Assert verifica una variable contra una operación en el caso de ser nulo manda una excepción
        Assert.IsNotNull(_transform, "ES NECESARIO PARA EL MOVIMIENTO TENER UN TRANSFORM");
        Assert.IsNotNull(_disparoOriginal, "DISPARO NO PUEDE SER NULO");
        Assert.AreNotEqual(0, _speed,  "VELOCIDAD DEBE SER MAYOR CERO");
    }

    // Update is called once per frame
    // Siempre vamos a tratar que ese sea lo más magro (ligero) posible
    // 1. Entrada de usuario
    // 2. Movimiento
    void Update()
    {
        Debug.LogWarning("UPDATE");

        // vamos a hacer polling por dispositivo
        // True - cuando en el cuadro anterior estaba libre y en este está presionada
        if(Input.GetKeyDown(KeyCode.Z)){
            print("KEY DOWN: Z");
        }

        // True - cuando en el cuadro anterior estaba presionada y en actual sigue presionada
        if(Input.GetKey(KeyCode.Z)){
            print("KEY: Z");
        }

        // True - cuando estaba presionada y ahora esta libre
        if(Input.GetKeyUp(KeyCode.Z)){
            print("KEY UP: Z");
        }

        if(Input.GetMouseButtonDown(0)){
            print("MOUSE BUTTON DOWN");
        }

        if (Input.GetMouseButtonUp(0)){
            print("MOUSE BUTTON UP");
        }

        // mapeo de hardware a un valor abstracto llamado eje
        // rando [-1, 1]

        // hacemos polling a un eje en lugar de hacrlo a hardware específico
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //print(horizontal + " " + vertical);

        // se pueden usar ejes como 
        if(Input.GetButtonDown("Jump")){
            print("JUMP");

            // Crear un objeto en tiempo de ejecución (runtime)
            // GameObject objeto = new GameObject("Disparo");

            // Si queremos un game object predefinido podemos usar Instantiate
            Instantiate(
                _disparoOriginal, 
                transform.position, 
                transform.rotation);
        }

        // como mover objetos
        // 4 opciones
        // 1. directamente con su transform
        // 2. por medio de character controller
        // 3. por medio de física
        // 4. por medio de navmesh (AI)
        transform.Translate(
            horizontal * _speed * Time.deltaTime,
            vertical * _speed * Time.deltaTime,
            0,
            Space.World);
    }

    // // Fixed? - fijo o limitado
    // // update que corre en intervarlo fijo en la configuración del projecto sirve para mantener constante el movimiento
    // // NO puede correr más frecuentemente que update 
    // void FixedUpdate()
    // {
    //     Debug.LogError("FIXED UPDATE");
    // }

    // // corre todos los cuadros una vez que los updates están terminados
    // void LateUpdate(){
    //     print("LATE UPDATE");
    // }
}