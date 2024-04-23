using System;
using System.Collections.Generic;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    public class DummyLog : MonoBehaviour
    {
        private const int _LOG_COUNT = 10;
        
        [SerializeField]
        private TextMesh m_textMesh = null!;
        
        private readonly List<string> _log = new(_LOG_COUNT);
        
        public void Log(string text)
        {
            _log.Add($"[{Time.frameCount:00000}] {text}");
            if (_log.Count > _LOG_COUNT)
            {
                _log.RemoveAt(0);
            }
            
            // キューの後ろから表示する
            var result = "";
            var count = _log.Count;
            for (var i = count - 1; i >= 0; i--)
            {
                result += _log[i] + "\n";
            }
            
            m_textMesh.text = result;
        }
    }
}