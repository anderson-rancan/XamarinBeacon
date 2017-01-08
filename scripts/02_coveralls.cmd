pushd ..\

for /f %%i in ('git rev-parse HEAD') do set commit_id=%%i
for /f %%i in ('git rev-parse --abbrev-ref HEAD') do set commit_branch=%%i

.\packages\coveralls.net.0.7.0\tools\csmacnz.Coveralls.exe --opencover -i ./coverage.xml --repoToken srgzmvxl5EkKPV5eD1gmU92LWNO9rOxza --commitId %commit_id% --commitBranch %commit_branch% --commitAuthor "anderson-rancan" --commitEmail "anderson.rancan@gmail.com" --commitMessage "local build"

popd