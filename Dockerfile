
FROM mcr.microsoft.com/dotnet/sdk:11.0 AS build
WORKDIR /src

COPY CreadorIAWeb.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish CreadorIAWeb.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:11.0
WORKDIR /app

# FFmpeg y yt-dlp para procesar y descargar videos.
RUN apt-get update \
    && apt-get install -y --no-install-recommends ffmpeg ca-certificates curl \
    && curl -fL https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp_linux -o /usr/local/bin/yt-dlp \
    && chmod a+rx /usr/local/bin/yt-dlp \
    && yt-dlp --version \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 10000
CMD ["sh", "-c", "dotnet CreadorIAWeb.dll --urls http://0.0.0.0:${PORT:-10000}"]
