// Copyright Edanoue, Inc. All Rights Reserved.

using System;
using System.Runtime.CompilerServices;
using Edanoue.Rx;

namespace stepseq
{
    /// <summary>
    /// </summary>
    public sealed class EntityState : IEntity, IDisposable
    {
        private readonly ReactiveProperty<float> _healthRp    = new();
        private readonly ReactiveProperty<float> _maxHealthRp = new();
        private readonly ReactiveProperty<float> _shieldRp    = new();
        
        private float _addHealthStack;
        private float _addMaxHealthStack;
        private float _addShieldStack;
        private float _subHealthStack;
        private float _subMaxHealthStack;
        private float _subShieldStack;
        
        public ReadOnlyReactiveProperty<float> Health
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _healthRp;
        }
        
        public ReadOnlyReactiveProperty<float> MaxHealth
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _maxHealthRp;
        }
        
        public ReadOnlyReactiveProperty<float> Shield
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _shieldRp;
        }
        
        
        public void Dispose()
        {
            _healthRp.Dispose();
            _maxHealthRp.Dispose();
            _shieldRp.Dispose();
        }
        
        public void AddStack(StackType type, float amount)
        {
            switch (type)
            {
                case StackType.AddHealth:
                {
                    _addHealthStack += amount;
                    break;
                }
                case StackType.AddMaxHealth:
                {
                    _addMaxHealthStack += amount;
                    break;
                }
                case StackType.AddShield:
                {
                    _addShieldStack += amount;
                    break;
                }
                case StackType.SubHealth:
                {
                    _subHealthStack += amount;
                    break;
                }
                case StackType.SubMaxHealth:
                {
                    _subMaxHealthStack += amount;
                    break;
                }
                case StackType.SubShield:
                {
                    _subShieldStack += amount;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        public void Solve(int seed)
        {
            // 現在の値をキャッシュしておく
            var virtualMaxHealth = _maxHealthRp.Value;
            var virtualHealth = _healthRp.Value;
            var virtualShield = _shieldRp.Value;
            var virtualDamage = 0f;
            
            // Solve Shield stack
            virtualShield += _addShieldStack;
            virtualShield -= _subShieldStack;
            _addShieldStack = 0;
            _subShieldStack = 0;
            virtualShield = Math.Max(virtualShield, 0);
            
            // Solve MaxHealth stack
            virtualMaxHealth += _addMaxHealthStack;
            virtualMaxHealth -= _subMaxHealthStack;
            _addMaxHealthStack = 0;
            _subMaxHealthStack = 0;
            virtualMaxHealth = Math.Max(virtualMaxHealth, 0);
            
            // Solve AddHealth stack
            virtualHealth += _addHealthStack;
            _addHealthStack = 0;
            virtualHealth = Math.Min(virtualHealth, virtualMaxHealth);
            
            // Solve SubHealth stack
            virtualDamage += _subHealthStack;
            _subHealthStack = 0;
            
            // シールドがダメージより多いのならばシールドだけ削る
            if (virtualShield >= virtualDamage)
            {
                virtualShield -= virtualDamage;
            }
            // シールドが耐えきれないならば, シールドを削って体力にダメージを与える
            else
            {
                virtualDamage -= virtualShield;
                virtualShield = 0f;
                virtualHealth -= virtualDamage;
            }
            
            // Update Rp
            _healthRp.Value = virtualHealth;
            _maxHealthRp.Value = virtualMaxHealth;
            _shieldRp.Value = virtualShield;
        }
        
        public void Clear()
        {
            _healthRp.Value = 0f;
            _maxHealthRp.Value = 0f;
            _shieldRp.Value = 0;
            _addHealthStack = 0;
            _addMaxHealthStack = 0;
            _addShieldStack = 0;
            _subHealthStack = 0;
            _subMaxHealthStack = 0;
            _subShieldStack = 0;
        }
    }
}