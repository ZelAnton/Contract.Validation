using System;

namespace JetBrains.Annotations
{
    /// <summary>
    /// Признак того, что описываемая сущность не может иметь пустое значение.
    ///   идентификаторы: значение не может иметь неизвестное или неопределённое значение, например идентификатор объекта не может равняться Consts.UnknownObjectId, Consts.NoObject или Consts.NavigatorUndefinedObjectID
    ///   строки: строка быть отличной от string.Empty
    ///   коллекция/перечисление/список/массив: в нём должен быть хотя бы один элемент.
    ///   Object: не может быть равен DBNull
    ///   Guid: не может быть равен Guid.Empty
    ///   ObligatoryObjectAttribute: не может равняться ObligatoryObjectAttributes.Zero или ObligatoryObjectAttributes.None.
    ///   и так далее по аналогии для всех других типов.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Field)]
    public class NotEmptyAttribute : Attribute
    {
    }

    /// <summary>
    /// Признак того, что описываемая сущность может иметь пустое значение.
    ///   идентификаторы: значение может иметь неизвестное или неопределённое значение, например идентификатор объекта может равняться Consts.UnknownObjectId, Consts.NoObject или Consts.NavigatorUndefinedObjectID
    ///   строки: строка может быть равна string.Empty
    ///   коллекция/перечисление/список/массив: в нём может не быть элементов.
    ///   Object: может быть равен DBNull
    ///   Guid: может быть равен Guid.Empty
    ///   ObligatoryObjectAttribute: может равняться ObligatoryObjectAttributes.Zero или ObligatoryObjectAttributes.None.
    ///   и так далее по аналогии для всех других типов.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Field)]
    public class CanBeEmptyAttribute : Attribute
    {
    }

    /// <summary>
    /// Признак того, что все элементы описываемой коллекции/перечислении/списка/массива не могут иметь пустое значение.
    ///   идентификаторы: значение не может иметь неизвестное или неопределённое значение, например идентификатор объекта не может равняться Consts.UnknownObjectId, Consts.NoObject или Consts.NavigatorUndefinedObjectID
    ///   строки: строка быть отличной от string.Empty
    ///   коллекция/перечисление/список/массив: в нём должен быть хотя бы один элемент.
    ///   Object: не может быть равен DBNull
    ///   Guid: не может быть равен Guid.Empty
    ///   ObligatoryObjectAttribute: не может равняться ObligatoryObjectAttributes.Zero или ObligatoryObjectAttributes.None.
    ///   и так далее по аналогии для всех других типов.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Delegate | AttributeTargets.Field)]
    public sealed class ItemNotEmptyAttribute : Attribute { }

    /// <summary>
    /// Признак того, что все элементы описываемой коллекции/перечислении/списка/массива могут иметь пустое значение.
    ///   идентификаторы: значение может иметь неизвестное или неопределённое значение, например идентификатор объекта может равняться Consts.UnknownObjectId, Consts.NoObject или Consts.NavigatorUndefinedObjectID
    ///   строки: строка может быть равна string.Empty
    ///   коллекция/перечисление/список/массив: в нём может не быть элементов.
    ///   Object: может быть равен DBNull
    ///   Guid: может быть равен Guid.Empty
    ///   ObligatoryObjectAttribute: может равняться ObligatoryObjectAttributes.Zero или ObligatoryObjectAttributes.None.
    ///   и так далее по аналогии для всех других типов.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Delegate | AttributeTargets.Field)]
    public sealed class ItemCanBeEmptyAttribute : Attribute { }

    /// <summary>Признак того, что описываемая строка должна иметь хотя бы один символ, отличный от пробела</summary>
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Field)]
    public class NotWhitespaceAttribute : Attribute
    {
    }

    /// <summary>Признак того, что файл по указанному пути должен существовать на диске</summary>
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Delegate | AttributeTargets.Field)]
    public class FileExistsAttribute : Attribute
    {
    }

    /// <summary>Признак того, что папка по указанному пути должна существовать на диске</summary>
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Delegate | AttributeTargets.Field)]
    public class DirectoryExistsAttribute : Attribute
    {
    }

    /// <summary>Признак того, что строки в описываемой коллекции/перечислении/списка/массива должны иметь хотя бы один символ, отличный от пробела</summary>
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Field)]
    public class ItemNotWhitespaceAttribute : Attribute
    {
    }

    /// <summary>Признак того, что свойство или поле становится отличным от Null после вызова метода, имя которого передаётся в качестве параметра</summary>
    [AttributeUsage(AttributeTargets.Property |
                    AttributeTargets.Field, AllowMultiple = true)]
    public sealed class NotNullAfterAttribute : Attribute
    {
        public NotNullAfterAttribute([NotNull, NotWhitespace] string methodName) => MethodName = methodName;

        [NotNull, NotWhitespace] public string MethodName { get; }
    }

    /// <summary>Признак того, что значение свойства или поля равно Null до вызова метода, имя которого передаётся в качестве параметра</summary>
    [AttributeUsage(AttributeTargets.Property |
                    AttributeTargets.Field, AllowMultiple = true)]
    public sealed class NullBeforeAttribute : Attribute
    {
        public NullBeforeAttribute([NotNull, NotWhitespace] string methodName) => MethodName = methodName;

        [NotNull, NotWhitespace] public string MethodName { get; }
    }

    /// <summary>Признак того, что идентификатор, к которому относится данный атрибут, должен описывать реально существующий Uri,
    ///          возможно соответствующей указанной схеме (напр. UriScheme.Http для Http адреса). Если схема не указана - она не проверяется</summary>
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Field)]
    public class CorrectUriAttribute : Attribute
    {
        [UsedImplicitly] private UriScheme Scheme { get; }

        public CorrectUriAttribute(UriScheme scheme = UriScheme.Any) => Scheme = scheme;
    }
}