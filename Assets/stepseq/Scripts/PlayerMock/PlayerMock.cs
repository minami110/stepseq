using System.Runtime.CompilerServices;
using Edanoue.Rx;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    public sealed class PlayerMock : MonoBehaviour
    {
        // バトル開始時のデフォルト体力
        private const float _DEFAULT_HEALTH = 100f;
        
        [SerializeField]
        private EntityStateVis m_entityStateVis = null!;
        
        [SerializeField]
        private Transform m_animateRoot = null!;
        
        [SerializeField]
        private Shader m_shader = null!;
        
        [SerializeField]
        private Renderer m_renderer = null!;
        
        private MaterialWrapper _material = null!;
        
        public EntityState State
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = new();
        
        private void Awake()
        {
            m_entityStateVis.SetEntityState(State);
            State.RegisterTo(destroyCancellationToken);
            
            _material = new MaterialWrapper(m_shader);
            _material.RegisterTo(destroyCancellationToken);
            m_renderer.sharedMaterial = _material;
            
            // EventManager のイベントを購読する
            EventManager.OnBattleStart
                .Subscribe(this, (_, state) =>
                {
                    // バトル開始時の初期化処理
                    state.State.Clear();
                    state.State.AddStack(StackType.AddHealth, _DEFAULT_HEALTH);
                    state.State.AddStack(StackType.AddMaxHealth, _DEFAULT_HEALTH);
                    state.State.Solve(0);
                })
                .RegisterTo(destroyCancellationToken);
            
            EventManager.OnPostUpdateQuantizeTime
                .Subscribe(this, (_, state) =>
                {
                    // バトル中の処理
                    state.State.Solve(0);
                })
                .RegisterTo(destroyCancellationToken);
        }
        
        // Update is called once per frame
        private void Update()
        {
            // 普段はゆるく 回転し続ける
            m_animateRoot.Rotate(Vector3.right, 10f * Time.deltaTime);
            m_animateRoot.Rotate(Vector3.up, 3f * Time.deltaTime);
            m_animateRoot.Rotate(Vector3.forward, 1f * Time.deltaTime);
        }
        
        internal void SetBaseColor(in Color color)
        {
            _material.SetColor(in color);
        }
    }
}