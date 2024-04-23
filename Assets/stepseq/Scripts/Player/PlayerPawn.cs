// Copyright Edanoue, Inc. All Rights Reserved.

using System;
using System.Buffers;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PlayerPawn : MonoBehaviour
    {
        [SerializeField]
        private Camera m_mainCamera = null!;
        
        private Rigidbody _rigidBody = null!;
        
        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _rigidBody.isKinematic = true;
        }
        
        private void Update()
        {
            // マウスの座標を取得して, ワールド位置に変換する
            var mousePosition = Input.mousePosition;
            var newPosition = m_mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));
            _rigidBody.position = newPosition;
            
            // マウスの左クリックを検知する
            if (Input.GetMouseButtonDown(0))
            {
                // SphereCast する
                var results = ArrayPool<Collider>.Shared.Rent(16);
                var count = Physics.OverlapSphereNonAlloc(newPosition, 0.1f, results, 1 << LayerMask.NameToLayer("Default"),
                    QueryTriggerInteraction.Collide);
                var resultsAsSpan = results.AsSpan(0, count);
                foreach(var col in resultsAsSpan)
                {
                    if (col.TryGetComponent(out IClickable clickable))
                    {
                        clickable.LeftClick(true);
                    }
                }
                ArrayPool<Collider>.Shared.Return(results);
            }
            
            // マウスの左クリックのリリースを検知する
            if (Input.GetMouseButtonUp(0))
            {
                // SphereCast する
                var results = ArrayPool<Collider>.Shared.Rent(16);
                var count = Physics.OverlapSphereNonAlloc(newPosition, 0.1f, results, 1 << LayerMask.NameToLayer("Default"),
                    QueryTriggerInteraction.Collide);
                var resultsAsSpan = results.AsSpan(0, count);
                foreach(var col in resultsAsSpan)
                {
                    if (col.TryGetComponent(out IClickable clickable))
                    {
                        clickable.LeftClick(false);
                    }
                }
                ArrayPool<Collider>.Shared.Return(results);
            }
        }
    }
}