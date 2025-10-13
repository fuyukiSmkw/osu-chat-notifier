// Copyright (c) 2025 MATRIX-feather. Licensed under the MIT Licence.
// Copyright (c) 2025 fuyukiS <fuyukiS@outlook.jp>. Licensed under the MIT License.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Reflection;

#nullable enable

namespace osu.Game.Rulesets.ChatNotifier.ListenerLoader.Utils;

public static class HandlerExtension
{
    public const BindingFlags INSTANCE_FLAG = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.GetField;

    private static FieldInfo? findFieldInstanceInBaseType(Type baseType, Type type)
    {
        var field = baseType.GetFields() // INSTANCE_FLAG
                            .FirstOrDefault(f => f.FieldType == type);

        if (field == null && baseType.BaseType != null)
            field = findFieldInstanceInBaseType(baseType.BaseType, type);

        return field;
    }

    public static FieldInfo? FindFieldInstance(this object obj, Type type)
    {
        return findFieldInstanceInBaseType(obj.GetType(), type);
    }

    public static object? FindInstance(this object obj, Type type)
    {
        var field = obj.FindFieldInstance(type);
        return field?.GetValue(obj);
    }

    private static FieldInfo? findFieldInstanceByName(Type baseType, string name)
    {
        var field = baseType.GetField(name); // INSTANCE_FLAG

        if (field == null && baseType.BaseType != null)
            field = findFieldInstanceByName(baseType.BaseType, name);

        return field;
    }

    public static FieldInfo? FindFieldInstance(this object obj, string name)
    {
        return findFieldInstanceByName(obj.GetType(), name);
    }

    public static object? FindInstance(this object obj, string name)
    {
        var field = obj.FindFieldInstance(name);
        return field?.GetValue(obj);
    }

    private static MethodInfo? findMethodByName(Type baseType, string name, Type returnType)
    {
        var method = baseType.GetMethod(name); // INSTANCE_FLAG
        if (method == null && baseType.BaseType != null)
            method = findMethodByName(baseType.BaseType, name, returnType);
        return method?.IsGenericMethodDefinition ?? false ? method?.MakeGenericMethod(returnType) : method;
    }

    public static Func<object?[], object?> FindMethod(this object obj, string name, Type returnType)
    {
        return parameters => findMethodByName(obj.GetType(), name, returnType)?.Invoke(obj, parameters);
    }
}
