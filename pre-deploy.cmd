dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\test\TauCode.Parsing.Tests\TauCode.Parsing.Tests.csproj
dotnet test -c Release .\test\TauCode.Parsing.Tests\TauCode.Parsing.Tests.csproj

nuget pack nuget\TauCode.Parsing.nuspec