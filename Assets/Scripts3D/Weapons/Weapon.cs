using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    protected Camera Camera;

    [SerializeField]
    protected Transform AttackPoint;

    [SerializeField]
    protected GameObject Round;

    [SerializeField]
    protected float Damage;
    private float movementCounter;
    private float idleCounter;

    [Header("Mouse Look Sway Settings")]
    [SerializeField] private float _smooth;
    [SerializeField] private float _swayMultiplier;

    [SerializeField]
    protected float Range;

    [SerializeField]
    protected float ShootForce;

    protected abstract void Fire();
    [Header("Bob Settings")]
    private Vector3 _weaponOrigin;
    private Vector3 _targetWeaponBobPosition;

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
        RotationSway();
        HeadBobController();
    }

    public void PlayMuzzleFlash()
    {
        MuzzleFlash.Play();
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
