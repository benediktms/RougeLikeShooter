using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] Camera FPCamera;
    [SerializeField] float Range = 100f;
    [SerializeField] float damage = 20f;
    [SerializeField] ParticleSystem MuzzleFlash;

    PlayerController PlayerController;

    [Header("Sway Settings")]
    [SerializeField] private float _smooth;
    [SerializeField] private float _swayMultiplier;

    private float _gunSwayDirectionX;
    private float _gunSwayDirectionY;

    private Quaternion _gunSwayRotationX;
    private Quaternion _gunSwayRotationY;

    private Quaternion _targetGunSwayRotation;

    private void OnStart()
    {
        PlayerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            PlayMuzzleFlash();
            Shoot();
        }
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
            // Debug.Log(hit.transform.name + " has been shot");
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

    private void GunSway()
    {
        _gunSwayDirectionX = PlayerController.mouseX * _swayMultiplier;
        _gunSwayDirectionY = PlayerController.mouseY * _swayMultiplier;

        _gunSwayRotationX = Quaternion.AngleAxis(-_gunSwayDirectionY, Vector3.right); // y direction is inverted by default
        _gunSwayRotationY = Quaternion.AngleAxis(_gunSwayDirectionX, Vector3.up);

        _targetGunSwayRotation = _gunSwayRotationX * _gunSwayRotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation,_targetGunSwayRotation, _smooth * Time.deltaTime);
    }
}
