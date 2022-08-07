using Assets.Helpers;
using UnityEngine;

public class MoveSway : MonoBehaviour
{
    [Header("Movement Bob Settings")]
    [SerializeField] 
    private float _amount;

    [SerializeField] 
    private float _speed;

    [SerializeField] 
    private float _transitionSpeed;

    private Vector3 _origin;
    private float _movementCounter; // Used to determine the position of the swayee in X-Y

    // Start is called before the first frame update
    void Start()
    {
        _origin = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMoveSway();
    }

    private void ApplyMoveSway()
    {
        var playerIsStationary = InputHelper.VerticalAxis == 0 && InputHelper.HorizontalAxis == 0;
        if (!playerIsStationary)
        {
            var targetPosition = HeadBob(_amount, _amount);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * _transitionSpeed);
        }

        Vector3 HeadBob(float horizontalIntensity, float verticalIntensity)
        {
            _movementCounter += Time.deltaTime * _speed;
            return _origin + new Vector3(Mathf.Cos(_movementCounter) * horizontalIntensity, Mathf.Sin(_movementCounter * 2) * verticalIntensity, 0);
        }
    }
}
