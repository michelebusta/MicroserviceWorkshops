FROM microsoft/aspnetcore:1.0.1
WORKDIR /app
COPY bin/Release/PublishOutput /app
ENTRYPOINT ["dotnet", "IdentityHistoryConsumer.dll"]
