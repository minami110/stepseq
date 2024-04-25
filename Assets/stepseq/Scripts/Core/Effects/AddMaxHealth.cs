// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    /// <summary>
    /// MaxHealth を増やすエフェクト
    /// Health は変わらないので注意
    /// </summary>
    public sealed class AddMaxHealth : EffectBase
    {
        internal override void Execute(IEntity target, object arg0)
        {
            target.AddStack(StackType.AddMaxHealth, (float)arg0);
        }
    }
}