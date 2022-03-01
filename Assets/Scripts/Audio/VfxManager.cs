using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VfxManager : MonoBehaviour
{
    private static VfxManager _instance ;
   public float oldvolume;
    public static VfxManager instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Audio Manager is Null");
            }

            return _instance;
        }
        
    }
    public  AudioSource Player;

    public AudioClip shoot;

    public AudioClip hit;

    public AudioClip death;

    public AudioClip health;

    public AudioClip Click;

    public AudioClip toggle;

    public AudioClip sheildHit;
    private void Awake()
    {

        Player = GetComponent<AudioSource>();
        if (_instance == null)
        {
            _instance = this;
         
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            
        }

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        oldvolume = Player.volume;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

   
    }

