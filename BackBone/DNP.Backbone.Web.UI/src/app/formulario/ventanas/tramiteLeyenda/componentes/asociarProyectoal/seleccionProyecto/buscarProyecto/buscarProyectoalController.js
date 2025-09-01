(function () {
    'use strict';

    buscarProyectoalController.$inject = [
        '$sessionStorage',
        'sesionServicios',
        'utilidades',
        'tramiteVigenciaFuturaServicio',
        'flujoServicios',
        '$scope',
        'trasladosServicio',
        'justificacionCambiosServicio',
        'constantesBackbone',
        'modalActualizaEstadoAjusteProyectoServicio',
        '$location',
        'aclaracionLeyendaServicio',
    ];

    function buscarProyectoalController(
        $sessionStorage,
        sesionServicios,
        utilidades,
        tramiteVigenciaFuturaServicio,
        flujoServicios,
        $scope,
        trasladosServicio,
        justificacionCambiosServicio,
        constantesBackbone,
        modalActualizaEstadoAjusteProyectoServicio,
        $location,
        aclaracionLeyendaServicio,
    ) {

        var vm = this;
        vm.lang = "es";
        //vm.estadoAlertaTemplate = 'src/app/panelPrincial/modales/tramite/plantillaSeleccionar.html';
        vm.tramiteId = $sessionStorage.tramiteId;
        vm.numeroTramite = $sessionStorage.idObjetoNegocio;
        vm.IdEntidad = $sessionStorage.idEntidad;
        vm.textoBuscar = '';
        vm.checkSeleccionar = false;
        vm.estadoTramite = '';
        vm.estadoAjusteCreado = false;
        vm.proyectoAsociado = false;
        vm.proyectoId = 0;
        vm.datosproyecto = {};
        vm.listavalores = [];
        $sessionStorage.EstadoAsociacionVF = '';
        $sessionStorage.EstadoAjusteCreado = false;
        $sessionStorage.EstadoDNpAplicado = true;

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;
        //Para mostar los botones en el paso 3


        //Valida que este en el paso 3 para mostrar los botones
        vm.mostrar = $sessionStorage.idNivel.toUpperCase() === constantesBackbone.idNivelElaboracionConcepto
        vm.actualizaEstado = actualizaEstado;
        vm.conceptoHabilitado = false;
        vm.valorHabilitado = false;
        vm.habilitaBotones = vm.habilitaBotones = $sessionStorage.nombreAccion.includes('Asociar Proyectos') && !$sessionStorage.soloLectura ? true : false;

        
        vm.tituloMensaje = "";
        vm.mensaje = ""

        /*variables configuación grids */

        vm.gridProyectos = {};
        vm.scrollbars = {
            NEVER: 0,
            ALWAYS: 1,
            WHEN_NEEDED: 2
        }
        vm.columnDefProyectos = [
            {
                field: 'BPIN',
                displayName: 'Código BPIN ',
                enableHiding: false,
                minWidth: 200,
                cellTemplate: '<div class="text-left"><label style="font-weight: 400; font-size: 16px; font-style: normal;">{{row.entity.BPIN}} </label></div > ',
            },
            {
                field: 'NombreProyecto',
                displayName: 'Nombre del Proyecto',
                enableHiding: false,
                minWidth: 800,
                with: 1000,
                cellTemplate: '<div style="text-align: left; overflow-y: scroll; height: 60px"><label style="font-weight: 400; font-size: 16px; font-style: normal;">{{row.entity.NombreProyecto}} </label></div > ',

            },
            {
                field: 'select',
                displayName: '',
                enableHiding: false,
                minWidth: 50,
                cellTemplate: '<div class="ui-grid-cell-contents ng-binding ng-scope"><input type="checkbox" ng-model="row.entity.select" ng-change="grid.appScope.cambiarSeleccion(row.entity.select, row.entity)" /></div>',
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
        /////////////////////////


        vm.inicializarBusqueda = inicializarBusqueda;
        function inicializarBusqueda() {

            trasladosServicio.obtenerDetallesTramite(vm.numeroTramite).then(function (result) {
                var x = result.data;
                if (x != null) {
                    $sessionStorage.tramiteId = x.TramiteId;
                    vm.tramiteId = $sessionStorage.tramiteId;
                    vm.cargarDatos(vm.tramiteId);
                }
            }, error => {
                console.log(error);
            });


            if (vm.proyectoId != 0)
                $sessionStorage.EstadoAsociacionVF = 'Con Asociación';
            else
                $sessionStorage.EstadoAsociacionVF = 'Sin Asociación';
            $sessionStorage.EstadoAjusteCreado = false;

           

            $scope.$watch(function () {
                if (vm.tramiteId != $sessionStorage.tramiteId) {
                    vm.tramiteId = $sessionStorage.tramiteId;
                    vm.cargarDatos(vm.tramiteId);
                }
                if ($sessionStorage.proyectoId === 'e') {
                    vm.tramiteId = '';
                    $sessionStorage.proyectoId = '';
                    vm.proyectoId = 0;
                    $sessionStorage.EstadoAsociacionVF = 'Sin Asociación';
                    $sessionStorage.EstadoAjusteCreado = false;
                    vm.proyectoAsociado = false;
                }
                vm.estadoTramite = $sessionStorage.EstadoAsociacionVF == "Diligenciamiento completado" ? "Con Asociación": $sessionStorage.EstadoAsociacionVF;
                vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
               

                return $sessionStorage;
            }, function (newVal, oldVal) {

            }, true);

            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            
            
            
        }

        vm.getTableHeightProyectos = function () {
            var rowHeight = 30;
            var headerHeight = 50;

            return {
                height: (((vm.datosProyectos.length + 1) * rowHeight + headerHeight) + 30) + "px"
            };
        }

        vm.abrirTooltip = function () {
            utilidades.mensajeInformacion('Se indica si el tramite de Aclaracion de leyenda, ya tiene un proyecto asociado.'
                + '<br>En caso de que no tenga asociación, puede buscar un proyecto y seguidamente asociarlo.'
                + '<br>Si no encuentra el proyecto, es porque no esta cumpliendo con las condiciones para poder ser asociado. '
                , false, "Asociar Proyecto")
        }

        vm.buscarProyecto = function () {
            vm.datosProyectos = [];
            var codBpinABuscar = vm.textoBuscar;

            //'202200000000068'
            aclaracionLeyendaServicio.ObtenerProyectoAsociacion(codBpinABuscar, vm.tramiteId).then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    vm.gridProyectos.columnDefs = [];

                    if (response.data.length == 0) {
                        parent.postMessage("cerrarModal", window.location.origin);
                        utilidades.mensajeWarning("El proyecto buscado no se encuentra o no cumple con las reglas de validación para asociar al trámite de Aclaración de Leyenda.", true, false, false, false, 'No se encontro el proyecto');
                    }
                    else {
                        response.data.forEach(archivo => {

                            vm.datosProyectos.push({
                                ProyectoId: archivo.ProyectoId,
                                BPIN: archivo.BPIN,
                                NombreProyecto: archivo.NombreProyecto,
                                select: false,
                                PeriodoProyectoId: archivo.PeriodoProyectoId,
                                Accion: archivo.Accion,
                                TipoProyecto: archivo.TipoProyecto,
                                TramiteId: vm.tramiteId,
                                EntidadId: vm.IdEntidad
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
                    aclaracionLeyendaServicio.asociarProyecto(x).then(function (response) {
                        if (response.data == 'True') {
                            $sessionStorage.proyectoId = x.ProyectoId;
                            vm.tramiteId = '';
                            $sessionStorage.EstadoAsociacionVF = 'Con Asociación';
                            $sessionStorage.EstadoAjusteCreado = false;
                            vm.proyectoAsociado = true;
                            vm.datosProyectos = [];
                            vm.gridProyectos.data = [];
                            parent.postMessage("cerrarModal", window.location.origin);
                            utilidades.mensajeSuccess("", false, false, false, "El proyecto ha sido asociado y guardado con éxito.");
                            //para guardar los capitulos modificados y que se llenen las lunas
                            guardarCapituloModificado();
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

        vm.cambiarSeleccion = function (valor, fila) {
            if (valor) {
                var posicion = 0;
                vm.datosProyectos.forEach(x => {
                    if (x.ProyectoId != fila.ProyectoId) {
                        vm.datosProyectos[posicion].select = false;
                    }
                    posicion++;
                });
            }

        }

        vm.cargarDatos = function (tramiteId) {
            if (tramiteId !== undefined) {
                aclaracionLeyendaServicio.obtenerDatosProyectoTramite(tramiteId).then(
                    function (respuesta) {
                        if (respuesta.data.ProyectoId != 0) {
                            $sessionStorage.proyectoId = respuesta.data.ProyectoId;
                            $sessionStorage.BPIN = respuesta.data.BPIN;
                            
                            vm.proyectoId = $sessionStorage.proyectoId;
                            vm.proyectoAsociado = true;
                            $sessionStorage.EstadoAsociacionVF = 'Con Asociación';
                            $sessionStorage.EstadoAjusteCreado = true;
                            vm.datosproyecto = respuesta.data;
                            $sessionStorage.anioFinalTramite = vm.datosproyecto.VigenciaFinal;
                            $sessionStorage.FechaFinal = vm.datosproyecto.FechaFinal
                            vm.estadoTramite = $sessionStorage.EstadoAsociacionVF;
                            vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                            var listaValores = [
                                {
                                    TipoRecurso: "Propios",
                                    AprobacionInicial: new Intl.NumberFormat().format(vm.datosproyecto.ValorInicialPropios),
                                    AprobacionVigente: new Intl.NumberFormat().format(vm.datosproyecto.ValorVigentePropios),
                                    ValorDispinible: new Intl.NumberFormat().format(vm.datosproyecto.ValorDisponiblePropios),
                                    VigenciasFuturas: new Intl.NumberFormat().format(vm.datosproyecto.ValorVigenciaFuturaPropios),

                                },
                                {
                                    TipoRecurso: "Nación",
                                    AprobacionInicial: new Intl.NumberFormat().format(vm.datosproyecto.ValorInicialNacion),
                                    AprobacionVigente: new Intl.NumberFormat().format(vm.datosproyecto.ValorVigenteNacion),
                                    ValorDispinible: new Intl.NumberFormat().format(vm.datosproyecto.ValorDisponibleNacion),
                                    VigenciasFuturas: new Intl.NumberFormat().format(vm.datosproyecto.ValorVigenciaFuturaNacion),

                                }
                            ]

                            vm.datosproyecto.listavalores = listaValores;
                        }
                    }
                ).then(function () {
                    var listaproyectos = [];
                    listaproyectos.push($sessionStorage.BPIN);
                    trasladosServicio.obtenerInstanciasActivasProyectos(listaproyectos)
                        .then(function (resultado) {
                            var listaqueyatiene = resultado.data;
                            if (listaqueyatiene.length > 0) {
                                $sessionStorage.EstadoAsociacionVF = 'En Verificación';
                                $sessionStorage.EstadoAjusteCreado = true;
                            }
                        }).then(function () {
                            vm.estadoTramite = $sessionStorage.EstadoAsociacionVF;
                            vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                            if (vm.estadoAjusteCreado) {
                                obtenerEstadoInstanciaProyecto();
                            }
                        });

                });
            }
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
        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-asociarproyectoseleccionproyec');
            vm.seccionCapitulo = span.textContent;


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








    angular.module('backbone').component('buscarProyectoal', {
        templateUrl: "src/app/formulario/ventanas/tramiteLeyenda/componentes/asociarProyectoal/seleccionProyecto/buscarProyecto/buscarProyectoal.html",
        controller: buscarProyectoalController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            deshabilitarBotonDevolverSeccionProyecto: '&',
            tramiteid:'@',
        }
    });

})();