// Copyright Edanoue, Inc. All Rights Reserved.

using Edanoue.Rx;
using UnityEngine;

namespace stepseq
{
    public sealed class EntityStateVis : MonoBehaviour
    {
        [SerializeField]
        private TextMesh m_textHealth = null!;
        
        [SerializeField]
        private TextMesh m_textMaxHealth = null!;
        
        [SerializeField]
        private TextMesh m_textShield = null!;
        
        private PlayerState _playerState = null!;
        
        internal void SetEntityState(PlayerState playerState)
        {
            _playerState = playerState;
            
            // Bind Health
            _playerState.Health
                .Subscribe(this, (x, state) => { state.m_textHealth.text = $"Health: {x}"; })
                .RegisterTo(destroyCancellationToken);
            
            // Bind MaxHealth
            _playerState.MaxHealth
                .Subscribe(this, (x, state) => { state.m_textMaxHealth.text = $"{x}"; })
                .RegisterTo(destroyCancellationToken);
            
            // Bind Shield
            _playerState.Shield
                .Subscribe(this, (x, state) => { state.m_textShield.text = $"Shield: {x}"; })
                .RegisterTo(destroyCancellationToken);
        }
    }
}