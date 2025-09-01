(function () {
    'use strict';

    seleccionarVigenciasController.$inject = [
        '$sessionStorage',
        'sesionServicios',
        'utilidades',
        'flujoServicios',
        'trasladosServicio',
        'constantesBackbone',
        'justificacionCambiosServicio',
        '$scope',
        'tramiteLiberacionServicio'
    ];

    function seleccionarVigenciasController(
        $sessionStorage,
        sesionServicios,
        utilidades,
        flujoServicios,
        trasladosServicio,
        constantesBackbone,
        justificacionCambiosServicio,
        $scope,
        tramiteLiberacionServicio
    ) {
        var vm = this;
        vm.lang = "es";

        vm.IdEntidad = $sessionStorage.idEntidad;
        vm.textoBuscar = '';
        vm.checkSeleccionar = false;
        vm.estadoTramite = '';
        vm.estadoAjusteCreado = false;
        vm.tramiteAsociado = false;
        vm.proyectoId = 0;
        vm.datosproyecto = {};
        vm.listavalores = [];
        $sessionStorage.EstadoAsociacionVF = '';
        $sessionStorage.EstadoDNpAplicado = true;
        vm.estad = "'Con vigencia futura asociada'";
        vm.sinestad = "'Sin vigencia futura asociada'";
        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;
        //Para mostar los botones en el paso 3


        //Valida que este en el paso 3 para mostrar los botones
        vm.mostrar = $sessionStorage.idNivel.toUpperCase() === constantesBackbone.idNivelElaboracionConcepto;
        vm.solopaso1 = !$sessionStorage.soloLectura;// habilita solo en paso 1
        vm.actualizaEstado = actualizaEstado;
        vm.conceptoHabilitado = false;
        vm.valorHabilitado = false;


        vm.tituloMensaje = "";
        vm.mensaje = ""

        /*variables configuación grids */

        vm.gridProyectos = {};
        vm.gridProyectos2 = {};
        vm.scrollbars = {
            NEVER: 0,
            ALWAYS: 1,
            WHEN_NEEDED: 2
        }
        vm.columnDefProyectos = [
            {
                field: 'CodProceso',
                displayName: 'Código del proceso',
                enableHiding: false,
                minWidth: 220,
                cellTemplate: '<div class="text-left"><label style="font-weight: 400; font-size: 16px; font-style: normal;">{{row.entity.CodProceso}} </label></div > ',
            },
            {
                field: 'NombreProceso',
                displayName: 'Nombre del Proceso',
                enableHiding: false,
                minWidth: 330,
                cellTemplate: '<div class="text-left"><label style="font-weight: 400; font-size: 16px; font-style: normal;">{{row.entity.NombreProceso}} </label></div > ',

            },
            {
                field: 'Fecha',
                displayName: 'Fecha',
                enableHiding: false,
                minWidth: 180,
                cellTemplate: '<div class="text-left"><label style="font-weight: 400; font-size: 16px; font-style: normal;">{{row.entity.fecha | date:"dd/MM/yyyy"}} </label></div > ',
            },
            {
                field: 'objContratacion',
                displayName: 'Objeto de la contratación',
                enableHiding: false,
                minWidth: 400,
                cellTemplate: '<div style="text-align: left; overflow-y: scroll; height: 60px"><label style="font-weight: 400; font-size: 16px; font-style: normal;">{{row.entity.objContratacion}} </label></div > ',
            },
            {
                field: 'select',
                displayName: 'Seleccionar',
                enableHiding: false,
                minWidth: 50,
                cellTemplate: '<div class="ui-grid-cell-contents ng-binding ng-scope">' +
                    '<input ng-if="!row.entity.asociado && grid.appScope.solopaso1" type="checkbox" ng-model="row.entity.select" ng-change="grid.appScope.cambiarSeleccion(row.entity.select, row.entity)" />' +
                    '<button ng-if="row.entity.asociado && grid.appScope.solopaso1" class="btnaccion" tooltip-class="customClass" uib-tooltip="Eliminar Registro" ng-click="grid.appScope.eliminarVigencia(row.entity)"><span> <img src="Img/IcoDelete.svg"></span></button> '+
                               '</div>',
            },
        ];
        

        vm.datosProyectos = [];
        ///////////////////

        /*Carga grids */

        vm.gridProyectos = {
            enableSorting: true,
            enableRowSelection: false,
            enableFullRowSelection: false,
            selectionRowHeaderWidth: 35,
            rowHeight: 20,
            multiSelect: false,
            enableRowHeaderSelection: false,
            enableColumnMenus: false,
            columnDefs: vm.columnDefProyectos,
            showHeader: false,
            data: vm.datosProyectos
        };

        vm.gridProyectos.appScopeProvider = vm;
        vm.gridProyectos.onRegisterApi = function (gridApi) {
            vm.gridApi = gridApi;
        };
        //grid2
        vm.gridProyectos2 = {
            enableSorting: true,
            enableRowSelection: false,
            enableFullRowSelection: false,
            selectionRowHeaderWidth: 35,
            rowHeight: 20,
            multiSelect: false,
            enableRowHeaderSelection: false,
            enableColumnMenus: false,
            columnDefs: vm.columnDefProyectos,
            showHeader: false,
            data: vm.datosProyectos
        };

        vm.gridProyectos2.appScopeProvider = vm;
        vm.gridProyectos2.onRegisterApi = function (gridApi) {
            vm.gridApi = gridApi;
        };
        /////////////////////////

        vm.init = function () {

            $scope.$watch(() => vm.tramiteproyectoid
                , (newVal, oldVal) => {
                    if (newVal != 0 && newVal != undefined) {
                        vm.buscarProyecto();
                        $sessionStorage.EstadoAsociacionVF = 'Con Asociación';
                        //vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                    }
                    else
                        $sessionStorage.EstadoAsociacionVF = 'Sin Asociación';
                     
                    
                }, true);

            
        }

        vm.getTableHeightProyectos = function () {
            var rowHeight = 30;
            var headerHeight = 50;

            return {
                height: (((vm.datosProyectos.length + 1) * rowHeight + headerHeight) + 30) + "px"
            };
        }

        vm.abrirTooltip = function () {
            utilidades.mensajeInformacion('Se indica si para la liberacion, ya tiene un proyecto asociado.'
                + '<br>En caso de que no tenga asociación, puede buscar un proyecto y seguidamente asociarlo.'
                + '<br>Si no encuentra el proyecto, es porque no esta cumpliendo con las condiciones para poder ser asociado. '
                , false, "Asociar Proyecto")
        }

        vm.buscarProyecto = function () {
            vm.datosProyectos = [];
            var numTramiteABuscar = $sessionStorage.proyectoId;//vm.textoBuscar;

            tramiteLiberacionServicio.ObtenerTramitesVFparaLiberar(numTramiteABuscar).then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    vm.gridProyectos.columnDefs = [];

                    if (response.data.length == 0) {
                        parent.postMessage("cerrarModal", window.location.origin);
                        utilidades.mensajeWarning("No se encontraron proyectos que: Posean Autorizaciones de Vigencias Futuras para ser liberadas.", true, false, false, false, 'No se encontro el tramite');
                    }
                    else {
                        response.data.forEach(archivo => {

                            if (archivo.NumeroTramite.split('*').length > 1) {
                                vm.tramiteAsociado = true;
                                vm.estadoAjusteCreado = true;
                                vm.estadoTramite = 'Con vigencia futura asociada';
                            }
                            else {
                                vm.tramiteAsociado = false;
                                vm.estadoAjusteCreado = false;
                                vm.estadoTramite = 'Sin vigencia futura asociada';
                            }
                            $sessionStorage.tramiteAsociado = vm.tramiteAsociado;

                            /*vm.tramiteProyectoId = archivo*/
                            vm.datosProyectos.push({
                                tramiteId: archivo.Id,
                                CodProceso: archivo.NumeroTramite,
                                NombreProceso: archivo.Descripcion,
                                fecha: archivo.fecha,
                                objContratacion: archivo.ObjContratacion,
                                tipotramiteId: archivo.tipotramiteId,
                                select: vm.tramiteAsociado,
                                asociado: vm.tramiteAsociado
                            });
                            
                        });
                        vm.textoBuscar = '';
                        vm.gridProyectos.showHeader = false;
                        vm.gridProyectos.columnDefs = vm.columnDefProyectos;
                        vm.gridProyectos.data = vm.datosProyectos;
                    }
                }
            }, error => {
                console.log(error);
            });
        }

        vm.crearAsociacion = function () {
            var posicion = 0;
            
            vm.datosProyectos.forEach(x => {
                if (x.select) {
                    var libDto = { tramiteProyectoId: vm.tramiteproyectoid, tramiteId: x.tramiteId };
                    tramiteLiberacionServicio.GuardarLiberacionVigenciaFutura(libDto).then(function (response) {
                        if (response.data == "OK") {
                            
                            vm.tramiteAsociado = true;
                            vm.datosProyectos = [];
                            vm.gridProyectos.data = [];
                            parent.postMessage("cerrarModal", window.location.origin);
                            utilidades.mensajeSuccess("", false, false, false, "El tramite ha sido asociado y guardado con éxito.");
                            //para guardar los capitulos modificados y que se llenen las lunas
                            guardarCapituloModificado();

                            crearInstancia(x.tramiteId);

                            vm.buscarProyecto();
                        }
                        else {
                            swal('', response.data, 'error');
                        }
                        vm.estadoTramite = $sessionStorage.EstadoAsociacionVF;
                        vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                    }, error => {
                        console.log(error);
                    });
                    vm.datosProyectos[posicion].select = false;
                }
                posicion++;
            });
        }

        function crearInstancia (tramite_Id) {
            var usuarioRolId = sesionServicios.obtenerUsuarioIdsRoles()[0];
            const tramiteDto = {
                FlujoId: $sessionStorage.idFlujoIframe,
                ObjetoId: tramite_Id,
                UsuarioId: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                RolId: usuarioRolId,
                TipoObjetoId: '9C5EF8C1-DA05-48B9-BA29-00C9EFD7A774',  //tramite
                ListaEntidades: [$sessionStorage.idEntidad],
                IdInstancia: $sessionStorage.idInstancia,
                //Proyectos: [{
                //    IdInstancia: $sessionStorage.idInstancia,
                //    IdTipoObjeto: '9C5EF8C1-DA05-48B9-BA29-00C9EFD7A774',
                //    IdObjetoNegocio: tramite_Id,
                //    IdEntidad: $sessionStorage.idEntidad,
                //    IdAccion: $sessionStorage.idAccion,
                //    ProyectoId: $sessionStorage.proyectoId,
                //    FlujoId: $sessionStorage.idFlujoIframe
                //}]
            };

            flujoServicios.generarInstancia(tramiteDto)
                .then(function (resultado) {
                    if (resultado.length <= 0 || (resultado[0].MensajeOperacion !== undefined && resultado[0].Exitoso === false)) {
                        utilidades.mensajeError('No se creó el ajuste. ' + resultado[0].MensajeOperacion);
                        return;
                    }
                    var instanciasFallidas = resultado.filter(function (instancia) {
                        return !instancia.Exitoso;
                    });
                    var cantidadInstanciasFallidas = instanciasFallidas.lenght;

                    if (cantidadInstanciasFallidas) {
                        utilidades.mensajeError('Se crearon ' + (resultado.length - cantidadInstanciasFallidas).toString() + ' instancias de ' + resultado.length.toString());
                    }

                });
        }

        vm.cambiarSeleccion = function (valor, fila) {

            if (valor) {
                var posicion = 0;
                vm.datosProyectos.forEach(x => {
                    if (x.tramiteId != fila.tramiteId) {
                        vm.datosProyectos[posicion].select = false;
                    }
                    posicion++;
                });
            }
        }

        vm.eliminarVigencia = function (fila) {
            if ($sessionStorage.sessionDocumentos === 100) {
                utilidades.mensajeError('Para eliminar la vigencia futura, primero elimine los documentos asociados al proceso.');
            }
            else {
                if (fila === undefined || fila === '' || fila === 0) {
                    utilidades.mensajeError('No ha seleccionado Vigencia Futura.');
                }
                else {
                    utilidades.mensajeWarning("¿Esta seguro de continuar?",
                        function funcionContinuar() {
                            var tramiteEliminarDto = {
                                tramiteId: fila.tramiteId,
                                tramiteProyectoId: 0
                            };

                            tramiteLiberacionServicio.EliminarLiberacionVigenciaFutura(tramiteEliminarDto)
                                .then(function (response) {
                                    if (response.status == "200") {
                                        vm.actualizadetalle++;
                                        utilidades.mensajeSuccess("", false, false, false, "La vigencia futura ha sido eliminada de la tabla con éxito.");

                                        var proyAdelete = vm.datosProyectos.find(x => x.tramiteId === fila.tramiteId);
                                        vm.datosProyectos.splice(proyAdelete, 1);

                                        vm.gridProyectos.columnDefs = vm.columnDefProyectos;
                                        vm.gridProyectos.data = vm.datosProyectos;

                                        vm.buscarProyecto();

                                        vm.tramiteAsociado = false;
                                        $sessionStorage.tramiteAsociado = vm.tramiteAsociado;

                                        if (vm.datosProyectos.length === 0)
                                            eliminarCapitulosModificados();
                                    }
                                })
                                .catch(error => {
                                    if (error.status == "400") {
                                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                                        return;
                                    }
                                    utilidades.mensajeError("Error al realizar la operación");
                                });
                        },
                        function funcionCancelar(reason) {
                            console.log("reason", reason);
                        },
                        "Aceptar",
                        "Cancelar",
                        "La vigencia futura será eliminada de la tabla."
                    )
                }
            }
        }

        vm.cancelar = function () {

        }


        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
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
            const span = document.getElementById('id-capitulo-selecionarvigenciafuturaseleccionarvigenciasfuturas');
            vm.seccionCapitulo = span.textContent;
            vm.nombreComponente = 'selecionarvigenciafuturaseleccionarvigenciasfuturas';

        }

        vm.guardado = function (nombreComponenteHijo) {
            vm.callback();
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        }
        //Para mostar los botones en el paso 3
        function actualizaEstado(ajuste) {
            vm.ajuste = ajuste;
            let tituloMensaje = "", mensaje = "";
            vm.model.observacion = "";
            if (ajuste === 0) {
                tituloMensaje = "El proyecto se devolverá a Ajuste del proyecto para ampliar concepto";
                mensaje = "La ampliación del concepto se reflejá en la pestaña Justificación de este formulario, cuando se responda a esta solicitud desde el formulario Ajuste del proyecto.<br/>¿Está seguro de continuar? "
            }
            else {
                tituloMensaje = "El proyecto se devolverá al paso 1 Crear Trámite.<br/> El proyecto se devolvera a control posterior";
                mensaje = "Este formulario desaparecerá  de sus procesos pendientes hasta  que reciba  nuevamente  los permisos de edición.<br/>¿Está seguro de continuar? "
            }

            utilidades.mensajeWarning(
                mensaje,
                function funcionContinuar() {
                    if (ajuste === 0) {

                        vm.tituloMensaje = "Solicitud para ampliar concepto";
                        vm.mensaje = "Escriba sus razones de devolver el formularío para ampliar concepto* ";
                    }
                    else {

                        vm.tituloMensaje = "Solicitud para ajustar valores";
                        vm.mensaje = "Escriba sus razones de devolver el formularío para ajustar valores*";
                    }
                    angular.element('#IPModal').modal('show');




                },
                function funcionCancelar(reason) {
                    console.log("reason", reason);
                },
                "Aceptar",
                "Cancelar",
                tituloMensaje);
        }

    }

    angular.module('backbone').component('seleccionarVigencias', {
        templateUrl: "src/app/formulario/ventanas/tramiteLiberacion/componentes/seleccionarVigencias/seleccionarVigencias.html",
        controller: seleccionarVigenciasController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            tramiteid: '@',
            tipotramiteid: '@',
            tramiteproyectoid: '@',
            deshabilitarBotonDevolverAsociarProyectoVF: '&',
            notificacioncambios: '&',
            deshabilitarBotonDevolverSeccionProyecto: '&',
        }
    });

})();