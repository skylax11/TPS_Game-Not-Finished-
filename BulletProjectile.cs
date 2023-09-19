using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        float speed = 100f;
        rb.velocity = transform.forward * speed;
        StartCoroutine("callBack");
    }
    IEnumerator callBack()
    {
        yield return new WaitForSeconds(2f);
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);

    }
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
    public void resetVelo()
    {
        rb.velocity = Vector3.zero;
        float speed = 100f;
        rb.velocity = transform.forward * speed;
    }
}
