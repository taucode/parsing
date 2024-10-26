dotnet restore

dotnet build TauCode.Parsing.sln -c Debug
dotnet build TauCode.Parsing.sln -c Release

dotnet test TauCode.Parsing.sln -c Debug
dotnet test TauCode.Parsing.sln -c Release

nuget pack nuget\TauCode.Parsing.nuspec