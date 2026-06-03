using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Abogados_MiguelRojas_JURIDIBASE.Migrations
{
    /// <inheritdoc />
    public partial class M1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "areasDerecho",
                columns: table => new
                {
                    idAreaDerecho = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreAreaDerecho = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcionAreaDerecho = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    estadoAreaDerecho = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_areasDerecho", x => x.idAreaDerecho);
                });

            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    idCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreCliente = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcionCliente = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    dniCliente = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    rucCliente = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    telefonoCliente = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    direccionCliente = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    correoCliente = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    estadoCliente = table.Column<bool>(type: "bit", nullable: false),
                    tipoCliente = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente", x => x.idCliente);
                });

            migrationBuilder.CreateTable(
                name: "especialista",
                columns: table => new
                {
                    idEspecialista = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreEspecialista = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcionEspecialista = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    estadoEspecialista = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    dniEspecialista = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    disponibilidadEspecialista = table.Column<bool>(type: "bit", nullable: false),
                    telefonoEspecialista = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    correoEspecialista = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_especialista", x => x.idEspecialista);
                });

            migrationBuilder.CreateTable(
                name: "servicio",
                columns: table => new
                {
                    idServicio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreServicio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcionServicio = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    estadoServicio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    costoBase = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicio", x => x.idServicio);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreUsuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    passwordUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.idUsuario);
                });

            migrationBuilder.CreateTable(
                name: "abogados",
                columns: table => new
                {
                    idAbogado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreAbogado = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    apellidoAbogado = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    telefonoAbogado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dniAbogado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    correoAbogado = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    especialidadAbogado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    estadoAbogado = table.Column<bool>(type: "bit", nullable: false),
                    id_Usuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abogados", x => x.idAbogado);
                    table.ForeignKey(
                        name: "FK_abogados_usuario_id_Usuario",
                        column: x => x.id_Usuario,
                        principalTable: "usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notificacion",
                columns: table => new
                {
                    idNotificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tituloNotificacion = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    mensajeNotificacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    leido = table.Column<bool>(type: "bit", nullable: false),
                    fechaNotificacion = table.Column<DateOnly>(type: "date", nullable: false),
                    id_Usuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notificacion", x => x.idNotificacion);
                    table.ForeignKey(
                        name: "FK_notificacion_usuario_id_Usuario",
                        column: x => x.id_Usuario,
                        principalTable: "usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "abogadoArea",
                columns: table => new
                {
                    idAbogadoArea = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_Abogado = table.Column<int>(type: "int", nullable: false),
                    id_AreaDerecho = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abogadoArea", x => x.idAbogadoArea);
                    table.ForeignKey(
                        name: "FK_abogadoArea_abogados_id_Abogado",
                        column: x => x.id_Abogado,
                        principalTable: "abogados",
                        principalColumn: "idAbogado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_abogadoArea_areasDerecho_id_AreaDerecho",
                        column: x => x.id_AreaDerecho,
                        principalTable: "areasDerecho",
                        principalColumn: "idAreaDerecho",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbogadoServicio",
                columns: table => new
                {
                    idAbogadoServicio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_ServicioLegal = table.Column<int>(type: "int", nullable: false),
                    id_Abogado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbogadoServicio", x => x.idAbogadoServicio);
                    table.ForeignKey(
                        name: "FK_AbogadoServicio_abogados_id_Abogado",
                        column: x => x.id_Abogado,
                        principalTable: "abogados",
                        principalColumn: "idAbogado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbogadoServicio_servicio_id_ServicioLegal",
                        column: x => x.id_ServicioLegal,
                        principalTable: "servicio",
                        principalColumn: "idServicio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "caso",
                columns: table => new
                {
                    idCaso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tituloCaso = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcionCaso = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    estadoCaso = table.Column<bool>(type: "bit", nullable: false),
                    id_Abogado = table.Column<int>(type: "int", nullable: false),
                    id_Cliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_caso", x => x.idCaso);
                    table.ForeignKey(
                        name: "FK_caso_abogados_id_Abogado",
                        column: x => x.id_Abogado,
                        principalTable: "abogados",
                        principalColumn: "idAbogado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_caso_cliente_id_Cliente",
                        column: x => x.id_Cliente,
                        principalTable: "cliente",
                        principalColumn: "idCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cita",
                columns: table => new
                {
                    idCita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    asuntoLegalCita = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    detallesAdicionalesCita = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    fechaHoraCita = table.Column<DateOnly>(type: "date", nullable: false),
                    estadoCita = table.Column<bool>(type: "bit", nullable: false),
                    id_Abogado = table.Column<int>(type: "int", nullable: false),
                    id_Cliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cita", x => x.idCita);
                    table.ForeignKey(
                        name: "FK_cita_abogados_id_Abogado",
                        column: x => x.id_Abogado,
                        principalTable: "abogados",
                        principalColumn: "idAbogado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cita_cliente_id_Cliente",
                        column: x => x.id_Cliente,
                        principalTable: "cliente",
                        principalColumn: "idCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audiencia",
                columns: table => new
                {
                    idAudiencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    direccionAudiencia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    tipoAudiencia = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    linkAudiencia = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    fechaAudiencia = table.Column<DateOnly>(type: "date", nullable: false),
                    horaAudiencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    id_Abogado = table.Column<int>(type: "int", nullable: false),
                    id_Caso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audiencia", x => x.idAudiencia);
                    table.ForeignKey(
                        name: "FK_audiencia_abogados_id_Abogado",
                        column: x => x.id_Abogado,
                        principalTable: "abogados",
                        principalColumn: "idAbogado",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_audiencia_caso_id_Caso",
                        column: x => x.id_Caso,
                        principalTable: "caso",
                        principalColumn: "idCaso",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "expediente",
                columns: table => new
                {
                    idExpediente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tituloExpediente = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    tipoExpediente = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    resumenExpediente = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    estadoExpediente = table.Column<bool>(type: "bit", nullable: false),
                    victima = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    victimario = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    fechaInicio = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    fechaCierre = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    id_Caso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expediente", x => x.idExpediente);
                    table.ForeignKey(
                        name: "FK_expediente_caso_id_Caso",
                        column: x => x.id_Caso,
                        principalTable: "caso",
                        principalColumn: "idCaso",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pago",
                columns: table => new
                {
                    idPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    metodoPago = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    monto = table.Column<float>(type: "real", nullable: false),
                    fechaPago = table.Column<DateOnly>(type: "date", nullable: false),
                    id_Caso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pago", x => x.idPago);
                    table.ForeignKey(
                        name: "FK_pago_caso_id_Caso",
                        column: x => x.id_Caso,
                        principalTable: "caso",
                        principalColumn: "idCaso",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "documento",
                columns: table => new
                {
                    idDocumentoLegal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreDocumento = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    rutaDocumento = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    fechaCreacion = table.Column<DateOnly>(type: "date", nullable: false),
                    id_Expediente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documento", x => x.idDocumentoLegal);
                    table.ForeignKey(
                        name: "FK_documento_expediente_id_Expediente",
                        column: x => x.id_Expediente,
                        principalTable: "expediente",
                        principalColumn: "idExpediente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_abogadoArea_id_Abogado",
                table: "abogadoArea",
                column: "id_Abogado");

            migrationBuilder.CreateIndex(
                name: "IX_abogadoArea_id_AreaDerecho",
                table: "abogadoArea",
                column: "id_AreaDerecho");

            migrationBuilder.CreateIndex(
                name: "IX_abogados_id_Usuario",
                table: "abogados",
                column: "id_Usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbogadoServicio_id_Abogado",
                table: "AbogadoServicio",
                column: "id_Abogado");

            migrationBuilder.CreateIndex(
                name: "IX_AbogadoServicio_id_ServicioLegal",
                table: "AbogadoServicio",
                column: "id_ServicioLegal");

            migrationBuilder.CreateIndex(
                name: "IX_audiencia_id_Abogado",
                table: "audiencia",
                column: "id_Abogado");

            migrationBuilder.CreateIndex(
                name: "IX_audiencia_id_Caso",
                table: "audiencia",
                column: "id_Caso");

            migrationBuilder.CreateIndex(
                name: "IX_caso_id_Abogado",
                table: "caso",
                column: "id_Abogado");

            migrationBuilder.CreateIndex(
                name: "IX_caso_id_Cliente",
                table: "caso",
                column: "id_Cliente");

            migrationBuilder.CreateIndex(
                name: "IX_cita_id_Abogado",
                table: "cita",
                column: "id_Abogado");

            migrationBuilder.CreateIndex(
                name: "IX_cita_id_Cliente",
                table: "cita",
                column: "id_Cliente");

            migrationBuilder.CreateIndex(
                name: "IX_documento_id_Expediente",
                table: "documento",
                column: "id_Expediente");

            migrationBuilder.CreateIndex(
                name: "IX_expediente_id_Caso",
                table: "expediente",
                column: "id_Caso",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notificacion_id_Usuario",
                table: "notificacion",
                column: "id_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_pago_id_Caso",
                table: "pago",
                column: "id_Caso");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "abogadoArea");

            migrationBuilder.DropTable(
                name: "AbogadoServicio");

            migrationBuilder.DropTable(
                name: "audiencia");

            migrationBuilder.DropTable(
                name: "cita");

            migrationBuilder.DropTable(
                name: "documento");

            migrationBuilder.DropTable(
                name: "especialista");

            migrationBuilder.DropTable(
                name: "notificacion");

            migrationBuilder.DropTable(
                name: "pago");

            migrationBuilder.DropTable(
                name: "areasDerecho");

            migrationBuilder.DropTable(
                name: "servicio");

            migrationBuilder.DropTable(
                name: "expediente");

            migrationBuilder.DropTable(
                name: "caso");

            migrationBuilder.DropTable(
                name: "abogados");

            migrationBuilder.DropTable(
                name: "cliente");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
