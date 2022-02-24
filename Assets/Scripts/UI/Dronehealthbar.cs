using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dronehealthbar : MonoBehaviour
{
    public Camera maincam;
    public int health;
    public Enemy enemyscript;
    public int numhearts;

    public Image[] hearts;
    public Sprite heart;

    public Transform target;
   
    // Start is called before the first frame update
    void Start()
    {
        enemyscript = gameObject.GetComponentInParent<Enemy>();

        maincam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        spritechange();
        uimove();
    }

    private void uimove()
    {
        Vector3 wantedpos = maincam.ScreenToWorldPoint(target.position);
        transform.position = wantedpos;
    }

    private void spritechange()
    {
        health = enemyscript.health;
       for(int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
            
        }
    }
}
