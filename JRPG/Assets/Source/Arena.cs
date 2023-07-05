using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    public Squad PlayerSquad { get; private set; }
    public Squad EnemySquad { get; private set; }

    public static Arena Singleton { get; private set; }

    public static event Action StartBattle;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(this);

        PlayerSquad = FindObjectOfType<PlayerSquad>();
        EnemySquad = FindObjectOfType<EnemySquad>();


        Squad.AllUnitDefeated += CheckSqaudSide;
    }

    private void OnDestroy()
    {
        Squad.AllUnitDefeated -= CheckSqaudSide;
    }

    private void Start()
    {
        StartBattle?.Invoke();
    }

    private void CheckSqaudSide(Squad.SquadSide squadSide)
    {
        if (squadSide == Squad.SquadSide.PLAYER)
            ShowLoseMessage();
        else
            ShowWinMessage();
    }

    private void ShowLoseMessage()
    {
        Debug.Log("Вы проиграли!");
    }

    private void ShowWinMessage()
    {
        Debug.Log("Вы победили!");
    }
}
