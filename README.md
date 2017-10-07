# Harness

Harness is a .NET library designed to manage the state of a Mongo database during testing.

## About
Harness is designed to aid unit and integration testing by putting a Mongo database in a known state at the beginning of a test, or suite of tests. The framework allows you to control your code’s dependency on a Mongo database, therefore increasing test consistency and repeat-ability.

Harness is an open source project, licensed under the [MIT License](https://github.com/AMCN41R/Harness/blob/dev/LICENSE)

Harness was inspired by [NDBUnit](https://github.com/NDbUnit/NDbUnit).

## Getting Harness
Harness is availble on [nuget](https://www.nuget.org/packages/Harness) and can be installed with...
```
Install-Package Harness
```

## Basic Usage
Harness will put one or more Mongo databases into a state defined by a json settings file and one or more json data files.

### Settings File
The settings file must have a `.json` extension. It is a json object that contains a `databases` property that is an array of `database` objects.

#### Database Object
- **databseName**: The name of the database that will be used. 
- **connectionString**: The connection string of the mongo server where the database should be created. 
- **collectionNameSuffix**: A string that will be added to the end of each collection name specified in the collections array. 
- **dropFirst**: A boolean value that indicates whether or not this database should be dropped if it already exists, and re-created. 
- **collections**: An array of `collection` objects. 

#### Collection Object
- **collectionName**: The name of the collection that will be used. 
- **dropFirst**: A boolean value that indicates whether or not this collection should be dropped if it already exists, and re-created. 
- **dataFileLocation**: The path to the json file that contains the data that should be inserted into this collection.

#### Example settings file, 'ExampleSettings.json'
```
{
    "databases": [
        {
            "databaseName": "TestDb1",
            "connectionString": "mongodb://localhost:27017",
            "collectionNameSuffix": "",
            "dropFirst": true,
            "collections": [
                {
                    "collectionName": "TestCollection1",
                    "dataFileLocation": "Collection1.json",
                    "dropFirst": false
                },
                {
                    "collectionName": "TestCollection2",
                    "dataFileLocation": "Collection2.json",
                    "dropFirst": false
                }
            ]
        }
    ]
}
```

### Data File
Test data files must have a `.json` extension and contain an array of json objects.

#### Example data file, 'Collection1.json'
```
[
    {
        "_id": { "$oid": "56a69c36d1894801d0ce3d05" },
        "Col1b": "Value1b",
        "Col2b": "Value2b"
    },
    {
        "_id": { "$oid": "56a69c36d1894801d0ce3d06" },
        "Col1b": "Value3b",
        "Col2b": "Value4b"
    }
]
```

Point your test class at the settings file using the `HarnessConfig` attribute, and extend `HarnessBase` to have the database configuration run before each test.


### Example test class using XUnit
```csharp
[HarnessConfig(ConfigFilePath = "ExampleSettings.json")]
public class AutoConfigureDatabase : HarnessBase
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var classUnderTest = new ClassUnderTest();

        // Act
        var result = classUnderTest.GetCollectionRecordCount("TestCollection1");

        // Assert
        Assert.Equal(2, result);
    }
}
```

## HarnessBase Class
The `HarnessBase` class is an abstract class that, during construction, loads the Harness settings and data files, and configures the Mongo instance defined inside the settings file.
Using the `HarnessBase` class with a Harness Settings file and Harness Data file(s) is the simplest and easiest way to configure a Mongo instance...

1.	Create a json file containing an array of json objects (one file per mongo collection)
2.	Create a settings file
3.	Make your test class extend `HarnessBase`
4.	Optionally annotate your test class with the `HarnessConfig` attribute to specify the file path of your settings file. If the attribute is not present, or a configuration file path is not specified on the attribute, a default value of [ClassName].json will be used.


## XUnit Examples

### Auto Configuration
To use Harness:
- add the `HarnessConfig` attribute to the class that contains the tests 
- specify the relative path to the Harness settings file by setting the ConfigFilePath variable 
- have the class that contains the tests extend `HarnessBase` 

When a test is run, the `HarnessBase` class will use the settings file to put the Mongo databases specified within it into the desired state. This will happen automatically when the class is constructed.

The `HarnessBase` class exposes the `IMongoClient` objects that it created while setting up the databases so that, if required, they can be re-used in the tests. This is exposed as a `Dictionary<string, IMongoClient>` where the dictionary key is the mongo server connection string.

```csharp
[HarnessConfig(ConfigFilePath = "ExampleSettings.json")]
public class AutoConfigureDatabase : HarnessBase
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var classUnderTest = new ClassUnderTest();

        // Act
        var result = classUnderTest.GetCollectionRecordCount("TestCollection1");

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Test2()
    {
        // Arrange
        var classUnderTest = new ClassUnderTest();

        // The method we are testing needs an instance of IMongoClient.
        // Rather than create a new one, we can re-use the one that was 
        // created by the HarnessBase class when it was setting up the 
        // databases.
        var mongoClient = this.MongoConnections["mongodb://localhost:20719"];

        // Act
        var result = classUnderTest.GetCollectionRecordCount(mongoClient, "TestCollection1");

        // Assert
        Assert.Equal(2, result);
    }
}
```

### Manual Configuration
To use Harness:
- add the `HarnessConfig` attribute to the class that contains the tests 
- specify the relative path to the Harness settings file by setting the ConfigFilePath variable 
- specify `AutoRun = false` to stop the databases being created 
- have the class that contains the tests extend `HarnessBase` on class construction 

Setting AutoRun = false, tells the `HarnessBase` class not to setup the databases until the BuildDatabase() method is called.

The `HarnessBase` class exposes the IMongoClient objects that it created while setting up the databases so that, if required, they can be re-used in the tests. This is exposed as a `Dictionary<string, IMongoClient>` where the dictionary key is the mongo server connection string.

```csharp
[HarnessConfig(ConfigFilePath = "ExampleSettings.json", AutoRun = false)]
public class ManuallyConfigureDatabase : HarnessBase
{
    [Fact]
    public void Test1()
    {
        // Arrange

        // As AutoRun is set to false on the class attribute, the BuildDatabase()
        // must be called to tell the HarnessBase class to setup the databases.
        this.BuildDatabase();

        var classUnderTest = new ClassUnderTest();

        // Act
        var result = classUnderTest.GetCollectionRecordCount("TestCollection1");

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Test2()
    {
        // Arrange

        // As AutoRun is set to false on the class attribute, the BuildDatabase()
        // must be called to tell the HarnessBase class to setup the databases.
        this.BuildDatabase();

        var classUnderTest = new ClassUnderTest();
        var mongoClient = this.MongoConnections["mongodb://localhost:20719"];

        // Act
        var result = classUnderTest.GetCollectionRecordCount(mongoClient, "TestCollection1");

        // Assert
        Assert.Equal(2, result);
    }
}
```

### Test Fixture Configuration
To use Harness with an XUnit ClassFixture:
- add the `HarnessConfig` attribute to the class fixture.
- specify the relative path to the Harness settings file by setting the ConfigFilePath variable
- have the class fixture extend `HarnessBase`

When a test is run, the `HarnessBase` class will use the settings file to 
put the mongo databases specified within it into the desired state.
This will happen automatically when the class constructor is called.

The `HarnessBase` class exposes the IMongoClient objects that it created
while setting up the databases so that, if required, they can be re-used
in the tests. This is exposed as a `Dictionary<string, IMongoClient>`
where the dictionary key is the mongo server connection string.

```csharp
[HarnessConfig(ConfigFilePath = "ExampleSettings.json")]
public class DatabaseFixture : HarnessBase, IDisposable
{
    public DatabaseFixture()
    {
        // Any other setup stuff...
    }

    public void Dispose()
    {
        // Any cleaning up...
    }
}

public class UsingTheBaseClassWithClassFixture : IClassFixture<DatabaseFixture>
{
    public UsingTheBaseClassWithClassFixture(DatabaseFixture fixture)
    {
        this.Fixture = fixture;
    }

    private DatabaseFixture Fixture { get; }

    [Fact]
    public void Test1()
    {
        // Arrange
        var classUnderTest = new ClassUnderTest();

        // Act
        var result = classUnderTest.GetCollectionRecordCount("TestCollection1");

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Test2()
    {
        // Arrange
        var classUnderTest = new ClassUnderTest();
        var mongoClient = this.Fixture.MongoConnections["mongodb://localhost:27017"];

        // Act
        var result = classUnderTest.GetCollectionRecordCount(mongoClient, "TestCollection1");

        // Assert
        Assert.Equal(2, result);
    }
}
```


## Fluent Configuration with the SettingsBuilder and HarnessManager classes
With the `SettingsBuilder` and `HarnessManager` classes you can use Harness to setup your Mongo databases without the need for configuration files.

### SettingsBuilder
The `SettingsBuilder` class provides a fluent api to build a configuration object for the `HarnessManager`. You can continue to use json files to load test data, or implement the `IDataProvider` interface to give Harness the data from code.

#### Example using json files
This example creates a configuration for one database called 'TestDb1', that has two collections...
```csharp
var settings =
    new SettingsBuilder()
        .AddDatabase("TestDb1")
        .WithConnectionString("mongodb://localhost:27017")
        .DropDatabaseFirst()
        .AddCollection("col1", true, "path/to/Collection1.json")
        .AddCollection("col2", true, "path/to/Collection2.json")
        .Build();
```

#### Example using IDataProvider
This example creates a configuration for a database called 'TestDb2', that has one collection...
```csharp
var settings =
    new SettingsBuilder()
        .AddDatabase("TestDb2")
        .WithConnectionString("mongodb://localhost:27017")
        .DropDatabaseFirst()
        .AddCollection<Person>("people", true, new PersonDataProvider())
        .Build();


public class PersonDataProvider : IDataProvider
{
    public IEnumerable<object> GetData()
    {
        return new List<Person>
        {
            new Person {FirstName = "Peter", LastName = "Venkman", Age = 31},
            new Person {FirstName = "Ray", LastName = "Stantz", Age = 32},
            new Person {FirstName = "Egon", LastName = "Spengler", Age = 33}
        };
    }
}

public class Person
{
    public ObjectId Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}
```

#### Adding Mongo Conventions
The `SettingsBuilder` api also allows you to register global Mongo conventions before the configuration is run. This example shows how to add the `CamelCaseElementNameConvention` to all classes whose namespace starts with "MyProject"...
```csharp
var settings =
    new SettingsBuilder()
        .AddConvention(new CamelCaseElementNameConvention(), x => x.Namespace.StartsWith("MyProject"))
        .AddDatabase("TestDb1")
        .WithConnectionString("mongodb://localhost:27017")
        .DropDatabaseFirst()
        .AddCollection("col1", true, "path/to/Collection1.json")
        .Build();
```

### HarnessManager
Once you have your settings object, you can pass it to the `HarnessManager` and use it to setup the database. The `build()` method returns a `Dictionary<string, IMongoClient>` containing the MongoClient instances (one per unique connection string in the settings) for re-use. The dictionary key if the connection string.

```csharp
var mongoClients =
    new HarnessManager()
        .UsingSettings(settings) // the settings object built using the SettingsBuilder
        .Build();
```

### XUnit Example
Here, we call `build()` in the test class constructor which means the configuration will run before each test in the class is executed...
```csharp
public class SettingsBuilderWithDataFiles
{
    public SettingsBuilderWithDataFiles()
    {
        var settings =
            new SettingsBuilder()
                .AddDatabase("TestDb1")
                .WithConnectionString("mongodb://localhost:27017")
                .DropDatabaseFirst()
                .AddCollection("col1", true, "path/to/Collection1.json")
                .AddCollection("col2", true, "path/to/Collection2.json")
                .Build();

        this.MongoConnections =
            new HarnessManager()
                .UsingSettings(settings)
                .Build();
    }

    private Dictionary<string, IMongoClient> MongoConnections { get; }

    [Fact]
    public void Test1()
    {
        // Arrange
        var classUnderTest = new ClassUnderTest();

        // Act
        var result = classUnderTest.GetCollectionRecordCount("col1");

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Test2()
    {
        // Arrange
        var classUnderTest = new ClassUnderTest();
        var mongoClient = this.MongoConnections["mongodb://localhost:27017"];

        // Act
        var result = classUnderTest.GetCollectionRecordCount(mongoClient, "col1");

        // Assert
        Assert.Equal(2, result);
    }
}
```

## API Documentation
You can find the API documentation [here](http://amcn41r.github.io/harness).