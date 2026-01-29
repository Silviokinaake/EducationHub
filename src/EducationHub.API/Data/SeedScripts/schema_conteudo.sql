CREATE TABLE "Cursos" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Cursos" PRIMARY KEY,
    "Titulo" varchar(250) NOT NULL,
    "Descricao" varchar(500) NOT NULL,
    "CargaHoraria" TEXT NOT NULL,
    "Instrutor" varchar(100) NOT NULL,
    "Situacao" INTEGER NOT NULL,
    "Nivel" varchar(50) NOT NULL,
    "Valor" TEXT NOT NULL,
    "ConteudoProgramatico_Objetivo" varchar(250) NOT NULL,
    "ConteudoProgramatico_Conteudo" varchar(1000) NOT NULL,
    "ConteudoProgramatico_Metodologia" varchar(500) NOT NULL,
    "ConteudoProgramatico_Bibliografia" varchar(500) NOT NULL
);


CREATE TABLE "Aulas" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Aulas" PRIMARY KEY,
    "CursoId" TEXT NOT NULL,
    "Titulo" varchar(150) NOT NULL,
    "ConteudoAula" varchar(5000) NOT NULL,
    "MaterialDeApoio" varchar(1000) NOT NULL,
    "Duracao" TEXT NOT NULL,
    CONSTRAINT "FK_Aulas_Cursos_CursoId" FOREIGN KEY ("CursoId") REFERENCES "Cursos" ("Id")
);


CREATE INDEX "IX_Aulas_CursoId" ON "Aulas" ("CursoId");


