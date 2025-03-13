# Настройка Google-аутентификации

[Содержание](Содержание.md)

Краткая инструкция:
1. Перейти на https://console.cloud.google.com/apis/credentials
2. Создаём приложение:
   - CREATE CREDENTIALS (кнопка в хэдере страницы) > OAuth client ID
   - Application type > Web application
   - Нажать `CREATE`
3. Перейти в меню созданного клиента (Раздел OAuth 2.0 Client ID на https://console.cloud.google.com/apis/credentials)
4. В открывшейся странице в разделе Authorized redirect URIs добавить URL: `<baseAppUrl>/api/v1/auth/callback`
    - Для локального запуска можно взять такой `<baseAppUrl>`: `http://localhost:8080`.
      Тогда в Authorized redirect URIs нужно добавить `http://localhost:8080/api/v1/auth/callback`
5. В разделе Additional information находим Client ID и Client secret.
6. Добавляем их в соответствующие поля в secrets.json

Более подробная [инструкция от Microsort](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-8.0)

Примечания:
1. Изменения в Authorized redirect URIs могут примениться не сразу, поэтому аутентификация локально может заработать тоже не сразу. Обычно нужно подождать примерно 5 минут.
