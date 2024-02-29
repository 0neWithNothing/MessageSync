Generator рализовал с помощью masstranist worker, а Executor с помощью masstransit consumer. <br/>
Запуск - ```docker compose up --build -d``` <br/>
Количество экземпляров Generator и Executor можно изменить в ```docker-compose.yml``` файле: <br/>
```
    deploy:
      mode: replicated
      replicas: 1
``` <br/>
Смотрю логи через docker desctop
