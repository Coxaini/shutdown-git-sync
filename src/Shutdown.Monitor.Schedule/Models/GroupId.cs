namespace Shutdown.Monitor.Schedule.Models;

public readonly struct GroupId : IEquatable<GroupId>, IComparable<GroupId>
{
    public int MainGroup { get; }
    public int? SubGroup { get; }

    public GroupId(int mainGroup, int subGroup)
    {
        MainGroup = mainGroup;
        SubGroup = subGroup;
    }

    public GroupId(int mainGroup)
    {
        MainGroup = mainGroup;
        SubGroup = null;
    }

    public bool Equals(GroupId other)
    {
        return MainGroup == other.MainGroup && SubGroup == other.SubGroup;
    }

    public override bool Equals(object? obj)
    {
        return obj is GroupId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(MainGroup, SubGroup);
    }

    public static bool operator ==(GroupId left, GroupId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(GroupId left, GroupId right)
    {
        return !(left == right);
    }

    public int CompareTo(GroupId other)
    {
        var mainGroupComparison = MainGroup.CompareTo(other.MainGroup);
        if (mainGroupComparison != 0) return mainGroupComparison;
        return Nullable.Compare(SubGroup, other.SubGroup);
    }
}