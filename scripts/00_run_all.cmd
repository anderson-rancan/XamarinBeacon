call 01_coverage
call 02_coveralls
call 03_sonarqube

del ..\coverage.xml
del ..\TestResult.xml