..\..\tools\mutantbuild\mutantbuild @debug(1.0).response /property:Configuration=Release /property:OutputPath=%cd%\bin\ /target:Clean;Rebuild ..\nunit.sln
..\..\tools\wix\candle NUnit.wxs UI.wxi bin.wxi doc.wxi src.wxi samples.wxi
..\..\tools\wix\light NUnit.wixobj UI.wixobj bin.wixobj doc.wixobj src.wixobj samples.wixobj -out NUnit-2.2.1.msi