using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionPanel : MonoBehaviour
{
    [SerializeField] private UnitActionTimeInfo _unitActionTimeInfoPrefab;

    private List<UnitActionTimeInfo> _unitActionTimeInfoBlocks = new List<UnitActionTimeInfo>();

    public void CreateInfoBlock(Unit unit)
    {
        var block = Instantiate(_unitActionTimeInfoPrefab);
        block.transform.SetParent(transform);

        block.Unit = unit;
        block.ActionTimeText.text = unit.CurrentActionTime.ToString();
        block.UnitAvatar.sprite = unit.UnitUI.Avatar;

        _unitActionTimeInfoBlocks.Add(block);
    }

    public void UpdateActionTime()
    {
        foreach (var block in _unitActionTimeInfoBlocks)
        {
            block.ActionTimeText.text = block.Unit.CurrentActionTime.ToString();
        }
    }

    public void RemoveInfoBlock(Unit unit)
    {
        var block = Array.Find(_unitActionTimeInfoBlocks.ToArray(), block => block.Unit == unit);

        _unitActionTimeInfoBlocks.Remove(block);
        Destroy(block.gameObject);
    }
}
