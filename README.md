# Zephyrus

**Zephyrus** — система управления закупками, реализованная как набор микросервисов на платформе .NET 10. Проект строится на принципах чистой архитектуры, Vertical Slice Architecture и паттерна CQRS.

---

<h1 align="left">Архитектурная схема</h1>

<div align="center">
  <img src="https://github.com/ushkdn/Zephyrus/blob/development/architecture.png" height="400" width="300" />
</div>

---

## Содержание

- [Архитектурные принципы](#архитектурные-принципы)
- [Микросервисы](#микросервисы)
- [Инфраструктура](#инфраструктура)
- [Структура проекта](#структура-проекта)
- [База данных — схемы таблиц](#база-данных--схемы-таблиц)
- [NuGet-пакеты](#nuget-пакеты)
- [Конфигурация окружения](#конфигурация-окружения)
- [Запуск проекта](#запуск-проекта)

---

## Архитектурные принципы

| Принцип | Описание |
|---|---|
| **Clean Architecture** | Разделение на слои Domain → Application → Infrastructure → Presentation → WebApi |
| **Vertical Slice Architecture** | Каждая фича изолирована в отдельной папке с полным набором файлов (Request, Handler, Response, Validator) |
| **CQRS** | Команды и запросы строго разделены; обработчики регистрируются через MediatR |
| **Domain-Driven Design** | Каждый микросервис имеет собственную доменную модель и изолированную базу данных |

---

## Микросервисы

| Сервис | Контейнер | Порт | Описание |
|---|---|---|---|
| **Gateway** | `zephyrus.gateway` | `7001` | API-шлюз на базе YARP Reverse Proxy. Единая точка входа для всех клиентов |
| **Identity** | `zephyrus.identity` | `7002` | Аутентификация и управление пользователями. JWT + Refresh Tokens, Reset/Forgot Password через email |
| **Catalog** | `zephyrus.catalog` | `7000` | Справочник товаров и категорий. Иерархические категории, управление продуктами |
| **Supplier** | `zephyrus.supplier` | `7005` | Управление поставщиками и прайс-листами. Привязка продуктов к поставщикам с ценой и валютой |
| **Procurement** | `zephyrus.procurement` | `7004` | Закупочный процесс. Заявки на закупку (Purchase Requests) и заказы (Orders) |
| **Notification** | `zephyrus.notification` | `7003` | Внутренние уведомления. Создание, хранение и пометка уведомлений как прочитанных |
| **UI** | `zephyrus.ui` | `3000` | Фронтенд-приложение |

### Функциональность по сервисам

#### Identity
- `POST /auth/sign-up` — регистрация пользователя
- `POST /auth/sign-in` — вход, выдача JWT + Refresh Token
- `POST /auth/refresh-token` — обновление JWT
- `POST /auth/forgot-password` — запрос сброса пароля (письмо на email)
- `POST /auth/reset-password` — сброс пароля по коду из письма
- `GET /users/{id}` — получение профиля пользователя

#### Catalog
- `GET/POST /categories` — список категорий / создание
- `GET/PUT/DELETE /categories/{id}` — категория по ID / обновление / удаление
- `GET/POST /products` — список продуктов / создание
- `GET/PUT/DELETE /products/{id}` — продукт по ID / обновление / удаление

#### Supplier
- `GET/POST /suppliers` — список поставщиков / создание
- `GET/PUT/DELETE /suppliers/{id}` — поставщик по ID / обновление / удаление
- `GET /suppliers/{id}/products` — товары поставщика
- `POST /suppliers/{id}/products` — добавление товара поставщику
- `PUT/DELETE /suppliers/{id}/products/{productId}` — обновление / удаление товара поставщика

#### Procurement
- `GET/POST /purchase-requests` — список заявок / создание
- `GET /purchase-requests/{id}` — заявка по ID
- `PUT /purchase-requests/{id}/approve` — одобрение заявки
- `PUT /purchase-requests/{id}/reject` — отклонение заявки
- `GET/POST /orders` — список заказов / создание (из одной или нескольких заявок)
- `GET /orders/{id}` — заказ по ID
- `PUT /orders/{id}/confirm` — подтверждение заказа
- `PUT /orders/{id}/deliver` — доставка заказа
- `PUT /orders/{id}/cancel` — отмена заказа

#### Notification
- `GET /notifications/{recipientId}` — уведомления получателя
- `POST /notifications` — создание уведомления
- `PUT /notifications/{id}/read` — пометить как прочитанное

---

## Инфраструктура

| Сервис | Образ | Порт | Назначение |
|---|---|---|---|
| **PostgreSQL (Identity)** | `postgres:latest` | `5433` | База данных Identity-сервиса |
| **PostgreSQL (Catalog)** | `postgres:latest` | `5434` | База данных Catalog-сервиса |
| **PostgreSQL (Supplier)** | `postgres:latest` | `5435` | База данных Supplier-сервиса |
| **PostgreSQL (Procurement)** | `postgres:latest` | `5436` | База данных Procurement-сервиса |
| **PostgreSQL (Notification)** | `postgres:latest` | `5437` | База данных Notification-сервиса |
| **Redis** | `redis:latest` | `6379` | Кэширование (коды сброса пароля, сессионные данные) |
| **RabbitMQ** | `rabbitmq:3-management` | `5672` / `15672` | Брокер сообщений между микросервисами (MassTransit) |
| **Seq** | `datalust/seq:2024` | `5051` | Централизованный сбор и просмотр структурированных логов |
| **pgAdmin** | `dpage/pgadmin4:latest` | `5052` | Веб-интерфейс для управления PostgreSQL |

---

## Структура проекта

```
Zephyrus/
├── architecture.png
├── README.md
├── CHANGELOG.md
└── src/
    ├── docker-compose.yml
    ├── .env.development.example
    ├── .env.shared.development.example
    ├── .env.production.example
    ├── .env.shared.production.example
    │
    ├── Zephyrus.SharedKernel/          # Общие утилиты: DbUp, DotNetEnv, конфигурация
    ├── Zephyrus.Logger/                # Фабрика Serilog для всех сервисов
    │
    ├── Zephyrus.Gateway/
    │   └── Zephyrus.Gateway.WebApi/    # YARP Reverse Proxy
    │
    ├── Zephyrus.Identity/
    │   ├── Zephyrus.Identity.Domain/
    │   ├── Zephyrus.Identity.Application/
    │   ├── Zephyrus.Identity.Infrastructure/
    │   ├── Zephyrus.Identity.Presentation/
    │   └── Zephyrus.Identity.WebApi/
    │
    ├── Zephyrus.Catalog/
    │   ├── Zephyrus.Catalog.Domain/
    │   ├── Zephyrus.Catalog.Application/
    │   ├── Zephyrus.Catalog.Infrastructure/
    │   ├── Zephyrus.Catalog.Presentation/
    │   └── Zephyrus.Catalog.WebApi/
    │
    ├── Zephyrus.Supplier/
    │   ├── Zephyrus.Supplier.Domain/
    │   ├── Zephyrus.Supplier.Application/
    │   ├── Zephyrus.Supplier.Infrastructure/
    │   ├── Zephyrus.Supplier.Presentation/
    │   └── Zephyrus.Supplier.WebApi/
    │
    ├── Zephyrus.Procurement/
    │   ├── Zephyrus.Procurement.Domain/
    │   ├── Zephyrus.Procurement.Application/
    │   ├── Zephyrus.Procurement.Infrastructure/
    │   ├── Zephyrus.Procurement.Presentation/
    │   └── Zephyrus.Procurement.WebApi/
    │
    └── Zephyrus.Notification/
        ├── Zephyrus.Notification.Domain/
        ├── Zephyrus.Notification.Application/
        ├── Zephyrus.Notification.Infrastructure/
        ├── Zephyrus.Notification.Presentation/
        └── Zephyrus.Notification.WebApi/
```

### Слои каждого микросервиса

| Слой | Содержимое |
|---|---|
| **Domain** | Сущности (`*Entity`), перечисления (`Enums/`) |
| **Application** | CQRS-обработчики (`Features/`), интерфейсы репозиториев, Pipeline Behaviors (Logging, Validation) |
| **Infrastructure** | Реализации репозиториев (Dapper), SQL-миграции (DbUp), брокер сообщений (MassTransit), внешние сервисы |
| **Presentation** | MVC-контроллеры |
| **WebApi** | `Program.cs`, Middleware (ExceptionMiddleware), настройки аутентификации, Swagger |

---

## База данных — схемы таблиц

### Identity

**users**
| Колонка | Тип | Описание |
|---|---|---|
| id | UUID PK | Идентификатор |
| email | VARCHAR(256) UNIQUE | Email пользователя |
| password | TEXT | Хэш пароля (BCrypt) |
| first_name | VARCHAR(100) | Имя |
| middle_name | VARCHAR(100) | Отчество |
| last_name | VARCHAR(100) | Фамилия |
| role | SMALLINT | Роль пользователя |
| is_active | BOOLEAN | Активен ли аккаунт |
| date_created / date_updated | TIMESTAMPTZ | Аудит |

**refresh_tokens**
| Колонка | Тип | Описание |
|---|---|---|
| id | UUID PK | Идентификатор |
| user_id | UUID FK → users | Владелец токена |
| token | TEXT UNIQUE | Значение токена |
| date_expires | TIMESTAMPTZ | Дата истечения |
| date_created / date_updated | TIMESTAMPTZ | Аудит |

### Catalog

**categories**
| Колонка | Тип | Описание |
|---|---|---|
| id | UUID PK | Идентификатор |
| name | VARCHAR(100) UNIQUE | Название категории |
| parent_id | UUID FK → categories | Родительская категория (иерархия) |
| date_created / date_updated | TIMESTAMPTZ | Аудит |

**products**
| Колонка | Тип | Описание |
|---|---|---|
| id | UUID PK | Идентификатор |
| name | VARCHAR(200) UNIQUE | Название продукта |
| description | TEXT | Описание |
| unit | VARCHAR(20) | Единица измерения |
| category_id | UUID FK → categories | Категория |
| is_active | BOOLEAN | Доступен ли продукт |
| date_created / date_updated | TIMESTAMPTZ | Аудит |

### Supplier

**suppliers**
| Колонка | Тип | Описание |
|---|---|---|
| id | UUID PK | Идентификатор |
| name | VARCHAR(200) UNIQUE | Название поставщика |
| contact_person | VARCHAR(200) | Контактное лицо |
| email | VARCHAR(200) | Email |
| phone | VARCHAR(50) | Телефон |
| is_active | BOOLEAN | Активен ли поставщик |
| date_created / date_updated | TIMESTAMPTZ | Аудит |

**supplier_products**
| Колонка | Тип | Описание |
|---|---|---|
| id | UUID PK | Идентификатор |
| supplier_id | UUID FK → suppliers | Поставщик |
| product_id | UUID | Ссылка на продукт (Catalog) |
| price | NUMERIC(18,2) | Цена |
| currency | VARCHAR(10) | Валюта (ISO 4217) |
| is_available | BOOLEAN | Доступен ли для заказа |
| date_updated | TIMESTAMPTZ | Аудит |

### Procurement

**purchase_requests**
| Колонка | Тип | Описание |
|---|---|---|
| id | UUID PK | Идентификатор |
| product_id | UUID | Ссылка на продукт (Catalog) |
| quantity | NUMERIC(18,3) | Требуемое количество |
| requested_by | UUID | Инициатор заявки (Identity) |
| status | VARCHAR(9) | `Pending` / `Approved` / `Rejected` |
| comment | TEXT | Комментарий при отклонении |
| date_created / date_updated | TIMESTAMPTZ | Аудит |

**orders**
| Колонка | Тип | Описание |
|---|---|---|
| id | UUID PK | Идентификатор |
| supplier_id | UUID | Поставщик (Supplier) |
| total_price | NUMERIC(18,2) | Итоговая сумма |
| status | VARCHAR(9) | `Created` / `Confirmed` / `Delivered` / `Cancelled` |
| created_by | UUID | Создатель заказа (Identity) |
| date_created / date_updated | TIMESTAMPTZ | Аудит |

**order_items**
| Колонка | Тип | Описание |
|---|---|---|
| id | UUID PK | Идентификатор |
| order_id | UUID FK → orders | Заказ |
| purchase_request_id | UUID FK → purchase_requests | Связанная заявка |
| unit_price | NUMERIC(18,2) | Цена за единицу |
| currency | VARCHAR(10) | Валюта (ISO 4217) |
| total_price | NUMERIC(18,2) | Итого по позиции |
| date_created / date_updated | TIMESTAMPTZ | Аудит |

### Notification

**notifications**
| Колонка | Тип | Описание |
|---|---|---|
| id | UUID PK | Идентификатор |
| recipient_id | UUID | Получатель (Identity) |
| title | VARCHAR(200) | Заголовок |
| message | VARCHAR(1000) | Текст уведомления |
| type | VARCHAR(100) | Тип уведомления |
| is_read | BOOLEAN | Прочитано ли |
| date_created | TIMESTAMPTZ | Дата создания |

---

## NuGet-пакеты

### Zephyrus.SharedKernel

| Пакет | Версия | Назначение |
|---|---|---|
| `dbup-postgresql` | 7.0.1 | Выполнение SQL-миграций при старте сервиса |
| `DotNetEnv` | 3.1.1 | Загрузка `.env`-файлов |
| `Microsoft.Extensions.Configuration.Abstractions` | 10.0.5 | Абстракции конфигурации |
| `Microsoft.Extensions.Configuration.Binder` | 10.0.5 | Привязка конфигурации к объектам |
| `Microsoft.Extensions.Configuration.EnvironmentVariables` | 10.0.5 | Провайдер переменных окружения |
| `Microsoft.Extensions.Hosting.Abstractions` | 10.0.5 | Абстракции хоста |

### Zephyrus.Logger

| Пакет | Версия | Назначение |
|---|---|---|
| `Serilog` | 4.3.1 | Ядро структурированного логирования |
| `Serilog.AspNetCore` | 10.0.0 | Интеграция Serilog с ASP.NET Core |
| `Serilog.Extensions.Logging` | 10.0.0 | Мост между Serilog и Microsoft.Extensions.Logging |
| `Serilog.Settings.Configuration` | 10.0.0 | Настройка Serilog через appsettings.json |
| `Serilog.Sinks.Console` | 6.1.1 | Вывод логов в консоль |
| `Serilog.Sinks.File` | 7.0.0 | Запись логов в файл |
| `Serilog.Sinks.Seq` | 9.0.0 | Отправка логов в Seq |

### Application-слои (все сервисы)

| Пакет | Версия | Назначение |
|---|---|---|
| `MediatR` | 14.1.0 | CQRS — диспетчеризация команд и запросов |
| `FluentValidation` | 12.1.1 | Декларативная валидация входных данных |
| `FluentValidation.DependencyInjectionExtensions` | 12.1.1 | Регистрация валидаторов через DI |
| `Microsoft.Extensions.DependencyInjection.Abstractions` | 10.0.5 | Абстракции DI-контейнера |
| `Microsoft.Extensions.Logging.Abstractions` | 10.0.5 | Абстракции логирования |
| `MassTransit.Abstractions` *(Procurement only)* | 8.3.6 | Контракты сообщений MassTransit |

### Infrastructure-слои (Catalog, Supplier, Procurement, Notification)

| Пакет | Версия | Назначение |
|---|---|---|
| `Dapper` | 2.1.72 | Micro-ORM для работы с PostgreSQL |
| `Npgsql` | 10.0.2 | ADO.NET-провайдер для PostgreSQL |
| `MassTransit` | 8.3.6 | Абстракция брокера сообщений |
| `MassTransit.RabbitMQ` | 8.3.6 | Транспорт MassTransit через RabbitMQ |

### Identity.Infrastructure (дополнительно)

| Пакет | Версия | Назначение |
|---|---|---|
| `BCrypt.Net-Next` | 4.1.0 | Хэширование паролей |
| `MailKit` | 4.16.0 | Отправка email через SMTP |
| `StackExchange.Redis` | 2.12.14 | Клиент Redis для кэширования |
| `System.IdentityModel.Tokens.Jwt` | 8.16.0 | Генерация и валидация JWT |

### WebApi-слои (все сервисы)

| Пакет | Версия | Назначение |
|---|---|---|
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 10.0.5 | JWT Bearer аутентификация |
| `Swashbuckle.AspNetCore.SwaggerGen` | 9.0.4 | Генерация OpenAPI-спецификации |
| `Swashbuckle.AspNetCore.SwaggerUI` | 9.0.4 | Swagger UI |
| `Microsoft.AspNetCore.OpenApi` | 9.0.8 | Поддержка OpenAPI в ASP.NET Core |

### Gateway.WebApi

| Пакет | Версия | Назначение |
|---|---|---|
| `Yarp.ReverseProxy` | 2.3.0 | YARP — реверс-прокси и маршрутизация |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 10.0.5 | Проверка JWT на уровне шлюза |

---

## Конфигурация окружения

Конфигурация разделена на два файла для каждого окружения (`development` / `production`):

### `.env.development` — строки подключения к БД и Redis

```env
# Identity Service
IDENTITY_ConnectionStrings__IdentityDatabase=Host=...;Port=...;Database=...;Username=...;Password=...
IDENTITY_ConnectionStrings__Redis=localhost:6379

# Catalog Service
CATALOG_ConnectionStrings__CatalogDatabase=Host=...;Port=...;Database=...;Username=...;Password=...

# Notification Service
NOTIFICATION_ConnectionStrings__NotificationDatabase=Host=...;Port=...;Database=...;Username=...;Password=...

# Procurement Service
PROCUREMENT_ConnectionStrings__ProcurementDatabase=Host=...;Port=...;Database=...;Username=...;Password=...

# Supplier Service
SUPPLIER_ConnectionStrings__SupplierDatabase=Host=...;Port=...;Database=...;Username=...;Password=...
```

### `.env.shared.development` — общие параметры для всех сервисов

```env
RabbitMqSettings__Host=localhost
RabbitMqSettings__Port=5672
RabbitMqSettings__Username=guest
RabbitMqSettings__Password=guest

JwtSettings__SecretKey=your_jwt_secret_key_min_32_characters

Serilog__WriteTo__1__Args__serverUrl=http://localhost:5051

EmailSettings__Host=smtp.example.com
EmailSettings__Username=your@email.com
EmailSettings__Password=your_password
EmailSettings__From=your@email.com
```

> Заполните значения на основе `.env.development.example` и `.env.shared.development.example`.

---

## Запуск проекта

### Требования

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) + Docker Compose

### Локальная разработка

1. Скопируйте файлы окружения:

```bash
cp src/.env.development.example src/.env.development
cp src/.env.shared.development.example src/.env.shared.development
```

2. Заполните значения в созданных `.env`-файлах.

3. Запустите инфраструктуру (БД, Redis, RabbitMQ, Seq, pgAdmin):

```bash
cd src
docker compose up zephyrus.identity.database zephyrus.catalog.database \
  zephyrus.supplier.database zephyrus.procurement.database \
  zephyrus.notification.database redis rabbitmq seq pgadmin -d
```

4. Запустите нужный микросервис из его директории:

```bash
dotnet run --project Zephyrus.Identity/Zephyrus.Identity.WebApi
```

### Production (Docker Compose полностью)

1. Заполните `.env.production` и `.env.shared.production`.

2. Запустите все сервисы:

```bash
cd src
docker compose up --build -d
```

### Адреса инструментов после запуска

| Инструмент | URL |
|---|---|
| Swagger (Identity) | `http://localhost:7002/swagger` |
| Swagger (Catalog) | `http://localhost:7000/swagger` |
| Swagger (Supplier) | `http://localhost:7005/swagger` |
| Swagger (Procurement) | `http://localhost:7004/swagger` |
| Swagger (Notification) | `http://localhost:7003/swagger` |
| Gateway | `http://localhost:7001` |
| UI | `http://localhost:3000` |
| Seq (логи) | `http://localhost:5051` |
| RabbitMQ Management | `http://localhost:15672` |
| pgAdmin | `http://localhost:5052` |

---

## Лицензия

Лицензия: [LICENSE](./LICENSE)
