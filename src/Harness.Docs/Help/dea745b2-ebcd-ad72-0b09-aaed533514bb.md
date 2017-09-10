# HarnessBase.MongoConnections Property 
 

Gets the dictionary of mongo clients. The mongo server connection string is used as the key.

**Namespace:**&nbsp;<a href="c306edfe-5c5e-b933-d794-fef44c8f4ffc">Harness</a><br />**Assembly:**&nbsp;Harness (in Harness.dll) Version: 1.2.0.0 (1.2.0.0)

## Syntax

**C#**<br />
``` C#
public Dictionary<string, IMongoClient> MongoConnections { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property MongoConnections As Dictionary(Of String, IMongoClient)
	Get
```

**F#**<br />
``` F#
member MongoConnections : Dictionary<string, IMongoClient> with get

```


#### Property Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/xfhwa508" target="_blank">Dictionary</a>(<a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">String</a>, IMongoClient)

## See Also


#### Reference
<a href="36459884-00cd-2c3d-acf5-b0cdf2a8da1b">HarnessBase Class</a><br /><a href="c306edfe-5c5e-b933-d794-fef44c8f4ffc">Harness Namespace</a><br />