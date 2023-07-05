using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UnitMover), typeof(UnitAnimation), typeof(UnitAI))]
[RequireComponent(typeof(UnitUI))]
public abstract class Unit : MonoBehaviour, IPointerClickHandler
{
    public enum UnitSide
    {
        PLAYER,
        ENEMY
    }

    [SerializeField] [Min(1)] private int _maxHP;
    [SerializeField] [Min(0)] protected int _damage;
    [SerializeField] [Min(0)] protected int _defense;
    [SerializeField] [Min(1)] protected int _actionDelay;
    [SerializeField] [Min(0)] protected float _attackRange;

    [HideInInspector] public UnitSide Side;

    protected UnitAnimation _animation;
    protected UnitMover _mover;
    protected int _currentHP;

    private UnitAI _unitAI;

    public int CurrentActionTime { get; private set; }
    public UnitUI UnitUI { get; private set; }

    private static bool _waitingAction = false;

    public static event Action<Unit> ActionReady;
    public static event Action EndUnitTurn;
    public static event Action<Unit> DeathUnit;
    public static event Action<Unit> ChoisingUnit;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _animation = GetComponent<UnitAnimation>();
        _mover = GetComponent<UnitMover>();
        UnitUI = GetComponent<UnitUI>();
        _unitAI = GetComponent<UnitAI>();

        _currentHP = _maxHP;
        UnitUI.UpdateHPBar(_maxHP, _currentHP);

        Arena.StartBattle += WaitingAction;
        EndUnitTurn += WaitingAction;
    }

    private void OnDestroy()
    {
        Arena.StartBattle -= WaitingAction;
        EndUnitTurn -= WaitingAction;
    }

    private void WaitingAction()
    {
        StartCoroutine(Waiting());
    }
    private IEnumerator Waiting()
    {
        while (_waitingAction == false)
        {
            CurrentActionTime += 1;

            if (CurrentActionTime >= _actionDelay)
            {
                _waitingAction = true;
                CurrentActionTime = 0;

                ActionReady?.Invoke(this);

                if (Side != UnitSide.PLAYER)
                    StartCoroutine(AIAttack());
            }

            yield return null;
        }
    }

    protected abstract IEnumerator UnitBaseAttack(Unit target);
    protected abstract IEnumerator UnitUseSkill(Unit target);


    public Coroutine BaseAttack(Unit target)
    {
        UnitUI.ToggleTurnUnitArrow(false);
        target.UnitUI.ToggleChoisingUnitArrow(false);
        
        StartCoroutine(UnitBaseAttack(target));

        _waitingAction = false;
        EndUnitTurn?.Invoke();

        return null;
    }

    public Coroutine UseSkill(Unit target)
    {
        UnitUI.ToggleTurnUnitArrow(false);
        target.UnitUI.ToggleChoisingUnitArrow(false);

        StartCoroutine(UnitUseSkill(target));

        _waitingAction = false;
        EndUnitTurn?.Invoke();

        return null;
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            throw new InvalidOperationException();

        _currentHP -= Mathf.Max(damage - _defense, 0);

        UnitUI.UpdateHPBar(_maxHP, _currentHP);

        if (_currentHP <= 0)
        {
            _animation.Death();
            DeathUnit?.Invoke(this);
            Destroy(gameObject);
        }
    }

    private IEnumerator AIAttack()
    {
        yield return _unitAI.AIAttack(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ChoisingUnit?.Invoke(this);
    }
}
