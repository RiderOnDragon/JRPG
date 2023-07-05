using System.Collections;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Vector3 _stayPoint;

    private void Awake()
    {
        _stayPoint = transform.position;
    }

    public Coroutine Move(Transform target, float distance)
    {
        return StartCoroutine(MoveTo(target.position, distance));
    }

    public Coroutine MoveToStayPoint()
    {
        return StartCoroutine(MoveTo(_stayPoint, 0));
    }

    private IEnumerator MoveTo(Vector3 target, float distance)
    {
        while (Vector3.Distance(transform.position, target) > distance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, _speed);

            yield return null;
        }

    }
}
