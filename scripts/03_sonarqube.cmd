pushd ..\

C:\dev\SonarQube\sonar-scanner-msbuild-2.2.0.24\SonarQube.Scanner.MSBuild.exe begin /k:"XamarinBeacon" /d:"sonar.host.url=https://sonarqube.com" /d:"sonar.login=db459b51c0eca6abb9a48485cf2b997dc8b32751" /d:sonar.cs.opencover.reportsPaths="coverage.xml"

"C:\Program Files (x86)\MSBuild\14.0\Bin\MsBuild.exe" /t:Rebuild

C:\dev\SonarQube\sonar-scanner-msbuild-2.2.0.24\SonarQube.Scanner.MSBuild.exe end /d:"sonar.login=db459b51c0eca6abb9a48485cf2b997dc8b32751"

popd