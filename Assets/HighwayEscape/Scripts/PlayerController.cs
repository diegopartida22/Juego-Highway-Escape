﻿using UnityEngine;
using System.Collections;
using SgLib;

public class PlayerController : MonoBehaviour
{
public static event System.Action PlayerDied;

[Header("Gameplay References")]
public CameraController cameraController;
public Shader curvedWorld;
public GameObject healthEmpty;

public GameObject healthFull;
public GameObject healthEmpty1;

public GameObject healthFull1;
public GameObject healthEmpty2;

public GameObject healthFull2;
public float currentSpeed;
public float maxAngularVelocity = 15;
[Header("Gameplay Config")]
public float initialHealth = 1;
public float currentHealth;

private Rigidbody rigid;
private bool finishTurn;

// Calls this when the player dies and game over
public void Die()
{
    // Fire event
    if (PlayerDied != null)
        PlayerDied();
}

void Start()
{
    currentHealth = initialHealth;
    GameObject currentCharacter = CharacterManager.Instance.characters[CharacterManager.Instance.CurrentCharacterIndex];
    Mesh charMesh = currentCharacter.GetComponent<MeshFilter>().sharedMesh;
    Material charMaterial = currentCharacter.GetComponent<Renderer>().sharedMaterial;
    GetComponent<MeshFilter>().mesh = charMesh;
    GetComponent<MeshRenderer>().material = charMaterial;
    GetComponent<Renderer>().material.shader = curvedWorld;

    finishTurn = true;
    currentSpeed = 20;
    rigid = GetComponent<Rigidbody>();


    rigid.maxAngularVelocity = maxAngularVelocity;
    rigid.ResetCenterOfMass();
}

void FixedUpdate()
{
    //Move player and particle
    if (GameManager.Instance.GameState == GameState.Playing)
    {
        rigid.velocity = Vector3.forward * currentSpeed;
    }
}

    // Update is called once per frame
    void Update()
{
    if (GameManager.Instance.GameState.Equals(GameState.Playing))
    {
        if (finishTurn)
        {
            // Movement to the right
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(TurnRight());
                StartCoroutine(Rotate(GameManager.Instance.rotateAngle));
                cameraController.MoveRight();
            }

            // Movement to the left
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(TurnLeft());
                StartCoroutine(Rotate(-GameManager.Instance.rotateAngle));
                cameraController.MoveLeft();
            }

            // Increase speed
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                currentSpeed += GameManager.Instance.increaseSpeedFactor * Time.deltaTime;
                if (currentSpeed > 95)
                {
                    currentSpeed = 95;
                }
            }

            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            {
                if (currentSpeed > GameManager.Instance.playerSpeed)
                {
                    currentSpeed -= GameManager.Instance.increaseSpeedFactor * Time.deltaTime / 10;
                }
            }

            // Decrease speed
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                currentSpeed -= GameManager.Instance.increaseSpeedFactor * Time.deltaTime;
                if (currentSpeed < 20)
                {
                    currentSpeed = 20;
                }
            }
        }
    }
}
    IEnumerator TurnRight()
    {
        finishTurn = false;

        yield return new WaitForFixedUpdate();

        float startX = Mathf.Round(transform.position.x);
        float endX = startX + 4f;

        if (endX <= 8)
        {
            float t = 0;
            while (t < GameManager.Instance.turnTime)
            {
                t += Time.deltaTime;
                float fraction = t / GameManager.Instance.turnTime;
                float newX = Mathf.Lerp(startX, endX, fraction);
                Vector3 newPos = transform.position;
                newPos.x = newX;
                transform.position = newPos;
                yield return null;
            }
        }      
        finishTurn = true;
    }

    IEnumerator TurnLeft()
    {
        finishTurn = false;

        yield return new WaitForFixedUpdate();

        float startX = Mathf.Round(transform.position.x);
        float endX = startX - 4f;

        if (endX >= -8)
        {
            float t = 0;
            while (t < GameManager.Instance.turnTime)
            {
                t += Time.deltaTime;
                float fraction = t / GameManager.Instance.turnTime;
                float newX = Mathf.Lerp(startX, endX, fraction);
                Vector3 newPos = transform.position;
                newPos.x = newX;
                transform.position = newPos;
                yield return null;
            }
        }

        finishTurn = true;
    }

    IEnumerator Rotate(float angle)
    {
        finishTurn = false;

        yield return new WaitForFixedUpdate();

        if (transform.position.x < 8 && transform.position.x > -8)
        {
            Quaternion startRot = transform.rotation;
            Quaternion endRot = Quaternion.Euler(0, angle, 0);
            float t = 0;
            while (t < GameManager.Instance.turnTime / 2f)
            {
                t += Time.deltaTime;
                float fraction = t / (GameManager.Instance.turnTime / 2f);
                transform.rotation = Quaternion.Lerp(startRot, endRot, fraction);
                yield return null;
               
            }

            float r = 0;
            while (r < GameManager.Instance.turnTime / 2f)
            {
                r += Time.deltaTime;
                float fraction = r / (GameManager.Instance.turnTime / 2f);
                transform.rotation = Quaternion.Lerp(endRot, startRot, fraction);
                yield return null;
            }

        }
        finishTurn = true;
    }

    void OnCollisionEnter(Collision col)
    {
    

        if (GameManager.Instance.GameState.Equals(GameState.Playing))
        {
            if (col.gameObject.CompareTag("Car")) //Hit another car
            {
                //print ("Health: " + initialHealth);
                initialHealth/=2;

                if (initialHealth == 2.5)
                {
                    healthEmpty.SetActive(true);
                    healthFull.SetActive(false);
                }

                if (initialHealth == 1.25)
                {
                    healthEmpty1.SetActive(true);
                    healthFull1.SetActive(false);
                }

                if (initialHealth == 0.625)
                {
                    healthEmpty2.SetActive(true);
                    healthFull2.SetActive(false);
                }     
                //print ("Health: " + initialHealth);
                SoundManager.Instance.PlaySound(SoundManager.Instance.hitObstacle);

                CarController carController = col.gameObject.GetComponent<CarController>();
                Vector3 dirCollision = (col.transform.position - transform.position).normalized;
                carController.stopMoving = true;
                carController.stopTurn = true;

                Rigidbody carRigid = col.gameObject.GetComponent<Rigidbody>();
                carRigid.constraints = RigidbodyConstraints.None;

                StartCoroutine(ImmuneToCollision(carRigid, 1f));

                if (initialHealth == 0.625) //Game over
                {
                    SoundManager.Instance.PlaySound(SoundManager.Instance.gameOver);
                    Die();
                    PlayerDied();
                    cameraController.ShakeCamera();
                    rigid.constraints = RigidbodyConstraints.None;             
                    StartCoroutine(AddForce(rigid, true, 500f, 550f, dirCollision, col.rigidbody));
                }

                int dir;
                if (col.transform.position.x - rigid.position.x < 0) { dir = 1; }
                else
                {
                    dir = -1;
                }
                rigid.AddForce(new Vector3(40 * dir, 0, (200 - currentSpeed)*2), ForceMode.Impulse);
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gold") && GameManager.Instance.GameState.Equals(GameState.Playing))
        {
            CoinManager.Instance.AddCoins(1);
            SoundManager.Instance.PlaySound(SoundManager.Instance.hitGold);
            other.gameObject.SetActive(false);
            other.transform.parent = CoinManager.Instance.transform;
        }
    }

    IEnumerator AddForce(Rigidbody rigid, bool isForPlayer, float minForce, float maxForce, Vector3 dirCollision,Rigidbody other)
    {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForFixedUpdate();
            Vector3 torqueDir = (isForPlayer) ? (-dirCollision * 500f) : (dirCollision * 40f);
            rigid.AddTorque(torqueDir);
        }
        yield return new WaitForEndOfFrame();
        Vector3 angularV = rigid.angularVelocity;
        angularV.x /= Mathf.Abs(angularV.x);
        angularV.y /= Mathf.Abs(angularV.y);
        angularV.z /= Mathf.Abs(angularV.z);

        rigid.angularVelocity = angularV * maxAngularVelocity;
    }

    IEnumerator ImmuneToCollision(Rigidbody rigid, float time)
    {
        rigid.detectCollisions = false;
        yield return new WaitForSeconds(time);
        rigid.detectCollisions = true;
    }
}


