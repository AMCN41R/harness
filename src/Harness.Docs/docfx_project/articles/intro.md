---
title: 'Introducing Harness, an integration test harness for MongoDb'
tags:
  - Harness
  - 'C#'
  - Mongo
  - MongoDb
  - Test
  - Testing
  - Unit Test
  - Unit Testing
  - Integration Test
  - Integration Testing
date: 2017-07-11 09:32:04
---

> Originally posted on [Ramblings](https://amcn41r.github.io/blog/) by [Alex McNair](https://github.com/AMCN41R)

## What is Harness?
Harness is a free and open source .NET library designed to manage the state of a Mongo database during testing.

At its core, it endeavours to aid unit and integration testing by putting one or more Mongo databases in a known state at the beginning of a test, or suite of tests. The framework allows you to control your code’s dependency on a Mongo database, therefore increasing test consistency and repeat-ability.

## Why do I need this?
If you want to perform integration tests on your Mongo repositories, it would be really helpful to have a known set of data in your target database so that you can write accurate and consistent tests. Harness allows you to do exactly that!

## How do I get it?
The easiest way to include it in a project is via [nuget](https://www.nuget.org/packages/Harness/), and can be installed with...
```
Install-Package Harness
```

## What's the simplest way to get it working?
Do you like config files or fluent configuration in code? Either way, Harness has you covered. You can find examples of both [here](https://github.com/AMCN41R/Harness/blob/master/README.md), or see below for a simple config file example...

Once you have added Harness to you project, one way to get up and running is with 2 simple json files...

#### Settings File
The settings file must have a `.json` extension. It is a json object that contains a `databases` property that is an array of `database` objects.

##### Example settings file, 'ExampleSettings.json'
```json
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
                    "dataFileLocation": "Collection1.json", // this is the path to a data file described below
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

#### Data File
Test data files must have a .json extension and contain an array of json objects.

##### Example data file, 'Collection1.json'
```json
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

Once you have created the settings and data files, all you need to do is make your class of tests extend `HarnessBase`, and give it the `[HarnessConfig]` attribute with the path to your settings file...

```csharp
using Harness;

[HarnessConfig(ConfigFilePath = "path/to/settings.json")]
public class MyMongoIntegrationTests : HarnessBase {
    // tests go here
    ...
}
```

## Are there more examples?
You can check out examples of how to get started with both the config files and fluent configuration, as well as several XUnit examples on the Harness [GitHub](https://github.com/AMCN41R/harness) page.


## Contributing
If you find any issues, or have any suggestions, feel free to [log an issue](https://github.com/AMCN41R/Harness/issues) or create a [pull request](https://github.com/AMCN41R/Harness/pulls).


## License
[MIT License](https://github.com/AMCN41R/Harness/blob/dev/LICENSE)