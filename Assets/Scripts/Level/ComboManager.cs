using System.Collections;
using System.Collections.Generic;
using UnityUtils.BaseClasses;
using UnityEngine;

namespace Level
{
    public class ComboManager : MonoBehaviour
    {
        [SerializeField] private int _currentComboAmount = 0;
        public bool IsComboStreak { get; set; }

        private void OnEnable()
        {
            MiniEventSystem.OnMidCellFill += IncreaseCombo;
        }

        private void OnDisable()
        {
            MiniEventSystem.OnMidCellFill -= IncreaseCombo;
        }

        public void IncreaseCombo()
        {
            _currentComboAmount++;

            if (_currentComboAmount >= 2)
                MiniEventSystem.OnComboIncrease?.Invoke();
        }

        public void ResetCombo()
        {
            _currentComboAmount = 0;
        }

        public int GetCurrentComboAmount => _currentComboAmount;
    }
}
