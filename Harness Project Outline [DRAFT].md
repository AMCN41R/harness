# Harness #

Harness is a .NET library designed to manage the state of a Mongo database during testing.

## About ##
Harness is designed to aid unit and integration testing by putting a Mongo database in a known state at the beginning of test, or suite of tests. The framework allows you to control your code’s dependency on a Mongo database, therefore increasing test consistency and repeat-ability.

Harness will be open source, available to download via nuget, and is available now on the Branded3 github account:

[Harness on github](https://github.com/Branded3/Harness)

Harness borrows ideas from [NDBUnit](https://github.com/NDbUnit/NDbUnit).

## Design Basics ##
The basic design principle is to have a set of classes that will put one or more Mongo databases in a state defined by a json settings file, using data from one or more json/bson data files. The data file could be a mongo dump, bson file, or new-line delimited json file that could be parsed as a bson document.


Example settings file:

	{
  		"SaveOutput": false,
  		"Databases": [
    		{
		      "DatabaseName": "TestDb1",
		      "ConnectionString": "mongodb://localhost:27017",
		      "DatabaseNameSuffix": "",
		      "CollectionNameSuffix": "",
		      "DropFirst": false,
		      "Collections": [
		        {
		          "CollectionName": "TestCollection1",
		          "DataFileLocation": "C:\\TestData\\Collection1.json",
		          "DropFirst": false
		        },
		        {
		          "CollectionName": "TestCollection2",
		          "DataFileLocation": "C:\\TestData\\Collection2.json",
		          "DropFirst": false
		        }
		      ]
		    }
		  ]
		}

Example data file:

	{"Col1": "Value1","Col2": "Value2"}
	{"Col1": "Value3","Col2": "Value4"}



## Features ##
### HarnessBase ###
The HarnessBase class is an abstract class that, during construction, loads the Harness Settings and Data files and configures the Mongo instance defined inside the settings file.
Using the HarnessBase class with a Harness Settings file and a Harness Data file(s) is the simplest and easiest way to configure a Mongo instance...

1.	Create a new line delimited .json file containing json objects (one file per mongo collection)
2.	Create a settings file
3.	Make your test class extend HarnessBase
4.	Optionally annotate your test class with the HarnessConfig attribute to specify the file path of your settings file. If the attribute is not present, or a configuration file path is not specified on the attribute, a default value of [ClassName].json will be used.

### HarnessBase Variations – Manual & XUnit Test Fixture ###
Variations of the HarnessBase class that work in a similar way but provide a bit more control for the developers/scenarios that will require it.

##### Manual #####
A manual version on the HarnessBase class will work in the same way (inheritance and attribute) but will require a method call to configure Mongo, rather than it happening in the constructor.

##### XUnit Test Fixture #####
A version that is configured to work with the XUnit class fixture design (a shared context between tests, similar to NUnit SetUp and TearDown). The default behaviour of XUnit is to create a new instance of the test class for each test inside it. If using the HarnessBase, this would result in the database being deleted (depending on the settings) and re-created for every test. Using this base class will ensure this only happens once for a group of tests.

### HarnessManager ###
The HarnessManager class will allow full control of all the features that Harness provides. It will give full control over the Harness settings and control/access to the data and underlying connection objects. It will allow the user to build settings and configurations dynamically and without the need of an external settings file. It will allow the user to configure a mongo instance from inside a test, or class of their choosing, or to create their own base class.

## Documentation ##
The source will contain full documentation on each of the Harness classes, a quick start guide and a specification for the json settings and data files.

## Examples ##
The source will include example projects showing how to use each of the Harness classes with the 3 most common test frameworks, XUnit, NUnit and MSTest.

## Roadmap ##
v0.1 - Deliver HarnessBase, documentation & xunit examples

v0.2 - Configuration for Nuget availability

v0.3 - Extend the example projects to include NUnit and MSTest

v0.4 - Deliver the base class variations, documentation & examples

v1.0 - Deliver the HarnessManager class, documentation and examples

v2.0 - Add Elastic Search implementation?

v3.0 - Add plug-in support?




