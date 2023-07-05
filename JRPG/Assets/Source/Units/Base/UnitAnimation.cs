using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UnitAnimation : MonoBehaviour
{
    private Animator _animator;

    private const string _runParametr = "Run";
    private const string _attackParametr = "Attack";
    private const string _deathParametr = "Death";


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartRun()
    {
        _animator.SetBool(_runParametr, true);
    }

    public void StopRun()
    {
        _animator.SetBool(_runParametr, false);
    }

    public void Attack()
    {
        _animator.SetTrigger(_attackParametr);
    }

    public Coroutine Death()
    {
        return StartCoroutine(StartDeath());
    }

    private IEnumerator StartDeath()
    {
        _animator.SetTrigger(_deathParametr);
        yield return new WaitForSecondsRealtime(_animator.GetCurrentAnimatorStateInfo(0).length + 1);
    }
}
