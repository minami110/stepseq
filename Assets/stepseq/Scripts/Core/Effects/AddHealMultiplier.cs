// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    /// <summary>
    /// Health を増やす (回復する) エフェクト
    /// </summary>
    public sealed class AddHealMultiplier : EffectBase
    {
        internal override void Execute(IEntity target, object arg0)
        {
            target.AddStack(StackType.AddHealMultiplier, (float)arg0);
        }
    }
}