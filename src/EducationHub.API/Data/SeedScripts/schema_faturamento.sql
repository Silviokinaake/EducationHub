CREATE TABLE "Pagamentos" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Pagamentos" PRIMARY KEY,
    "AlunoId" TEXT NOT NULL,
    "PreMatriculaId" TEXT NOT NULL,
    "Valor" decimal(18,2) NOT NULL,
    "DataPagamento" datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    "Status" INTEGER NOT NULL,
    "TokenCartao" varchar(200) NULL,
    "NumeroCartaoMascarado" varchar(50) NULL
);


CREATE INDEX "IX_Pagamentos_AlunoId" ON "Pagamentos" ("AlunoId");


CREATE INDEX "IX_Pagamentos_PreMatriculaId" ON "Pagamentos" ("PreMatriculaId");


