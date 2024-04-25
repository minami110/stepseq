// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    /// <summary>
    /// Evasion (回避率) を増やすエフェクト
    /// </summary>
    public sealed class AddEvasion : EffectBase
    {
        internal override void Execute(IEntity target, object arg0)
        {
            target.AddStack(StackType.AddEvasion, (float)arg0);
        }
    }
}