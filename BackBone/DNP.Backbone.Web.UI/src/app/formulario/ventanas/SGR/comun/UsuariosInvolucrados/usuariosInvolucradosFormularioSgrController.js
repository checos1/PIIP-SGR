(function () {
    'use strict';

    usuariosInvolucradosFormularioSgrController.$inject = [
        '$scope',
        'usuariosInvolucradosSgrServicio',
        '$sessionStorage',
        'utilidades',
        'justificacionCambiosServicio',
        '$uibModal',
        'transversalSgrServicio',
        'solicitarCtusSgrServicio'
    ];

    function usuariosInvolucradosFormularioSgrController(
        $scope,
        usuariosInvolucradosSgrServicio,
        $sessionStorage,
        utilidades,
        justificacionCambiosServicio,
        $uibModal,
        transversalSgrServicio,
        solicitarCtusSgrServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.gridOptions;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.IdInstancia = $sessionStorage.idInstancia;
        vm.IdAccion = $sessionStorage.idAccion;
        vm.tipotramiteid = $sessionStorage.tipoTramiteId;
        vm.tramiteid = $sessionStorage.tipoTramiteId;
        vm.accionActiva = $sessionStorage.estadoAccion == 'Ejecutada';
        vm.idInstanciaPadre = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.InstanciaPadreId : null;

        vm.clickIconoEditar = clickIconoEditar;
        vm.clickIconoGuardar = clickIconoGuardar;
        vm.clickIconoCancelar = clickIconoCancelar;
        vm.clickIconoEliminar = clickIconoEliminar;
        vm.abrirModalAgregarInvolucrado = abrirModalAgregarInvolucrado;
        vm.abrirToolTip = abrirToolTip;
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        //variables locales
        vm.TipoRolesConcepto = [];
        vm.involucrados = [];

        ////#region Metodos

        vm.init = function () {
            consultar();

            $scope.$on("GuardadoInformacionBasica", function (evt, data) {
                habilitaCapitulo(data);
            });
        };

        $scope.$watch('vm.involucrados', function () {
            if (vm.involucrados && vm.involucrados.length > 0) {
                vm.TipoRolesConcepto = [...new Map(vm.involucrados.map((item) => [item["TipoRolConceptoId"], item])).values()];
            }
        });

        function consultar() {
            ObtenerProyectoViabilidadInvolucrados(vm.objetonegocioid, vm.tipoconceptoviabilidadid);
            ObtenerProyectoCtus(vm.objetonegocioid, vm.idInstanciaPadre == '00000000-0000-0000-0000-000000000000' ? vm.IdInstancia : vm.idInstanciaPadre);
        }

        function habilitaCapitulo(activar) {
            var seccionNoHabilitadaInvolucados = document.getElementById('seccionNoHabilitadaInvolucados');
            var seccionHabilitadaInvolucrados = document.getElementById('seccionHabilitadaInvolucados');

            if (!activar) {
                if (seccionNoHabilitadaInvolucados != undefined) {
                    seccionNoHabilitadaInvolucados.classList.remove('hidden');
                    vm.disabled = true;
                }

                if (seccionHabilitadaInvolucrados != undefined) {
                    seccionHabilitadaInvolucrados.classList.add('hidden');
                }
            } else {
                if (seccionHabilitadaInvolucrados != undefined) {
                    seccionHabilitadaInvolucrados.classList.remove('hidden');
                    vm.disabled = false;
                }

                if (seccionNoHabilitadaInvolucados != undefined) {
                    seccionNoHabilitadaInvolucados.classList.add('hidden');
                }
            }
        }

        function ObtenerProyectoViabilidadInvolucrados(proyectoId, tipoConceptoViabilidadId) {
            return usuariosInvolucradosSgrServicio.ObtenerProyectoViabilidadInvolucrados(proyectoId, tipoConceptoViabilidadId).then(
                function (involucrados) {
                    vm.involucrados = involucrados.data;
                    vm.disabled = $sessionStorage.soloLectura;
                    habilitaCapitulo(true);

                    EstablecerPropiedadesAdicionales();
                }
            );
        }

        function ObtenerProyectoCtus(proyectoId, instanciaId) {
            solicitarCtusSgrServicio.SGR_Proyectos_LeerProyectoCtus(proyectoId, instanciaId)
                .then(function (response) {
                    if (response.data != null) {
                        vm.ProyectoCtus = response.data;
                        vm.disabled = false;
                    }
                }, function (error) {
                    utilidades.mensajeError('Ocurrió un problema al leer la información del CTUS.');
                    return "";
                });
        }

        function EstablecerPropiedadesAdicionales() {
            vm.involucrados.forEach((element) => {
                element.Editando = element.Editando === null ? false : element.Editando;
                element.ProyectoId = vm.objetonegocioid;
            });
        }

        function clickIconoEditar(Involucrado) {
            Involucrado.Editando = true;
        }

        function clickIconoGuardar(Involucrado) {
            Guardar(Involucrado);
        }

        function clickIconoCancelar(Involucrado) {
            consultar();

            var NombreObligatorio = document.getElementById('NombreObligatorio' + Involucrado.Id + "_" + Involucrado.TipoRolConceptoId);
            var AreaObligatoria = document.getElementById('AreaObligatoria' + Involucrado.Id + "_" + Involucrado.TipoRolConceptoId);

            if (NombreObligatorio != undefined) {
                NombreObligatorio.classList.add('hidden');
            }

            if (AreaObligatoria != undefined) {
                AreaObligatoria.classList.add('hidden');
            }

            Involucrado.Editando = false;
        }

        function clickIconoEliminar(Involucrado) {
            var Id = Involucrado.Id;

            utilidades.mensajeWarning(
                "¿Está seguro de continuar?",
                function funcionContinuar() {

                    usuariosInvolucradosSgrServicio.EliminarProyectoViabilidadInvolucrados(Id).then(function () {
                        consultar();

                        utilidades.mensajeSuccess("Usuario involucrado fue eliminado con éxito.", false, false, false);
                    })
                },
                function funcionCancelar(reason) {
                    console.log("reason", reason);
                },
                "Aceptar",
                "Cancelar",
                "El usuario involucrado asociado será eliminado.");
        }

        function Guardar(involucrado) {
            if (!validar(involucrado)) { return; }
            return usuariosInvolucradosSgrServicio.guardarProyectoViabilidadInvolucrados(involucrado)
                .then(function (response) {
                    if (response.data != '') {
                        var respuesta = jQuery.parseJSON(response.data)
                        if (respuesta.StatusCode == "200") {
                            guardarCapituloModificado();
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

                            $("#EditarDG").html("EDITAR");
                            vm.init();
                            vm.activar = true;
                            involucrado.Editando = false;
                            vm.limpiarErrores();
                        }
                        else {

                            utilidades.mensajeError("Error al realizar la operación" + respuesta.ReasonPhrase, false);
                        }
                    }
                    else {
                        utilidades.mensajeError("Error al realizar la operación" + respuesta.ReasonPhrase, false);
                    }
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación");
                });
        }

        function validar(involucrado) {
            var valida = true;
            var NombreObligatorio = document.getElementById('NombreObligatorio' + involucrado.Id + "_" + involucrado.TipoRolConceptoId);
            var AreaObligatoria = document.getElementById('AreaObligatoria' + involucrado.Id + "_" + involucrado.TipoRolConceptoId);

            if (involucrado.Nombre.length <= 5) {
                if (NombreObligatorio != undefined) {
                    NombreObligatorio.classList.remove('hidden');
                    valida = false;
                }
            }
            else {
                if (NombreObligatorio != undefined) {
                    NombreObligatorio.classList.add('hidden');
                }
            }

            if (involucrado.Area.length <= 5) {
                if (AreaObligatoria != undefined) {
                    AreaObligatoria.classList.remove('hidden');
                    valida = false;
                }
            }
            else {
                if (AreaObligatoria != undefined) {
                    AreaObligatoria.classList.add('hidden');
                }
            }

            return valida;
        }

        function abrirModalAgregarInvolucrado(TipoRolConcepto) {

            if (TipoRolConcepto.TipoConceptoViabilidadId === 1) {
                const ventanasPermitidas = new Set(['viabilidadSgr', 'elaboracionEntidadNacionalSgr', 'requisitosViabilidadCteiSgr', 'ElaboracionCormagdalenaSgr']);

                transversalSgrServicio.SGR_Transversal_LeerParametro("RolFirmaConceptoViabilidadSGR")
                    .then(function (respuestaParametro) {
                        if (respuestaParametro.data) {
                            var roles = $sessionStorage.listadoAccionesTramite.find((f) => ventanasPermitidas.has(f.Ventana));

                            if (roles != null && roles.Roles.length > 0) {
                                vm.IdRol = (TipoRolConcepto.TipoRolConceptoId === 2) ? roles.Roles[0].IdRol : respuestaParametro.data.Valor;

                                // Asignar valor a identidadVentana según la ventana actual
                                var identidadVentana = (roles.Ventana === 'elaboracionEntidadNacionalSgr' || roles.Ventana === 'requisitosViabilidadCteiSgr' || roles.Ventana === 'ElaboracionCormagdalenaSgr') ? $sessionStorage.EntidadAdscritaId : $sessionStorage.idEntidad;

                                // Manejar casos específicos según el tipo de concepto y ventana
                                if (TipoRolConcepto.TipoRolConceptoId === 1 && (roles.Ventana === 'elaboracionEntidadNacionalSgr' || roles.Ventana === 'requisitosViabilidadCteiSgr' || roles.Ventana === 'ElaboracionCormagdalenaSgr')) {
                                    vm.IdRol = roles.Roles[0].IdRol;
                                } else if (TipoRolConcepto.TipoRolConceptoId === 2) {
                                    if (roles.Ventana === 'elaboracionEntidadNacionalSgr' || roles.Ventana === 'ElaboracionCormagdalenaSgr') {
                                        vm.IdRol = utilidades.obtenerParametroTransversal('RolEmisionConceptoSGR');
                                    }                                  
                                }

                                sendDataToModal(TipoRolConcepto, vm.IdRol, identidadVentana);
                            }
                        }
                    }, function (error) {
                        utilidades.mensajeError(error);
                    });
            } else if (TipoRolConcepto.TipoConceptoViabilidadId == 3) {
                const ventanasPermitidas = new Set(['ElaboracionctusIntegradoSgr', 'ElaboracionCormagdalenaSgr']);
                var roles = $sessionStorage.listadoAccionesTramite.find((f) => ventanasPermitidas.has(f.Ventana));
                if (roles != null && roles.Roles.length > 0) {
                    var idEntidadVentana = (roles.Ventana === 'ElaboracionctusIntegradoSgr' || roles.Ventana === 'ElaboracionCormagdalenaSgr') ? $sessionStorage.EntidadAdscritaId : $sessionStorage.idEntidad;
                    vm.IdRol = (TipoRolConcepto.TipoRolConceptoId === 2) ? vm.ProyectoCtus.RolEmiteId : vm.ProyectoCtus.RolTecnicoId;
                    sendDataToModal(TipoRolConcepto, vm.IdRol, idEntidadVentana);
                }
            } else {
                if (!vm.ProyectoCtus) return;

                // Asignar valor a identidadVentana según el tipo de concepto
                vm.IdRol = (TipoRolConcepto.TipoRolConceptoId === 2) ? vm.ProyectoCtus.RolEmiteId : vm.ProyectoCtus.RolTecnicoId;

                sendDataToModal(TipoRolConcepto, vm.IdRol, vm.ProyectoCtus.EntidadDestino);
            }
        }

        //#region Metodos    

        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
                //ObtenerSeccionCapitulo();
            }
        });

        function sendDataToModal(TipoRolConcepto, rolId, entidadId) {
            var data = {
                TipoConceptoViabilidadId: TipoRolConcepto.TipoConceptoViabilidadId,
                TipoRolConceptoId: TipoRolConcepto.TipoRolConceptoId,
                ProyectoId: vm.objetonegocioid,
                InvolucradosActuales: vm.involucrados.filter(x => x.Nombre !== null),
                RolFiltroId: rolId,
                EntidadId: entidadId
            };

            let modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: "/src/app/formulario/ventanas/SGR/comun/usuariosInvolucrados/modalAgregarInvolucradosSgr.html",
                controller: 'modalAgregarInvolucradosSgrController',
                controllerAs: "vm",
                openedClass: "modal-contentDNP",
                resolve: {
                    involucrado: function () {
                        return data;
                    },
                },
            });

            modalInstance.result.then(data => {
                if (data != null) {
                    guardarCapituloModificado();
                    consultar();
                    vm.limpiarErrores();
                    vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                }
            });
        }

        function onClickCancelar() {
            vm.archivo = undefined;
            vm.nombreArchivo = undefined;
            vm.desactivarGuardar = true;
        }

        function ocultarMensajeError() {

        }

        function mostrarMensajeError(mensajeError, elemento, numeroCamposError) {

        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,

            }
            justificacionCambiosServicio.eliminarCapitulosModificados(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombrecomponentepaso);
            vm.seccionCapitulo = span.textContent;
        }

        function abrirToolTip(TipoRolConcepto) {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <span class='tituhori' >Usuarios involucrados para " + TipoRolConcepto + " concepto </span>");
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.limpiarErrores = function (errores) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.remove("ico-advertencia");
            }

        }

        vm.notificacionValidacionPadre = function (errores) {

            vm.limpiarErrores(errores);
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var isValid = true;
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == null ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            var nameArr = p.Error.split('-');
                            var TipoError = nameArr[0].toString();
                            if (TipoError == 'SGRVDP1') {
                                vm.validarErrores2(p.Descripcion, false, "A");
                            }
                            else
                                if (TipoError == 'SGRVDP2') {
                                    vm.validarErrores2(p.Descripcion, false, "B");
                                }
                        });
                        var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
                        if (idSpanAlertComponent != undefined) {
                            idSpanAlertComponent.classList.add("ico-advertencia");
                        }
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.errores = {

        }

        /* ------------------------ FIN Validaciones ---------------------------------*/
    }

    angular.module('backbone').component('usuariosInvolucradosFormularioSgr', {
        restrict: 'E',
        transclude: true,
        scope: {
            modelo: '='
        },
        templateUrl: "/src/app/formulario/ventanas/SGR/comun/usuariosInvolucrados/usuariosInvolucradosFormularioSgr.html",
        controller: usuariosInvolucradosFormularioSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            nivelarchivo: '@',
            objetonegocioid: '@',
            tipoconceptoviabilidadid: '@',
            nombrecomponentepaso: '@',
            ProyectoId: '@',
        }
    }).directive('noNumbers', function () {
        return {
            restrict: 'A',
            link: function (scope, element) {
                // Bloquear escritura de números
                element.bind('keypress', function (event) {
                    var charCode = event.which || event.keyCode;
                    // Rango de códigos ASCII para números: 48–57
                    if (charCode >= 48 && charCode <= 57) {
                        event.preventDefault();
                    }
                });

                // Bloquear pegado si contiene números
                element.on('paste', function (event) {
                    // Obtener el texto pegado
                    var pastedText = (event.originalEvent || event).clipboardData.getData('text/plain');

                    // Verificar si contiene números
                    if (/\d/.test(pastedText)) {
                        // Prevenir el pegado si hay números
                        event.preventDefault();
                        return false;
                    }
                });
            }
        };
    });

})();