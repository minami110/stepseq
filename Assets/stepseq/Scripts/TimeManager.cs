using System.Threading;
using Edanoue.Rx;
using UnityEngine;

namespace stepseq
{
    // スクリプトの実行順序を早める, 中心的なスクリプトでいろんなものから参照されるため
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    public class TimeManager : MonoBehaviour
    {
        private const float _STOP_TIME   = -0.01f;
        private const float _START_TIME  = 0f;
        private const float _END_TIME    = 1f;
        private const int   _TRACK_COUNT = 8;
        
        [SerializeField]
        [Range(1f, 300f)]
        private float m_beatPerMinute = 15f;
        
        private float                    _currentTime;
        private CancellationTokenSource? _isPlayingCts;
        
        private void Awake()
        {
            _currentTime = _STOP_TIME;
            EventManager.QuantizeTime.Value = -1;
            EventManager.LoopCount.Value = -1;
        }
        
        private void OnDestroy()
        {
            if (_isPlayingCts is null)
            {
                return;
            }
            
            _isPlayingCts.Cancel();
            _isPlayingCts.Dispose();
            _isPlayingCts = null;
        }
        
        public void Play()
        {
            if (_isPlayingCts is not null)
            {
                return;
            }
            
            _isPlayingCts = new CancellationTokenSource();
            _currentTime = 0f;
            EventManager.OnBattleStart.OnNext(Unit.Default);
            EventManager.LoopCount.Value = 0;
            EventManager.QuantizeTime.Value = 0;
            EventManager.OnPostUpdateQuantizeTime.OnNext(Unit.Default);
            
            _ = PlayLoopAsync(_isPlayingCts.Token);
        }
        
        public void Stop()
        {
            if (_isPlayingCts is null)
            {
                return;
            }
            
            _isPlayingCts.Cancel();
            _isPlayingCts.Dispose();
            _isPlayingCts = null;
            
            _currentTime = _STOP_TIME;
            EventManager.OnBattleEnd.OnNext(Unit.Default);
            EventManager.LoopCount.Value = -1;
            EventManager.QuantizeTime.Value = -1;
        }
        
        private async Awaitable PlayLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Awaitable.NextFrameAsync(token);
                
                _currentTime += Time.deltaTime / (60f / m_beatPerMinute);
                if (_currentTime >= _END_TIME)
                {
                    _currentTime = _START_TIME;
                    EventManager.QuantizeTime.Value = 0;
                    EventManager.OnPostUpdateQuantizeTime.OnNext(Unit.Default);
                    EventManager.LoopCount.Value++;
                }
                else
                {
                    var quantizeTime = (int)(_currentTime / _END_TIME * _TRACK_COUNT);
                    if (EventManager.QuantizeTime.Value != quantizeTime)
                    {
                        EventManager.QuantizeTime.Value = quantizeTime;
                        EventManager.OnPostUpdateQuantizeTime.OnNext(Unit.Default);
                    }
                }
            }
        }
    }
}