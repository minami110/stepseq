// Copyright Edanoue, Inc. All Rights Reserved.

using Edanoue.Rx;

namespace stepseq
{
    public static class EventManager
    {
        /// <summary>
        /// バトル開始時に呼ばれるイベント
        /// </summary>
        public static readonly Subject<Unit> OnBattleStart = new();
        
        /// <summary>
        /// バトル終了時に呼ばれるイベント
        /// </summary>
        public static readonly Subject<Unit> OnBattleEnd = new();
        
        /// <summary>
        /// バトル中の QuantizeTime (ステムの位置) の値 
        /// </summary>
        public static readonly ReactiveProperty<int> QuantizeTime = new();
        
        /// <summary>
        /// QuantizeTime の値が変更された後に呼ばれるイベント
        /// </summary>
        public static readonly Subject<Unit> OnPostUpdateQuantizeTime = new();
        
        /// <summary>
        /// バトル中の LoopCount (トラックのループ数) の値  
        /// </summary>
        public static readonly ReactiveProperty<int> LoopCount = new();
    }
}