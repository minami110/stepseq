using System;
using System.Runtime.CompilerServices;
using Edanoue.Rx;
using UnityEngine;

namespace stepseq
{
    // スクリプトの実行順序を早める, 中心的なスクリプトでいろんなものから参照されるため
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    public sealed class MousePlayerState : MonoBehaviour
    {
        private static MousePlayerState? _instance;
        
        [SerializeField]
        private Transform m_playerTransform = null!;
        
        
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
        }
        
        private void OnDestroy()
        {
            _instance = null;
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MousePlayerState GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            
            throw new InvalidOperationException("PlayerState is not initialized.");
        }
    }
}