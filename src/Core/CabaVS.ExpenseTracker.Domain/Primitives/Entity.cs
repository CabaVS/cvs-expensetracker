namespace CabaVS.ExpenseTracker.Domain.Primitives;

public abstract class Entity(Guid id) : IEquatable<Entity>
{
    public Guid Id { get; } = id;
    
    public static bool operator ==(Entity? first, Entity? second)
    {
        return first is not null &&
               first.Equals(second);
    }
    
    public static bool operator !=(Entity? first, Entity? second)
    {
        return !(first == second);
    }

    public bool Equals(Entity? other)
    {
        return other is not null && 
               other.GetType() == GetType() &&
               other.Id == Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is not null && 
               obj.GetType() == GetType() &&
               obj is Entity entity &&
               entity.Id == Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id.GetHashCode(),
            GetType().GetHashCode()) * 47;
    }
}