# PeopleBuy API 3.0

API REST para a plataforma **PeopleBuy** — marketplace brasileiro para gerenciamento de usuários, ofertas de produtos e serviços, categorias, avaliações e busca por geolocalização.

## Stack Tecnológica

| Componente | Tecnologia |
|------------|-----------|
| Framework | ASP.NET Core 6.0 |
| ORM | Entity Framework Core 6 |
| Banco de Dados | SQL Server / LocalDB |
| Autenticação | JWT Bearer |
| Documentação | Swagger / OpenAPI 3 |
| Hashing de Senha | PBKDF2 (SHA-256) |

---

## Pré-requisitos

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- SQL Server ou SQL Server LocalDB
- `dotnet-ef` tools: `dotnet tool install --global dotnet-ef`

---

## Instalação e Configuração

### 1. Clonar o repositório

```bash
git clone https://github.com/dclaumanndeveloper/peoplebuyapi3.0.git
cd PeopleBuyAPI3.0
```

### 2. Restaurar dependências

```bash
dotnet restore
```

### 3. Configurar a string de conexão

Edite `PeopleBuy/appsettings.json` e ajuste `ConnectionStrings:DefaultConnection` para o seu servidor SQL Server.

### 4. Configurar o segredo JWT

> **Importante:** Em produção, use [User Secrets](https://docs.microsoft.com/aspnet/core/security/app-secrets) ou variáveis de ambiente para a chave JWT. Nunca commite a chave real.

```bash
cd PeopleBuy
dotnet user-secrets set "Jwt:Key" "SuaChaveSecretaComMinimode32Caracteres!"
```

### 5. Aplicar as migrations do banco de dados

```bash
cd PeopleBuy
dotnet ef database update
```

### 6. Executar a aplicação

```bash
dotnet run
```

A API estará disponível em:
- **HTTP:** `http://localhost:5171`
- **HTTPS:** `https://localhost:7171`
- **Swagger UI:** `https://localhost:7171/swagger`

---

## Autenticação

A API usa **JWT Bearer Token**. Endpoints de escrita exigem autenticação; endpoints de leitura de catálogo são públicos.

### Fluxo de autenticação

#### 1. Cadastrar usuário

```http
POST /api/auth/register
Content-Type: application/json

{
  "usuario": "meuemail@exemplo.com",
  "senha": "minhasenha123",
  "tipoLogin": "Fisica"
}
```

`tipoLogin` aceita `"Fisica"` (pessoa física) ou `"Juridica"` (empresa).

#### 2. Fazer login e obter token

```http
POST /api/auth/login
Content-Type: application/json

{
  "usuario": "meuemail@exemplo.com",
  "senha": "minhasenha123"
}
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiry": "2024-01-15T14:30:00Z",
  "tipoLogin": "Fisica"
}
```

#### 3. Usar o token nas requisições protegidas

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## Endpoints da API

### Autenticação

| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| POST | `/api/auth/login` | Não | Autentica e retorna JWT |
| POST | `/api/auth/register` | Não | Cadastra novo usuário |

### Ofertas

| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| GET | `/api/ofertas` | Não | Lista todas as ofertas |
| GET | `/api/ofertas/{id}` | Não | Retorna oferta por ID |
| GET | `/api/ofertas/proximas` | Não | Busca por geolocalização |
| POST | `/api/ofertas` | Sim | Cria nova oferta |
| PUT | `/api/ofertas/{id}` | Sim | Atualiza oferta |
| DELETE | `/api/ofertas/{id}` | Sim | Remove oferta |

### Ofertas Diárias

| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| GET | `/api/ofertasdiarias` | Não | Lista ofertas diárias |
| GET | `/api/ofertasdiarias/{id}` | Não | Retorna oferta diária por ID |
| GET | `/api/ofertasdiarias/proximas` | Não | Busca por geolocalização |
| POST | `/api/ofertasdiarias` | Sim | Cria oferta diária |
| PUT | `/api/ofertasdiarias/{id}` | Sim | Atualiza oferta diária |
| DELETE | `/api/ofertasdiarias/{id}` | Sim | Remove oferta diária |

### Categorias e Subcategorias

| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| GET | `/api/categorias` | Não | Lista categorias |
| GET | `/api/categorias/{id}` | Não | Retorna categoria |
| POST | `/api/categorias` | Sim | Cria categoria |
| PUT | `/api/categorias/{id}` | Sim | Atualiza categoria |
| DELETE | `/api/categorias/{id}` | Sim | Remove categoria |
| GET | `/api/subcategorias` | Não | Lista subcategorias |
| GET | `/api/subcategorias/{id}` | Não | Retorna subcategoria |
| POST | `/api/subcategorias` | Sim | Cria subcategoria |
| PUT | `/api/subcategorias/{id}` | Sim | Atualiza subcategoria |
| DELETE | `/api/subcategorias/{id}` | Sim | Remove subcategoria |

### Usuários

| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| GET | `/api/fisicas` | Sim | Lista pessoas físicas |
| GET | `/api/fisicas/{id}` | Sim | Retorna pessoa física |
| POST | `/api/fisicas` | Sim | Cadastra pessoa física |
| PUT | `/api/fisicas/{id}` | Sim | Atualiza pessoa física |
| DELETE | `/api/fisicas/{id}` | Sim | Remove pessoa física |
| GET | `/api/juridicas` | Sim | Lista empresas |
| GET | `/api/juridicas/{id}` | Sim | Retorna empresa |
| POST | `/api/juridicas` | Sim | Cadastra empresa |
| PUT | `/api/juridicas/{id}` | Sim | Atualiza empresa |
| DELETE | `/api/juridicas/{id}` | Sim | Remove empresa |

### Interações

| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| GET | `/api/avaliacoes` | Não | Lista avaliações |
| GET | `/api/avaliacoes/{id}` | Não | Retorna avaliação |
| POST | `/api/avaliacoes` | Sim | Registra avaliação |
| PUT | `/api/avaliacoes/{id}` | Sim | Atualiza avaliação |
| DELETE | `/api/avaliacoes/{id}` | Sim | Remove avaliação |
| GET | `/api/favoritos` | Sim | Lista favoritos |
| POST | `/api/favoritos` | Sim | Adiciona favorito |
| DELETE | `/api/favoritos/{id}` | Sim | Remove favorito |

### Imagens

| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| GET | `/api/imagens` | Não | Lista imagens |
| GET | `/api/imagens/{id}` | Não | Retorna metadados |
| POST | `/api/imagens/upload` | Sim | Upload de arquivo (multipart/form-data) |
| DELETE | `/api/imagens/{id}` | Sim | Remove imagem |

---

## Busca por Geolocalização

Use os endpoints `/proximas` para encontrar ofertas dentro de um raio de distância:

```
GET /api/ofertasdiarias/proximas?latitude=-23.5505&longitude=-46.6333&raioKm=5
```

**Parâmetros:**

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-----------|
| `latitude` | double | Sim | Latitude em graus decimais |
| `longitude` | double | Sim | Longitude em graus decimais |
| `raioKm` | double | Não | Raio em km (padrão: 10, máximo: 500) |

O cálculo usa a **fórmula de Haversine** sobre o raio médio da Terra (6.371 km), retornando a distância em linha reta.

---

## Upload de Imagens

```http
POST /api/imagens/upload
Authorization: Bearer {token}
Content-Type: multipart/form-data

arquivo: [arquivo.jpg]
```

**Restrições:**
- Formatos aceitos: `jpg`, `jpeg`, `png`, `webp`
- Tamanho máximo: 5 MB
- Imagens acessíveis via URL pública

**Resposta:**
```json
{
  "id": 1,
  "nome": "produto-foto",
  "extensao": ".jpg",
  "caminhoArquivo": "/uploads/3fa85f64-5717-4562-b3fc-2c963f66afa6.jpg"
}
```

---

## Esquema de Dados

```
Login (usuário + senha hash PBKDF2)
  ├── Fisica (CPF, nome, email) → Login (1:1)
  └── Juridica (CNPJ, razão social) → Login (1:1), Localizacao (1:1)

Categoria
  └── SubCategoria → Categoria (N:1)

Juridica
  └── Oferta → Juridica (N:1), Categoria (N:1), Imagem (N:1)

OfertaDiaria (lat/lon embutidos) → Categoria (N:1)

Login
  ├── Avaliacao → Login (N:1), Oferta (N:1) | Pontos: 0–5
  └── Favorito → Login (N:1), Oferta (N:1)
```

### Validações

| Campo | Validação |
|-------|-----------|
| CPF | Algoritmo oficial com dígitos verificadores |
| CNPJ | Algoritmo oficial com dígitos verificadores |
| Avaliacao.Pontos | Entre 0 e 5 |
| Imagem (upload) | Extensão permitida + tamanho máximo 5MB |

---

## Executar os Testes

```bash
dotnet test
```

Cobertura:
- Fórmula de Haversine (GeoLocalizacao)
- Validação de CPF e CNPJ
- Hashing e verificação de senhas (PBKDF2)

---

## Configurações de Ambiente

| Chave | Descrição |
|-------|-----------|
| `ConnectionStrings:DefaultConnection` | String de conexão SQL Server |
| `Jwt:Key` | Chave secreta JWT (mínimo 32 caracteres) |
| `Jwt:Issuer` | Issuer do token (padrão: `PeopleBuyAPI`) |
| `Jwt:Audience` | Audience (padrão: `PeopleBuyClients`) |
| `Jwt:ExpiryMinutes` | Validade do token em minutos (padrão: 60) |
| `Cors:AllowedOrigins` | Array de origens permitidas para CORS |

---

## Executar Migrations

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration --project PeopleBuy

# Aplicar ao banco
dotnet ef database update --project PeopleBuy
```

---

## Contribuição

1. Fork o repositório
2. Crie uma branch: `git checkout -b feature/minha-feature`
3. Commit: `git commit -m 'feat: adiciona minha feature'`
4. Push: `git push origin feature/minha-feature`
5. Abra um Pull Request

---

## Licença

Este projeto está sob a licença MIT.
