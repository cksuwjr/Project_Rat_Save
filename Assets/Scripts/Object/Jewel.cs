using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jewel : PoolObject
{
    [SerializeField] private int value = 1;
    [SerializeField] private bool acquirable = false;
    private Rigidbody rb;

    private void Awake()
    {
        TryGetComponent<Rigidbody>(out rb);
    }

    public void InitJewel()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector3(Random.Range(-2f, 2f), 6, Random.Range(-2f, 2f)), ForceMode.Impulse);
        acquirable = false;
        Invoke("AcquiableTrue", 0.7f);
    }

    public void AcquiableTrue()
    {
        acquirable = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Entity>()) return;
        if (other.isTrigger) return;
        if (!acquirable) return;

        if(other.CompareTag("Player"))
        {
            GameManager.Instance.Money += value;
            acquirable = false;
            CancelInvoke("AcquiableTrue");
            ReturnToPool();
        }
    }
}
