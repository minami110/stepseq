// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    /// <summary>
    /// Health を減らす (攻撃する) エフェクト
    /// </summary>
    public sealed class SubHealth : EffectBase
    {
        internal override void Execute(IEntity target, object arg0)
        {
            target.AddStack(StackType.SubHealth, (float)arg0);
        }
    }
}