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
        
        [SerializeField]
        private Shader m_shader;
        
        private void Awake()
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("HintBox is a singleton and should only be initialized once.");
            }
            
            _instance = this;
            m_logger.Log("");
        }
        
        private void OnDestroy()
        {
            _instance = null;
        }
        
        // Update is called once per frame
        private void Update()
        {
            // 普段はゆるく 回転し続ける
            transform.Rotate(Vector3.right, 10f * Time.deltaTime);
            transform.Rotate(Vector3.up, 3f * Time.deltaTime);
            transform.Rotate(Vector3.forward, 1f * Time.deltaTime);
        }
        
        public static void TakeDamage(float damage)
        {
            // ToDo:
            GetInstance().m_logger.Log($"{damage}");
        }
        
        void IEntity.TakeDamage(float damage)
        {
            throw new NotImplementedException();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DummyEnemy GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            
            throw new InvalidOperationException("HintBox is not initialized.");
        }
    }
}