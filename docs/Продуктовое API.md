# Production

[Содержание](Содержание.md)

### Запуск
1. Настроить Google-аутенфикацию
2. Положить файлы конфигов (appsettings.json и secrets.json) в одну папку
2. Прописать в переменной среды `CONFIG_DIRECTORY` абсолютный путь до этой папки
3. Запустить приложение в Production-окружении ([подробнее](https://learn.microsoft.com/ru-ru/aspnet/core/fundamentals/environments?view=aspnetcore-7.0#environments)), чтобы отключились ExceptionDevPages, и чтобы при поиске конфигов учитывалась `CONFIG_DIRECTORY`