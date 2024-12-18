﻿using System;
using CodeBase.Gameplay.Mechanics;
using UnityEngine;

namespace CodeBase.Gameplay.Levels
{
    public class HeartLevel : Level
    {
        public override event Action<bool> OnCompleted;
        public event Action<int> OnHealthChanged;

        [SerializeField] private Task[] _tasks;
        [SerializeField] private int _health;
        [SerializeField] private HeartLevelEmotion _heartLevelEmotion;
        [SerializeField] private DamageTrigger[] _damageTriggers;

        private int _currentTaskIndex;
        
        private void Start()
        {
            _tasks[0].OnComplete += StartNextTask;
            _tasks[0].StartTask();
            _damageTriggers[0].gameObject.SetActive(true);
            
            DragHandler.Instance.OnDamagedClick += DecreaseHealth;
        }
        
        private void DecreaseHealth()
        {
            _health--;
            OnHealthChanged?.Invoke(_health);
            _heartLevelEmotion.StartShow(false);
            print("decrease");

            if (_health <= 0)
            {
                _tasks[_currentTaskIndex].OnComplete -= StartNextTask;
                DragHandler.Instance.OnDamagedClick -= DecreaseHealth;
                DragHandler.Instance.CancelDrag();
                OnCompleted?.Invoke(false);
            }
        }

        private void StartNextTask()
        {
            _tasks[_currentTaskIndex].OnComplete -= StartNextTask;
            _damageTriggers[_currentTaskIndex].gameObject.SetActive(false);
            if (_tasks[_currentTaskIndex] is not TimeDelayTask) 
                _heartLevelEmotion.StartShow(true);
            
            _currentTaskIndex++;
            
            if (_currentTaskIndex < _tasks.Length)
            {
                _tasks[_currentTaskIndex].OnComplete += StartNextTask;
                _tasks[_currentTaskIndex].StartTask();
                _damageTriggers[_currentTaskIndex].gameObject.SetActive(true);
            }
            else
                OnCompleted?.Invoke(true);
        }

        private void OnDestroy() => 
            DragHandler.Instance.OnDamagedClick -= DecreaseHealth;
    }
}
