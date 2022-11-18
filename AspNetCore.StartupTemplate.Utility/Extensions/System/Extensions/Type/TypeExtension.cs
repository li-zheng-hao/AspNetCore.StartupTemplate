using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace System;

public static class TypeExtension
{
    public static T? CreateInstance<T>(this Type type)
    {
        var obj = Activator.CreateInstance(type);
        if (obj is null)
            return default;
        return (T)obj;
    }

    public static T? CreateInstance<T>(this Type type, params object[] args)
    {
        var obj = Activator.CreateInstance(type, args);
        if (obj is null)
            return default;
        return (T)obj;
    }

    public static bool IsNullableType(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

    public static bool IsVisibleAndVirtual([NotNull] this MethodInfo methodInfo)
    {
        if (methodInfo.IsStatic || methodInfo.IsFinal)
            return false;

        return methodInfo.IsVirtual &&
               (methodInfo.IsPublic || methodInfo.IsFamily || methodInfo.IsFamilyOrAssembly);
    }

    public static bool IsVisible(this MethodBase methodBase)=> methodBase.IsPublic || methodBase.IsFamily || methodBase.IsFamilyOrAssembly;

    public static bool IsVisible(this FieldInfo fieldInfo) => fieldInfo.IsPublic || fieldInfo.IsFamily || fieldInfo.IsFamilyOrAssembly; 
    public static bool HasEmptyConstructor([NotNull] this Type @this) => @this.GetConstructors(BindingFlags.Instance).Any(c => c.GetParameters().Length == 0);
}