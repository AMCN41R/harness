# SettingsBuilder.AddDatabase Method 
 

Adds a new database configuration to the <a href="aea1a0da-0211-3e8d-e69f-7300dd07906e">HarnessConfiguration</a> object.

**Namespace:**&nbsp;<a href="71b20054-d355-35ae-710d-5484ba2d4fce">Harness.Settings</a><br />**Assembly:**&nbsp;Harness (in Harness.dll) Version: 1.2.0.0 (1.2.0.0)

## Syntax

**C#**<br />
``` C#
public ISettingsBuilderConnectionString AddDatabase(
	string name
)
```

**VB**<br />
``` VB
Public Function AddDatabase ( 
	name As String
) As ISettingsBuilderConnectionString
```

**F#**<br />
``` F#
abstract AddDatabase : 
        name : string -> ISettingsBuilderConnectionString 
override AddDatabase : 
        name : string -> ISettingsBuilderConnectionString 
```


#### Parameters
&nbsp;<dl><dt>name</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />The name of the MongoDb database.</dd></dl>

#### Return Value
Type: <a href="f2f6da3f-37e0-8c04-2eed-1ce4b36c52bf">ISettingsBuilderConnectionString</a><br />An <a href="f2f6da3f-37e0-8c04-2eed-1ce4b36c52bf">ISettingsBuilderConnectionString</a> instance.

#### Implements
<a href="1d4e13d1-2b3d-2a3d-b26a-db590c89c73c">ISettingsBuilder.AddDatabase(String)</a><br /><a href="9266817d-6a21-8345-19eb-ea610453284b">ISettingsBuilderDatabase.AddDatabase(String)</a><br />

## See Also


#### Reference
<a href="4372e2fd-49d0-eab3-c580-8409deaf89ae">SettingsBuilder Class</a><br /><a href="71b20054-d355-35ae-710d-5484ba2d4fce">Harness.Settings Namespace</a><br />