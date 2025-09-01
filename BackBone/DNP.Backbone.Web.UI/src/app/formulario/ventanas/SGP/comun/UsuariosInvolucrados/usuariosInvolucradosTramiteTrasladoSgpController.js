(function () {
    'use strict';

    usuariosInvolucradosTramiteTrasladoSgpController.$inject = [
        '$scope',
        'usuariosInvolucradosSgpServicio',
        '$sessionStorage',
        'utilidades',
        '$timeout',
        'justificacionCambiosServicio',
        '$uibModal',
    ];

    function usuariosInvolucradosTramiteTrasladoSgpController(
        $scope,
        usuariosInvolucradosSgpServicio,
        $sessionStorage,
        utilidades,
        $timeout,
        justificacionCambiosServicio,
        $uibModal,
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

        vm.clickIconoEditar = clickIconoEditar;
        vm.clickIconoGuardar = clickIconoGuardar;
        vm.clickIconoCancelar = clickIconoCancelar;
        vm.clickIconoEliminar = clickIconoEliminar;
        vm.abrirModalAgregarInvolucrado = abrirModalAgregarInvolucrado;
        vm.abrirToolTip = abrirToolTip;

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        //variables locales
        vm.TipoRolesConcepto = [];
        vm.involucrados = [];

        ////#region Metodos

        vm.init = function () {
            consultar();
        };

        $scope.$watch('vm.involucrados', function () {
            if (vm.involucrados.length > 0) {
                vm.TipoRolesConcepto = [...new Map(vm.involucrados.map((item) => [item["TipoRolConceptoId"], item])).values()];
            }
        });

        function consultar() { 
            ObtenerProyectoViabilidadInvolucrados($sessionStorage.InstanciaSeleccionada.tramiteId, vm.IdInstancia, 6);
        }

        function ObtenerProyectoViabilidadInvolucrados(proyectoId, instanciaId, tipoConceptoViabilidadId) {
            return usuariosInvolucradosSgpServicio.ObtenerProyectoViabilidadInvolucrados(proyectoId, instanciaId, tipoConceptoViabilidadId).then(
                function (involucrados) {
                    vm.involucrados = involucrados.data;
                    vm.disabled = $sessionStorage.soloLectura;

                    EstablecerPropiedadesAdicionales();
                }
            );
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

                    usuariosInvolucradosSgpServicio.EliminarProyectoViabilidadInvolucrados(Id).then(function () {
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
            involucrado.ProyectoId = $sessionStorage.InstanciaSeleccionada.tramiteId;
            if (!validar(involucrado)) { return; }
            return usuariosInvolucradosSgpServicio.guardarProyectoViabilidadInvolucrados(involucrado)
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

            var data = {
                TipoConceptoViabilidadId: 6,
                TipoRolConceptoId: TipoRolConcepto.TipoRolConceptoId,
                ProyectoId: $sessionStorage.tramiteId,
                InvolucradosActuales: vm.involucrados.filter(x => x.Nombre !== null)
            }

            let modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: "/src/app/formulario/ventanas/SGP/comun/usuariosInvolucrados/modalAgregarInvolucradosTramiteTrasladoSgp.html",
                controller: 'modalAgregarInvolucradosTramiteTrasladoSgpController',
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
                    vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                }
            });
        }

        //#region Metodos    

        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
                //ObtenerSeccionCapitulo();
            }
        });

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
                ProyectoId: $sessionStorage.tramiteId,
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
            if (TipoRolConcepto === "Elaboró") {
                utilidades.mensajeInformacionN('', null, "<span class='anttituhori' > ¿Qué es esto? </span><br /><br /> <span class='tituhori' >Elaboración de la ficha. <br />" +
                    "" + " </span>", "Se indica el nombre de la(s) persona(s) que participa(n) en el proceso y que se muestran en la ficha, en la sección de elaboración del documento.");
            }
            else {
                utilidades.mensajeInformacionN('', null, "<span class='anttituhori' > ¿Qué es esto? </span><br /><br /> <span class='tituhori' >Emisión de la ficha. <br />" +
                    "" + " </span>", "Se indica el nombre de la(s) persona(s) que participa(n) en el proceso y que se muestran en la ficha, en la sección de emisión del documento.");
            }
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.limpiarErrores = function (errores) {

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

    angular.module('backbone').component('usuariosInvolucradosTramiteTrasladoSgp', {
        restrict: 'E',
        transclude: true,
        scope: {
            modelo: '='
        },
        templateUrl: "/src/app/formulario/ventanas/SGP/comun/usuariosInvolucrados/usuariosInvolucradosTramiteTrasladoSgp.html",
        controller: usuariosInvolucradosTramiteTrasladoSgpController,
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
            tipoConceptoViabilidadId: '@',
            nombrecomponentepaso: '@',
            ProyectoId: '@',
        }
    });

})();