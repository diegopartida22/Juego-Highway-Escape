using UnityEngine;
using System.Collections;
using SgLib;

public class PoliceController : MonoBehaviour
{


public float currentSpeed;
public float maxAngularVelocity = 15;

private Rigidbody rigid;
private bool finishTurn;


void Start()
{



    finishTurn = true;
    //currentSpeed = GameManager.Instance.initialSpeed;
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
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(TurnRight());
                StartCoroutine(Rotate(GameManager.Instance.rotateAngle));
   
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(TurnLeft());
                StartCoroutine(Rotate(-GameManager.Instance.rotateAngle));;
            }
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

}
