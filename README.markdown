# VkEmailService - Микросервисная архитектура для email-сервиса ВКонтакте

Современная микросервисная система для сегментирования пользователей, A/B тестирования и управления email-кампаниями.

## Архитектура

Система состоит из 5 микросервисов:

- **API Gateway** — Единая точка входа, маршрутизация запросов
- **User Segmentation Service** — Сегментирование пользователей по критериям
- **A/B Testing Service** — Управление экспериментами и распределение пользователей
- **Email Campaign Service** — Создание и управление email-кампаниями
- **Analytics Service** — Сбор метрик и аналитика

## Технологический стек

- **.NET 8** — Основная платформа
- **PostgreSQL** — Основная база данных
- **Redis** — Кэширование
- **Apache Kafka** — Асинхронная обработка событий
- **Docker** — Контейнеризация
- **Entity Framework Core** — ORM
- **MediatR** — CQRS паттерн
- **AutoMapper** — Маппинг объектов
- **Serilog** — Структурированное логирование

## Быстрый старт

### Предварительные требования

Проверьте установленные версии:

```
dotnet --version  # >= 8.0
docker --version
docker-compose --version
```

### 1. Клонирование репозитория

```
git clone https://github.com/your-username/VkEmailService.git
cd VkEmailService
```

### 2. Запуск всей системы

Запуск одной командой:

```bash
#!/bin/bash

echo "Запуск VkEmailService - Микросервисная архитектура для email-сервиса ВКонтакте"
echo "======================================================================"

# Функция для проверки статуса сервиса
check_service_health() {
    local service_name=$1
    local url=$2
    local max_attempts=30
    local attempt=1

    echo "Проверка $service_name..."
    while [ $attempt -le $max_attempts ]; do
        if curl -s -f "$url" > /dev/null 2>&1; then
            echo "$service_name запущен успешно!"
            return 0
        fi
        echo "Попытка $attempt/$max_attempts для $service_name..."
        sleep 2
        attempt=$((attempt + 1))
    done
    echo "$service_name не удалось запустить за отведенное время"
    return 1
}

# Останавливаем все контейнеры
echo "Останавливаем существующие контейнеры..."
docker compose down -v

# Очищаем Docker
echo "Очистка Docker системы..."
docker system prune -f

# Запускаем инфраструктуру поэтапно
echo "Запуск PostgreSQL и Redis..."
docker compose up -d postgres redis

# Ждем PostgreSQL и Redis
echo "Ожидание PostgreSQL и Redis (15 сек)..."
sleep 15

# Проверяем PostgreSQL
echo "Проверка PostgreSQL..."
docker compose exec postgres pg_isready -U admin -d vkemail

# Запускаем ZooKeeper
echo "Запуск ZooKeeper..."
docker compose up -d zookeeper

# Ждем ZooKeeper
echo "Ожидание ZooKeeper (60 сек)..."
sleep 60

# Проверяем статус ZooKeeper
echo "Проверка ZooKeeper..."
docker compose ps zookeeper

# Запускаем Kafka
echo "Запуск Kafka..."
docker compose up -d kafka

# Ждем Kafka
echo "Ожидание Kafka (45 сек)..."
sleep 45

# Проверяем все сервисы инфраструктуры
echo "Статус инфраструктуры:"
docker compose ps postgres redis zookeeper kafka

# Запускаем микросервисы
echo "Запуск микросервисов..."
docker compose up -d --build

# Ждем запуска микросервисов
echo "Ожидание запуска микросервисов (30 сек)..."
sleep 30

# Проверяем health endpoints
echo "Проверка работоспособности сервисов..."

check_service_health "API Gateway" "http://localhost:5113/health"
check_service_health "User Segmentation" "http://localhost:5285/health"
check_service_health "A/B Testing" "http://localhost:5071/health"
check_service_health "Email Campaign" "http://localhost:5218/health"
check_service_health "Analytics" "http://localhost:5133/health"

echo ""
echo "ВСЕ СЕРВИСЫ ЗАПУЩЕНЫ УСПЕШНО!"
echo "======================================================================"
echo "Сервисы доступны по адресам:"
echo "┌─────────────────────────┬──────────────────────────────────────────┐"
echo "│ Сервис                  │ URL                                      │"
echo "├─────────────────────────┼──────────────────────────────────────────┤"
echo "│ API Gateway             │ http://localhost:5113/swagger            │"
echo "│ User Segmentation       │ http://localhost:5285/swagger            │"
echo "│ A/B Testing             │ http://localhost:5071/swagger            │"
echo "│ Email Campaign          │ http://localhost:5218/swagger            │"
echo "│ Analytics               │ http://localhost:5133/swagger            │"
echo "└─────────────────────────┴──────────────────────────────────────────┘"
echo ""
echo "Прямые ссылки для быстрого доступа:"
echo "• User Segmentation API: http://localhost:5285/swagger"
echo "• A/B Testing API:       http://localhost:5071/swagger"
echo "• Email Campaign API:    http://localhost:5218/swagger"
echo "• Analytics API:         http://localhost:5133/swagger"
echo "• API Gateway:           http://localhost:5113/swagger"
echo ""
echo "Мониторинг:"
echo "• Статус всех сервисов:  docker compose ps"
echo "• Логи сервиса:          docker compose logs -f [service-name]"
echo "• Остановка:             docker compose down"
echo ""
echo "Готово к использованию!"
```

### 3. Проверка работоспособности

Откройте в браузере:
- http://localhost:5113/swagger — API Gateway
- http://localhost:5285/swagger — User Segmentation
- http://localhost:5071/swagger — A/B Testing
- http://localhost:5218/swagger — Email Campaign
- http://localhost:5133/swagger — Analytics

## API Документация и примеры

### API Gateway (порт 5113)

Все запросы проходят через Gateway по паттерну: `/api/{service}/{endpoint}`

#### Примеры запросов через Gateway:

```
# Получить все сегменты через Gateway
curl http://localhost:5113/api/usersegmentation/segments

# Создать эксперимент через Gateway
curl -X POST http://localhost:5113/api/abtesting/experiments \
-H "Content-Type: application/json" \
-d '{
"name": "Email Subject Test",
"description": "Тестирование заголовков писем",
"trafficSplit": "50/50"
}'
```

### User Segmentation Service (порт 5285)

#### Создание сегмента пользователей

```
curl -X POST http://localhost:5285/api/segments \
-H "Content-Type: application/json" \
-d '{
"name": "Молодые пользователи Москвы",
"description": "Пользователи 18-25 лет из Москвы",
"criteria": "{\"ageRange\":{\"min\":18,\"max\":25},\"locations\":[\"Москва\"]}"
}'
```

**Ожидаемый ответ:**
```
{
"id": "123e4567-e89b-12d3-a456-426614174000",
"name": "Молодые пользователи Москвы",
"description": "Пользователи 18-25 лет из Москвы",
"criteria": "{\"ageRange\":{\"min\":18,\"max\":25},\"locations\":[\"Москва\"]}",
"createdAt": "2025-01-15T10:30:00Z",
"updatedAt": "2025-01-15T10:30:00Z"
}
```

#### Получение пользователей сегмента

```
curl -X POST http://localhost:5285/api/segments/123e4567-e89b-12d3-a456-426614174000/users \
-H "Content-Type: application/json" \
-d '{
"ageRange": {"min": 18, "max": 25},
"locations": ["Москва"]
}'
```

### A/B Testing Service (порт 5071)

#### Создание A/B теста

```
curl -X POST http://localhost:5071/api/experiments \
-H "Content-Type: application/json" \
-d '{
"name": "Тест заголовков писем",
"description": "Сравнение двух вариантов заголовков",
"trafficSplit": "50/50",
"variants": [
{
"name": "Вариант A",
"weight": 0.5,
"content": "Специальное предложение для вас!"
},
{
"name": "Вариант B",
"weight": 0.5,
"content": "Не упустите скидку 50%!"
}
]
}'
```

#### Назначение пользователя в эксперимент

```
curl -X POST http://localhost:5071/api/experiments/123e4567-e89b-12d3-a456-426614174000/assign-user \
-H "Content-Type: application/json" \
-d '"user-id-here"'
```

### Email Campaign Service (порт 5218)

#### Создание email кампании

```
curl -X POST http://localhost:5218/api/campaigns \
-H "Content-Type: application/json" \
-d '{
"name": "Весенняя распродажа",
"subject": "{{\#if experiment}}{{experiment.subject}}{{else}}Весенние скидки до 70%!{{/if}}",
"content": "<h1>Привет, {{user.name}}!</h1><p>Специально для вас скидка {{discount}}%</p>",
"segmentId": "123e4567-e89b-12d3-a456-426614174000",
"experimentId": "456e7890-e89b-12d3-a456-426614174000",
"status": "Draft"
}'
```

### Analytics Service (порт 5133)

#### Создание события

```
curl -X POST http://localhost:5133/api/events \
-H "Content-Type: application/json" \
-d '{
"userId": "user-123",
"campaignId": "campaign-456",
"eventType": "email_open",
"metadata": "{\"device\":\"mobile\",\"location\":\"Moscow\"}"
}'
```

#### Получение метрик кампании

```
curl http://localhost:5133/api/events/metrics/campaign-456
```

**Ожидаемый ответ:**
```
{
"ctr": 0.15,
"cr": 0.08,
"bounceRate": 0.02
}
```

## Разработка

### Локальный запуск отдельного сервиса

```
# User Segmentation Service
cd src/Services/UserSegmentation
dotnet run

# A/B Testing Service
cd src/Services/ABTesting
dotnet run
```

### Применение миграций

```
# UserSegmentation
cd src/Services/UserSegmentation
dotnet ef migrations add Initial --context UserSegmentationDbContext
dotnet ef database update --connection "Host=localhost;Port=5432;Database=vkemail;Username=admin;Password=password"

# ABTesting
cd src/Services/ABTesting
dotnet ef migrations add Initial --context ABTestingDbContext
dotnet ef database update --connection "Host=localhost;Port=5432;Database=vkemail;Username=admin;Password=password"

# EmailCampaign
cd src/Services/EmailCampaign
dotnet ef migrations add Initial --context EmailCampaignDbContext
dotnet ef database update --connection "Host=localhost;Port=5432;Database=vkemail;Username=admin;Password=password"

# Analytics
cd src/Services/Analytics
dotnet ef migrations add Initial --context AnalyticsDbContext
dotnet ef database update --connection "Host=localhost;Port=5432;Database=vkemail;Username=admin;Password=password"
```

## Docker команды

```
# Запуск всех сервисов
docker compose up -d

# Запуск только инфраструктуры
docker compose up -d postgres redis zookeeper kafka

# Просмотр логов
docker compose logs -f usersegmentation
docker compose logs -f abtesting

# Остановка
docker compose down

# Полная очистка
docker compose down -v
docker system prune -f
```

## Мониторинг и отладка

### Health Checks

```
curl http://localhost:5113/health  # API Gateway
curl http://localhost:5285/health  # User Segmentation
curl http://localhost:5071/health  # A/B Testing
curl http://localhost:5218/health  # Email Campaign
curl http://localhost:5133/health  # Analytics
```

### Подключение к базе данных

```
# PostgreSQL
docker exec -it $(docker compose ps -q postgres) psql -U admin -d vkemail

# Список схем
\dn

# Список таблиц в схеме
\dt usersegmentation.*
\dt abtesting.*
\dt emailcampaign.*
\dt analytics.*
```

### Проверка Redis

```
# Подключение к Redis
docker exec -it $(docker compose ps -q redis) redis-cli

# Просмотр ключей
KEYS *

# Получение значения
GET "segment:123:criteria:..."
```

## Тестирование

```
# Запуск всех тестов
dotnet test

# Запуск тестов конкретного сервиса
cd tests/UserSegmentation.Tests
dotnet test

# Интеграционные тесты с Testcontainers
cd tests/Integration.Tests
dotnet test
```

## Пример полного сценария использования

### 1. Создание пользовательского сегмента

```
# Создаем сегмент "Активные пользователи"
SEGMENT_ID=$(curl -s -X POST http://localhost:5285/api/segments \
-H "Content-Type: application/json" \
-d '{
"name": "Активные пользователи",
"description": "Пользователи с активностью за последние 7 дней",
"criteria": "{\"activityPeriod\":{\"from\":\"2025-01-08T00:00:00Z\",\"to\":\"2025-01-15T23:59:59Z\"}}"
}' | jq -r '.id')

echo "Создан сегмент: $SEGMENT_ID"
```

### 2. Создание A/B теста

```
# Создаем эксперимент для тестирования заголовков
EXPERIMENT_ID=$(curl -s -X POST http://localhost:5071/api/experiments \
-H "Content-Type: application/json" \
-d '{
"name": "Тест email заголовков",
"description": "A/B тест для оптимизации открываемости писем",
"trafficSplit": "50/50",
"variants": [
{"name": "Стандартный", "weight": 0.5, "content": "Новые товары в каталоге"},
{"name": "Эмоциональный", "weight": 0.5, "content": "Горячие новинки уже здесь!"}
]
}' | jq -r '.id')

echo "Создан эксперимент: $EXPERIMENT_ID"
```

### 3. Создание email кампании

```
# Создаем кампанию, связанную с сегментом и экспериментом
CAMPAIGN_ID=$(curl -s -X POST http://localhost:5218/api/campaigns \
-H "Content-Type: application/json" \
-d "{
\"name\": \"Рассылка новых товаров\",
\"subject\": \"{{variant.content}}\",
\"content\": \"<h1>Привет!</h1><p>Посмотрите наши новинки</p>\",
\"segmentId\": \"$SEGMENT_ID\",
\"experimentId\": \"$EXPERIMENT_ID\",
\"status\": \"Ready\"
}" | jq -r '.id')

echo "Создана кампания: $CAMPAIGN_ID"
```

### 4. Запуск эксперимента

```
# Запускаем A/B тест
curl -X PUT "http://localhost:5071/api/experiments/$EXPERIMENT_ID/start"
echo "Эксперимент запущен"
```

### 5. Симуляция событий

```
# Создаем события открытия письма
curl -X POST http://localhost:5133/api/events \
-H "Content-Type: application/json" \
-d "{
\"userId\": \"user-001\",
\"campaignId\": \"$CAMPAIGN_ID\",
\"eventType\": \"email_open\",
\"metadata\": \"{\\\"variant\\\":\\\"A\\\"}\"
}"

curl -X POST http://localhost:5133/api/events \
-H "Content-Type: application/json" \
-d "{
\"userId\": \"user-002\",
\"campaignId\": \"$CAMPAIGN_ID\",
\"eventType\": \"email_click\",
\"metadata\": \"{\\\"variant\\\":\\\"B\\\"}\"
}"
```

### 6. Получение результатов

```
curl http://localhost:5133/api/events/metrics/$CAMPAIGN_ID
```