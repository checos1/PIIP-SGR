(function () {
    'use strict';

    agregarCdpProyectoSgp.$inject = [
        '$scope',
        'utilidades',
        'tramiteSGPServicio',
        '$uibModal',
        'cargaMasivaCdpSgpService',
        '$sessionStorage',
        'justificacionCambiosServicio'
    ];

    function agregarCdpProyectoSgp(
        $scope,
        utilidades,
        tramiteSGPServicio,
        $uibModal,
        cargaMasivaCdpSgpService,
        $sessionStorage,
        justificacionCambiosServicio
    ) {
        var vm = this;
        vm.cdpList = [];
        vm.cdpListError = [];
        vm.cdpListRaw = [];
        vm.archivoCargaMasivaValido = false;
        vm.archivoErrorList = [];
        vm.nombreComponente = 'informacionpresupuestalsgpactoadmtramitesgp';

        vm.init = function () {
            vm.setEvents();
        }
        vm.habilitaBotones = $sessionStorage.nombreAccion.includes('Asociar Proyectos') ? true : false;// habilita solo en paso 1
        vm.habilitaBotones = true;
        vm.soloLectura = false;
        //vm.habilitaBotones = $sessionStorage.nombreAccion.includes('Asociar Proyectos') && !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1

        //Declara los observables sobre los parametros del entrada del componente
        vm.setEvents = function () {
            $scope.$watch('vm.proyectoid', function () {
                if (vm.proyectoid !== '') {
                    vm.getCdpExistentesPorProyecto();
                }
            });
            $scope.$watch('vm.errores', function () {
                if (vm.errores != undefined && vm.errores !== null && vm.errores !== '' && vm.errores !== []) {
                    const errores = JSON.parse(vm.errores);
                    if (errores["informacionpresupuestalsgpactoadmtramitesgp"] != undefined && errores["informacionpresupuestalsgpactoadmtramitesgp"] != null &&
                        errores["informacionpresupuestalsgpactoadmtramitesgp"].length > 0) {
                        vm.cdpListError = errores["informacionpresupuestalsgpactoadmtramitesgp"].filter(p => p.proyectoid == vm.proyectoid);
                        vm.cdp002([...vm.cdpListError])
                    } else {
                        vm.limpiarErrores()
                    }
                }
            });
        }

        // Carga los actos administrativos existentes por proyecto id
        vm.getCdpExistentesPorProyecto = function () {
            vm.cdpList = [];
            vm.cdpListRaw = [];
            if (vm.tramiteid != undefined && vm.proyectoid != undefined) {
                tramiteSGPServicio.obtenerProyectoRequisitosPorTramite(vm.proyectoid, vm.tramiteid)
                    .then(function (response) {
                        if (response.status === 200) {
                            vm.convertirModeloACdp(response);
                            vm.soloLectura = $sessionStorage.soloLectura;
                        }
                    }, function (error) {
                        utilidades.mensajeError('No fue posible consultar los actos administrativos asociados al proyecto');
                    });
            }
        }

        // Mapea el listado de actos administrativos recibido desde el back a un objeto procesable desde el front
        vm.convertirModeloACdp = function (response) {
            vm.cdpListRaw = response.data;

            vm.cdpList = response.data.map((cdp) => {
                const listaValoresCdp = cdp.ListaTiposRequisito.find(w => w.Descripcion == 'Acto administrativo').ListaValores;
                const valorCdp = listaValoresCdp.find(w => w.TipoValor.TipoValorFuente == 'Valor Aporta');
                const valorTotalCdp = listaValoresCdp.find(w => w.TipoValor.TipoValorFuente == 'Valor');

                return {
                    Id: cdp.Id,
                    Descripcion: cdp.Descripcion,
                    FechaCDP: new Date(cdp.Fecha),
                    IdPresupuestoValoresCDP: 0,
                    IdPresupuestoValoresAportaCDP: 0,
                    IdProyectoRequisitoTramite: cdp.Id,
                    IdProyectoTramite: cdp.TramiteProyectoId,
                    IdTipoRequisito: cdp.TipoRequisitoId,
                    NumeroCDP: cdp.Numero,
                    NumeroContratoCDP: cdp.NumeroContrato,
                    NumeroContratoCDPcorto: cdp.NumeroContrato.substring(0, 78),
                    show: true,
                    Tipo: 3,
                    UnidadEjecutora: cdp.UnidadEjecutora,
                    ValorCDP: valorCdp.Valor,
                    ValorTotalCDP: valorTotalCdp.Valor,
                    IdValorTotalCDP: valorTotalCdp.TipoValor.Id,
                    IdValorAportaCDP: valorCdp.TipoValor.Id,
                    IdProyecto: vm.proyectoid,
                    IdTramite: vm.tramiteid,
                    IdTipoRol: vm.TipoRolId,
                    IdRol: vm.rolid,
                    
                }
            });
            console.log("vm.cdpList ",vm.cdpList)
            if (vm.cdpList.length > 0) {
                guardarCapituloModificado();
            }
            else {
                eliminarCapitulosModificados();
            }
        }

        // Lanza el modal de creación/ edición de actos administrativos
        vm.crearCdp = function (editarCdpId = 0) {
            var templateUrl = "src/app/formulario/ventanas/comun/certificadoDisponibilidadPresupuestalSgp/componentes/agregarCdpProyectoSgpModal/agregarCdpProyectoSgpModal.html";            
            try {
                let modalInstance = $uibModal.open({
                    templateUrl: templateUrl,
                    controller: 'agregarCdpProyectoSgpModal',
                    controllerAs: "vm",
                    openedClass: "modal-contentDNP",
                    resolve: {
                        proyectoId: function () {
                            return vm.proyectoid;
                        },
                        tramiteId: function () {
                            return vm.tramiteid;
                        },
                        rolId: function () {
                            return vm.rolid;
                        },
                        cdpList: function () {
                            return vm.cdpList;
                        },
                        editarCdpId: function () {
                            return editarCdpId === undefined ? 0 : editarCdpId
                        }
                    },
                });
                modalInstance.result.then(data => {
                    if (data != null) {
                        if (data == 'ok') {
                            vm.getCdpExistentesPorProyecto();
                        }
                    }
                });
            } catch (error) {
            }
        }

        vm.editarCDP = function (cdpId) {
            vm.crearCdp(cdpId);
        }

        // Remueve un actos administrativos del listado y actualiza la información de actos administrativos por proyecto
        vm.eliminarCdp = function (idCdp) {
            utilidades.mensajeWarning("Esto eliminará la información correspondiente al acto administrativo. ¿Esta seguro de continuar?",
                function funcionContinuar() {
                    let listaCdpFiltrada = [];
                    if (vm.cdpList.length === 1) {
                        listaCdpFiltrada = vm.cdpList;
                        listaCdpFiltrada[0]['Descripcion'] = 'BorrarTodo';
                    } else {
                        listaCdpFiltrada = vm.cdpList.filter(w => w.Id !== idCdp);
                    }
                    vm.actualizaListadoCdp(listaCdpFiltrada, true);
                },
                function funcionCancelar(reason) {
                    console.log("reason", reason);
                },
                "Aceptar",
                "Cancelar",
                "El acto administrativo será eliminado."
            )
        }

        vm.actualizaListadoCdp = function (cdpList, esBorrado = false) {
            tramiteSGPServicio.actualizarTramitesRequisitos(cdpList)
                .then(function (response) {
                    let exito = response.data;
                    if (exito) {
                        vm.getCdpExistentesPorProyecto();
                        vm.removerArchivo();

                        if (esBorrado) {
                            utilidades.mensajeSuccess("Operación realizada con éxito");
                        }
                    }
                    else {
                        utilidades.mensajeError("Error al realizar la operación", false);
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

        // Carga la plantilla de ejemplo para carga masiva de actos administrativos
        vm.generarPlantillaEjemplo = function () {
            cargaMasivaCdpSgpService.generarDocumentoEjemploCdp();
        }

        vm.seleccionarArchivo = function () {
            vm.archivoCargaMasivaValido = false;
            vm.archivoErrorList = [];
        }

        vm.removerArchivo = function () {
            vm.archivoCargaMasivaValido = false;
            vm.archivoErrorList = [];
            vm.files = null;
        }

        vm.validarArchivo = function () {
            if (vm.files == null || vm.files[0] === undefined) {
                return;
            }

            cargaMasivaCdpSgpService.procesarArchivoCdp(vm.files[0])
                .then(function (response) {
                    const modelResponse = cargaMasivaCdpSgpService.validarArchivo(response, vm.cdpList);
                    vm.archivoCargaMasivaValido = modelResponse.isValid;
                    vm.archivoErrorList = modelResponse.errorList;
                }, function (onReject) {
                    vm.archivoCargaMasivaValido = false;
                    vm.archivoErrorList = [onReject];
                }
                )
        }

        vm.cargarArchivo = function () {
            if (!vm.archivoCargaMasivaValido || vm.files == null) {
                return;
            }

            cargaMasivaCdpSgpService.procesarArchivoCdp(vm.files[0])
                .then(function (response) {
                    const modelList = cargaMasivaCdpSgpService.convertirRespuestaAModelo(response, vm.tramiteid, vm.proyectoid, vm.rolid);
                    const newRequestModel = vm.cdpList.concat(modelList);
                    vm.actualizaListadoCdp(newRequestModel);
                }
                )
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
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

        //para guardar los capitulos modificados y que se llenen las lunas
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
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        /*--------------------- Validaciones ---------------------*/
        vm.limpiarErrores = function () {
            const listadoErroresContainer = document.getElementsByClassName("row-errores");

            for (var i = 0; i < listadoErroresContainer.length; i++) {
                if (!listadoErroresContainer[i].classList.contains("d-none")) {
                    listadoErroresContainer[i].classList.add("d-none");
                }
            }
        }

        vm.cdp002 = function (listErrores) {
            if (listErrores.length > 0) {
                listErrores.forEach(item => {
                    const el = document.getElementById("seccion-validacion-" + item.CdpId);
                    const elText = document.getElementById("seccion-txt-validacion-" + item.CdpId);
                    if (el != undefined && el != null && elText != undefined && elText != null) {
                        el.classList.remove('d-none');
                    }
                })
            }
        }
    }

    angular.module('backbone').component('agregarCdpProyectoSgp', {
        templateUrl: "src/app/formulario/ventanas/comun/certificadoDisponibilidadPresupuestalSgp/componentes/agregarCdpProyectoSgp/agregarCdpProyectoSgp.html",
        controller: agregarCdpProyectoSgp,
        controllerAs: "vm",
        bindings: {
            proyectoid: '@',
            tramiteid: '@',
            rolid: '@',
            errores: '<',
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&'
        }
    });
})();