FROM mcr.microsoft.com/dotnet/aspnet:6.0

ENV CONFIG_DIRECTORY="/kwishes/configs"

RUN mkdir -p /kwishes/configs  \
    && mkdir -p /kwishes/static

COPY . /kwishes/bin/

CMD dotnet /kwishes/bin/KWishes.API.dll --urls "http://0.0.0.0:8080"