# Harness

Harness is a .NET library designed to manage the state of a Mongo database during testing.

## About
Harness is designed to aid unit and integration testing by putting a Mongo database in a known state at the beginning of test, or suite of tests. The framework allows you to control your code’s dependency on a Mongo database, therefore increasing test consistency and repeat-ability.

Harness is an open source project, licensed under the [MIT License](https://github.com/AMCN41R/Harness/blob/dev/LICENSE)

Harness was inspired by [NDBUnit](https://github.com/NDbUnit/NDbUnit).

## Basic Usage
Harness will put one or more Mongo databases into a state defined by a json settings file and one or more json data files.

### Example settings file, 'ExampleSettings.json'
```
{
    "Databases": [
        {
            "DatabaseName": "TestDb1",
            "ConnectionString": "mongodb://localhost:27017",
            "DatabaseNameSuffix": "",
            "CollectionNameSuffix": "",
            "DropFirst": true,
            "Collections": [
                {
                    "CollectionName": "TestCollection1",
                    "DataFileLocation": "Collection1.json",
                    "DropFirst": false
                },
                {
                    "CollectionName": "TestCollection2",
                    "DataFileLocation": "Collection2.json",
                    "DropFirst": false
                }
            ]
        }
    ]
}
```

### Example data file, 'Collection1.json'
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

Point your test class at the settings file using the HarnessConfig attribute, and extend HarnessBase to have the database configuration run before each test.


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

## Settings File
TODO

## Data File
TODO

## HarnessConfig Attribute
TODO

## HarnessBase Class
The HarnessBase class is an abstract class that, during construction, loads the Harness settings and data files, and configures the Mongo instance defined inside the settings file.
Using the HarnessBase class with a Harness Settings file and a Harness Data file(s) is the simplest and easiest way to configure a Mongo instance...

1.	Create json file containing an array of json objects (one file per mongo collection)
2.	Create a settings file
3.	Make your test class extend HarnessBase
4.	Optionally annotate your test class with the HarnessConfig attribute to specify the file path of your settings file. If the attribute is not present, or a configuration file path is not specified on the attribute, a default value of [ClassName].json will be used.

##### Manual #####
A manual version on the HarnessBase class will work in the same way (inheritance and attribute) but will require a method call to configure Mongo, rather than it happening in the constructor.

##### XUnit Test Fixture #####
A version that is configured to work with the XUnit class fixture design (a shared context between tests, similar to NUnit SetUp and TearDown). The default behaviour of XUnit is to create a new instance of the test class for each test inside it. If using the HarnessBase, this would result in the database being deleted (depending on the settings) and re-created for every test. Using this base class will ensure this only happens once for a group of tests.


## XUnit Examples

### Auto Configuration
To use Harness:
- add the HarnessConfig attribute to the class that contains the tests 
- specify the relative path to the Harness settings file by setting the ConfigFilePath variable 
- have the class that contains the tests extend HarnessBase 

When a test is run, the HarnessBase class will use the settings file to put the mongo databases specified within it into the desired state. This will happen automatically when the class constructor is called.

The HarnessBase class exposes the IMongoClient objects that it created while setting up the databases so that, if required, they can be re-used in the tests. This is exposed as a a `Dictionary<string, IMongoClient>` where the dictionary key in the mongo server connection string.

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
        var mongoClient = base.MongoConnections["mongodb://localhost:20719"];

        // Act
        var result = classUnderTest.GetCollectionRecordCount(mongoClient, "TestCollection1");

        // Assert
        Assert.Equal(2, result);
    }
}
```

### Manual Configuration
To use Harness:
- add the HarnessConfig attribute to the class that contains the tests 
- specify the relative path to the Harness settings file by setting the ConfigFilePath variable 
- specify AUtoRun = false to stop the databases being created 
- have the class that contains the tests extend HarnessBase on class construction 

Setting AutoRun = false, tells the HarnessBase class not to setup the databases until the BuildDatabase() method is called.

The HarnessBase class exposes the IMongoClient objects that it created while setting up the databases so that, if required, they can be re-used in the tests. This is exposed as a a `Dictionary<string, IMongoClient>` where the dictionary key in the mongo server connection string.

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
        base.BuildDatabase();

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
        base.BuildDatabase();

        var classUnderTest = new ClassUnderTest();

        // The method we are testing needs an instance of IMongoClient.
        // Rather than create a new one, we can re-use the one that was 
        // created by the HarnessBase class when it was setting up the 
        // databases.
        var mongoClient = base.MongoConnections["mongodb://localhost:20719"];

        // Act
        var result = classUnderTest.GetCollectionRecordCount(mongoClient, "TestCollection1");

        // Assert
        Assert.Equal(2, result);
    }
}
```

### Test Fixture Configuration
TODO

## Advanced Configuration with the HarnessManager Class [Coming Soon]
The HarnessManager class allows full control of all the features that Harness provides. It gives full control over the Harness settings and control/access to the data and underlying connection objects. It allows the user to build settings and configurations dynamically and without the need of an external settings file. It allows the user to configure a mongo instance from inside a test, or class of their choosing, or to create their own base class.
