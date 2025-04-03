FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS="http://+:5000"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /workspace

COPY ["OCR.AIVision.Api/OCR.AIVision.Api.csproj", "OCR.AIVision.Api/"]
COPY ["OCR.AIVision.Application/OCR.AIVision.Application.csproj", "OCR.AIVision.Application/"]
COPY ["OCR.AIVision.Domain/OCR.AIVision.Domain.csproj", "OCR.AIVision.Domain/"]
RUN dotnet restore "OCR.AIVision.Api/OCR.AIVision.Api.csproj"

COPY . .
WORKDIR "/workspace/OCR.AIVision.Api"
RUN dotnet build "OCR.AIVision.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "OCR.AIVision.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "OCR.AIVision.Api.dll"]