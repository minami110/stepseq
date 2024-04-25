using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Threading;
using Edanoue.Rx;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider))]
    internal sealed class SampleUi : MonoBehaviour, IDraggable
    {
        [SerializeField]
        private string m_sampleTypeName = string.Empty;
        
        [SerializeField]
        private Shader m_buttonShader = null!;
        
        [SerializeField]
        private Renderer m_buttonRenderer = null!;
        
        [SerializeField]
        private TextMesh m_nameText = null!;
        
        [SerializeField]
        private TextMesh m_priceText = null!;
        
        [SerializeField]
        private TextMesh m_categoryText = null!;
        
        [SerializeField]
        private LayerMask m_searchSampleSlotLayerMask = 0;
        
        private Vector3?   _dragStartPosition;
        private Material   _material   = null!;
        private SampleBase _sampleBase = null!;
        
        private State                    _state = State.IN_SHOP;
        private CancellationTokenSource? _trackMouseCts;
        
        private void Awake()
        {
            // Sample のインスタンスを作成する
            // 文字列から型を検索してクラスを生成する
            var type = Type.GetType("stepseq." + m_sampleTypeName);
            if (type == null)
            {
                throw new InvalidOperationException($"Type not found: {m_sampleTypeName}");
            }
            
            _sampleBase = (SampleBase)Activator.CreateInstance(type);
            _sampleBase.RegisterTo(destroyCancellationToken);
            
            // Init Material
            _material = new Material(m_buttonShader);
            m_buttonRenderer.sharedMaterial = _material;
            
            // Init Look
            SetButtonColor(new Color(0.18f, 0.5f, 0.22f));
            
            // Init TextMesh
            m_nameText.text = _sampleBase.GetType().Name;
            m_priceText.text = $"${_sampleBase.Price.ToString()}";
            var categories = _sampleBase.Categories;
            m_categoryText.text = categories.Length > 0 ? string.Join(", ", categories) : "None";
        }
        
        private void OnDestroy()
        {
            DestroyImmediate(_material);
            _trackMouseCts?.Cancel();
            _trackMouseCts?.Dispose();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (_dragStartPosition.HasValue == false)
            {
                SetButtonColor(new Color(0.48f, 0.39f, 0.09f));
                HintBox.SetText(_sampleBase.HintText);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (_dragStartPosition.HasValue == false)
            {
                SetButtonColor(new Color(0.18f, 0.5f, 0.22f));
                HintBox.ClearText();
            }
        }
        
        void IClickable.LeftClick(bool isPressed)
        {
            if (isPressed)
            {
                if (_dragStartPosition.HasValue)
                {
                    return;
                }
                
                // 初回クリック時
                _dragStartPosition = transform.position;
                SetButtonColor(new Color(1f, 0.82f, 0.15f));
                HintBox.ClearText();
                
                // マウス位置を追従する Task を開始する
                _trackMouseCts = new CancellationTokenSource();
                _ = TrackMousePositionLoopAsync(_trackMouseCts.Token);
            }
            else
            {
                if (!_dragStartPosition.HasValue)
                {
                    return;
                }
                
                // マウス位置の追従タスクを終了する
                _trackMouseCts?.Cancel();
                _trackMouseCts?.Dispose();
                _trackMouseCts = null;
                
                // ドラッグ終了時の処理
                var restPosition = _dragStartPosition.Value;
                _dragStartPosition = null;
                SetButtonColor(new Color(0.18f, 0.5f, 0.22f));
                
                //購入確定エリア(インベントリ内) なら購入する, 購入に失敗したら元の場所に戻す
                if (SearchSlotAndAssign() == 0)
                {
                    transform.position = restPosition;
                }
            }
        }
        
        private int SearchSlotAndAssign()
        {
            var result = 0;
            
            var results = ArrayPool<Collider>.Shared.Rent(16);
            var count = Physics.OverlapSphereNonAlloc(
                transform.position,
                0.1f,
                results,
                m_searchSampleSlotLayerMask,
                QueryTriggerInteraction.Collide);
            var resultsAsSpan = results.AsSpan(0, count);
            foreach (var col in resultsAsSpan)
            {
                // Sample Slot の上で離した場合
                if (col.TryGetComponent(out SampleSlot slot))
                {
                    if (!_sampleBase.AssignToSlot(slot))
                    {
                        continue;
                    }
                    
                    // 購入確定
                    result = 1;
                    
                    // State を更新する
                    if (_state == State.IN_SHOP)
                    {
                        _state = State.IN_PLYAER_SLOT;
                    }
                    
                    // ショップに消されるので, ショップから抜けておく
                    var trs = transform;
                    trs.SetParent(slot.transform);
                    
                    // もうショップにないので, Price を消しておく
                    m_priceText.text = string.Empty;
                    
                    // Slot の位置に移動しておく
                    trs.position = slot.transform.position;
                    break;
                }
                
                // ShopSellTriggerUi の上で離した場合
                if (col.TryGetComponent(out ShopSellTriggerUi sellTrigger))
                {
                    if (_state == State.IN_PLYAER_SLOT)
                    {
                        // プレイヤーの所持品の状態で売られた
                        result = 2;
                        _sampleBase.Sell();
                        Destroy(gameObject);
                        break;
                    }
                }
            }
            
            ArrayPool<Collider>.Shared.Return(results);
            return result;
        }
        
        private async Awaitable TrackMousePositionLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                transform.position = MousePlayerState.Position;
                await Awaitable.NextFrameAsync(token);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetButtonColor(in Color color)
        {
            _material.color = color;
        }
        
        private enum State
        {
            IN_SHOP,
            IN_PLYAER_SLOT
        }
    }
}