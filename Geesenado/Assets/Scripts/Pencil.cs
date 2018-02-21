﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers;
using UnityEngine;

namespace Assets.Scripts
{
    public class Pencil : MonoBehaviour, IWeapon
    {
        // Bodies
        public Rigidbody2D playerBody;
        public Rigidbody2D pencilBody;
        public SpriteRenderer pencilSprite;
        public GameObject pencilPrefab;

        // Location
        float timeToFire = 0;

        // Timing
        private static float TIMEOUT = .8f;
        private bool isLive;
        private float countdown;

        public Pencil()
        {
            MaxAmmo = 1;
            Ammo = 1;
        }

        public int Ammo
        {
            get { return 1; }
            set { if (value != 1) Ammo = 1; }
        }

        public int MaxAmmo
        {
            get { return 1; }
            set
            {
                // Melee Weapons always have an ammmo value of 1.
                if (value != 1) MaxAmmo = 1;
            }
        }

        public float Damage
        {
            get { return 0.1f; }
            set { if (value != 0.1f) Damage = 0.1f; }
        }

        /** <summary>Returns the time left for pencil that is firing</summary> */
        public float Countdown
        {
            get { return countdown; }
            private set { countdown = value; }
        }

        /** <summary>Returns if the Pencil is currently Fire()ing</summary> */
        public bool IsLive
        {
            get { return isLive; }
            private set { isLive = value; }
        }


        public void Awake()
        {
        }

        public void Start()
        {
            this.MaxAmmo = 1;
            this.Ammo = 1;
            pencilBody.GetComponent<SpriteRenderer>().enabled = false;
            this.IsLive = false;
            this.Countdown = TIMEOUT;
            Physics2D.IgnoreCollision(playerBody.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        }

        /**
         * <summary>Spawns a new pencil object in 
         * the direction of that the character is facing</summary>
         * 
         */
        public void Fire(float damagePoints = 0.1f, Constants.DamageType damageType = Constants.DamageType.Static)
        {
            Debug.Log("Pencil- received Fire() command - JG");

            this.isLive = true;

            Vector2 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector2 directionToMouse = new Vector2(
                mousePosition.x - transform.position.x,
                mousePosition.y - transform.position.y
            );
            /*
              transform.up = direction;
              pencilBody.GetComponent<SpriteRenderer>().enabled = true;

              */
            if (countdown <= 0)
            {
                this.Countdown = TIMEOUT;
                var pencilFab = (GameObject)Instantiate(
                    pencilPrefab,
                    playerBody.position,
                    playerBody.transform.rotation
                );
                pencilFab.GetComponent<Rigidbody2D>().transform.up = directionToMouse;

                Destroy(pencilFab, .75f);
            }
        }

        public void FixedUpdate()
        {
            if (isLive)
            {
                /*
                pencilBody.GetComponent<SpriteRenderer>().enabled = true;
                Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPos,
                    5 * Time.deltaTime
                );
                
                */
                this.Countdown -= Time.deltaTime;
                if (countdown <= 0)
                {
                    // Time's up
                    Debug.Log("Pencil- countdown complete");
                    isLive = false;
                }
            }

            pencilBody.position = new Vector2(playerBody.position.x, playerBody.position.y);

        }

        public void LateUpdate()
        {
        }


    }
}
