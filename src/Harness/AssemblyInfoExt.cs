// Expose internal classes to script and unit test projects.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Harness.Tests.Unit")]
[assembly: InternalsVisibleTo("Harness.Tests.Integration")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]