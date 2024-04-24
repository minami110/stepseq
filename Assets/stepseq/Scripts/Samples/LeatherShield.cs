// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    public sealed class LeatherShield : SampleBase
    {
        protected override int GetPrice()
        {
            return 5;
        }
        
        protected override string GetHintText()
        {
            return "10 シールドを得る";
        }
        
        public override void Execute(EntityState from, EntityState to)
        {
            // 実行者にシールド 10 を与える
            EffectManager.AddEffect<TakeShield>(from, 10f);
        }
    }
}