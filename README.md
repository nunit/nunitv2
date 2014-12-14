NUnit V2
========

**NUnit V2 IS NO LONGER MAINTAINED OR UPDATED.**

**Bugs should be reported in the main [NUnit 3.0 repository](https://github.com/nunit/nunit).**

The final version of NUnit V2 is 2.6.4. That code is available here for anyone who needs it. Further NUnit
development is taking place under a separate NUnit 3.0 project. That project used to be located on
Launchpad at http://launchpad.net/nunit-3.0 but it was moved to GitHub at the end of October 2013.

NUnit is a test framework for all .NET languages, running on Microsoft .NET and Mono.

NUnit is written in C# but can run tests written in any language that generates managed code. Initially ported 
from JUnit, it has been completely redesigned to take advantage of the features of the common language runtime, 
such as custom attributes, generics and lambda expressions.

NUnit V2 was initially hosted on Sourceforge. The last Sourceforge release was NUnit 2.5.2. Beginning with NUnit 2.5.3, 
releases were made on Launchpad, and may still be found there.

Important note about using async/await
----
NUnit 2.6.3 introduced support for .NET 4.5's async/await. 2.6.3 support is limited to .NET 4.5 and won't work with the [Async Targeting Pack] that Microsoft released to backport async/await to .NET 4.0.

NUnit 2.6.4 adds support for the [Async Targeting Pack]. You should use 2.6.4 if you are using async/await and targeting .NET 4.0.

[Async Targeting Pack]:http://blogs.msdn.com/b/bclteam/p/asynctargetingpackkb.aspx