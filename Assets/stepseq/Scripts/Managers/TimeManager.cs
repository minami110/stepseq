using System;
using System.Runtime.CompilerServices;
using Edanoue.Rx;
using UnityEngine;

namespace stepseq
{
    // スクリプトの実行順序を早める, 中心的なスクリプトでいろんなものから参照されるため
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    public class TimeManager : MonoBehaviour
    {
        private const  float        _STOP_TIME   = -0.01f;
        private const  float        _START_TIME  = 0f;
        private const  float        _END_TIME    = 1f;
        private const  int          _TRACK_COUNT = 8;
        private static TimeManager? _instance;
        
        [SerializeField]
        [Range(1f, 1200f)]
        private float m_beatPerMinute = 30f;
        
        private readonly ReactiveProperty<int> _activeTrack = new(-1);
        
        private readonly ReactiveProperty<float> _currentTime = new(0f);
        
        private bool _isPlaying;
        
        public static ReadOnlyReactiveProperty<float> CurrentTime => GetInstance()._currentTime;
        public static ReadOnlyReactiveProperty<int> ActiveTrack => GetInstance()._activeTrack;
        
        private void Awake()
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("PlayerState is a singleton and should only be initialized once.");
            }
            
            _instance = this;
            
            _currentTime.Value = _STOP_TIME;
            _currentTime.RegisterTo(destroyCancellationToken);
        }
        
        private void Update()
        {
            if (_isPlaying)
            {
                _currentTime.Value += Time.deltaTime / (60f / m_beatPerMinute);
                if (_currentTime.Value >= _END_TIME)
                {
                    _currentTime.Value = _START_TIME;
                    _activeTrack.Value = 0;
                }
                else
                {
                    _activeTrack.Value = (int)(_currentTime.Value / _END_TIME * _TRACK_COUNT);
                }
            }
            else
            {
                _currentTime.Value = _STOP_TIME;
                _activeTrack.Value = -1;
            }
        }
        
        private void OnDestroy()
        {
            _instance = null;
        }
        
        public void Play()
        {
            _isPlaying = true;
        }
        
        public void Stop()
        {
            _isPlaying = false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TimeManager GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            
            throw new InvalidOperationException("PlayerState is not initialized.");
        }
    }
}