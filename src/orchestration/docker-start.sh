#!/bin/bash

# Cores para output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${BLUE}🚀 Iniciando Aquasmart com Docker...${NC}"

# Verificar se docker está instalado
if ! command -v docker &> /dev/null; then
    echo -e "${RED}❌ Docker não está instalado${NC}"
    echo -e "${YELLOW}📥 Instale o Docker: https://docs.docker.com/get-docker/${NC}"
    exit 1
fi

# Verificar se docker-compose está instalado
if ! command -v docker-compose &> /dev/null; then
    echo -e "${RED}❌ Docker Compose não está instalado${NC}"
    echo -e "${YELLOW}📥 Instale o Docker Compose: https://docs.docker.com/compose/install/${NC}"
    exit 1
fi

# Verificar se o arquivo docker-compose.yml existe
if [ ! -f "docker-compose.yml" ]; then
    echo -e "${RED}❌ Arquivo docker-compose.yml não encontrado${NC}"
    exit 1
fi

# Parar containers antigos se existirem
echo -e "${BLUE}🛑 Parando containers antigos...${NC}"
docker-compose down 2>/dev/null

# Construir imagens
echo -e "${BLUE}📦 Construindo imagens...${NC}"
docker-compose build

# Subir containers
echo -e "${BLUE}🐳 Subindo containers...${NC}"
docker-compose up -d

# Aguardar banco de dados ficar pronto
echo -e "${BLUE}⏳ Aguardando PostgreSQL ficar pronto...${NC}"
sleep 5

# Aguardar backend ficar pronto
echo -e "${BLUE}⏳ Aguardando API ficar pronta...${NC}"
sleep 10

# Verificar status
echo -e "${BLUE}📊 Status dos containers:${NC}"
docker-compose ps

echo -e "\n${GREEN}✅ Aquasmart rodando com sucesso!${NC}"
echo -e "${GREEN}📍 Web: http://localhost:5048${NC}"
echo -e "${GREEN}📍 API: http://localhost:5047${NC}"
echo -e "${GREEN}📍 Scalar (API Docs): http://localhost:5047/scalar${NC}"
echo -e "${GREEN}📍 PostgreSQL: localhost:5432${NC}"

echo -e "\n${BLUE}🔧 Credenciais do Banco:${NC}"
echo -e "  Usuário: postgres"
echo -e "  Senha: ADYLLSXNPOSTGRES"
echo -e "  Banco: db_aquasmart"

echo -e "\n${BLUE}📋 Comandos úteis:${NC}"
echo -e "  ${YELLOW}docker-compose logs -f${NC}      # Ver logs em tempo real"
echo -e "  ${YELLOW}docker-compose logs -f server${NC}  # Ver logs do Server"
echo -e "  ${YELLOW}docker-compose logs -f web${NC}     # Ver logs do Web"
echo -e "  ${YELLOW}docker-compose down${NC}         # Parar todos os containers"
echo -e "  ${YELLOW}docker-compose restart${NC}      # Reiniciar todos os containers"
echo -e "  ${YELLOW}docker-compose exec postgres psql -U postgres${NC}  # Acessar PostgreSQL"

echo -e "\n${BLUE}🐟 Aquasmart - Tecnologia para aquicultura sustentável${NC}"