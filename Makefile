build:
	dotnet build .

test:
	dotnet test Netezos.Tests --nologo --verbosity normal --filter FullyQualifiedName!~Rpc