using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public Rigidbody rigidBody;
    float speed = 120f;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rigidBody.velocity = transform.forward * speed;
        StartCoroutine("callBack");
    }
    IEnumerator callBack()
    {
        yield return new WaitForSeconds(2f);
        rigidBody.velocity = Vector3.zero;
        gameObject.SetActive(false);

    }
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
    public void resetVelo()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.velocity = transform.forward * speed;
    }
}
