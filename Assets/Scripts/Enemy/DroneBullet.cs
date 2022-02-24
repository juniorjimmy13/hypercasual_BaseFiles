using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBullet : MonoBehaviour
{
    #region Variables
    public Collider bulletcol;
    public PlayerHealth Health;
    public GameManager Manager;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        bulletcol = GetComponent<Collider>();

       Manager = GameObject.FindGameObjectWithTag("Manager").GetComponentInParent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        deathafterspawn();
    }
    private void deathafterspawn()
    {
      
        Destroy(gameObject, 8f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if (collision.gameObject.CompareTag("Player"))
        {
            Manager.multiplier = 1;
            Health = collision.gameObject.GetComponentInParent<PlayerHealth>();
            Health.currentHealth -= 1;
        }
    }



}
