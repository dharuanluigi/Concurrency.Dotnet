FROM mcr.microsoft.com/dotnet/sdk:6.0.419-jammy-amd64 AS builder
WORKDIR /build
COPY ./Concurrency.Dotnet.csproj .
RUN dotnet restore -r linux-x64
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0.27-jammy-chiseled
WORKDIR /app
ENV ASPNETCORE_URLS http://*:9999
COPY --from=builder /build/out .
ENTRYPOINT [ "/app/Concurrency.Dotnet" ]
