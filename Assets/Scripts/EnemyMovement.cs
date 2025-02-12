using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyMovementType type;
    private Rigidbody body;
    private float _speed = 2.0f;
    private float zigzagFrequency = 5f;
    private float AMPLITUDE = .1f;


    public void OnEnable()
    {
        body = GetComponent<Rigidbody>();
        _speed = GetRandomValueFloat(
            new RandomSelection(2.0f, 5.0f, .7f),
            new RandomSelection(6.0f, 8.0f, .3f)
        ); 
        //Random.Range(2.0f, 6.0f);

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

    int GetRandomValueInt(params RandomSelection[] selections)
    {
        float rand = Random.value;
        float currentProb = 0;
        foreach (var selection in selections)
        {
            currentProb += selection.probability;
            if (rand <= currentProb) return selection.GetValueInt();
        }

        //will happen if the input's probabilities sums to less than 1
        //throw error here if that's appropriate
        return -1;
    }

    float GetRandomValueFloat(params RandomSelection[] selections)
    {
        float rand = Random.value;
        float currentProb = 0;
        foreach (var selection in selections)
        {
            currentProb += selection.probability;
            if (rand <= currentProb) return selection.GetValueFloat();
        }

        //will happen if the input's probabilities sums to less than 1
        //throw error here if that's appropriate
        return -1;
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

