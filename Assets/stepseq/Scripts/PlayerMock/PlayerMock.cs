using System;
using System.Runtime.CompilerServices;
using Edanoue.Rx;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    public sealed class PlayerMock : MonoBehaviour
    {
        [SerializeField]
        private EntityStateVis m_entityStateVis = null!;
        
        [SerializeField]
        private Transform m_animateRoot = null!;
        
        [SerializeField]
        private Shader m_shader = null!;
        
        [SerializeField]
        private Renderer m_renderer = null!;
        
        private Material _material = null!;
        
        public EntityState State
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = new();
        
        private void Awake()
        {
            m_entityStateVis.SetEntityState(State);
            State.RegisterTo(destroyCancellationToken);
            
            _material = new Material(m_shader);
            m_renderer.sharedMaterial = _material;
        }
        
        // Update is called once per frame
        private void Update()
        {
            // 普段はゆるく 回転し続ける
            m_animateRoot.Rotate(Vector3.right, 10f * Time.deltaTime);
            m_animateRoot.Rotate(Vector3.up, 3f * Time.deltaTime);
            m_animateRoot.Rotate(Vector3.forward, 1f * Time.deltaTime);
        }
        
        private void OnDestroy()
        {
            DestroyImmediate(_material);
        }
        
        internal void SetBaseColor(in Color color)
        {
            _material.color = color;
        }
    }
}