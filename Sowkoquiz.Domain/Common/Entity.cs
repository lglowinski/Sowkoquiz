using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sowkoquiz.Domain.Common;

public abstract class Entity(int? id)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; } = id;
    protected abstract int HashCodeSeed { get; }

    public override bool Equals(object? obj) 
        => obj is Entity entity && Equals(entity);

    private bool Equals(Entity other) 
        => other.Id == Id;

    public override int GetHashCode() 
        => Id.GetHashCode() * HashCodeSeed + 23 << 1;
}