using System;
using System.Runtime.CompilerServices;
using Edanoue.Rx;
using UnityEngine;

namespace stepseq
{
    // スクリプトの実行順序を早める, 中心的なスクリプトでいろんなものから参照されるため
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    public sealed class PlayerState : MonoBehaviour
    {
        private static PlayerState? _instance;
        
        [SerializeField]
        private Transform m_playerTransform = null!;
        
        [SerializeField]
        [Range(0, 1000)]
        private int m_awakeMoney = 100;
        
        [SerializeField]
        [Range(0, 100)]
        private float m_awakeMaxHealth = 100f;
        
        private readonly ReactiveProperty<float> _healthRp    = new();
        private readonly ReactiveProperty<float> _maxHealthRp = new();
        private readonly ReactiveProperty<int>   _moneyRp     = new();
        
        /// <summary>
        /// The Money property represents the amount of money held by the player.
        /// </summary>
        /// <value>
        /// The amount of money held by the player.
        /// </value>
        public static ReadOnlyReactiveProperty<int> Money
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetInstance()._moneyRp;
        }
        
        /// <summary>
        /// The MaxHealth property represents the maximum health value that a player can have.
        /// </summary>
        /// <value>
        /// The maximum health value.
        /// </value>
        /// <remarks>
        /// This property is read-only and can be accessed through the PlayerState class.
        /// </remarks>
        public static ReadOnlyReactiveProperty<float> MaxHealth
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetInstance()._maxHealthRp;
        }
        
        public static ReadOnlyReactiveProperty<float> Health
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetInstance()._healthRp;
        }
        
        public static Vector3 Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetInstance().m_playerTransform.position;
        }
        
        private void Awake()
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("PlayerState is a singleton and should only be initialized once.");
            }
            
            _instance = this;
            
            // Initialize the reactive properties.
            _maxHealthRp.Value = m_awakeMaxHealth;
            _healthRp.Value = m_awakeMaxHealth;
            _moneyRp.Value = m_awakeMoney;
            _maxHealthRp.RegisterTo(destroyCancellationToken);
            _healthRp.RegisterTo(destroyCancellationToken);
            _moneyRp.RegisterTo(destroyCancellationToken);
        }
        
        public static void AddMoney(int amount)
        {
            // ToDo: 現時点では負の値も許容する
            GetInstance()._moneyRp.Value += amount;
        }
        
        public static void AddHealth(float amount)
        {
            var instance = GetInstance();
            
            var newHealth = instance._healthRp.Value + amount;
            if (newHealth > instance._maxHealthRp.Value)
            {
                instance._healthRp.Value = instance._maxHealthRp.Value;
            }
            else if (newHealth < 0)
            {
                instance._healthRp.Value = 0;
                instance.OnDead();
            }
            else
            {
                instance._healthRp.Value = newHealth;
            }
        }
        
        private void OnDead()
        {
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static PlayerState GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            
            throw new InvalidOperationException("PlayerState is not initialized.");
        }
    }
}