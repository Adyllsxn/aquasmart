using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aquasmart.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeiturasSensor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Temperatura = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    Ph = table.Column<decimal>(type: "numeric(3,1)", precision: 3, scale: 1, nullable: false),
                    Oxigenio = table.Column<decimal>(type: "numeric(4,2)", precision: 4, scale: 2, nullable: false),
                    NivelAgua = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeiturasSensor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Mensagem = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Gravidade = table.Column<int>(type: "integer", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Lida = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ValorRegistado = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    LeituraSensorId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alertas_LeiturasSensor_LeituraSensorId",
                        column: x => x.LeituraSensorId,
                        principalTable: "LeiturasSensor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comandos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Acao = table.Column<int>(type: "integer", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExecutadoPor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Manual"),
                    LeituraSensorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comandos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comandos_LeiturasSensor_LeituraSensorId",
                        column: x => x.LeituraSensorId,
                        principalTable: "LeiturasSensor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_LeituraSensorId",
                table: "Alertas",
                column: "LeituraSensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_Lida_Timestamp",
                table: "Alertas",
                columns: new[] { "Lida", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_Timestamp",
                table: "Alertas",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Comandos_LeituraSensorId",
                table: "Comandos",
                column: "LeituraSensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comandos_Timestamp",
                table: "Comandos",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Comandos_Tipo_Timestamp",
                table: "Comandos",
                columns: new[] { "Tipo", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_LeiturasSensor_Timestamp",
                table: "LeiturasSensor",
                column: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropTable(
                name: "Comandos");

            migrationBuilder.DropTable(
                name: "LeiturasSensor");
        }
    }
}
