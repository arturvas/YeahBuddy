FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/backend/YeahBuddy.API/YeahBuddy.API.csproj", "src/backend/YeahBuddy.API/"]
COPY ["src/shared/YeahBuddy.Communication/YeahBuddy.Communication.csproj", "src/shared/YeahBuddy.Communication/"]
COPY ["src/shared/YeahBuddy.Exceptions/YeahBuddy.Exceptions.csproj", "src/shared/YeahBuddy.Exceptions/"]
COPY ["src/backend/YeahBuddy.Application/YeahBuddy.Application.csproj", "src/backend/YeahBuddy.Application/"]
COPY ["src/backend/YeahBuddy.Domain/YeahBuddy.Domain.csproj", "src/backend/YeahBuddy.Domain/"]
COPY ["src/backend/YeahBuddy.Infrastructure/YeahBuddy.Infrastructure.csproj", "src/backend/YeahBuddy.Infrastructure/"]
RUN dotnet restore "src/backend/YeahBuddy.API/YeahBuddy.API.csproj"
COPY . .
WORKDIR "/src/src/backend/YeahBuddy.API"
RUN dotnet build "./YeahBuddy.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./YeahBuddy.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YeahBuddy.API.dll"]
