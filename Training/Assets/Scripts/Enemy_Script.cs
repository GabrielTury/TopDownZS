using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    [SerializeField] float enemySpeed;
    public Transform playerTransform;
    public int health;

    // Update is called once per frame

    private void Start()
    {
        StartCoroutine(LerpingToPlayer());
    }
    void Update()
    {
        CheckDead();
    }
    void CheckDead()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    IEnumerator LerpingToPlayer()
    {
        while(Vector2.Distance(transform.position, playerTransform.position) >= 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, enemySpeed * Time.deltaTime);

            yield return null;           
        }
        if(Vector2.Distance(transform.position, playerTransform.position) <= 0.05f)
        {
            StopCoroutine(LerpingToPlayer());
            print("a");
        }

    }
}
