# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy toàn bộ code vào và build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Railway yêu cầu chạy trên cổng 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Quan trọng: Thay 'Shop-Demo-BE-TBP.dll' bằng tên project của bạn
ENTRYPOINT ["dotnet", "DEMO_Shop"]
