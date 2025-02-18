using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Globalization;

public static class GameSettingGenerator
{
    private const string CONFIG_ASSET_PATH = "Assets/Prefabs/Core/GameConfig.asset";
    private const string OUTPUT_PATH = "Assets/Scripts/Core/Config/GameSetting.cs";

    private const string Null = "null";
    private const string FloatFormat = "f";
    private const string DoubleFormat = "d";

    [MenuItem("Tools/Generate GameSetting.cs")]
    public static void GenerateGameSetting()
    {
        GameConfig config = AssetDatabase.LoadAssetAtPath<GameConfig>(CONFIG_ASSET_PATH);

        if (config == null)
        {
            Debug.LogError("GameConfig not find: " + CONFIG_ASSET_PATH);
            return;
        }

        FieldInfo[] fields = typeof(GameConfig).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("// Этот файл сгенерирован автоматически. Не изменяйте вручную!");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine();
        sb.AppendLine($"public static class GameSetting");
        sb.AppendLine("{");

        foreach (var field in fields)
        {
            object value = field.GetValue(config);
            string valueString = ConvertValueToCodeString(value, field.FieldType);
            if (valueString == null)
                continue;

            sb.AppendLine($"    public static readonly {GetTypeName(field.FieldType)} {NormalizeFieldName(field.Name)} = {valueString};");
        }

        sb.AppendLine("}");
        Directory.CreateDirectory(Path.GetDirectoryName(OUTPUT_PATH));
        File.WriteAllText(OUTPUT_PATH, sb.ToString());
        AssetDatabase.Refresh();
        Debug.Log("GameSetting сгенерирован по пути: " + OUTPUT_PATH);
    }

    private static string ConvertValueToCodeString(object value, Type type)
    {
        if (value == null)
            return Null;

        switch (type)
        {
            case Type t when t == typeof(string):
                return $"\"{value}\"";

            case Type t when t == typeof(float):
                return value.ToString() + FloatFormat;

            case Type t when t == typeof(double):
                return value.ToString() + DoubleFormat;

            case Type t when t == typeof(bool):
                return value.ToString().ToLower();

            case Type t when t.IsPrimitive:
                return value.ToString();

            case Type t when t.IsArray:
                return ConvertArrayToCodeString(value, t);

            case Type t when t == typeof(ShopConfig):
                return ConvertShopConfigToCodeString(value as ShopConfig);

            case Type t when t == typeof(GameCurrency):
                return ConvertGameCurrencyToCodeString((GameCurrency)value);

            case Type t when t == typeof(CombatConfig):
                return ConvertCombatConfigToCodeString(value as CombatConfig);

            case Type t when t == typeof(SavingSystemConfig):
                return ConvertSavingSystemConfigToCodeString(value as SavingSystemConfig);

            default:
                Debug.LogWarning($"Поле {value} типа {type.Name} не может быть автоматически сгенерировано.");
                return null;
        }
    }

    private static string ConvertArrayToCodeString(object value, Type arrayType)
    {
        Array array = value as Array;

        if (array == null)
            return Null;

        Type elementType = arrayType.GetElementType();
        string elementTypeName = GetTypeName(elementType);
        string[] elements = new string[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            object element = array.GetValue(i);
            string elementCode = ConvertValueToCodeString(element, elementType);
            elements[i] = elementCode;
        }

        return $"new {elementTypeName}[] {{ {string.Join(", ", elements)} }}";
    }

    private static string ConvertGameCurrencyToCodeString(GameCurrency gameCurrency)
    {
        string typeCode = $"{nameof(TypeGameCurrency)}.{gameCurrency.Type}";
        string iconName = gameCurrency.IconResourceName != null ? gameCurrency.IconResourceName : "";
        string iconCode = $"\"{iconName}\"";
        Color color = gameCurrency.Color;
        string r = color.r.ToString("0.######", CultureInfo.InvariantCulture);
        string g = color.g.ToString("0.######", CultureInfo.InvariantCulture);
        string b = color.b.ToString("0.######", CultureInfo.InvariantCulture);
        string a = color.a.ToString("0.######", CultureInfo.InvariantCulture);
        string colorCode = $"new Color({r}f, {g}f, {b}f, {a}f)";

        return $"new {nameof(GameCurrency)}({typeCode}, {iconCode}, {colorCode})";
    }

    private static string ConvertShopConfigToCodeString(ShopConfig shopConfig)
    {
        if (shopConfig == null)
            return Null;

        Array currencies = shopConfig.Currencies.ToArray();
        string arrayCode = ConvertValueToCodeString(currencies, currencies.GetType());

        return $"new {nameof(ShopConfig)}({arrayCode})";
    }

    private static string ConvertCombatConfigToCodeString(CombatConfig combatConfig)
    {
        if (combatConfig == null)
            return Null;

        string maxValueAccuracy = combatConfig.MaxValueAccuracy.ToString("0.######", CultureInfo.InvariantCulture);
        string maxValueDamage = combatConfig.MaxValueDamage.ToString("0.######", CultureInfo.InvariantCulture);
        string maxValueDistance = combatConfig.MaxValueDistance.ToString("0.######", CultureInfo.InvariantCulture);
        string maxValueArmor = combatConfig.MaxValueArmor.ToString("0.######", CultureInfo.InvariantCulture);

        return $"new {nameof(CombatConfig)}({maxValueAccuracy}f, {maxValueDamage}f, {maxValueDistance}f, {maxValueArmor}f)";
    }

    private static string ConvertSavingSystemConfigToCodeString(SavingSystemConfig savingSystemConfig)
    {
        if (savingSystemConfig == null)
            return Null;

        string fileName = string.IsNullOrEmpty(savingSystemConfig.FileName) ? Null : savingSystemConfig.FileName;

        return $"new {nameof(SavingSystemConfig)}(\"{fileName}\", {nameof(TypeSerializationStrategy)}.{savingSystemConfig.TypeSerializationStrategy})";
    }

    private static string GetTypeName(Type type)
    {
        return type.Name;
    }

    private static string NormalizeFieldName(string fieldName)
    {
        while (!string.IsNullOrEmpty(fieldName) && !char.IsLetter(fieldName[0]))
        {
            fieldName = fieldName.Substring(1);
        }

        if (string.IsNullOrEmpty(fieldName))
            return "Unknown";

        if (char.IsUpper(fieldName[0]) == false)
            fieldName = char.ToUpper(fieldName[0]) + fieldName.Substring(1);

        return fieldName;
    }
}