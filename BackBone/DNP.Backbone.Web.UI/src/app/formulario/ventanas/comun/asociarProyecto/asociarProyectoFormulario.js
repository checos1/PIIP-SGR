(function () {
    'use strict';

    asociarProyectoFormulario.$inject = [
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
        'comunesServicio'
    ];

    function asociarProyectoFormulario(
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
        comunesServicio
    ) {

        var vm = this;
        vm.lang = "es";
        
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
        vm.instanciaId = $sessionStorage.idInstancia;
        vm.parametros = {
            idFlujo: $sessionStorage.idFlujoIframe,
            tipoEntidad: 'Nacional',
            idInstancia: vm.instanciaId,
            IdEntidad: vm.IdEntidad
        };

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;
        //Para mostar los botones en el paso 3


        //Valida que este en el paso 3 para mostrar los botones
        vm.mostrar = $sessionStorage.idNivel.toUpperCase() === constantesBackbone.idNivelElaboracionConcepto
        vm.actualizaEstado = actualizaEstado;
        vm.conceptoHabilitado = false;
        vm.valorHabilitado = false;


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


        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

            }
        });

        vm.init = function () {

            $scope.$watch(() => vm.tramiteid
                , (newVal, oldVal) => {
                    if (newVal) {
                        vm.cargarDatos(vm.tramiteid);
                    }
                }, true);

            if (vm.proyectoId != 0) {
                $sessionStorage.EstadoAsociacionVF = 'Con Asociación';
                $sessionStorage.EstadoAjusteCreado = true;
            }
            else {
                $sessionStorage.EstadoAsociacionVF = 'Sin Asociación';
                $sessionStorage.EstadoAjusteCreado = false;
            }
            vm.estadoTramite = $sessionStorage.EstadoAsociacionVF;
            vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
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
            var codBpinABuscar = vm.textoBuscar;

            comunesServicio.ObtenerProyectoAsociacion(codBpinABuscar, vm.tramiteid).then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    vm.gridProyectos.columnDefs = [];

                    if (response.data.length == 0) {
                        parent.postMessage("cerrarModal", window.location.origin);
                        utilidades.mensajeWarning("El proyecto buscado no se encuentra o no cumple con las reglas de validación para asociar.", true, false, false, false, 'No se encontro el proyecto');
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
                                TramiteId: vm.tramiteid,
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
                    comunesServicio.asociarProyecto(x).then(function (response) {
                        if (response.data == 'True') {
                            $sessionStorage.proyectoId = x.ProyectoId;
                            vm.tramiteId = '';
                            $sessionStorage.EstadoAsociacionVF = 'Con Asociación';
                            $sessionStorage.EstadoAjusteCreado = true;
                            vm.estadoTramite = $sessionStorage.EstadoAsociacionVF;
                            vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                            vm.proyectoAsociado = true;
                            vm.datosProyectos = [];
                            vm.gridProyectos.data = [];
                            parent.postMessage("cerrarModal", window.location.origin);
                            utilidades.mensajeSuccess("", false, false, false, "El proyecto ha sido asociado y guardado con éxito.");
                            //para guardar los capitulos modificados y que se llenen las lunas
                            guardarCapituloModificado();
                            vm.cargarDatos(vm.tramiteid);
                            var usuarioRolId = sesionServicios.obtenerUsuarioIdsRoles()[0];
                            const tramiteDto = {
                                FlujoId: vm.parametros.idFlujo,
                                ObjetoId: x.BPIN,
                                UsuarioId: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                                RolId: usuarioRolId,
                                TipoObjetoId: 'bc154cba-50a5-4209-81ce-7c0ff0aec2ce',  //proyecto
                                ListaEntidades: [vm.IdEntidad],
                                IdInstancia: vm.instanciaId,
                                Proyectos: [{
                                    IdInstancia: vm.instanciaId,
                                    IdTipoObjeto: 'bc154cba-50a5-4209-81ce-7c0ff0aec2ce',
                                    IdObjetoNegocio: x.BPIN,
                                    IdEntidad: vm.IdEntidad,
                                    IdAccion: $sessionStorage.idAccion,
                                    ProyectoId: $sessionStorage.proyectoId,
                                    FlujoId: vm.parametros.idFlujo
                                }]
                            };


                        }
                        else {
                            swal('', response.data, 'error');
                        }
                       
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

                            vm.proyectoId = respuesta.data.ProyectoId;
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
            if (vm.seccionCapitulo !== 'NO') { 
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
        }
        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            try {
                const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
                vm.seccionCapitulo = span.textContent;
            }
            catch {
                vm.seccionCapitulo = 'NO';
            }

        }

        vm.guardado = function (nombreComponenteHijo) {
            vm.guardadoevent({ nombreComponenteHijo: nombreComponenteHijo });

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


        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores(errores);
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                if (erroresJson != undefined) {
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.limpiarErrores = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }



        }

        vm.validarAsociarProyectos = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>"+ errores  +"</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }
        vm.errores = {
            'VFO004': vm.validarAsociarProyectos,

        }

    }








    angular.module('backbone').component('asociarProyectoFormulario', {
        templateUrl: "src/app/formulario/ventanas/comun/asociarProyecto/asociarProyectoFormulario.html",
        controller: asociarProyectoFormulario,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            deshabilitarBotonDevolverSeccionProyecto: '&',
            tramiteid: '@',
            tipotramiteid: '@',
            nombrecomponentepaso: '@',
            notificacionvalidacion: '&',
            notificacionestado: '&',
        }
    });

})();