﻿global using UmbConstants = Umbraco.Cms.Core.Constants;
global using UmbEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

using Newtonsoft.Json;

using NUnit.Framework;
using uSync.Migrations.Context;
using uSync.Migrations.Migrators;
using uSync.Migrations.Migrators.Models;
using uSync.Migrations.Serialization;

namespace uSync.Migrations.Tests.Migrators;

public abstract class MigratorTestBase
{
    protected SyncMigrationContext? _context;
    protected ISyncPropertyMigrator?  _migrator;

    [SetUp]
    public virtual void Setup()
    {
        _context = new SyncMigrationContext(Guid.NewGuid(), "", "", true, 7);
    }

    protected string ConvertResultToJsonTestResult(object? value)
    {
        if (value == null) return JsonConvert.SerializeObject(string.Empty);

        var jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new SyncMigrationsContractResolver(),
            Formatting = Formatting.Indented,
        };

        return JsonConvert.SerializeObject(value, jsonSerializerSettings);
    }

    protected abstract SyncMigrationDataTypeProperty GetMigrationDataTypeProperty();
    protected abstract SyncMigrationContentProperty GetMigrationContentProperty(string value);

    public abstract void DatabaseTypeAsExpected();

    protected void DatabaseTypeAsExpectedBase(string expectedType)
    {
        var databaseType = _migrator!.GetDatabaseType(GetMigrationDataTypeProperty(), _context!);
        Assert.AreEqual(expectedType, databaseType);
    }

    public abstract void EditorAliasAsExpected();
    protected void EditorAliasAsExpectedBase(string expectedAlias)
    {
        var editorAlias = _migrator!.GetEditorAlias(GetMigrationDataTypeProperty(), _context!);
        Assert.AreEqual(expectedAlias, editorAlias);
    }

    public abstract void ConfigValueAsExpected();
    public abstract void ContentValueAsExpected(string value, string expected);

    protected void ContentValueAsExpectedBase(string value, string expected)
    {
        var contentValue = _migrator!.GetContentValue(GetMigrationContentProperty(value), _context!);
        Assert.AreEqual(expected, contentValue);
    }
}
