using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] Camera FPCamera;
    [SerializeField] float Range = 100f;
    [SerializeField] float damage = 20f;
    [SerializeField] ParticleSystem MuzzleFlash;
    [SerializeField] Transform EquippedWeapon;

    [SerializeField]  PlayerController PlayerController;

    [Header("Sway Settings")]
    [SerializeField] private float _smooth;
    [SerializeField] private float _swayMultiplier;

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            PlayMuzzleFlash();
            Shoot();
        }

        GunSway();
    }

    public void PlayMuzzleFlash()
    {
        MuzzleFlash.Play();
    }

    private void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, Range))
        {
            Debug.DrawRay(transform.position, FPCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            EnemyHealth target = hit.transform.GetComponentInParent<EnemyHealth>();
            if(target == null)
            {
                return;
            }
            
            target.TakeDamage(damage);
        }
        else
        {
            return;
        }
    }

    void TiltSway()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * _swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, _smooth * Time.deltaTime);
    }
}
