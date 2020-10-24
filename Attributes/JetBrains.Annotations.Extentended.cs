using System;

namespace JetBrains.Annotations
{
    /// <summary>
    /// An indication that the described entity cannot have an empty value.
    ///   The value of identifiers cannot be empty or undefined.
    ///   Strings cannot be null or equal to string.Empty.
    ///   Collections, arrays, list ot enumeration can not be empty.
    ///   Not DBNull or Guid.Empty values allowed.
    ///   And so on for other types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    public class NotEmptyAttribute : Attribute
    { }

    /// <summary>
    /// An indication that the described entity can have an empty value.
    ///   The value of identifiers can be empty or undefined.
    ///   Strings can be equal to string.Empty.
    ///   Collections, arrays, list ot enumeration can be empty.
    ///   Object value can be equal to DBNull.
    ///   Guid can be Guid.Empty.
    ///   And so on for other types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    public class CanBeEmptyAttribute : Attribute
    { }


    /// <summary>
    /// An indication that all elements of the described collection or enumeration cannot have an empty value.
    ///   The value of identifiers cannot be empty or undefined.
    ///   Strings cannot be null or equal to string.Empty.
    ///   Collections, arrays, list ot enumeration can not be empty.
    ///   Not DBNull or Guid.Empty values allowed.
    ///   And so on for other types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field)]
    public sealed class ItemNotEmptyAttribute : Attribute
    { }

    /// <summary>
    /// An indication that any elements of the described collection or enumeration can have an empty value.
    ///   The value of identifiers can be empty or undefined.
    ///   Strings can be equal to string.Empty.
    ///   Collections, arrays, list ot enumeration can be empty.
    ///   Object value can be equal to DBNull.
    ///   Guid can be Guid.Empty.
    ///   And so on for other types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field)]
    public sealed class ItemCanBeEmptyAttribute : Attribute
    { }

    /// <summary>A sign that the described string must have at least one character other than a space.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    public class NotWhitespaceAttribute : Attribute
    { }

    /// <summary>An indication that all strings in the described collection or enumeration must have at least one character other than a space.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    public class ItemNotWhitespaceAttribute : Attribute
    { }

    /// <summary>An indication that the described entity string must contain a Guid.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    public class GuidStrAttribute : Attribute
    { }

    /// <summary>An indication that the described entity string must contain a non empty Guid.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    public class NotEmptyGuidAttribute : Attribute
    { }

    /// <summary>An indication that the file at the specified path must exist on the disk.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field)]
    public class FileExistsAttribute : Attribute
    { }

    /// <summary>An indication that the folder at the specified path must exist on the disk.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field)]
    public class DirectoryExistsAttribute : Attribute
    { }


    /// <summary>A sign that the identifier to which this attribute belongs should describe a correct Uri,
    ///     optionally matching the specified scheme (For example UriScheme.Http).
    ///     If the scheme is not specified, it is not checked.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    public class CorrectUriAttribute : Attribute
    {
        [UsedImplicitly] private UriScheme Scheme { get; }

        public CorrectUriAttribute(UriScheme scheme = UriScheme.Any)
            => Scheme = scheme;
    }

    /// <summary>A sign that the described number must be greater than zero.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    public class PositiveNumberAttribute : Attribute
    { }

    /// <summary>A sign that the described number is must be equal to or greater than zero.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    public class ZeroOrPositiveNumberAttribute : Attribute
    { }

    /// <summary>An indication that the described number is must be less than zero/</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    public class NegativeNumberAttribute : Attribute
    { }

    /// <summary>Sorting direction</summary>
    public enum SortDirection { Ascending = 0, Descending = 1}

    /// <summary>An indication that the described collection or enumeration must be sorted.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
                    AttributeTargets.Delegate | AttributeTargets.Field | AttributeTargets.Event |
                    AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.GenericParameter)]
    public sealed class SortedAttribute : Attribute
    {
        public SortDirection SortDirection { get; }

        public SortedAttribute(SortDirection sortDirection = SortDirection.Ascending)
            => SortDirection = sortDirection;
    }
}