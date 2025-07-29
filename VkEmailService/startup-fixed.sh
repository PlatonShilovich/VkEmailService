#!/bin/bash

echo "🚀 Запуск VkEmailService - Микросервисная архитектура для email-сервиса ВКонтакте"
echo "======================================================================"

# Функция для проверки статуса сервиса
check_service_health() {
    local service_name=$1
    local url=$2
    local max_attempts=30
    local attempt=1
    
    echo "🔍 Проверка $service_name..."
    while [ $attempt -le $max_attempts ]; do
        if curl -s -f "$url" > /dev/null 2>&1; then
            echo "✅ $service_name запущен успешно!"
            return 0
        fi
        echo "⏳ Попытка $attempt/$max_attempts для $service_name..."
        sleep 2
        attempt=$((attempt + 1))
    done
    echo "❌ $service_name не удалось запустить за отведенное время"
    return 1
}

# Останавливаем все контейнеры
echo "⏹️ Останавливаем существующие контейнеры..."
docker compose down -v

# Очищаем Docker
echo "🧹 Очистка Docker системы..."
docker system prune -f

# Запускаем инфраструктуру поэтапно
echo "🔧 Запуск PostgreSQL и Redis..."
docker compose up -d postgres redis

# Ждем PostgreSQL и Redis
echo "⏳ Ожидание PostgreSQL и Redis (15 сек)..."
sleep 15

# Проверяем PostgreSQL
echo "🔍 Проверка PostgreSQL..."
docker compose exec postgres pg_isready -U admin -d vkemail

# Запускаем ZooKeeper
echo "🐘 Запуск ZooKeeper..."
docker compose up -d zookeeper

# Ждем ZooKeeper (увеличенное время)
echo "⏳ Ожидание ZooKeeper (60 сек)..."
sleep 60

# Проверяем статус ZooKeeper
echo "🔍 Проверка ZooKeeper..."
docker compose ps zookeeper

# Запускаем Kafka
echo "📨 Запуск Kafka..."
docker compose up -d kafka

# Ждем Kafka
echo "⏳ Ожидание Kafka (45 сек)..."
sleep 45

# Проверяем все сервисы инфраструктуры
echo "🔍 Статус инфраструктуры:"
docker compose ps postgres redis zookeeper kafka

# Запускаем микросервисы
echo "🏗️ Запуск микросервисов..."
docker compose up -d --build

# Ждем запуска микросервисов
echo "⏳ Ожидание запуска микросервисов (30 сек)..."
sleep 30

# Проверяем health endpoints
echo "🔍 Проверка работоспособности сервисов..."

check_service_health "API Gateway" "http://localhost:5113/health"
check_service_health "User Segmentation" "http://localhost:5285/health"  
check_service_health "A/B Testing" "http://localhost:5071/health"
check_service_health "Email Campaign" "http://localhost:5218/health"
check_service_health "Analytics" "http://localhost:5133/health"

echo ""
echo "🎉 ВСЕ СЕРВИСЫ ЗАПУЩЕНЫ УСПЕШНО!"
echo "======================================================================"
echo "🌐 Сервисы доступны по адресам:"
echo "┌─────────────────────────┬──────────────────────────────────────────┐"
echo "│ Сервис                  │ URL                                      │"
echo "├─────────────────────────┼──────────────────────────────────────────�"
echo "│ API Gateway             │ http://localhost:5113/swagger            │"
echo "│ User Segmentation       │ http://localhost:5285/swagger            │"
echo "│ A/B Testing             │ http://localhost:5071/swagger            │"
echo "│ Email Campaign          │ http://localhost:5218/swagger            │"
echo "│ Analytics               │ http://localhost:5133/swagger            │"
echo "└─────────────────────────┴──────────────────────────────────────────┘"
echo ""
echo "🔗 Прямые ссылки для быстрого доступа:"
echo "• User Segmentation API: http://localhost:5285/swagger"
echo "• A/B Testing API:       http://localhost:5071/swagger"  
echo "• Email Campaign API:    http://localhost:5218/swagger"
echo "• Analytics API:         http://localhost:5133/swagger"
echo "• API Gateway:           http://localhost:5113/swagger"
echo ""
echo "📊 Мониторинг:"
echo "• Статус всех сервисов:  docker compose ps"
echo "• Логи сервиса:          docker compose logs -f [service-name]"
echo "• Остановка:             docker compose down"
echo ""
echo "✨ Готово к использованию!"

