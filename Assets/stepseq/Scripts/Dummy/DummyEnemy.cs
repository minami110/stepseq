using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    public class DummyEnemy : MonoBehaviour, IEntity
    {
        private static DummyEnemy? _instance;
        
        [SerializeField]
        private DummyLog m_logger = null!;
        
        private readonly EntityState _entityState = new();
        
        private void Awake()
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("HintBox is a singleton and should only be initialized once.");
            }
            
            _instance = this;
            m_logger.Log("");
        }
        
        // Update is called once per frame
        private void Update()
        {
            // 普段はゆるく 回転し続ける
            transform.Rotate(Vector3.right, 10f * Time.deltaTime);
            transform.Rotate(Vector3.up, 3f * Time.deltaTime);
            transform.Rotate(Vector3.forward, 1f * Time.deltaTime);
        }
        
        private void OnDestroy()
        {
            _entityState.Dispose();
            _instance = null;
        }
        
        void IEntity.AddStack(StackType type, float amount)
        {
            _entityState.AddStack(type, amount);
        }
        
        void IEntity.SolveHealth(int seed)
        {
            _entityState.SolveHealth(seed);
        }
        
        // Note: internal にしているのは Debug 用途
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static DummyEnemy GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            
            throw new InvalidOperationException("HintBox is not initialized.");
        }
    }
}