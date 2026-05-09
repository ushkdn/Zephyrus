# Changelog

Все значимые изменения в проекте Zephyrus фиксируются в этом файле.

Формат основан на [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

---

## [Unreleased]

---

## [0.6.0] — 2026-05-09

### Added
- Возможность создавать заказ (Order) из нескольких заявок на закупку (Purchase Requests) одновременно
- Enum `Currency` для кодов валют вместо строки на уровне кода (в БД тип остался `VARCHAR`)

### Fixed
- Статусы Order и PurchaseRequest заменены с `int`-перечисления на строку (`VARCHAR`) в базе данных и доменной модели — устраняет проблему несоответствия значений при сериализации
- Удалено поле `unit` из Procurement-сервиса — единица измерения уже хранится в Catalog-сервисе, дублирование было избыточным

---

## [0.5.0] — 2026-05-06 / 2026-05-08

### Added
- Примеры файлов окружения: `.env.development.example`, `.env.shared.development.example`, `.env.production.example`, `.env.shared.production.example`
- Сервис отправки email (MailKit) в Identity-сервисе
- Загрузчик `.env`-файлов через DotNetEnv в SharedKernel

### Changed
- Все чувствительные конфигурации (строки подключения, JWT-ключ, SMTP, RabbitMQ) вынесены из `appsettings.json` в `.env`-файлы; базовые настройки сервисов остались в `appsettings.json`

### Fixed
- Файл `.gitignore` перемещён из корня репозитория в директорию `src/`

---

## [0.4.0] — 2026-05-04 / 2026-05-05

### Added
- Обработчики `ForgotPassword` и `ResetPassword` в Identity-сервисе
- Redis-кэширование через StackExchange.Redis: хранение кодов сброса пароля и временных данных сессии
- `CodeGenerator` — утилита генерации случайных кодов для операций сброса пароля
- Централизованное структурированное логирование через Serilog для всех микросервисов (Console + File + Seq sinks)
- Отдельный shared-проект `Zephyrus.Logger` с фабрикой Serilog, переиспользуемой всеми сервисами

### Fixed
- Исправлена отсутствующая очередь RabbitMQ для маршрута Procurement → Catalog; сообщения больше не теряются при старте сервиса

---

## [0.3.0] — 2026-04-29

### Changed
- Изменены порты всех микросервисов для устранения конфликтов:
  - Catalog: `7000`, Gateway: `7001`, Identity: `7002`, Notification: `7003`, Procurement: `7004`, Supplier: `7005`

---

## [0.2.0] — 2026-04-10

### Added
- Полностью реализованы бэкенд-сервисы: **Catalog**, **Supplier**, **Procurement**, **Notification**
  - Каждый сервис содержит полный стек: Domain, Application (CQRS + Validation), Infrastructure (Dapper + DbUp + MassTransit), Presentation, WebApi
  - SQL-миграции через DbUp при старте сервиса
  - MassTransit + RabbitMQ для межсервисного взаимодействия (проверка существования продукта через Catalog)
- Добавлен **Gateway**-сервис (`Zephyrus.Gateway`) на базе YARP Reverse Proxy
  - Единая точка входа для клиентских запросов
  - JWT-аутентификация на уровне шлюза
- Добавлен `ExceptionMiddleware` во все WebApi-проекты для унифицированной обработки ошибок
- Swagger/OpenAPI подключён ко всем сервисам
- `docker-compose.yml` с полным стеком: 5 БД PostgreSQL, Redis, RabbitMQ, Seq, pgAdmin, все микросервисы, UI

---

## [0.1.0] — 2026-03-19 / 2026-03-26

### Added
- Инициализация репозитория и базовой структуры проекта
- Реализован **Identity**-сервис (`Zephyrus.Identity`):
  - Домен: `UserEntity`, `RefreshTokenEntity`, `UserRole`
  - Аутентификация: `SignUp`, `SignIn`, `RefreshToken`
  - JWT-сервис (`JwtService`) и хэширование паролей (`BCrypt.Net-Next`)
  - Репозитории пользователей и refresh-токенов на Dapper + Npgsql
  - SQL-миграции: `users`, `refresh_tokens`
  - Контроллеры `AuthController`, `UserController`
- Создан `Zephyrus.SharedKernel` — общие абстракции и утилиты (DbUp runner, конфигурация)
- Pipeline Behaviors в Application-слое: `LoggingBehavior`, `ValidationBehavior`
- Загружена архитектурная схема `architecture.png`
