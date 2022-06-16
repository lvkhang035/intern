using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandgunFire : MonoBehaviour
{
    public GameObject theGun;
    public GameObject muzzleFlash;
    public AudioSource gunFire;
    public bool isFiring = false;
    public AudioSource emptySound;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(GlobalAmmo.handgunAmmo < 1)
            {
                emptySound.Play();
            }
            else
            {
                if (isFiring == false)
                 {
                    StartCoroutine(FiringHandgun());
                 }
            } 
        }
    }
    IEnumerator FiringHandgun()
    {
        isFiring = true;
        GlobalAmmo.handgunAmmo -= 1;
        theGun.GetComponent<Animator>().Play("HandgunFire");
        muzzleFlash.SetActive(true);
        gunFire.Play();
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        theGun.GetComponent<Animator>().Play("New State");
        isFiring = false;
    }
}
