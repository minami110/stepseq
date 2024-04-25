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
        
        public static readonly ReactiveProperty<int> QuantizedTime          = new();
        public static readonly Subject<Unit>         OnPostUpdateQuantizeTime = new();
        
        public static readonly ReactiveProperty<int> LoopCount = new();
    }
}