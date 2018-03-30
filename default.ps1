
task default -depends Clean, Build, Docs

task Clean {
    "Deleting /bin contents..."
    Get-ChildItem -inc bin -rec | Remove-Item -rec -force
    "Deleting /bin contents...DONE"

    "Deleting /obj contents..."
    Get-ChildItem -inc obj -rec | Remove-Item -rec -force
    "Deleting /obj contents...DONE"

    "Deleting Old Test Results..."
    Get-ChildItem -inc TestResults -rec | Remove-Item -rec -force
    "Deleting Old Test Results...DONE"
}

task Build {
    dotnet build .\src\Harness.sln -c Release
}

task Test {
    $testdlls = 
        ".\src\Harness.Tests.Unit\bin\Release\netcoreapp2.0\Harness.Tests.Unit.dll",
        ".\src\Harness.Tests.Integration\bin\Release\netcoreapp2.0\Harness.Tests.Integration.dll"

    dotnet vstest  $testdlls --Parallel --logger:trx
}

task Docs {
    "Generating documentation..."
    .\tools\docfx\docfx .\src\Harness.Docs\docfx_project\docfx.json
    "Generating documentation...DONE"

    "Cleaning old docs folder..."
    Get-ChildItem -Path .\Docs\ -Recurse | Remove-Item -Recurse -Force
    "Cleaning old docs folder...DONE"

    "Copying to docs to root..."
    Copy-Item ".\src\Harness.Docs\docfx_project\_site\*" -Destination ".\docs\" -Recurse
    "Copying docs to root...DONE"
}

task Pack {
    "Packing deployables..."
    dotnet pack .\src\Harness\ --configuration Release --output ..\..\tools\nuget\versions
    
    if ($lastexitcode -ne 0) {
        throw "Packing task failed! Build failed."
    }
    "Packing deployables...DONE"
}