using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyTools.Tools;

public class EnemyMovement : MonoBehaviour
{
    private EnemyMovementType type;
    private Rigidbody body;
    private float _speed = 2.0f;
    private float zigzagFrequency = 5f;
    private float AMPLITUDE = .1f; //Amplitudeo of zigzag movement


    public void OnEnable()
    {
        body = GetComponent<Rigidbody>();
        _speed = GetRandomValueFloat(
            new RandomSelection(2.0f, 5.0f, .7f),
            new RandomSelection(6.0f, 8.0f, .3f)
        ); 

        //Chooses a random enemy movement with probabilty of "STRAIGHT" being 70 percent, and "ZIGZAG" being 30 percent
        int random = GetRandomValueInt(
            new RandomSelection(0, 0, .7f),
            new RandomSelection(1, 1, .3f)
        );

        type = (EnemyMovementType)random;
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public void Move()
    {
        switch (type)
        {
            case EnemyMovementType.STRAIGHT:
                MoveStraight();
                break;
            case EnemyMovementType.ZIGZAG:
                MoveInZigZag();
                break;
            default: break;
        }
    }

    void MoveStraight()
    {
        body.MovePosition(body.position + Vector3.down * (_speed * Time.deltaTime));
    }

    private void MoveInZigZag()
    {
        body.transform.position += new Vector3(AMPLITUDE * Mathf.Cos(Time.time * zigzagFrequency), -1 * _speed * Time.deltaTime, 0f);
    }
}

