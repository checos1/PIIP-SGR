(function () {
    'use strict';

    conceptoTecnicoController.$inject = [
        '$scope',
        '$sessionStorage',
        'constantesBackbone',
        'conceptoTecnicoServicio',
        'viabilidadServicio',
        'conceptodirecciontecnicaServicio',
        'sesionServicios',
        'solicitarconceptoServicio',
        'utilidades',
        'trasladosServicio',
    ];



    function conceptoTecnicoController(
        $scope,
        $sessionStorage,
        constantesBackbone,
        conceptoTecnicoServicio,
        viabilidadServicio,
        conceptodirecciontecnicaServicio,
        sesionServicios,
        solicitarconceptoServicio,
        utilidades,
        trasladosServicio,
    ) {
        var vm = this;
        vm.lang = "es";
        vm.Observaciones = "";
        vm.concepto = $sessionStorage.concepto;
        vm.cantpreguntasabiertas = 0;
        vm.cantpreguntascerradas = 0;
        vm.activo = false;
        vm.rol = constantesBackbone.nameRControlPosteriorViabilidad;
        vm.activarradio = 1;
        vm.analista = false;
        vm.conceptoiguales = [];

        vm.peticion = {
            IdUsuario: usuarioDNP,
            IdObjeto: idTipoProyecto,
            Aplicacion: nombreAplicacionBackbone,
            ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
            IdFiltro: vm.EntityTypeCatalogOptionId,
        };

        vm.peticionobtenercdt = {
            TramiteId: $sessionStorage.TramiteId,
            NivelId: constantesBackbone.idfaseControlPosteriorTramites,//'E8FC3694-C566-4944-A487-DAA494EB3581'
        };

        vm.ConceptoDireccionTecnicaTramite = [{
            FaseId: 0,
            ProjectId: 0,
            InstanciaId: 0,
            FormularioId: 0,
            CR: 0,
            AgregarRequisitos: "",
            Usuario: "",
            Fecha: "",
            Observaciones: "",
            Cumple: 0,// Boolean
            Definitivo: 0,// Boolean
            PreguntaId: 0,
            Pregunta: "",
            Respuesta: "",
            ObservacionPregunta: "",
            OpcionesRespuesta: [
                {
                    OpcionId: 0,
                    ValorOpcion: "",
                }
            ],
            NombreRol: "",
            NombreNivel: "",
            CuestionarioProyectoId: 0,
            TramiteId: 0,
            EsPreguntaAbierta: 0,
            EsConcepto: 0,
        }];
        //Inicio
        vm.init = function () {

            ObtenerConceptoDireccionTecnicaTramite();
            CargarUsuarioConcepto();
            AdministrarVistaFormulario();
            ValidarNuevaEncuesta();
        };

        function AdministrarVistaFormulario() {
            var i = 0
            vm.peticion.ListaIdsRoles.forEach(rol => {
                if (i == 0) {
                    if (rol.toUpperCase() == constantesBackbone.idRAnalistaDIFP) {
                        vm.activo = false;
                        vm.analista = true;
                        i = 1;
                    }
                    else if (rol.toUpperCase() == constantesBackbone.idRControlPosteriorDireccionesTecnicas && vm.concepto != null && vm.concepto.length > 0) {
                        vm.activo = true;
                        i = 1;
                    }
                    else {
                        vm.activo = false;
                    }
                }
            });
            if (vm.activo == true) {
                vm.concepto.forEach(conc => {
                    if (conc.IdUsuarioDNP != vm.peticion.IdUsuario) {
                        conc.Visible = false;
                    }
                    else {
                        vm.conceptoiguales.push(conc);
                    }
                });

                if (vm.conceptoiguales.length == 0) {
                    vm.activo = false;
                }
                if (vm.conceptoiguales.length >= 1) {
                    vm.concepto.forEach(conc => {
                        if (conc.Id != vm.conceptoiguales[vm.conceptoiguales.length - 1].Id) {
                            conc.Visible = false;
                        }
                        else {
                            if (conc.Enviado == true) {
                                vm.activo = false;
                                conc.Visible = true;
                            }
                        }
                    });
                }
            }
            else {
                if (vm.concepto != null && cm.concepto != undefined)
                    vm.concepto.forEach(conc => {
                        if (conc.Enviado == false) {
                            conc.Visible = false;
                        }
                    });
            }
        }
        function ObtenerConceptoDireccionTecnicaTramite() {

            conceptodirecciontecnicaServicio.ObtenerConceptoDireccionTecnicaTramite(vm.peticionobtenercdt)
                .then(resultado => {
                    vm.ConceptoDireccionTecnicaTramite = resultado.data;
                    var usuario = 0;
                    vm.ConceptoDireccionTecnicaTramite.forEach(dt => {
                        vm.Observaciones = dt.Observaciones;
                        dt.Respuesta = parseInt(dt.Respuesta);
                        if (dt.EsPreguntaAbierta == 1) {
                            vm.cantpreguntasabiertas = vm.cantpreguntasabiertas + 1;
                        }
                        if (dt.EsPreguntaAbierta == 2) {
                            vm.cantpreguntascerradas = vm.cantpreguntascerradas + 1;
                        }
                        if (usuario != dt.Usuario) {
                            var i = 0;
                            vm.concepto.forEach(conc => {

                                if (conc.IdUsuarioDNP == dt.Usuario && i == 0) {
                                    dt.EsConcepto = 1;
                                    i = i + 1;
                                }
                            });
                        }
                        usuario = dt.Usuario;
                    });
                });
        }

        function ValidarNuevaEncuesta() {
            if (!vm.analista) {
                vm.conceptoiguales.forEach(conc => {
                    if (conc.Activo && !conc.Enviado) {
                        vm.peticionobtenercdt = {
                            TramiteId: 0,
                            NivelId: constantesBackbone.idfaseControlPosteriorTramites,//'E8FC3694-C566-4944-A487-DAA494EB3581'
                        };
                        ObtenerConceptoDireccionTecnicaTramite();
                    }
                });
            }
        }
        vm.fechaHoy = formatearFecha(obtenerFechaSinHoras(new Date()));

        function obtenerFechaSinHoras(fecha) {
            return new Date(Date.UTC(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), fecha.getHours(), fecha.getMinutes(), fecha.getSeconds()));
        }

        function formatearFecha(fecha) {
            let fechaString = fecha.toISOString();
            return fechaString.substring(0, 19);
        }

        function CargarUsuarioConcepto() {
            var i = 0;
            if (vm.concepto != undefined) {
                vm.concepto.forEach(con => {
                    vm.peticion.IdFiltro = con.EntityTypeCatalogOptionId;
                    solicitarconceptoServicio.ObtenerAnalistasSubDireccionTecnica(vm.peticion)
                        .then(resultado => {
                            vm.analistas = resultado.data;
                            //if ($sessionStorage.concepto[$sessionStorage.concepto.length - 1].Id > 0) {
                            if (con.Id > 0) {
                                vm.analistas.forEach(dt => {

                                    if (dt.IdUsuarioDnp == vm.concepto[i].IdUsuarioDNP) {
                                        vm.concepto[i].Usuario = dt.Nombre;
                                        i = i + 1;
                                    }
                                });
                            }
                        })
                });
            }
        }

        vm.GuardarPreguntasConceptoDireccionTecnica = function (response) {
            validarCampos();
            if (vm.cumple === true) {
                if (vm.ConceptoDireccionTecnicaTramite[0].InstanciaId == null) {
                    CargarParametrosConcepto();
                }
                else {
                    vm.ConceptoDireccionTecnicaTramite.forEach(con => {
                        con.Usuario = usuarioDNP;
                    });
                }
                conceptodirecciontecnicaServicio.GuardarConceptoDireccionTecnicaTramite(vm.ConceptoDireccionTecnicaTramite)
                    .then(function (response) {
                        if (response.data) {
                            ObtenerSolicitarConcepto();
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            ReasignarRadicadoORFEO(usuarioDNP, "-")
                            vm.activo = false;
                            CargarConcepto();
                        } else {
                            swal('', "Error al realizar la operación", 'error');
                            //vm.callback({ arg: true });
                        }
                    });
            }
            else {
                utilidades.mensajeError('El formulario no esta diligenciado en su totalidad');
                //swal('El formulario no esta diligenciado en su totalidad', 'Revise las campos nuevamente.', 'warning');
            }
        }

        function ReasignarRadicadoORFEO(usuarioIdOrigen, usuarioIdDestino) {

            vm.UsuarioOrigen = {
                Login: usuarioIdOrigen
            };

            vm.UsuarioDestino = {
                Login: usuarioIdDestino
            };

            vm.ReasignacionRadicadoDto = {
                UsuarioOrigen: vm.UsuarioOrigen,
                UsuarioDestino: vm.UsuarioDestino,
                TramiteId: $sessionStorage.TramiteId,
            };

            trasladosServicio.ReasignarRadicadoORFEO(vm.ReasignacionRadicadoDto).then(function (response) {

            });
        }

        function CargarConcepto() {
            solicitarconceptoServicio.ObtenerSolicitarConcepto(vm.peticion)
                .then(resultado => {
                    vm.concepto = resultado.data;
                    $sessionStorage.concepto = vm.concepto;
                    AdministrarVistaFormulario();
                });
        }
        function ObtenerSolicitarConcepto() {
            vm.peticion.IdFiltro = $sessionStorage.TramiteId;
            vm.activarbotones = true;
            solicitarconceptoServicio.ObtenerSolicitarConcepto(vm.peticion)
                .then(resultado => {
                    vm.concepto = resultado.data;
                    AdministrarVistaFormulario();
                })
        }

        function CargarParametrosConcepto() {
            vm.ConceptoDireccionTecnicaTramite.forEach(preguntas => {
                preguntas.InstanciaId = constantesBackbone.idfaseControlPosteriorTramites;
                preguntas.FormularioId = '00000000-0000-0000-0000-000000000000';
                preguntas.CR = 1;
                preguntas.AgregarRequisitos = false;
                preguntas.Usuario = usuarioDNP;//vm.concepto[0].IdUsuarioDNP;
                preguntas.Fecha = vm.fechaHoy;
                preguntas.Cumple = true;
                preguntas.TramiteId = $sessionStorage.TramiteId;
                preguntas.Observaciones = vm.ConceptoDireccionTecnicaTramite[1].Observaciones;
                if (preguntas.EsPreguntaAbierta == 1) {
                    preguntas.ObservacionPregunta = preguntas.ObservacionPregunta;
                    preguntas.Respuesta = "";
                }
                if (preguntas.EsPreguntaAbierta == 2) {
                    preguntas.Respuesta = preguntas.Respuesta;
                    preguntas.ObservacionPregunta = "";
                }
            });
        }

        function validarCampos() {
            vm.cumple = true;
            var i = 0
            vm.ConceptoDireccionTecnicaTramite.forEach(preguntas => {
                if (i == 0) {
                    if (preguntas.EsPreguntaAbierta == 1 && (preguntas.ObservacionPregunta == null || preguntas.ObservacionPregunta == '')) {
                        vm.cumple = false;
                        i = 1;
                    }
                    if (preguntas.EsPreguntaAbierta == 2) {
                        if (preguntas.Observaciones == null || preguntas.Observaciones == '') {
                            vm.cumple = false;
                            i = 1;
                        }
                        else {
                            vm.ConceptoDireccionTecnicaTramite[0].Observaciones = vm.ConceptoDireccionTecnicaTramite[1].Observaciones;
                        }
                        if (preguntas.Respuesta == '0' || preguntas.Respuesta == null) {
                            vm.cumple = false;
                            i = 1;
                        }
                    }
                }
            });

            return vm.cumple
        }

    }

    angular.module('backbone').component('conceptoTecnico', {

        templateUrl: "src/app/formulario/ventanas/tramites/componentes/conceptotecnico/conceptoTecnico.html",
        controller: conceptoTecnicoController,
        controllerAs: "vm",
        bindings: {
        }
    });

})();