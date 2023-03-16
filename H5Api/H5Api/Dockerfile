FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY H5Api/H5Api/bin/Debug/net6.0/publish/ App/
WORKDIR /App
ENTRYPOINT [ "dotnet", "H5Api.dll" ]