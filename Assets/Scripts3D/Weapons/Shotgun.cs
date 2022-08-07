using Assets.Helpers;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] 
    private int _pelletsPerShell = 10;

    [SerializeField] 
    private float _spread = 2;
        
    void Update()
    {
        if (InputHelper.PrimaryFire)
        {
            Fire();
        }
    }

    private void Fire()
    {
        var direction = GetFireDirection();
        var bullets = GetBullets(direction);
        FireBullets(bullets);
    }

    private Vector3 GetFireDirection()
    {
        var ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        Vector3 targetPoint = Physics.Raycast(ray, out var hit) ? hit.point : ray.GetPoint(Range); // range -> ~75?
        return targetPoint - AttackPoint.position;
    }

    private GameObject[] GetBullets(Vector3 centreDirection)
    {
        GameObject[] bullets = new GameObject[_pelletsPerShell];
        for (var i = 0; i < _pelletsPerShell; ++i)
        {
            var bullet = Instantiate(Round, AttackPoint.position, Quaternion.identity);
            var bulletDirection = ApplySpread(centreDirection);
            bullet.transform.forward = bulletDirection;
            bullets[i] = bullet;
        }

        return bullets;
    }

    private Vector3 ApplySpread(Vector3 directionWithoutSpread)
    {
        if (_spread == 0)
        {
            return directionWithoutSpread;
        }

        float xSpread = Random.Range(-_spread, _spread);
        float ySpread = Random.Range(-_spread, _spread);
        var shotSpread = new Vector3(xSpread, ySpread);

        return directionWithoutSpread + shotSpread;
    }

    private void FireBullets(GameObject[] bullets)
    {
        foreach (var bullet in bullets)
        {
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward.normalized * ShootForce, ForceMode.Impulse);
        }

    }
}
