using NUnit.Framework;
using stepseq;

public class EntityStateTests
{
    [Test]
    public void Health00()
    {
        using var player0 = new EntityState();
        
        // 体力を初期化
        player0.AddStack(StackType.Health, 100f);
        
        // ダメージを与える
        player0.AddStack(StackType.HealthDamage, 10f);
        player0.SolveHealth(0);
        Assert.That(player0.Health.CurrentValue, Is.EqualTo(90f));
    }
    
    [Test]
    public void Shield00()
    {
        using var player0 = new EntityState();
        
        // 体力を初期化
        player0.AddStack(StackType.Health, 100f);
        
        // シールドを追加してダメージを与える
        player0.AddStack(StackType.Shield, 20f);
        player0.AddStack(StackType.HealthDamage, 10f);
        player0.SolveHealth(0);
        Assert.That(player0.Shield.CurrentValue, Is.EqualTo(10f));
        Assert.That(player0.Health.CurrentValue, Is.EqualTo(100f));
        
        // シールドを超えるダメージを与える
        player0.AddStack(StackType.HealthDamage, 20f);
        player0.SolveHealth(0);
        Assert.That(player0.Shield.CurrentValue, Is.EqualTo(0f));
        Assert.That(player0.Health.CurrentValue, Is.EqualTo(90f));
    }
}