using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

public class GUIManager : MonoBehaviour
{
    public TMP_Text _texto;

    // Start is called before the first frame update
    void Start(){
        Assert.IsNotNull(_texto, "Texto no puede ser nulo");
        _texto.text = "Hola";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}