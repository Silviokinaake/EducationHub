CREATE TABLE "Alunos" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Alunos" PRIMARY KEY,
    "UsuarioId" TEXT NOT NULL,
    "Nome" varchar(150) NOT NULL,
    "Email" varchar(150) NOT NULL,
    "DataNascimento" datetime2 NOT NULL
);


CREATE TABLE "Certificados" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Certificados" PRIMARY KEY,
    "AlunoId" TEXT NOT NULL,
    "CursoId" TEXT NOT NULL,
    "TituloCurso" varchar(200) NOT NULL,
    "DataEmissao" datetime2 NOT NULL,
    "Codigo" varchar(100) NOT NULL,
    CONSTRAINT "FK_Certificados_Alunos_AlunoId" FOREIGN KEY ("AlunoId") REFERENCES "Alunos" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Matriculas" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Matriculas" PRIMARY KEY,
    "CursoId" TEXT NOT NULL,
    "AlunoId" TEXT NOT NULL,
    "Valor" decimal(18,2) NOT NULL,
    "DataMatricula" datetime2 NOT NULL,
    "DataAtivacao" TEXT NULL,
    "DataConclusao" TEXT NULL,
    "Status" INTEGER NOT NULL,
    CONSTRAINT "FK_Matriculas_Alunos_AlunoId" FOREIGN KEY ("AlunoId") REFERENCES "Alunos" ("Id") ON DELETE CASCADE
);


CREATE TABLE "MatriculaHistoricos" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_MatriculaHistoricos" PRIMARY KEY,
    "CursoId" TEXT NOT NULL,
    "ProgressoPercentual" REAL NOT NULL,
    "DataUltimaAtualizacao" datetime2 NOT NULL,
    "MatriculaId" TEXT NOT NULL,
    CONSTRAINT "FK_MatriculaHistoricos_Matriculas_MatriculaId" FOREIGN KEY ("MatriculaId") REFERENCES "Matriculas" ("Id") ON DELETE CASCADE
);


CREATE UNIQUE INDEX "IX_Alunos_Email" ON "Alunos" ("Email");


CREATE INDEX "IX_Certificados_AlunoId" ON "Certificados" ("AlunoId");


CREATE UNIQUE INDEX "IX_Certificados_Codigo" ON "Certificados" ("Codigo");


CREATE INDEX "IX_MatriculaHistoricos_MatriculaId" ON "MatriculaHistoricos" ("MatriculaId");


CREATE INDEX "IX_Matriculas_AlunoId" ON "Matriculas" ("AlunoId");


CREATE INDEX "IX_Matriculas_CursoId" ON "Matriculas" ("CursoId");


