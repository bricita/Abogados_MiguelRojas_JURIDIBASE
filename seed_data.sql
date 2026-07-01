-- ============================================================
-- SCRIPT DE DATOS SIMULADOS — JURIDIBASE (Estudio Miguel Rojas)
-- ============================================================
-- IMPORTANTE: Ejecutar solo si la tabla está vacía o truncar antes.
-- ============================================================

-- ============================================================
-- 1. ROLES (ya existen por seed de migración, solo referencia)
-- ============================================================
-- idRol = 1 -> Abogado
-- idRol = 2 -> Administrador
-- idRol = 3 -> Usuario

-- ============================================================
-- 2. USUARIOS (3 adicionales al seed existente)
-- ============================================================
SET IDENTITY_INSERT [dbo].[usuario] ON;
-- El idUsuario=1 (Miguel Rojas) ya existe por seed
INSERT INTO [dbo].[usuario] ([idUsuario], [nombreUsuario], [passwordUsuario], [RolId]) VALUES
(2, N'Ana Torres',      N'12345', 2),  -- Administradora
(3, N'Carlos Mendoza',  N'12345', 1),  -- Abogado
(4, N'Lucia Fernandez', N'12345', 1),  -- Abogada
(5, N'Pedro Castillo',  N'12345', 1),  -- Abogado
(6, N'Rosa Gutierrez',  N'12345', 3),  -- Usuario (asistente)
(7, N'Jorge Paredes',   N'12345', 3);  -- Usuario (asistente)
SET IDENTITY_INSERT [dbo].[usuario] OFF;

-- ============================================================
-- 3. ABOGADOS (1:1 con Usuario)
-- ============================================================
SET IDENTITY_INSERT [dbo].[abogados] ON;
INSERT INTO [dbo].[abogados] ([idAbogado], [nombreAbogado], [apellidoAbogado], [telefonoAbogado], [dniAbogado], [correoAbogado], [especialidadAbogado], [estadoAbogado], [id_Usuario]) VALUES
(1, N'Miguel', N'Rojas',     N'999111001', N'12345678', N'miguel.rojas@estudiojuridico.pe', N'Penal',                  1, 1),
(2, N'Carlos', N'Mendoza',   N'999111002', N'23456789', N'carlos.mendoza@estudiojuridico.pe', N'Civil',                 1, 3),
(3, N'Lucia',  N'Fernandez', N'999111003', N'34567890', N'lucia.fernandez@estudiojuridico.pe', N'Laboral',               1, 4),
(4, N'Pedro',  N'Castillo',  N'999111004', N'45678901', N'pedro.castillo@estudiojuridico.pe', N'Corporativo',           0, 5);
SET IDENTITY_INSERT [dbo].[abogados] OFF;

-- ============================================================
-- 4. AREAS DE DERECHO
-- ============================================================
SET IDENTITY_INSERT [dbo].[areasDerecho] ON;
INSERT INTO [dbo].[areasDerecho] ([idAreaDerecho], [nombreAreaDerecho], [descripcionAreaDerecho], [estadoAreaDerecho]) VALUES
(1, N'Penal',                    N'Delitos y defensa penal, proceso penal peruano',                                  1),
(2, N'Civil',                    N'Contratos, obligaciones, propiedad, responsabilidad civil',                       1),
(3, N'Laboral',                  N'Derecho del trabajo, despidos, beneficios sociales',                              1),
(4, N'Familia',                  N'Divorcio, tenencia, alimentos, adopción',                                        1),
(5, N'Corporativo',              N'Constitución de empresas, fusiones, compliance',                                 1),
(6, N'Tributario',               N'Impuestos, SUNAT, tributación corporativa',                                      1),
(7, N'Constitucional',           N'Amparo, habeas corpus, procesos constitucionales',                               1),
(8, N'Administrativo',           N'Procedimientos administrativos, contrataciones con el Estado',                   1);
SET IDENTITY_INSERT [dbo].[areasDerecho] OFF;

-- ============================================================
-- 5. SERVICIOS LEGALES
-- ============================================================
SET IDENTITY_INSERT [dbo].[servicio] ON;
INSERT INTO [dbo].[servicio] ([idServicio], [nombreServicio], [descripcionServicio], [estadoServicio], [costoBase]) VALUES
(1, N'Consulta Legal',        N'Asesoría jurídica presencial o virtual de 1 hora',                N'Activo', 150.0),
(2, N'Defensa Penal',         N'Defensa en procesos penales en todas las instancias',             N'Activo', 3000.0),
(3, N'Elaboración de Contratos', N'Redacción y revisión de contratos civiles y comerciales',     N'Activo', 500.0),
(4, N'Proceso de Divorcio',   N'Trámite de divorcio ulterior o de mutuo acuerdo',                N'Activo', 2000.0),
(5, N'Constitución de Empresa', N'Constitución de EIRL o SAC, registros públicos',               N'Activo', 800.0),
(6, N'Demanda Laboral',       N'Patrocinio en procesos laborales ante el Poder Judicial',        N'Activo', 2500.0),
(7, N'Recurso de Amparo',     N'Interposición y seguimiento de proceso constitucional de amparo', N'Activo', 1800.0),
(8, N'Defensa Tributaria',    N'Defensa ante SUNAT y Tribunal Fiscal',                           N'Activo', 2200.0),
(9, N'Certificado de Liquidación', N'Cálculo y certificación de beneficios sociales laborales',  N'Activo', 350.0);
SET IDENTITY_INSERT [dbo].[servicio] OFF;

-- ============================================================
-- 6. ABOGADO <-> AREA (muchos a muchos)
-- ============================================================
SET IDENTITY_INSERT [dbo].[abogadoArea] ON;
INSERT INTO [dbo].[abogadoArea] ([idAbogadoArea], [id_Abogado], [id_AreaDerecho]) VALUES
(1,  1, 1),  -- Miguel Rojas -> Penal
(2,  1, 7),  -- Miguel Rojas -> Constitucional
(3,  2, 2),  -- Carlos Mendoza -> Civil
(4,  2, 5),  -- Carlos Mendoza -> Corporativo
(5,  3, 3),  -- Lucia Fernandez -> Laboral
(6,  3, 6),  -- Lucia Fernandez -> Tributario
(7,  4, 5),  -- Pedro Castillo -> Corporativo
(8,  4, 8);  -- Pedro Castillo -> Administrativo
SET IDENTITY_INSERT [dbo].[abogadoArea] OFF;

-- ============================================================
-- 7. ABOGADO <-> SERVICIO (muchos a muchos)
-- ============================================================
SET IDENTITY_INSERT [dbo].[AbogadoServicio] ON;
INSERT INTO [dbo].[AbogadoServicio] ([idAbogadoServicio], [id_ServicioLegal], [id_Abogado]) VALUES
(1,  1, 1),  -- Miguel Rojas -> Consulta Legal
(2,  2, 1),  -- Miguel Rojas -> Defensa Penal
(3,  7, 1),  -- Miguel Rojas -> Recurso de Amparo
(4,  1, 2),  -- Carlos Mendoza -> Consulta Legal
(5,  3, 2),  -- Carlos Mendoza -> Elaboración de Contratos
(6,  5, 2),  -- Carlos Mendoza -> Constitución de Empresa
(7,  1, 3),  -- Lucia Fernandez -> Consulta Legal
(8,  6, 3),  -- Lucia Fernandez -> Demanda Laboral
(9,  9, 3),  -- Lucia Fernandez -> Certificado de Liquidación
(10, 1, 4),  -- Pedro Castillo -> Consulta Legal
(11, 5, 4),  -- Pedro Castillo -> Constitución de Empresa
(12, 8, 4);  -- Pedro Castillo -> Defensa Tributaria
SET IDENTITY_INSERT [dbo].[AbogadoServicio] OFF;

-- ============================================================
-- 8. CLIENTES
-- ============================================================
SET IDENTITY_INSERT [dbo].[cliente] ON;
INSERT INTO [dbo].[cliente] ([idCliente], [nombreCliente], [descripcionCliente], [dniCliente], [rucCliente], [telefonoCliente], [direccionCliente], [correoCliente], [estadoCliente], [tipoCliente], [idAbogado]) VALUES
(1, N'Maria Lopez',      N'Cliente particular, ama de casa',                        N'12345679', N'20123456789', N'987654321', N'Av. Los Olivos 123, San Martin',       N'maria.lopez@gmail.com',     1, N'Natural',  1),
(2, N'Juan Perez',       N'Empresario del rubro textil',                            N'23456780', N'20123456789', N'987654322', N'Jr. La Unión 456, Cercado de Lima',     N'jperez@textilesperu.com',   1, N'Jurídico', 1),
(3, N'Inversiones San Cristóbal SAC', N'Empresa de inversiones inmobiliarias',      N'00000001', N'20567890123', N'987654323', N'Av. Pardo 789, Miraflores',             N'contacto@scristobal.com',   1, N'Jurídico', 2),
(4, N'Carmen Huaman',    N'Docente universitaria, proceso de divorcio',             N'34567891', NULL,           N'987654324', N'Calle Los sauces 321, San Borja',       N'chuaman@unmsm.edu.pe',      1, N'Natural',  3),
(5, N'Roberto Sanchez',  N'Ingeniero, demanda laboral',                             N'45678902', N'10098765432', N'987654325', N'Av. Primavera 654, Surco',              N'roberto@outlook.com',       1, N'Natural',  3),
(6, N'Grupo Constructor del Sur SA', N'Constructora con procesos administrativos',  N'00000002', N'20678901234', N'987654326', N'Av. Benavides 1111, Barranco',          N'admin@gcsur.pe',            1, N'Jurídico', 4),
(7, N'Manuel Ortiz',     N'Pequeño empresario, asesoría corporativa',               N'56789012', N'10098765432', N'987654327', N'Pasaje Las Flores 222, Los Olivos',     N'mortiz@yahoo.com',          1, N'Natural',  2),
(8, N'Diana Prada',      N'Abogada independiente, necesita defensa tributaria',     N'67890123', NULL,           N'987654328', N'Calle Los Ficus 888, San Isidro',       N'dprada@estudio.pe',        1, N'Natural',  4);
SET IDENTITY_INSERT [dbo].[cliente] OFF;

-- ============================================================
-- 9. CASOS
-- ============================================================
SET IDENTITY_INSERT [dbo].[caso] ON;
INSERT INTO [dbo].[caso] ([idCaso], [tituloCaso], [descripcionCaso], [estadoCaso], [id_Abogado], [id_Cliente]) VALUES
(1, N'Defensa por estafa',                    N'Defensa de Maria Lopez acusada de estafa en compraventa de vehículo',              1, 1, 1),
(2, N'Contrato de arrendamiento industrial',  N'Elaboración y revisión de contrato de arrendamiento de local industrial',         1, 2, 3),
(3, N'Divorcio por separación de hecho',      N'Proceso de divorcio ulterior por separación de hecho de Carmen Huaman',           1, 3, 4),
(4, N'Demanda por despido arbitrario',        N'Demanda laboral de Roberto Sanchez contra su empleador por despido sin causa',    1, 3, 5),
(5, N'Constitución de empresa inmobiliaria',  N'Constitución de SAC para proyecto inmobiliario',                                 1, 2, 7),
(6, N'Proceso administrativo SUNAT',          N'Defensa de Grupo Constructor del Sur ante sanción de SUNAT',                      1, 4, 6),
(7, N'Habeas corpus por detención arbitraria', N'Interposición de habeas corpus a favor de familiar de Juan Perez',               1, 1, 2),
(8, N'Defensa tributaria persona natural',    N'Descargo ante SUNAT por omisión de ingresos de Diana Prada',                      1, 4, 8);
SET IDENTITY_INSERT [dbo].[caso] OFF;

-- ============================================================
-- 10. EXPEDIENTES (1:1 con Caso)
-- ============================================================
SET IDENTITY_INSERT [dbo].[expediente] ON;
INSERT INTO [dbo].[expediente] ([idExpediente], [tituloExpediente], [tipoExpediente], [resumenExpediente], [estadoExpediente], [victima], [victimario], [fechaInicio], [fechaCierre], [id_Caso]) VALUES
(1, N'Exp. Penal 001-2024 Estafa ML',       N'Penal',       N'Caso por estafa en compraventa de vehículo, monto S/25,000',                    1, N'Maria Lopez',     N'Juan Garcia',     N'2024-03-01', N'2025-06-15', 1),
(2, N'Exp. Civil 002-2024 Contrato Industrial', N'Civil',   N'Contrato de arrendamiento de local industrial por 5 años',                       1, N'Inversiones San Cristóbal', N'Inmobiliaria Los Andes SAC', N'2024-03-10', N'2024-04-20', 2),
(3, N'Exp. Familia 003-2024 Divorcio CH',   N'Familia',    N'Divorcio ulterior por separación de hecho de 4 años',                            1, N'Carmen Huaman',  N'Pedro Huaman',    N'2024-04-01', N'2025-08-30', 3),
(4, N'Exp. Laboral 004-2024 Despido RS',    N'Laboral',     N'Despido arbitrario sin causa justa, 8 años de servicio',                         1, N'Roberto Sanchez', N'Textiles del Norte SA', N'2024-05-01', N'2025-10-10', 4),
(5, N'Exp. Corporativo 005-2024 Constitución', N'Corporativo', N'Constitución de inmobiliaria, capital S/500,000',                             1, N'Manuel Ortiz',   N'Almacenes "Jorge"',               N'2024-06-01', N'2024-07-15', 5),
(6, N'Exp. Tributario 006-2024 SUNAT GCS',  N'Tributario',  N'Procedimiento administrativo ante SUNAT por omisión de IGV',                     1, N'Grupo Constructor del Sur SA', N'SUNAT', N'2024-07-01', N'2025-09-01', 6),
(7, N'Exp. Constitucional 007-2024 HC JP',  N'Constitucional', N'Habeas corpus por detención arbitraria de familiar',                          1, N'Juan Perez',     N'Policía Nacional del Perú', N'2024-08-01', N'2024-12-20', 7),
(8, N'Exp. Tributario 008-2024 Defensa DP', N'Tributario',  N'Descargo por omisión de ingresos en declaración anual 2023',                     1, N'Diana Prada',    N'SUNAT',           N'2024-09-01', N'2025-11-30', 8);
SET IDENTITY_INSERT [dbo].[expediente] OFF;

-- ============================================================
-- 11. AUDIENCIAS
-- ============================================================
SET IDENTITY_INSERT [dbo].[audiencia] ON;
INSERT INTO [dbo].[audiencia] ([idAudiencia], [direccionAudiencia], [tipoAudiencia], [linkAudiencia], [fechaAudiencia], [horaAudiencia], [id_Abogado], [id_Caso]) VALUES
(1, N'Corte Superior de Lima - Sala Penal',    N'Audiencia de control de acusación',  N'https://zoom.us/j/111', N'2025-07-10', N'2025-07-10 09:00:00', 1, 1),
(2, N'Juzgado Civil de Lima - Sede Central',   N'Conciliación extrajudicial',          N'https://zoom.us/j/222', N'2025-06-20', N'2025-06-20 10:00:00', 2, 2),
(3, N'Juzgado de Familia - Sede San Isidro',   N'Audiencia de divorcio',               N'https://zoom.us/j/333', N'2025-09-05', N'2025-09-05 11:00:00', 3, 3),
(4, N'Juzgado Laboral - Sede Surco',           N'Audiencia de conciliación laboral',   N'https://zoom.us/j/444', N'2025-11-10', N'2025-11-10 09:30:00', 3, 4),
(5, N'Corte Superior de Lima - Sala Laboral',  N'Audiencia de juzgamiento',            N'https://zoom.us/j/555', N'2025-12-01', N'2025-12-01 08:30:00', 3, 4),
(6, N'Tribunal Fiscal - Sede Central',         N'Vista de causa tributaria',           N'https://zoom.us/j/666', N'2025-10-15', N'2025-10-15 10:00:00', 4, 6),
(7, N'Corte Superior de Lima - Sala Constitucional', N'Audiencia de habeas corpus',    N'https://zoom.us/j/777', N'2025-02-20', N'2025-02-20 15:00:00', 1, 7),
(8, N'Tribunal Fiscal - Sede Callao',          N'Vista de causa tributaria',           N'https://zoom.us/j/888', N'2025-12-05', N'2025-12-05 11:00:00', 4, 8);
SET IDENTITY_INSERT [dbo].[audiencia] OFF;

-- ============================================================
-- 12. CITAS
-- ============================================================
SET IDENTITY_INSERT [dbo].[cita] ON;
INSERT INTO [dbo].[cita] ([idCita], [asuntoLegalCita], [detallesAdicionalesCita], [fechaHoraCita], [estadoCita], [id_Abogado], [id_Cliente]) VALUES
(1, N'Primera consulta sobre denuncia penal',      N'La cliente traerá todas las denuncias policiales y el contrato de compraventa',            N'2025-07-05', 1, 1, 1),
(2, N'Firma de contrato de arrendamiento',         N'Revisión final del contrato y firma ante notario',                                         N'2025-06-18', 1, 2, 3),
(3, N'Reunión informativa proceso divorcio',       N'Explicación del proceso de divorcio, plazos y costos',                                      N'2025-07-12', 1, 3, 4),
(4, N'Revisión de documentos laborales',           N'El cliente traerá boletas, contratos y carta de despido',                                   N'2025-07-15', 1, 3, 5),
(5, N'Constitución de empresa',                    N'Definición de tipo societario, capital y objeto social',                                     N'2025-06-22', 1, 2, 7),
(6, N'Entrega de documentación SUNAT',             N'Revisión de notificaciones y preparación de descargo',                                       N'2025-07-20', 1, 4, 6),
(7, N'Coordinación hábeas corpus',                 N'Recopilación de pruebas y redacción de la demanda constitucional',                           N'2025-07-08', 1, 1, 2),
(8, N'Revisión de declaraciones juradas',          N'Análisis de declaraciones anuales 2023 y preparación de descargo',                          N'2025-07-25', 1, 4, 8),
(9, N'Seguimiento de caso penal',                  N'Avances de la investigación fiscal y estrategia de defensa',                                N'2025-07-28', 1, 1, 1),
(10, N'Consulta sobre compra de inmueble',         N'Evaluación legal de contrato de compraventa de inmueble',                                    N'2025-08-01', 0, 2, 7);
SET IDENTITY_INSERT [dbo].[cita] OFF;

-- ============================================================
-- 13. PAGOS
-- ============================================================
SET IDENTITY_INSERT [dbo].[pago] ON;
INSERT INTO [dbo].[pago] ([idPago], [metodoPago], [monto], [fechaPago], [idCliente], [idAbogado], [id_Caso]) VALUES
(1,  N'Yape',       150.0,   N'2025-03-05', 1, 1, 1),
(2,  N'Transferencia', 3000.0, N'2025-03-15', 1, 1, 1),
(3,  N'Transferencia', 500.0,  N'2025-03-20', 3, 2, 2),
(4,  N'Efectivo',   2000.0, N'2025-04-10', 4, 3, 3),
(5,  N'Transferencia', 2500.0, N'2025-05-15', 5, 3, 4),
(6,  N'Depósito',   800.0,  N'2025-06-01', 7, 2, 5),
(7,  N'Transferencia', 2200.0, N'2025-07-15', 6, 4, 6),
(8,  N'Yape',       1800.0, N'2025-08-20', 2, 1, 7),
(9,  N'Transferencia', 2200.0, N'2025-09-25', 8, 4, 8);
SET IDENTITY_INSERT [dbo].[pago] OFF;

-- ============================================================
-- 14. DOCUMENTOS LEGALES
-- ============================================================
SET IDENTITY_INSERT [dbo].[documento] ON;
INSERT INTO [dbo].[documento] ([idDocumentoLegal], [nombreDocumento], [rutaDocumento], [fechaCreacion], [id_Expediente]) VALUES
(1,  N'Denuncia Policial',                    N'/documentos/exp1/denuncia_policial.pdf',       N'2024-03-05', 1),
(2,  N'Contrato de Compraventa',              N'/documentos/exp1/contrato_compraventa.pdf',    N'2024-03-10', 1),
(3,  N'Contrato de Arrendamiento',            N'/documentos/exp2/contrato_arrendamiento.pdf',  N'2024-03-15', 2),
(4,  N'Demanda de Divorcio',                  N'/documentos/exp3/demanda_divorcio.pdf',        N'2024-04-05', 3),
(5,  N'Carta de Despido',                     N'/documentos/exp4/carta_despido.pdf',           N'2024-05-10', 4),
(6,  N'Boletas de Pago',                      N'/documentos/exp4/boletas_pago.pdf',            N'2024-05-12', 4),
(7,  N'Minuta de Constitución',               N'/documentos/exp5/minuta_constitucion.pdf',     N'2024-06-05', 5),
(8,  N'Notificación SUNAT',                   N'/documentos/exp6/notificacion_sunat.pdf',      N'2024-07-10', 6),
(9,  N'Demanda de Hábeas Corpus',             N'/documentos/exp7/demanda_habeas_corpus.pdf',   N'2024-08-05', 7),
(10, N'Declaración Jurada Anual 2023',        N'/documentos/exp8/ddjj_2023.pdf',               N'2024-09-10', 8),
(11, N'Descargo SUNAT',                       N'/documentos/exp8/descargo_sunat.pdf',          N'2024-10-01', 8),
(12, N'Sentencia Penal',                      N'/documentos/exp1/sentencia_penal.pdf',         N'2025-06-15', 1);
SET IDENTITY_INSERT [dbo].[documento] OFF;

-- ============================================================
-- 15. NOTIFICACIONES
-- ============================================================
SET IDENTITY_INSERT [dbo].[notificacion] ON;
INSERT INTO [dbo].[notificacion] ([idNotificacion], [tituloNotificacion], [mensajeNotificacion], [leido], [fechaNotificacion], [id_Usuario]) VALUES
(1,  N'Nueva cita asignada',          N'Se ha registrado una nueva cita con Maria Lopez para el 05/07/2025',                   0, N'2025-07-01', 1),
(2,  N'Pago recibido',                N'Se ha registrado un pago de S/150.00 de Maria Lopez (Caso #1)',                       0, N'2025-03-05', 1),
(3,  N'Audiencia programada',         N'Audiencia de control de acusación programada para el 10/07/2025 a las 09:00',        0, N'2025-06-15', 1),
(4,  N'Documento subido',             N'Se ha subido la sentencia penal al expediente #1',                                   0, N'2025-06-15', 1),
(5,  N'Nuevo caso asignado',          N'Se le ha asignado el caso "Divorcio por separación de hecho" de Carmen Huaman',      0, N'2025-04-01', 4),
(6,  N'Pago pendiente',               N'El cliente Roberto Sanchez tiene un pago pendiente de S/500.00',                     0, N'2025-06-20', 4),
(7,  N'Cita cancelada',               N'La cita de Manuel Ortiz del 01/08/2025 ha sido cancelada',                           0, N'2025-07-28', 2),
(8,  N'Notificación SUNAT recibida',  N'Se ha recibido una notificación de SUNAT para el caso administrativo #6',            0, N'2025-07-10', 5),
(9,  N'Tarea administrativa',         N'Recordatorio: actualizar expedientes físicos en archivo central',                    0, N'2025-07-05', 2),
(10, N'Bienvenida al sistema',        N'Bienvenido a JURIDIBASE. Por favor, complete su perfil de usuario.',                 1, N'2025-01-10', 7);
SET IDENTITY_INSERT [dbo].[notificacion] OFF;

-- ============================================================
-- 16. ESPECIALISTAS (tabla independiente)
-- ============================================================
SET IDENTITY_INSERT [dbo].[especialista] ON;
INSERT INTO [dbo].[especialista] ([idEspecialista], [nombreEspecialista], [descripcionEspecialista], [estadoEspecialista], [dniEspecialista], [disponibilidadEspecialista], [telefonoEspecialista], [correoEspecialista]) VALUES
(1, N'Dr. Ricardo Montero',     N'Psicólogo forense especializado en pericias psicológicas para casos de familia y penal',           N'Disponible',  N'98765432', 1, N'987000001', N'rmontero@f.pe'),
(2, N'CPC. Sofia Huaman',       N'Contadora pública especializada en peritajes contables y tributarios',                              N'Disponible',  N'87654321', 1, N'987000002', N'shuaman@cta.pe'),
(3, N'Ing. Mario Delgado',      N'Ingeniero de sistemas especializado en pericias informáticas y recuperación de datos',              N'Disponible',  N'76543210', 1, N'987000003', N'mdelgado@inf.pe'),
(4, N'Dr. Luis Alva',           N'Médico legista especializado en tanatología y lesiones',                                            N'Disponible',  N'65432109', 1, N'987000004', N'lalva@med.pe'),
(5, N'Lic. Patricia Neira',     N'Trabajadora social especializada en informes sociales para procesos de familia y menores',         N'Disponible',  N'54321098', 0, N'987000005', N'pneira@soc.pe'),
(6, N'Dr. Carlos Vega',         N'Criminólogo especializado en análisis de conducta criminal',                                       N'Disponible',  N'43210987', 1, N'987000006', N'vega@crim.pe');
SET IDENTITY_INSERT [dbo].[especialista] OFF;

-- ============================================================
-- FIN DEL SCRIPT
-- ============================================================
PRINT 'INSERCIÓN DE DATOS SIMULADOS COMPLETADA EXITOSAMENTE.';
GO
