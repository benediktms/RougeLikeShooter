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

    [SerializeField] PlayerController PlayerController;

    private float movementCounter;
    private float idleCounter;

    private Vector3 _weaponOrigin;
    private Vector3 _targetWeaponBobPosition;

    [Header("Mouse Look Sway Settings")]
    [SerializeField] private float _smooth;
    [SerializeField] private float _swayMultiplier;

    [Header("Idle Bob Settings")]
    [SerializeField] private float _idleGunSway;
    [SerializeField] private float _idleSwaySpeed;
    [SerializeField] private float _moveToIdleTransition;

    [Header("Movement Bob Settings")]
    [SerializeField] private float _movingGunSway;
    [SerializeField] private float _movingGunSwaySpeed;
    [SerializeField] private float _idleToMoveTransition;

    private void Start()
    {
        _weaponOrigin = EquippedWeapon.localPosition;
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            PlayMuzzleFlash();
            Shoot();
        }

        RotationSway();
        HeadBobController();
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

    void RotationSway()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * _swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, _smooth * Time.deltaTime);
    }

    void HeadBobController()
    {
        if(PlayerController.mouseX == 0 && PlayerController.mouseY == 0)
        {
            HeadBob(idleCounter, _idleGunSway,_idleGunSway);
            idleCounter += Time.deltaTime * _idleSwaySpeed;
            EquippedWeapon.localPosition = Vector3.Lerp(EquippedWeapon.localPosition, _targetWeaponBobPosition, Time.deltaTime * _moveToIdleTransition); // control lerp from movement to idle bob, smooth
        }
        else
        {
            HeadBob(movementCounter, _movingGunSway, _movingGunSway);
            movementCounter += Time.deltaTime * _movingGunSwaySpeed;
            EquippedWeapon.localPosition = Vector3.Lerp(EquippedWeapon.localPosition, _targetWeaponBobPosition, Time.deltaTime * _idleToMoveTransition); // control lerp from idle to movement bob, harsh
        }
    }

    void HeadBob(float location_z, float x_intensity, float y_intensity)
    {
        _targetWeaponBobPosition = _weaponOrigin + new Vector3(Mathf.Cos(location_z) * x_intensity, Mathf.Sin(location_z * 2) * y_intensity,0); // sin starts from the origin and goes up and down on axis
    }
}
