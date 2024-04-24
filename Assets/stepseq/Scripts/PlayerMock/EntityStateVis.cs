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
        private TextMesh m_textShield = null!;
        
        private EntityState _entityState = null!;
        
        internal void SetEntityState(EntityState entityState)
        {
            _entityState = entityState;
            
            // Bind Health
            _entityState.Health
                .Subscribe(this, (x, state) => { state.m_textHealth.text = $"Health: {x}"; })
                .RegisterTo(destroyCancellationToken);
            
            // Bind Shield
            _entityState.Shield
                .Subscribe(this, (x, state) => { state.m_textShield.text = $"Shield: {x}"; })
                .RegisterTo(destroyCancellationToken);
        }
    }
}