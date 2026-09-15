-- ========================================================
-- Script de Criação e População do Banco de Dados Royal Games
-- Compatível com PostgreSQL 14 / 15 / 16
-- ========================================================

-- Criação das tabelas

CREATE TABLE IF NOT EXISTS "Usuario" (
    "UsuarioId" SERIAL PRIMARY KEY,
    "Nome" VARCHAR(60) NOT NULL,
    "Email" VARCHAR(70) NOT NULL UNIQUE,
    "Senha" BYTEA NOT NULL,
    "StatusUsuario" BOOLEAN DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS "Plataforma" (
    "PlataformaId" SERIAL PRIMARY KEY,
    "Nome" VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS "Promocao" (
    "PromocaoId" SERIAL PRIMARY KEY,
    "Nome" VARCHAR(50) NOT NULL,
    "DataExpiração" TIMESTAMP(0) NOT NULL,
    "StatusPromocao" BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS "Genero" (
    "GeneroId" SERIAL PRIMARY KEY,
    "Nome" VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS "ClassificacaoIndicativa" (
    "ClassificacaoIndicativaId" SERIAL PRIMARY KEY,
    "Classificacao" VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS "Jogo" (
    "JogoId" SERIAL PRIMARY KEY,
    "Nome" VARCHAR(150),
    "Descricao" VARCHAR(255),
    "Preco" NUMERIC(10, 2),
    "StatusJogo" BOOLEAN DEFAULT TRUE,
    "Imagem" BYTEA NOT NULL,
    "UsuarioIdFK" INT REFERENCES "Usuario"("UsuarioId") ON DELETE SET NULL,
    "ClassificaçãoIdFK" INT REFERENCES "ClassificacaoIndicativa"("ClassificacaoIndicativaId") ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS "Log_Alteracao_Jogo" (
    "Log_Alteracao_Jogo_Id" SERIAL PRIMARY KEY,
    "DataAlteracao" TIMESTAMP(0) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "NomeAnterior" VARCHAR(100) NOT NULL,
    "PrecoAnterior" NUMERIC(10, 2) NOT NULL,
    "JogoId" INT REFERENCES "Jogo"("JogoId") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "JogoPlataforma" (
    "PlataformaIdFK" INT NOT NULL REFERENCES "Plataforma"("PlataformaId") ON DELETE CASCADE,
    "JogoIdFK" INT NOT NULL REFERENCES "Jogo"("JogoId") ON DELETE CASCADE,
    CONSTRAINT "Jogo_Plataforma_Id_FK" PRIMARY KEY ("PlataformaIdFK", "JogoIdFK")
);

CREATE TABLE IF NOT EXISTS "JogoGenero" (
    "JogoIdFK" INT NOT NULL REFERENCES "Jogo"("JogoId") ON DELETE CASCADE,
    "GeneroIdFK" INT NOT NULL REFERENCES "Genero"("GeneroId") ON DELETE CASCADE,
    CONSTRAINT "Jogo_Genero_Id_FK" PRIMARY KEY ("JogoIdFK", "GeneroIdFK")
);

CREATE TABLE IF NOT EXISTS "JogoPromocao" (
    "JogoIdFK" INT NOT NULL REFERENCES "Jogo"("JogoId") ON DELETE CASCADE,
    "PromocaoIdFK" INT NOT NULL REFERENCES "Promocao"("PromocaoId") ON DELETE CASCADE,
    "PrecoAtual" NUMERIC(10, 2) NOT NULL,
    CONSTRAINT "Jogo_Promocao_Id_FK" PRIMARY KEY ("JogoIdFK", "PromocaoIdFK")
);

-- ========================================================
-- TRIGGERS E FUNÇÕES NO POSTGRESQL
-- ========================================================

-- 1. Exclusão Lógica de Jogo
CREATE OR REPLACE FUNCTION fn_excluir_jogo()
RETURNS TRIGGER AS $$
BEGIN
    UPDATE "Jogo" SET "StatusJogo" = FALSE WHERE "JogoId" = OLD."JogoId";
    RETURN NULL; -- Cancela o DELETE físico
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS "trg_excluirJogo" ON "Jogo";
CREATE TRIGGER "trg_excluirJogo"
BEFORE DELETE ON "Jogo"
FOR EACH ROW
EXECUTE FUNCTION fn_excluir_jogo();

-- 2. Exclusão Lógica de Usuário
CREATE OR REPLACE FUNCTION fn_excluir_usuario()
RETURNS TRIGGER AS $$
BEGIN
    UPDATE "Usuario" SET "StatusUsuario" = FALSE WHERE "UsuarioId" = OLD."UsuarioId";
    RETURN NULL; -- Cancela o DELETE físico
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS "trg_ExclusaoUsuario" ON "Usuario";
CREATE TRIGGER "trg_ExclusaoUsuario"
BEFORE DELETE ON "Usuario"
FOR EACH ROW
EXECUTE FUNCTION fn_excluir_usuario();

-- 3. Log de Alteração de Jogo
CREATE OR REPLACE FUNCTION fn_log_alteracao_jogo()
RETURNS TRIGGER AS $$
BEGIN
    IF (OLD."Nome" IS DISTINCT FROM NEW."Nome" OR OLD."Preco" IS DISTINCT FROM NEW."Preco") THEN
        INSERT INTO "Log_Alteracao_Jogo" ("DataAlteracao", "JogoId", "NomeAnterior", "PrecoAnterior")
        VALUES (CURRENT_TIMESTAMP, OLD."JogoId", OLD."Nome", OLD."Preco");
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS "trg_AlteracaoJogo" ON "Jogo";
CREATE TRIGGER "trg_AlteracaoJogo"
AFTER UPDATE ON "Jogo"
FOR EACH ROW
EXECUTE FUNCTION fn_log_alteracao_jogo();

-- ========================================================
-- DADOS INICIAIS (SEED)
-- ========================================================

-- Usuário Admin (senha: admin@123 em hash SHA-256)
INSERT INTO "Usuario" ("Nome", "Email", "Senha", "StatusUsuario")
VALUES (
    'Carlos Lima (Admin)',
    'carlos@vhburguer.com',
    decode('a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'hex'),
    TRUE
)
ON CONFLICT ("Email") DO NOTHING;

-- Plataformas
INSERT INTO "Plataforma" ("Nome") VALUES
    ('PC'),
    ('PlayStation 4'),
    ('PlayStation 5'),
    ('Xbox One'),
    ('Xbox Series X/S'),
    ('Nintendo Switch')
ON CONFLICT DO NOTHING;

-- Gêneros
INSERT INTO "Genero" ("Nome") VALUES
    ('Terror'),
    ('Sandbox'),
    ('FPS'),
    ('Hack and Slash'),
    ('Soulslike'),
    ('RPG'),
    ('Ação'),
    ('Aventura')
ON CONFLICT DO NOTHING;

-- Classificações Indicativas
INSERT INTO "ClassificacaoIndicativa" ("Classificacao") VALUES
    ('Livre'),
    ('10 anos'),
    ('12 anos'),
    ('14 anos'),
    ('16 anos'),
    ('18 anos')
ON CONFLICT DO NOTHING;
