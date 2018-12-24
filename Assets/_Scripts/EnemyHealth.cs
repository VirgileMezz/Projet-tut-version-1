﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {

    [SerializeField] private int startingHealth = 20;
    [SerializeField] private float timeSinceLastHit = 0.5f;
    [SerializeField] private float dissapearSpeed = 2f;
    [SerializeField] private float expGagnable = 0.40f;


    //private AudioSource audio;
    private float timer = 0f;
    private Animator anim;
    private NavMeshAgent nav;
    private bool isAlive;
    private Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;
    private bool dissapearEnemy = false;
    private float currentHealth;   // trop de variableeeeeeeeeeeeeeeee
    private ParticleSystem blood;
    private GameObject player;
    private PlayerController pc;
   



	// Use this for initialization
	void Start () {

        GameManager.instance.RegisterEnemy(this);
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //audio = GetComponent<AudioSource>();
        isAlive = true;
        currentHealth = startingHealth;
        blood = GetComponentInChildren<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (dissapearEnemy)
        {
            transform.Translate(-Vector3.up * dissapearSpeed * Time.deltaTime); // gros gitan je sais mais on reglera ça plus tard bb
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(timer >= timeSinceLastHit && !GameManager.instance.GameOver)
        {
            if (other.tag == "PlayerWeapon")
            {
                Debug.Log("touche arme");
                timer = 0f;
            }
           
        }
    }

    public void takeHit()
    {
        if(currentHealth > 0)
        {
            //audio.PlayOneShot(audio.clip);
            anim.Play("Hurt");
            currentHealth -= pc.getPuissanceAttaque() ;
            blood.Play();
        }

        if(currentHealth <= 0)
        {
            isAlive = false;
            KillEnemy();
        }
    }

    void KillEnemy()
    {
        GameManager.instance.KilledEnemy(this);
        capsuleCollider.enabled = false;
        nav.enabled = false;
        anim.SetTrigger("EnemyDie");
        rigidbody.isKinematic = true;
        blood.Play();

        StartCoroutine(removeEnemy());
    }

    IEnumerator removeEnemy()
    {
        // attendre x sec apres que l'ennemie soit mort
        yield return new WaitForSeconds(4f); // je suis un connard qui utilise yield omfg
        dissapearEnemy = true;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);

    }

   


    public bool IsAlive()
    {
        return isAlive;
    }
    public float getExpGagnable()
    {
        return expGagnable;
    }
}
