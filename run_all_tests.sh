for test in $(find . -name "*Tests*.csproj"); do dotnet test $test; done;