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

    [SerializeField]
    protected float Range;

    [SerializeField]
    protected float ShootForce;

    protected abstract void Fire();
    [Header("Bob Settings")]
    private Vector3 _weaponOrigin;

    private void Start()
    {
        _weaponOrigin = EquippedWeapon.localPosition;
    }

    private void Update()
    {
        RotationSway();
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

    // void HeadBob(float location_z, float x_intensity, float y_intensity)
    // {
    //     _weaponOrigin.localPosition = new Vector3 (Mathf.Cos(location_z) * x_intensity, Mathf.Sin(location_z) * y_intensity,EquippedWeapon.z); // sin starts from the origin and goes up and down on axis
    // }
}
