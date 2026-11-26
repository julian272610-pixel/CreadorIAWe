FROM mcr.microsoft.com/dotnet/sdk:11.0 AS build
WORKDIR /src

COPY CreadorIAWeb.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish CreadorIAWeb.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:11.0
WORKDIR /app

# FFmpeg procesa los videos que los usuarios suben para crear clips MP4.
RUN apt-get update \
    && apt-get install -y --no-install-recommends ffmpeg ca-certificates \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 10000
CMD ["sh", "-c", "dotnet CreadorIAWeb.dll --urls http://0.0.0.0:${PORT:-10000}"]
