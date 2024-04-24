using System;
using System.Runtime.CompilerServices;
using Edanoue.Rx;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    public class PlayerMock : MonoBehaviour, IEntity
    {
        [SerializeField]
        private RuntimeLogger m_logger = null!;
        
        [SerializeField]
        private Transform m_animateRoot = null!;
        
        private readonly EntityState _entityState = new();
        
        private void Awake()
        {
            m_logger.Log("");
            _entityState.RegisterTo(destroyCancellationToken);
        }
        
        // Update is called once per frame
        private void Update()
        {
            // 普段はゆるく 回転し続ける
            m_animateRoot.Rotate(Vector3.right, 10f * Time.deltaTime);
            m_animateRoot.Rotate(Vector3.up, 3f * Time.deltaTime);
            m_animateRoot.Rotate(Vector3.forward, 1f * Time.deltaTime);
        }
        
        void IEntity.AddStack(StackType type, float amount)
        {
            _entityState.AddStack(type, amount);
        }
        
        void IEntity.SolveHealth(int seed)
        {
            _entityState.SolveHealth(seed);
        }
    }
}