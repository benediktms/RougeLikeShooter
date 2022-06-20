using Assets.Helpers;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected Camera Camera;

    [SerializeField]
    protected Transform AttackPoint;
    [SerializeField] Camera FPCamera;
    [SerializeField] ParticleSystem MuzzleFlash;
    [SerializeField] Transform EquippedWeaponLocation;
    [SerializeField] PlayerController PlayerController;

    [SerializeField]
    protected GameObject Round;

    [SerializeField]
    protected float Damage;
    private float _idleCounter;
    private float _movementCounter;

    [Header("Mouse Look Sway Settings")]
    [SerializeField] private float _smooth;
    [SerializeField] private float _swayMultiplier;

    [SerializeField]
    protected float Range;

    [SerializeField]
    protected float ShootForce;

    [Header("Bob Settings")]
    private Vector3 _weaponOrigin;
    private Vector3 _targetWeaponBobPosition;

    [Header("Idle Bob Settings")]
    [SerializeField] private float _idlePlayerWeaponSway;
    [SerializeField] private float _idlePlayerWeaponSwaySpeed;
    [SerializeField] private float _playerMoveToIdleTransitionSpeed;

    [Header("Movement Bob Settings")]
    [SerializeField] private float _movingPlayerGunSway;
    [SerializeField] private float _movingPlayerGunSwaySpeed;
    [SerializeField] private float _idlePlayerToMoveTransitionSpeed;

    private void Start()
    {
        _weaponOrigin = EquippedWeaponLocation.localPosition;
    }

    private void Update()
    {
        ApplyRotationSway();
        ApplyPlayerHeadBobMovement();
    }

    public void PlayMuzzleFlash()
    {
        MuzzleFlash.Play();
    }

    void ApplyRotationSway()
    {
        float mouseX = InputHelper.VerticalAxis * _swayMultiplier;
        float mouseY = InputHelper.HorizontalAxis * _swayMultiplier;

        Debug.Log(mouseX);

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, _smooth * Time.deltaTime);
    }

    void ApplyPlayerHeadBobMovement()
    {
        var playerIsStationary = InputHelper.VerticalAxis == 0 && InputHelper.HorizontalAxis == 0;

        if(playerIsStationary)
        {
            HeadBob(_idleCounter, _idlePlayerWeaponSway,_idlePlayerWeaponSway);
            _idleCounter += Time.deltaTime * _idlePlayerWeaponSwaySpeed;
            EquippedWeaponLocation.localPosition = Vector3.Lerp(EquippedWeaponLocation.localPosition, _targetWeaponBobPosition, Time.deltaTime * _playerMoveToIdleTransitionSpeed); // control lerp from movement to idle bob, smooth
        }
        else
        {
            HeadBob(_movementCounter, _movingPlayerGunSway, _movingPlayerGunSway);
            _movementCounter += Time.deltaTime * _movingPlayerGunSwaySpeed;
            EquippedWeaponLocation.localPosition = Vector3.Lerp(EquippedWeaponLocation.localPosition, _targetWeaponBobPosition, Time.deltaTime * _idlePlayerToMoveTransitionSpeed); // control lerp from idle to movement bob, harsh
        }
    }

    void HeadBob(float playerActionHeadBobType, float LeftToRightSwayLoopIntensity, float UpToDownSwayLoopIntensity)
    {
        _targetWeaponBobPosition = _weaponOrigin + new Vector3(Mathf.Cos(playerActionHeadBobType) * LeftToRightSwayLoopIntensity, Mathf.Sin(playerActionHeadBobType * 2) * UpToDownSwayLoopIntensity,0); // sin starts from the origin and goes up and down on axis
    }
}
