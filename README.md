# HotelBookingSystem

**HotelBookingSystem** — это backend-сервис для управления бронированиями отелей, реализованный на платформе ASP.NET Core с использованием современных подходов в архитектуре и безопасности. Проект разрабатывался как учебно-практический, но с ориентацией на реальные бизнес-задачи.

---

## Основные возможности

- Регистрация и аутентификация пользователей по ролям (JWT: Access/Refresh токены)
- Подтверждение email и бронирований через токены
- Управление бронированиями (создание, изменение, отмена)
- Роль менеджер имеет возможность управлять отелями, комнатами, бронированиями (создание, изменение, публикация/сокрытие, подтверждение, отмена)
- Email-уведомления с HTML-шаблонами
- Кэширование популярных GET-запросов
- Swagger-документация
- Контейнеризация через Docker
- Модульная архитектура, CQRS (MediatR), многослойный подход

---

## Быстрый старт

### Требования:

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- PostgreSQL (локально или в Docker)
- SMTP-сервер для отправки писем

### 1. Клонирование репозитория

```bash
git clone https://github.com/skymxxn/HotelBookingSystem.git
cd HotelBookingSystem
```

### 2. Настройка переменных окружения

Скопируйте и отредактируйте `.env`:

```bash
cp .env.example .env
```

Укажите:
- SMTP-данные
- Секреты для JWT
- Подключение к PostgreSQL
- Admin credentials (по желанию)

### 3. Запуск проекта

```bash
docker-compose up --build
```

### 4. Доступ

- API: [http://localhost:5000](http://localhost:5000)  
- Swagger: [http://localhost:5000/swagger](http://localhost:5000/swagger)

---

## Архитектура и структура

Проект построен по принципу **модульного монолита** с чётким разделением ответственности:

```
src/
├── HotelBooking.API             // Точка входа, маршрутизация, middleware
├── HotelBooking.Application     // Бизнес-логика, CQRS, DTO, валидации
├── HotelBooking.Infrastructure  // Email-сервис, кэш, JWT
├── HotelBooking.Persistence     // EF Core, миграции, контекст
└── HotelBooking.Domain          // Доменные сущности, правила, контракты
```

---

## Аутентификация и безопасность

- Используется JWT (Access и Refresh токены)
- Подтверждение email через токен-ссылку
- Отдельный токен для подтверждения бронирования
- Сброс пароля через токен-ссылку
- Конфигурация токенов через `appsettings.json` и `IOptions`

---

## Email-уведомления

Интеграция с SMTP. Поддерживаются следующие события:

- Подтверждение email
- Создание бронирования
- Сброс пароля

Каждое письмо оформлено в виде HTML-шаблона.

---

## Кэширование

- Используется `IMemoryCache`  
- Реализован `ICacheService` для централизованной логики кэширования
- Кэшируются часто запрашиваемые данные (например, список отелей и номеров)

---

## Тестовый пользователь

При первом запуске создаётся администратор:

```
Email: admin@hotel.com
Пароль: Admin123!
```

Изменения — в `.env`.

---

## Работа с миграциями

```bash
# Добавить миграцию
dotnet ef migrations add MigrationName --project src/HotelBooking.Persistence --startup-project src/HotelBooking.API

# Применить миграции
dotnet ef database update --project src/HotelBooking.Persistence

# Удалить последнюю миграцию
dotnet ef migrations remove --project src/HotelBooking.Persistence
```

---

## Возможности для развития

- Подключение Redis для распределённого кэша
- OAuth2 / Social login (Google, GitHub)
- Очереди (RabbitMQ / Kafka)
- Интеграционные и нагрузочные тесты
- Улучшенная RBAC-авторизация

---

---

## Автор

Разработчик: [Руслан (skymxxn)](https://github.com/skymxxn)  
Специализация: Backend (.NET)  
Проект создан как showcase с прицелом на продакшн-качество и понимание архитектурных паттернов.
