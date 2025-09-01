describe("plantillaPanelPrincipalTest", function () {

    window.beforeEach(module('backbone'));
    window.beforeEach(module('mocks.tests'));

    var $scope;
    var $controller;
    var $uibModal;
    var $log;
    var controladorPanelPrincipal;
    var servicioPanelPrincipalMock;

    var grupoEntidadesSinTramites = {
        data: {
            "Mensaje": "No se encontraron entidades con trámites disponibles.",
            "GruposEntidades": []
        }
    };

    var grupoEntidadesSinProyectos = {
        data: {
            "Mensaje": "No se encontraron entidades con proyectos disponibles.",
            "GruposEntidades": []
        }
    };

    var grupoEntidades2Entidades2Tramites = {
        data:
        {
            "Mensaje": "Exitoso.",
            "GruposEntidades": [
                {
                    "TipoEntidad": "Nacional",
                    "ListaEntidades": [
                        {
                            "IdEntidad": "xxx1",
                            "NombreEntidad": "TRAMITE DEPARTAMENTO NACIONAL DE TRANSPORTE - GESTION GENERAL",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "tram001",
                                    "NombreObjetoNegocio":
                                    "TRAMITE CONTROL Y SEGUIMIENTO MEDIANTE AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                                    "Criticidad": "Alta",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        }
                    ]
                },
                {
                    "TipoEntidad": "Territorial",
                    "ListaEntidades": [
                        {
                            "IdEntidad": "xxx3",
                            "NombreEntidad": "TRAMITE TERRITORIAL DE VIAS - GESTION GENERAL",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "tram004",
                                    "NombreObjetoNegocio":
                                    "TRAMITE INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                                    "Criticidad": "Alta",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        }
                    ]
                }
            ]
        }
    };

    var grupoEntidades5Entidades6Proyectos = {
        data: {
            "Mensaje": "Exitoso.",
            "GruposEntidades": [
                {
                    "TipoEntidad": "Nacional",
                    "ListaEntidades": [
                        {
                            "IdEntidad": "xxx1",
                            "NombreEntidad": "DEPARTAMENTO NACIONAL DE TRANSPORTE - GESTION GENERAL",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "proy001",
                                    "NombreObjetoNegocio":
                                    "PROYECTO CONTROL Y SEGUIMIENTO MEDIANTE AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                                    "Criticidad": "Alta",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        },
                        {
                            "IdEntidad": "xxx2",
                            "NombreEntidad": "DEPARTAMENTO NACIONAL DE PLANEACION",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "proy002",
                                    "NombreObjetoNegocio": "PROYECTO GESTION DEL CAMBIO DE LEY TAQUE",
                                    "Criticidad": "Media",
                                    "Estado": "No se que va aqui"
                                },
                                {
                                    "IdObjetoNegocio": "proy003",
                                    "NombreObjetoNegocio":
                                    "PROYECTO FINANCIERA A LA INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                                    "Criticidad": "Baja",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        }
                    ]
                },
                {
                    "TipoEntidad": "Territorial",
                    "ListaEntidades": [
                        {
                            "IdEntidad": "xxx3",
                            "NombreEntidad": "TERRITORIAL DE VIAS - GESTION GENERAL",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "proy004",
                                    "NombreObjetoNegocio":
                                    "PROYECTO INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                                    "Criticidad": "Alta",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        },
                        {
                            "IdEntidad": "xxx4",
                            "NombreEntidad": "TERRITORIAL DE GESTION ADMINISTRATIVA",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "proy005",
                                    "NombreObjetoNegocio":
                                    "PROYECTO AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION",
                                    "Criticidad": "Media",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        },
                        {
                            "IdEntidad": "xxx5",
                            "NombreEntidad": "TERRITORIAL DE GESTION ADMINISTRATIVA",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "proy006",
                                    "NombreObjetoNegocio":
                                    "PROYECTO AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION",
                                    "Criticidad": "Baja",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        }
                    ]
                }
            ]
        }
    };

    var grupoEntidadNacional2Entidades3Proyectos = {
        data: {
            "Mensaje": "Exitoso.",
            "GruposEntidades": [
                {
                    "TipoEntidad": "Nacional",
                    "ListaEntidades": [
                        {
                            "IdEntidad": "xxx1",
                            "NombreEntidad": "DEPARTAMENTO NACIONAL DE TRANSPORTE - GESTION GENERAL",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "proy001",
                                    "NombreObjetoNegocio":
                                    "PROYECTO CONTROL Y SEGUIMIENTO MEDIANTE AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                                    "Criticidad": "Alta",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        },
                        {
                            "IdEntidad": "xxx2",
                            "NombreEntidad": "DEPARTAMENTO NACIONAL DE PLANEACION",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "proy002",
                                    "NombreObjetoNegocio": "PROYECTO GESTION DEL CAMBIO DE LEY TAQUE",
                                    "Criticidad": "Media",
                                    "Estado": "No se que va aqui"
                                },
                                {
                                    "IdObjetoNegocio": "proy003",
                                    "NombreObjetoNegocio":
                                    "PROYECTO FINANCIERA A LA INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                                    "Criticidad": "Baja",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        }
                    ]
                }
            ]
        }
    };

    var grupoEntidadNacional2Entidades3Tramites = {
        data: {
            "Mensaje": "Exitoso.",
            "GruposEntidades": [
                {
                    "TipoEntidad": "Nacional",
                    "ListaEntidades": [
                        {
                            "IdEntidad": "xxx1",
                            "NombreEntidad": "DEPARTAMENTO NACIONAL DE TRANSPORTE - GESTION GENERAL",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "tram001",
                                    "NombreObjetoNegocio":
                                    "TRAMITE CONTROL Y SEGUIMIENTO MEDIANTE AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                                    "Criticidad": "Alta",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        },
                        {
                            "IdEntidad": "xxx2",
                            "NombreEntidad": "DEPARTAMENTO NACIONAL DE PLANEACION",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "tram002",
                                    "NombreObjetoNegocio": "TRAMITE GESTION DEL CAMBIO DE LEY TAQUE",
                                    "Criticidad": "Media",
                                    "Estado": "No se que va aqui"
                                },
                                {
                                    "IdObjetoNegocio": "tram003",
                                    "NombreObjetoNegocio":
                                    "TRAMITE FINANCIERA A LA INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                                    "Criticidad": "Baja",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        }
                    ]
                }
            ]
        }
    };

    var grupoEntidadesTerritorial3Entidades3Proyectos = {
        data: {
            "Mensaje": "Exitoso.",
            "GruposEntidades": [
                {
                    "TipoEntidad": "Territorial",
                    "ListaEntidades": [
                        {
                            "IdEntidad": "xxx3",
                            "NombreEntidad": "TERRITORIAL DE VIAS - GESTION GENERAL",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "proy004",
                                    "NombreObjetoNegocio":
                                    "PROYECTO INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                                    "Criticidad": "Alta",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        },
                        {
                            "IdEntidad": "xxx4",
                            "NombreEntidad": "TERRITORIAL DE GESTION ADMINISTRATIVA",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "proy005",
                                    "NombreObjetoNegocio":
                                    "PROYECTO AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION",
                                    "Criticidad": "Media",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        },
                        {
                            "IdEntidad": "xxx5",
                            "NombreEntidad": "TERRITORIAL DE GESTION ADMINISTRATIVA",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "proy006",
                                    "NombreObjetoNegocio":
                                    "PROYECTO AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION",
                                    "Criticidad": "Baja",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        }
                    ]
                }
            ]
        }
    };

    var grupoEntidadesTerritorial3Entidades3Tramites = {
        data: {
            "Mensaje": "Exitoso.",
            "GruposEntidades": [
                {
                    "TipoEntidad": "Territorial",
                    "ListaEntidades": [
                        {
                            "IdEntidad": "xxx3",
                            "NombreEntidad": "TERRITORIAL DE VIAS - GESTION GENERAL",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "tram004",
                                    "NombreObjetoNegocio":
                                    "TRAMITE INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                                    "Criticidad": "Alta",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        },
                        {
                            "IdEntidad": "xxx4",
                            "NombreEntidad": "TERRITORIAL DE GESTION ADMINISTRATIVA",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "tram005",
                                    "NombreObjetoNegocio":
                                    "TRAMITE AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION",
                                    "Criticidad": "Media",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        },
                        {
                            "IdEntidad": "xxx5",
                            "NombreEntidad": "TERRITORIAL DE GESTION ADMINISTRATIVA",
                            "ObjetosNegocio": [
                                {
                                    "IdObjetoNegocio": "tram006",
                                    "NombreObjetoNegocio":
                                    "TRAMITE AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION",
                                    "Criticidad": "Baja",
                                    "Estado": "No se que va aqui"
                                }
                            ]
                        }
                    ]
                }
            ]
        }
    };

    // ReSharper disable InconsistentNaming
    window.beforeEach(inject(function (_$controller_, $rootScope, _servicioPanelPrincipalMock_) {
        // ReSharper restore InconsistentNaming

        $controller = _$controller_;
        $scope = $rootScope.$new();
        $uibModal = null;
        $log = null;
        servicioPanelPrincipalMock = _servicioPanelPrincipalMock_;

        controladorPanelPrincipal = $controller('controladorPanelPrincipal', {
            $scope: $scope,
            servicioPanelPrincipal: _servicioPanelPrincipalMock_,
            $uibModal: $uibModal,
            $log: $log
        });
    }));

    window.it('Controlador Instanciado', function () {
        expect(controladorPanelPrincipal).toBeDefined();
    });

    window.it('Que muestre mensaje de proyectos no existentes.', function () {
        var mensaje = "No se encontraron entidades con proyectos disponibles.";
        var promise = window.Promise.resolve(grupoEntidadesSinProyectos);

        window.spyOn(servicioPanelPrincipalMock, "obtenerInbox").and.returnValue(promise);

        controladorPanelPrincipal.obtenerInbox('Proyecto').then(function () {
            expect(controladorPanelPrincipal.Mensaje).toMatch(mensaje);
        });
    });

    window.it('Que muestre mensaje de trámites no existentes.', function () {
        var mensaje = "No se encontraron entidades con trámites disponibles.";
        var promise = window.Promise.resolve(grupoEntidadesSinTramites);

        window.spyOn(servicioPanelPrincipalMock, "obtenerInbox").and.returnValue(promise);

        controladorPanelPrincipal.obtenerInbox('Tramite').then(function () {
            expect(controladorPanelPrincipal.Mensaje).toMatch(mensaje);
        });
    });

    window.it('Que cargue la cantidad de trámites', function () {
        var cantidadEsperada = 2;
        var promise = window.Promise.resolve(grupoEntidades2Entidades2Tramites);

        window.spyOn(servicioPanelPrincipalMock, "obtenerInbox").and.returnValue(promise);

        controladorPanelPrincipal.obtenerInbox('Tramite').then(function () {
            expect(controladorPanelPrincipal.cantidadDeTramites).toBe(cantidadEsperada);
        });

    });

    window.it('Que cargue la cantidad de proyectos', function () {
        var cantidadEsperada = 6;
        var promise = window.Promise.resolve(grupoEntidades5Entidades6Proyectos);

        window.spyOn(servicioPanelPrincipalMock, "obtenerInbox").and.returnValue(promise);

        controladorPanelPrincipal.obtenerInbox(idTipoProyecto).then(function () {
            expect(controladorPanelPrincipal.cantidadDeProyectos).toBe(cantidadEsperada);
        });
    });

    window.it('Que cargue información puntual de un proyecto', function () {
        var promise = window.Promise.resolve(grupoEntidades5Entidades6Proyectos);
        var proyecto = {
            IdObjetoNegocio: "proy001",
            NombreObjetoNegocio:
            "PROYECTO CONTROL Y SEGUIMIENTO MEDIANTE AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
            Criticidad: "Alta",
            Estado: "No se que va aqui"
        };
        var proyectoEncontrado = false;

        window.spyOn(servicioPanelPrincipalMock, "obtenerInbox").and.returnValue(promise);

        controladorPanelPrincipal.obtenerInbox(idTipoProyecto).then(function () {

            window.angular.forEach(controladorPanelPrincipal.gruposEntidadesProyectos,
                function (grupoEntidad) {
                    if (!proyectoEncontrado) {
                        window.angular.forEach(grupoEntidad.ListaEntidades,
                            function (entidad) {
                                window.angular.forEach(entidad.ObjetosNegocio,
                                    function (objetoNegocio) {

                                        if ((objetoNegocio.IdObjetoNegocio === proyecto.IdObjetoNegocio)
                                            && (objetoNegocio.NombreObjetoNegocio === proyecto.NombreObjetoNegocio)
                                            && (objetoNegocio.Criticidad === proyecto.Criticidad)
                                            && (objetoNegocio.Estado === proyecto.Estado)) {

                                            proyectoEncontrado = true;
                                        }
                                    });
                            });
                    }
                });
            expect(proyectoEncontrado).toBe(true);
        });
    });

    window.it('Que cargue información puntual de un trámite', function () {
        var promise = window.Promise.resolve(grupoEntidades2Entidades2Tramites);
        var tramite = {
            IdObjetoNegocio: "tram004",
            NombreObjetoNegocio:
            "TRAMITE INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
            Criticidad: "Alta",
            Estado: "No se que va aqui"
        };
        var tramiteEncontrado = false;

        window.spyOn(servicioPanelPrincipalMock, "obtenerInbox").and.returnValue(promise);

        controladorPanelPrincipal.obtenerInbox(idTipoTramite).then(function () {

            window.angular.forEach(controladorPanelPrincipal.gruposEntidadesTramites,
                function (grupoEntidad) {
                    if (!tramiteEncontrado) {
                        window.angular.forEach(grupoEntidad.ListaEntidades,
                            function (entidad) {
                                window.angular.forEach(entidad.ObjetosNegocio,
                                    function (objetoNegocio) {

                                        if ((objetoNegocio.IdObjetoNegocio === tramite.IdObjetoNegocio)
                                            && (objetoNegocio.NombreObjetoNegocio === tramite.NombreObjetoNegocio)
                                            && (objetoNegocio.Criticidad === tramite.Criticidad)
                                            && (objetoNegocio.Estado === tramite.Estado)) {

                                            tramiteEncontrado = true;
                                        }
                                    });
                            });
                    }
                });
            expect(tramiteEncontrado).toBe(true);
        });
    });

    window.it('Que cargue información puntual de una entidad en proyectos', function () {
        var promise = window.Promise.resolve(grupoEntidades5Entidades6Proyectos);
        var entidadProyecto = {
            IdEntidad: "xxx1",
            NombreEntidad: "DEPARTAMENTO NACIONAL DE TRANSPORTE - GESTION GENERAL",
            ObjetosNegocio: [
                {
                    IdObjetoNegocio: "proy001",
                    NombreObjetoNegocio:
                    "PROYECTO CONTROL Y SEGUIMIENTO MEDIANTE AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                    Criticidad: "Alta",
                    Estado: "No se que va aqui"
                }
            ]
        };
        var entidadEncontrada = false;

        window.spyOn(servicioPanelPrincipalMock, "obtenerInbox").and.returnValue(promise);

        controladorPanelPrincipal.obtenerInbox(idTipoProyecto).then(function () {

            window.angular.forEach(controladorPanelPrincipal.gruposEntidadesProyectos,
                function (grupoEntidad) {
                    if (!entidadEncontrada) {
                        window.angular.forEach(grupoEntidad.ListaEntidades,
                            function (entidad) {
                                if ((entidad.IdEntidad === entidadProyecto.IdEntidad)
                                    && (entidad.NombreEntidad === entidadProyecto.NombreEntidad)
                                    && (entidad.Criticidad.length === entidadProyecto.ObjetosNegocio.length))

                                    entidadEncontrada = true;
                            });
                    }
                });
            expect(entidadEncontrada).toBe(true);
        });
    });

    window.it('Que cargue información puntual de una entidad en trámites', function () {
        var promise = window.Promise.resolve(grupoEntidades2Entidades2Tramites);
        var entidadTramite = {
            IdEntidad: "xxx3",
            NombreEntidad: "TRAMITE TERRITORIAL DE VIAS - GESTION GENERAL",
            ObjetosNegocio: [
                {
                    IdObjetoNegocio: "tram004",
                    NombreObjetoNegocio:
                    "TRAMITE INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                    Criticidad: "Alta",
                    Estado: "No se que va aqui"
                }
            ]
        };

        var entidadEncontrada = false;

        window.spyOn(servicioPanelPrincipalMock, "obtenerInbox").and.returnValue(promise);

        controladorPanelPrincipal.obtenerInbox(idTipoProyecto).then(function () {

            window.angular.forEach(controladorPanelPrincipal.gruposEntidadesProyectos,
                function (grupoEntidad) {
                    if (!entidadEncontrada) {
                        window.angular.forEach(grupoEntidad.ListaEntidades,
                            function (entidad) {
                                if ((entidad.IdEntidad === entidadTramite.IdEntidad)
                                    && (entidad.NombreEntidad === entidadTramite.NombreEntidad)
                                    && (entidad.Criticidad.length === entidadTramite.ObjetosNegocio.length))

                                    entidadEncontrada = true;
                            });
                    }
                });
            expect(entidadEncontrada).toBe(true);
        });
    });

    window.it('Que cargue información puntual de tipo de entidad Nacional en Proyectos', function () {
        var promise = window.Promise.resolve(grupoEntidadNacional2Entidades3Proyectos);
        var tipoEntidadNacional = {
            TipoEntidad: "Nacional",
            ListaEntidades: [
                {
                    IdEntidad: "xxx1",
                    NombreEntidad: "DEPARTAMENTO NACIONAL DE TRANSPORTE - GESTION GENERAL",
                    ObjetosNegocio: [
                        {
                            IdObjetoNegocio: "proy001",
                            NombreObjetoNegocio:
                            "PROYECTO CONTROL Y SEGUIMIENTO MEDIANTE AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                            Criticidad: "Alta",
                            Estado: "No se que va aqui"
                        }
                    ]
                },
                {
                    IdEntidad: "xxx2",
                    NombreEntidad: "DEPARTAMENTO NACIONAL DE PLANEACION",
                    ObjetosNegocio: [
                        {
                            IdObjetoNegocio: "proy002",
                            NombreObjetoNegocio: "PROYECTO GESTION DEL CAMBIO DE LEY TAQUE",
                            Criticidad: "Media",
                            Estado: "No se que va aqui"
                        },
                        {
                            IdObjetoNegocio: "proy003",
                            NombreObjetoNegocio:
                            "PROYECTO FINANCIERA A LA INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                            Criticidad: "Baja",
                            Estado: "No se que va aqui"
                        }
                    ]
                }
            ]
        };
        var tipoEntidadEncontrada = false;

        window.spyOn(servicioPanelPrincipalMock, "obtenerInbox").and.returnValue(promise);

        controladorPanelPrincipal.obtenerInbox(idTipoProyecto).then(function () {

            window.angular.forEach(controladorPanelPrincipal.gruposEntidadesProyectos,
                function (grupoEntidad) {
                    if (!tipoEntidadEncontrada) {
                        if ((grupoEntidad.TipoEntidad === tipoEntidadNacional.TipoEntidad)
                            && (grupoEntidad.ListaEntidades.length === tipoEntidadNacional.ListaEntidades.length))

                            tipoEntidadEncontrada = true;
                    }
                });
            expect(tipoEntidadEncontrada).toBe(true);
        });
    });

    window.it('Que cargue información puntual de tipo de entidad Nacional en Tramites', function () {
        var promise = window.Promise.resolve(grupoEntidadNacional2Entidades3Tramites);
        var tipoEntidadNacional = {
            TipoEntidad: "Nacional",
            ListaEntidades: [
                {
                    IdEntidad: "xxx1",
                    NombreEntidad: "DEPARTAMENTO NACIONAL DE TRANSPORTE - GESTION GENERAL",
                    ObjetosNegocio: [
                        {
                            IdObjetoNegocio: "tram001",
                            NombreObjetoNegocio:
                            "TRAMITE CONTROL Y SEGUIMIENTO MEDIANTE AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                            Criticidad: "Alta",
                            Estado: "No se que va aqui"
                        }
                    ]
                },
                {
                    IdEntidad: "xxx2",
                    NombreEntidad: "DEPARTAMENTO NACIONAL DE PLANEACION",
                    ObjetosNegocio: [
                        {
                            IdObjetoNegocio: "tram002",
                            NombreObjetoNegocio: "TRAMITE GESTION DEL CAMBIO DE LEY TAQUE",
                            Criticidad: "Media",
                            Estado: "No se que va aqui"
                        },
                        {
                            IdObjetoNegocio: "tram003",
                            NombreObjetoNegocio:
                            "TRAMITE FINANCIERA A LA INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                            Criticidad: "Baja",
                            Estado: "No se que va aqui"
                        }
                    ]
                }
            ]
        };
        var tipoEntidadEncontrada = false;

        window.spyOn(servicioPanelPrincipalMock, "obtenerInbox").and.returnValue(promise);

        controladorPanelPrincipal.obtenerInbox(idTipoTramite).then(function () {

            window.angular.forEach(controladorPanelPrincipal.gruposEntidadesTramites,
                function (grupoEntidad) {
                    if (!tipoEntidadEncontrada) {
                        if ((grupoEntidad.TipoEntidad === tipoEntidadNacional.TipoEntidad)
                            && (grupoEntidad.ListaEntidades.length === tipoEntidadNacional.ListaEntidades.length))

                            tipoEntidadEncontrada = true;
                    }
                });
            expect(tipoEntidadEncontrada).toBe(true);
        });
    });

    window.it('Que cargue información puntual de tipo de entidad Territorial en Proyectos', function () {
        var promise = window.Promise.resolve(grupoEntidadesTerritorial3Entidades3Proyectos);
        var tipoEntidadNacional = {
            TipoEntidad: "Territorial",
            ListaEntidades: [
                {
                    IdEntidad: "xxx3",
                    NombreEntidad: "TERRITORIAL DE VIAS - GESTION GENERAL",
                    ObjetosNegocio: [
                        {
                            IdObjetoNegocio: "proy004",
                            NombreObjetoNegocio:
                            "PROYECTO INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                            Criticidad: "Alta",
                            Estado: "No se que va aqui"
                        }
                    ]
                },
                {
                    IdEntidad: "xxx4",
                    NombreEntidad: "TERRITORIAL DE GESTION ADMINISTRATIVA",
                    ObjetosNegocio: [
                        {
                            IdObjetoNegocio: "proy005",
                            NombreObjetoNegocio:
                            "PROYECTO AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION",
                            Criticidad: "Media",
                            Estado: "No se que va aqui"
                        }
                    ]
                },
                {
                    IdEntidad: "xxx5",
                    NombreEntidad: "TERRITORIAL DE GESTION ADMINISTRATIVA",
                    ObjetosNegocio: [
                        {
                            IdObjetoNegocio: "proy006",
                            NombreObjetoNegocio:
                            "PROYECTO AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION",
                            Criticidad: "Baja",
                            Estado: "No se que va aqui"
                        }
                    ]
                }
            ]
        };
        var tipoEntidadEncontrada = false;

        window.spyOn(servicioPanelPrincipalMock, "obtenerInbox").and.returnValue(promise);

        controladorPanelPrincipal.obtenerInbox(idTipoProyecto).then(function () {

            window.angular.forEach(controladorPanelPrincipal.gruposEntidadesProyectos,
                function (grupoEntidad) {
                    if (!tipoEntidadEncontrada) {
                        if ((grupoEntidad.TipoEntidad === tipoEntidadNacional.TipoEntidad)
                            && (grupoEntidad.ListaEntidades.length === tipoEntidadNacional.ListaEntidades.length))

                            tipoEntidadEncontrada = true;
                    }
                });
            expect(tipoEntidadEncontrada).toBe(true);
        });
    });

    window.it('Que cargue información puntual de tipo de entidad Territorial en Tramites', function () {
        var promise = window.Promise.resolve(grupoEntidadesTerritorial3Entidades3Tramites);
        var tipoEntidadNacional = {
            TipoEntidad: "Territorial",
            ListaEntidades: [
                {
                    IdEntidad: "xxx3",
                    NombreEntidad: "TERRITORIAL DE VIAS - GESTION GENERAL",
                    ObjetosNegocio: [
                        {
                            IdObjetoNegocio: "tram004",
                            NombreObjetoNegocio:
                            "TRAMITE INVERSION DE REGALIAS DIRECTAS A MUNICIPIOS, DISTRITOS Y DEPARTAMENTOS DEL PAIS",
                            Criticidad: "Alta",
                            Estado: "No se que va aqui"
                        }
                    ]
                },
                {
                    IdEntidad: "xxx4",
                    NombreEntidad: "TERRITORIAL DE GESTION ADMINISTRATIVA",
                    ObjetosNegocio: [
                        {
                            IdObjetoNegocio: "tram005",
                            NombreObjetoNegocio:
                            "TRAMITE AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION",
                            Criticidad: "Media",
                            Estado: "No se que va aqui"
                        }
                    ]
                },
                {
                    IdEntidad: "xxx5",
                    NombreEntidad: "TERRITORIAL DE GESTION ADMINISTRATIVA",
                    ObjetosNegocio: [
                        {
                            IdObjetoNegocio: "tram006",
                            NombreObjetoNegocio:
                            "TRAMITE AUDITORIA ADMINISTRATIVA Y FINANCIERA A LA INVERSION",
                            Criticidad: "Baja",
                            Estado: "No se que va aqui"
                        }
                    ]
                }
            ]
        };
        var tipoEntidadEncontrada = false;

        window.spyOn(servicioPanelPrincipalMock, "obtenerInbox").and.returnValue(promise);

        controladorPanelPrincipal.obtenerInbox(idTipoTramite).then(function () {

            window.angular.forEach(controladorPanelPrincipal.gruposEntidadesTramites,
                function (grupoEntidad) {
                    if (!tipoEntidadEncontrada) {
                        if ((grupoEntidad.TipoEntidad === tipoEntidadNacional.TipoEntidad)
                            && (grupoEntidad.ListaEntidades.length === tipoEntidadNacional.ListaEntidades.length))

                            tipoEntidadEncontrada = true;
                    }
                });
            expect(tipoEntidadEncontrada).toBe(true);
        });
    });
});