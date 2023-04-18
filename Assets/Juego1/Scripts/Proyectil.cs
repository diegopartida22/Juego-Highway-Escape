using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class Proyectil : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;

    [SerializeField]
    private float _tiempoDeAutodestruccion = 3;

    private GUIManager _gui;

    // Start is called before the first frame update
    void Start()
    {
        // Si voy a crear objetos en tiempo de ejecución
        // es necesario destruirlos para evitar fugas de memoria (memory leaks)
        Destroy(gameObject, _tiempoDeAutodestruccion);

        // ESTO VA A CAMBIAR
        GameObject guiGo = GameObject.Find("GUIManager");
        Assert.IsNotNull(guiGo, "GUIManager no encontrado");

        _gui = guiGo.GetComponent<GUIManager>();
        Assert.IsNotNull(_gui, "GUIManager no tiene componente GUIManager");

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(
            0,
            _speed * Time.deltaTime,
            0
        );
    }

    // COLISIONES
    // Cuando un objeto colisiona con otro se invoca este método
    // 1. Que todos los objetos que colisionan tengan un collider
    // 2. Al menos uno de los objetos tiene que tener un rigidbody
    // 3. El rigidbody debe estar en un objeto que se mueva para que se detecte la colisión y tenga fisicas

    void OnCollisionEnter(Collision c){
        // Se toca por primera vez
        // Objeto collision que recibió la colisión tiene información de la colisión

        // Como saber que hacer 
        // 1. Filtrar por tag
        // 2. Filtrar por layer (capa) (más eficiente)
        print("ENTER" + c.gameObject.name);
    }

    void OnCollisionStay(Collision c)
    {
        // Permanece en contacto
        print("STAY");
    }

    void OnCollisionExit(Collision c)
    {
        // Se deja de tocar
        print("EXIT");
    }

    // TRIGGER
    // Cuando un objeto entra en contacto con otro se invoca este método
    // 1. El collider debe estar en modo trigger

    void OnTriggerEnter(Collider c)
    {
        print ("TRIGGER ENTER");
    }

    void OnTriggerStay(Collider c)
    {
        print("TRIGGER STAY");
    }

    void OnTriggerExit(Collider c)
    {
        print("TRIGGER EXIT");   
        _gui._texto.text = "SALI " + transform.name; 
    }
}