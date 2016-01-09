
#r "bin\debug\Harness.dll"
#r "bin\debug\System.IO.Abstractions.dll"

open Harness
open System.IO.Abstractions

let fs = new FileSystem()

let sm = new Harness.Settings.SettingsManager(fs)

let s = sm.GetMongoConfiguration("C:\Dev\Harness\Harness\ExampleSettings.json")

//s.Databases.Item(0).ConnectionString

let m = new Harness.MongoSessionManager(s)

m.Build()


