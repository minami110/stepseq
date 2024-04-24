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
        // DeBuff stack
        private readonly ReactiveProperty<float> _healthDamageRp = new();
        
        // Base stack
        private readonly ReactiveProperty<float> _healthRp       = new();
        private readonly ReactiveProperty<float> _lifeSteal      = new();
        private readonly ReactiveProperty<float> _luckRp         = new();
        private readonly ReactiveProperty<float> _poisonRp       = new();
        private readonly ReactiveProperty<float> _shieldDamageRp = new();
        
        // Buff stack
        private readonly ReactiveProperty<float> _shieldRp = new();
        
        public ReadOnlyReactiveProperty<float> Health
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _healthRp;
        }
        
        public ReadOnlyReactiveProperty<float> Shield
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _shieldRp;
        }
        
        public ReadOnlyReactiveProperty<float> Luck
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _luckRp;
        }
        
        public ReadOnlyReactiveProperty<float> LifeSteal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _lifeSteal;
        }
        
        public ReadOnlyReactiveProperty<float> Poison
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _poisonRp;
        }
        
        public void Dispose()
        {
            // Base
            _healthRp.Dispose();
            // Buff
            _shieldRp.Dispose();
            _luckRp.Dispose();
            _lifeSteal.Dispose();
            // DeBuff
            _healthDamageRp.Dispose();
            _shieldDamageRp.Dispose();
            _poisonRp.Dispose();
        }
        
        public void AddStack(StackType type, float amount)
        {
            switch (type)
            {
                // Buff
                case StackType.Health:
                {
                    var newHealth = _healthRp.Value + amount;
                    newHealth = Math.Max(0, newHealth);
                    _healthRp.Value = newHealth;
                    break;
                }
                case StackType.Shield:
                {
                    var newShield = _shieldRp.Value + amount;
                    newShield = Math.Max(0, newShield);
                    _shieldRp.Value = newShield;
                    break;
                }
                case StackType.Luck:
                {
                    var newLuck = _luckRp.Value + amount;
                    newLuck = Math.Max(0, newLuck);
                    _luckRp.Value = newLuck;
                    break;
                }
                case StackType.LifeSteal:
                {
                    var newLifeSteal = _lifeSteal.Value + amount;
                    newLifeSteal = Math.Max(0, newLifeSteal);
                    _lifeSteal.Value = newLifeSteal;
                    break;
                }
                // DeBuff
                case StackType.HealthDamage:
                {
                    var newHealthDamage = _healthDamageRp.Value + amount;
                    newHealthDamage = Math.Max(0, newHealthDamage);
                    _healthDamageRp.Value = newHealthDamage;
                    break;
                }
                case StackType.ShieldDamage:
                {
                    var newShieldDamage = Math.Max(0, _shieldDamageRp.Value + amount);
                    _shieldDamageRp.Value = newShieldDamage;
                    break;
                }
                case StackType.Poison:
                {
                    var newPoison = _poisonRp.Value + amount;
                    newPoison = Math.Max(0, newPoison);
                    _poisonRp.Value = newPoison;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        public void SolveHealth(int seed)
        {
            // Solve shield damage
            var newShield = _shieldRp.Value - _shieldDamageRp.Value;
            _shieldDamageRp.Value = 0;
            _shieldRp.Value = Math.Max(0, newShield);
            
            // Solve health damage
            var healthDamage = _healthDamageRp.Value;
            _healthDamageRp.Value = 0;
            
            // Solve poison
            healthDamage += _poisonRp.Value;
            
            // Solve shield and health
            var currentShield = _shieldRp.Value;
            // シールドがダメージより多いのならばシールドだけ削る
            if (currentShield >= healthDamage)
            {
                _shieldRp.Value = currentShield - healthDamage;
            }
            // シールドが耐えきれないならば, シールドを削って体力にダメージを与える
            else
            {
                _shieldRp.Value = 0;
                healthDamage -= currentShield;
                _healthRp.Value -= healthDamage;
            }
        }
        
        public void Clear()
        {
            // Base
            _healthRp.Value = 0;
            // Buff
            _shieldRp.Value = 0;
            _luckRp.Value = 0;
            _lifeSteal.Value = 0;
            // DeBuff
            _healthDamageRp.Value = 0;
            _shieldDamageRp.Value = 0;
            _poisonRp.Value = 0;
        }
    }
}