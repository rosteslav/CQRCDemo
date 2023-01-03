#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Post.Query.Api/Post.Query.Api.csproj", "Post.Query.Api/"]
RUN dotnet restore "Post.Query.Api/Post.Query.Api.csproj"
COPY . .
WORKDIR "/src/Post.Query.Api"
RUN dotnet build "Post.Query.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Post.Query.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Post.Query.Api.dll"]