// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    public sealed class BlueBanana : SampleBase
    {
        protected override int GetPrice()
        {
            return 10;
        }
        
        protected override string GetHintText()
        {
            return "相手のシールド値だけ体力を回復";
        }
        
        public override void Execute(EntityState from, EntityState to)
        {
            // 相手のシールド値を参照する
            var value = to.Shield.CurrentValue;
            if (value <= 0)
            {
                return;
            }
            
            // 相手のシールド値だけ体力を回復する
            EffectManager.AddEffect<TakeHealth>(from, value);
        }
    }
}