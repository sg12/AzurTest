using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static AnimalFactory;

[Serializable]
public class Animal : MonoBehaviour
{
    [HideInInspector]
    public Food FoodToEat;
    private NavMeshAgent navAgent;

    private ReachFoodByAnimalHandler reachFoodEvent;
    private Rigidbody rigid;
    //private float timeStart = 0f;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
    }

    public void StartMoveToEat(Food foodToEat, ReachFoodByAnimalHandler reachFoodEvent, float speedAnimal)
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        FoodToEat = foodToEat;
        navAgent.isStopped = false;
        navAgent.speed = speedAnimal;
        navAgent.SetDestination(FoodToEat.transform.position);
        this.reachFoodEvent = reachFoodEvent;
        //timeStart = Time.timeSinceLevelLoad;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Food"))
        {
            if (other.gameObject.GetComponent<Food>().Equals(FoodToEat))
            {
                //Debug.Log("Reached");
                //float timeDelta = Time.timeSinceLevelLoad - timeStart;
                //if (timeDelta >= 5.0f)
                //    Debug.Log("time: " + timeDelta);
                navAgent.isStopped = true;
                //rigid.drag = 0f;
                reachFoodEvent.Invoke(this, other.gameObject.GetComponent<Food>());
                
            }
        }
    }

    public void UpdateSpeed(float speed)
    {
        navAgent.speed = speed;
    }

}
