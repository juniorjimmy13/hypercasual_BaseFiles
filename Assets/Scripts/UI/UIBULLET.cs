using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBULLET : MonoBehaviour
{
    public Image[] bullets;
    public shoot playerscript;
    public float currbullets;
    public Sprite buletsprite;
    // Start is called before the first frame update
    void Start()
    {
       shoot playerscript = GetComponent<shoot>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletcount();
    }

    private void bulletcount()
    {
        currbullets = playerscript.Magazinesize;
        for (int i = 0; i < bullets.Length; i++)
        {
            if (i < currbullets)
            {
                bullets[i].enabled = true;
            }
            else
            {
                bullets[i].enabled = false;
            }

        }
    }
}
