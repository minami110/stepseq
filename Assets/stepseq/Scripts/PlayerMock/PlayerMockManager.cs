using System;
using System.Runtime.CompilerServices;
using Edanoue.Rx;
using UnityEngine;

namespace stepseq
{
    // スクリプトの実行順序を早める, 中心的なスクリプトでいろんなものから参照されるため
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    public class PlayerMockManager : MonoBehaviour
    {
        private static   PlayerMockManager? _instance;
        
        private int _currentPlayerIndex = -1;
        
        [SerializeField]
        private TextMesh m_playerIndexText = null!;
        
        [SerializeField]
        private PlayerMock m_player0 = null!;
        
        [SerializeField]
        private PlayerMock m_player1 = null!;
        
        [SerializeField]
        private GameObject m_playerSlotRoot0 = null!;
        
        [SerializeField]
        private GameObject m_playerSlotRoot1 = null!;
        
        public static PlayerMock GetPlayerMock(int index)
        {
            return index switch
            {
                0 => GetInstance().m_player0,
                1 => GetInstance().m_player1,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private void Awake()
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("PlayerState is a singleton and should only be initialized once.");
            }
            
            _instance = this;
            
            // Initialize the slot
            var a = m_playerSlotRoot0.GetComponentsInChildren<SampleSlot>(true);
            foreach(var slot in a)
            {
                slot.PlayerIndex = 0;
            }
            var b = m_playerSlotRoot1.GetComponentsInChildren<SampleSlot>(true);
            foreach(var slot in b)
            {
                slot.PlayerIndex = 1;
            }
        }
        
        private void Start()
        {
            SwitchPlayerProfile(0);
        }
        
        private void OnDestroy()
        {
            _instance = null;
        }
        
        /// <summary>
        /// デバッグ用の機能, 画面の操作対象のプレイヤーを切り替える
        /// Unity Event 経由で GUI から呼ばれる
        /// </summary>
        /// <param name="playerIndex"></param>
        public void SwitchPlayerProfile(int playerIndex)
        {
            if (playerIndex >= 2)
            {
                throw new NotImplementedException();
            }
            
            if (playerIndex == _currentPlayerIndex)
            {
                return;
            }
            
            _currentPlayerIndex = playerIndex;
            m_playerIndexText.text = $"CurrentPlayer: {playerIndex}";
            if (playerIndex == 0)
            {
                m_playerSlotRoot0.SetActive(true);
                m_playerSlotRoot1.SetActive(false);
            }
            else
            {
                m_playerSlotRoot0.SetActive(false);
                m_playerSlotRoot1.SetActive(true);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static PlayerMockManager GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            
            throw new InvalidOperationException("PlayerState is not initialized.");
        }
    }
}