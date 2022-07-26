using Assets.Helpers;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    protected Transform AttackPoint;
    [SerializeField] public Camera Camera;
    [SerializeField] ParticleSystem MuzzleFlash;
    [SerializeField] Transform EquippedWeaponParent;
    [SerializeField] PlayerController PlayerController;

    [SerializeField]
    protected GameObject Round;

    [SerializeField]
    protected float Damage;
    private float _idleCounter;
    private float _movementCounter;

    private Vector3 _weaponOrigin;
    private Vector3 _targetWeaponBobPosition;

    [Header("Mouse Look Sway Settings")]
    [SerializeField] private float _smooth = 5;
    [SerializeField] private float _swayMultiplier = 10;

    [SerializeField]
    protected float Range;

    [SerializeField]
    protected float ShootForce;

    [Header("Bob Settings")]
    private Vector3 _weaponParentOrigin;
    private Vector3 _targetWeaponParentBobPosition;

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
        _weaponParentOrigin = EquippedWeaponParent.localPosition;
    }

    // private void Update()
    // {
    //     ApplyRotationSway();
    //     ApplyPlayerHeadBobMovement();
    // }

    public virtual void ApplyRotationSway()
    {
        var inputX = InputHelper.MouseXAxis > 0.5 ? InputHelper.MouseXAxis : 0;
        var inputY = InputHelper.MouseYAxis > 0.5 ? InputHelper.MouseYAxis : 0;
        if(inputX == 0 && inputY == 0) { return; }
        float mouseX = inputX * _swayMultiplier;
        float mouseY = inputY * _swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(-mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, _smooth * Time.deltaTime);

        Debug.Log("Input X - " + inputX);
        Debug.Log("Input X - " + inputY);

        Debug.Log("Target rotation" + targetRotation);
        Debug.Log("Local rotation" + transform.localRotation);

    }

    public virtual void ApplyPlayerHeadBobMovement()
    {
        var playerIsStationary = InputHelper.VerticalAxis == 0 && InputHelper.HorizontalAxis == 0;

        if(playerIsStationary)
        {
            HeadBob(_idleCounter, _idlePlayerWeaponSway,_idlePlayerWeaponSway);
            _idleCounter += Time.deltaTime * _idlePlayerWeaponSwaySpeed;
            EquippedWeaponParent.localPosition = Vector3.Lerp(EquippedWeaponParent.localPosition, _targetWeaponParentBobPosition, Time.deltaTime * _playerMoveToIdleTransitionSpeed); // control lerp from movement to idle bob, smooth
        }
        else
        {
            HeadBob(_movementCounter, _movingPlayerGunSway, _movingPlayerGunSway);
            _movementCounter += Time.deltaTime * _movingPlayerGunSwaySpeed;
            EquippedWeaponParent.localPosition = Vector3.Lerp(EquippedWeaponParent.localPosition, _targetWeaponParentBobPosition, Time.deltaTime * _idlePlayerToMoveTransitionSpeed); // control lerp from idle to movement bob, harsh
        }
    }

    void HeadBob(float playerActionHeadBobType, float LeftToRightSwayLoopIntensity, float UpToDownSwayLoopIntensity)
    {
        _targetWeaponParentBobPosition = _weaponParentOrigin + new Vector3(Mathf.Cos(playerActionHeadBobType) * LeftToRightSwayLoopIntensity, Mathf.Sin(playerActionHeadBobType * 2) * UpToDownSwayLoopIntensity,0); // sin starts from the origin and goes up and down on axis
    }
}
