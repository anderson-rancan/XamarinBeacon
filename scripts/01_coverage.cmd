pushd ..\

.\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user "-target:.\packages\NUnit.ConsoleRunner.3.5.0\tools\nunit3-console.exe" "-targetargs: .\XamarinBeacon.Console.Test.Unit\bin\Debug\XamarinBeacon.Console.Test.Unit.dll" -output:coverage.xml -returntargetcode

.\packages\ReportGenerator.2.5.2\tools\ReportGenerator.exe "-reports:coverage.xml" "-targetdir:.\.coverage"

popd