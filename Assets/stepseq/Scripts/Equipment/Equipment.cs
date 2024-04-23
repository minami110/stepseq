using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider))]
    public class Equipment : MonoBehaviour, IDraggable
    {
        [SerializeField]
        private Shader m_buttonShader = null!;
        
        [SerializeField]
        private Renderer m_buttonRenderer = null!;
        
        private Material _material = null!;
        private Vector3? _dragStartPosition;
        
        private void Awake()
        {
            _material = new Material(m_buttonShader);
            m_buttonRenderer.sharedMaterial = _material;
            SetButtonColor(new Color(0.18f, 0.5f, 0.22f));
        }
        
        private void OnDestroy()
        {
            DestroyImmediate(_material);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (_dragStartPosition.HasValue == false)
            {
                SetButtonColor(new Color(0.48f, 0.39f, 0.09f));
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (_dragStartPosition.HasValue == false)
            {
                SetButtonColor(new Color(0.18f, 0.5f, 0.22f));
            }
        }
        
        void IClickable.LeftClick(bool isPressed)
        {
            if (isPressed)
            {
                if (_dragStartPosition.HasValue == false)
                {
                    _dragStartPosition = transform.position;
                    SetButtonColor(new Color(1f, 0.82f, 0.15f));
                }
            }
            else
            {
                if (_dragStartPosition.HasValue)
                {
                    // ToDO: 購入確定エリア(インベントリ内) なら購入する
                    
                    // 購入キャンセルなので元の場所に戻す
                    transform.position = _dragStartPosition.Value;
                    _dragStartPosition = null;
                    SetButtonColor(new Color(0.18f, 0.5f, 0.22f));
                }
            }
        }
        
        private void Update()
        {
            if (_dragStartPosition.HasValue)
            {
                transform.position = PlayerState.Position;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetButtonColor(in Color color)
        {
            _material.color = color;
        }
    }
}