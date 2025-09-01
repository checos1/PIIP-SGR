(function () {
    'use strict';

    asociarMuchosUnTipoFormularioa.$inject = [
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

    function asociarMuchosUnTipoFormularioa(
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
        vm.actualizadetalle = 0;
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
        vm.habilitaBotones = $sessionStorage.nombreAccion.includes($sessionStorage.listadoAccionesTramite[0].Nombre) && !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1
        vm.instanciaId = $sessionStorage.idInstancia;
        vm.idFlujo = $sessionStorage.idFlujoIframe;
        vm.parametros = {
            idFlujo: $sessionStorage.idFlujoIframe,
            tipoEntidad: 'Nacional',
            idInstancia: vm.instanciaId,
            IdEntidad: vm.IdEntidad
        };
        vm.mostrarBuscar = false;
        vm.unsolotipooperacion = true;
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
                displayName: 'Código entidad / Nombre del Proyecto',
                enableHiding: false,
                minWidth: 700,
                with: 900,
                cellTemplate: '<div style="text-align: left; overflow-y: scroll; height: 60px"><label style="font-weight: 400; font-size: 16px; font-style: normal;">{{row.entity.NombreEntidad}} / {{row.entity.NombreProyecto}} </label></div > ',
            },
            {
                field: 'TipoProyecto',
                displayName: 'Estado / Fecha de ingreso',
                enableHiding: false,
                minWidth: 150,
                cellTemplate: '<div style="text-align: left; height: 60px"><div style="font-weight: 400; font-size: 16px; font-style: normal;">{{row.entity.TipoProyecto}} </div></div > ',
            },
            {
                field: 'select',
                displayName: '',
                enableHiding: false,
                minWidth: 50,
                cellTemplate: '<div class="ui-grid-cell-contents ng-binding ng-scope"><input type="checkbox" ng-model="row.entity.select" /></div>',
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
            data: vm.datosProyectos,
            excessRows: 10
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
            vm.mostrarBuscar = true;
            $scope.$watch(() => vm.tramiteid
                , (newVal, oldVal) => {
                    if (newVal) {
                        var tipoProyecto = 'D';
                        switch (vm.tipotramiteid) {
                            case "7"://Incorporaciones
                                tipoProyecto = 'D';
                            break;
                        }
                        vm.proyectoId = 0;
                        //ObtenerProyectos(vm.IdEntidad, tipoProyecto);
                        //ObtenerProyectos(vm.IdEntidad, vm.tipoaccion);
                    }
                    vm.actualizadetalle++;
                }, true);

            
            $scope.$watch(() => $sessionStorage.EstadoAsociacionVF
                , (newVal, oldVal) => {
                    //if ($sessionStorage.tipoaccion == vm.tipoaccion) {
                    //    vm.estadoTramite = $sessionStorage.EstadoAsociacionVF;
                    //    vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                    //}  
                    //if ($sessionStorage.EstadoAsociacionVF.Contains(vm.tipoproyecto)) {
                        
                    //    vm.estadoTramite = $sessionStorage.EstadoAsociacionVF.replace(vm.tipoproyecto, '');
                    //    vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                    //}  

                    if ($sessionStorage.EstadoAsociacionVF.indexOf(vm.tipoproyecto) != -1) {

                        vm.estadoTramite = $sessionStorage.EstadoAsociacionVF.replace(vm.tipoproyecto, '');
                        vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                    } 
                }, true);

            
        }

        vm.getTableHeightProyectos = function () {
            var rowHeight = 30;
            var headerHeight = 50;
            var height = 4 * (rowHeight + headerHeight) + 30;
            return {
                height: height + "px"
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
            var codBpinABuscar = vm.textoBuscar + vm.tipoconsulta;

            if (vm.textoBuscar == "") {
                utilidades.mensajeWarning("Ingrese un criterio de búsqueda.", true, false, false, false, 'Proyectos a buscar');
            }
            else {

                comunesServicio.ObtenerProyectoAsociacion(codBpinABuscar, vm.tramiteid).then(function (response) {
                    if (response === undefined || typeof response === 'string') {
                        vm.mensajeError = response;
                        utilidades.mensajeError(response);
                    } else {
                        vm.gridProyectos.columnDefs = [];

                        if (response.data.length == 0 && response.data !== null) {
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
                                    EntidadId: archivo.EntidadId,
                                    NombreEntidad: archivo.NombreEntidad
                                });


                                //// segun el tipotramite lista de los proyectos contracredito o credito
                                //var addTabla = false;
                                //switch (vm.tipotramiteid) {
                                //    case "7"://Incorporaciones
                                //        if (vm.tipoaccion == 'O') {
                                //            if (vm.listaProyectosC.find(p => p.BPIN === archivo.BPIN)) {
                                //                addTabla = true;
                                //            }
                                //        }
                                //        else {
                                //            if (vm.listaProyectosD.find(p => p.BPIN === archivo.BPIN))
                                //                addTabla = true;
                                //        }
                                //        break;
                                //}
                                //if (addTabla) {
                                //    vm.datosProyectos.push({
                                //        ProyectoId: archivo.ProyectoId,
                                //        BPIN: archivo.BPIN,
                                //        NombreProyecto: archivo.NombreProyecto,
                                //        select: false,
                                //        PeriodoProyectoId: archivo.PeriodoProyectoId,
                                //        Accion: archivo.Accion,
                                //        TipoProyecto: archivo.TipoProyecto,
                                //        TramiteId: vm.tramiteid,
                                //        EntidadId: archivo.EntidadId,
                                //        NombreEntidad: archivo.NombreEntidad
                                //    });
                                //}   
                            });
                            vm.textoBuscar = '';
                            vm.gridProyectos.showHeader = false;
                            vm.gridProyectos.columnDefs = vm.columnDefProyectos;
                            vm.gridProyectos.excessRows = vm.datosProyectos.length;
                            
                            vm.gridProyectos.data = vm.datosProyectos;
                        }
                    }
                }, error => {
                    console.log(error);
                });
            }            
        };

        function ObtenerProyectos(idEntidad, tipoProyecto) {
            if (idEntidad != null) {
                let listaProyectosGrid = [];
                var prm = {
                    idFlujo: vm.idFlujo,
                    tipoEntidad: 'a',
                    IdEntidad: idEntidad, idInstancia: vm.instanciaId
                };

                if (tipoProyecto == 'C' || tipoProyecto == 'O') {
                    comunesServicio.obtenerContraCreditos(prm)
                        .then(function (response) {
                            if (response.data !== null && response.data.length > 0) {
                                vm.listaProyectosC = response.data;
                                vm.mostrarBuscar = true;
                            }
                        });
                    return vm.listaProyectosC;
                }

                if (tipoProyecto == 'D') {
                    vm.listaProyectosD = [];
                    comunesServicio.obtenerCreditos(prm)
                        .then(function (response) {
                            if (response.data !== null && response.data.length > 0) {
                                vm.listaProyectosD = response.data;
                                vm.mostrarBuscar = true;
                            }
                        });
                    return vm.listaProyectosD;
                }
            }
        }


        vm.crearAsociacion = function () {
            let proyectos = [];
            vm.datosProyectos.forEach(x => {
                if (x.select) {
                    let c = {
                        ProyectoId: x.ProyectoId,
                        EntidadId: x.EntidadId,
                        //TipoProyecto: 'Credito',
                        TipoProyecto: vm.tipoproyecto,
                        NombreProyecto: x.NombreProyecto
                    };
                    proyectos.push(c);


                    //if (vm.tipotramiteid == 7) { //incorporacion   
                    //    if (vm.tipoaccion == 'O') {
                    //        var pc = vm.listaProyectosC.find(d => d.BPIN === x.BPIN);
                    //        let c = {
                    //            ProyectoId: pc.IdProyecto,
                    //            EntidadId: pc.IdEntidad,
                    //            //TipoProyecto: 'Credito',
                    //            TipoProyecto: vm.tipoproyecto,
                    //            NombreProyecto: pc.NombreProyecto
                    //        };
                    //        proyectos.push(c);
                    //    }
                    //    else {
                    //        var p = vm.listaProyectosD.find(d => d.BPIN === x.BPIN);
                    //        let c = {
                    //            ProyectoId: p.IdProyecto,
                    //            EntidadId: p.IdEntidad,
                    //            //TipoProyecto: 'Credito',
                    //            TipoProyecto: vm.tipoproyecto,
                    //            NombreProyecto: p.NombreProyecto
                    //        };
                    //        proyectos.push(c);
                    //    }
                        
                                              
                    //}                    
                }
            });

            vm.asociarProyecto(proyectos);           
        }

        vm.asociarProyecto = function (proyectos) {
            
            if (proyectos.length > 0) {

                var prm = {
                    TramiteId: vm.tramiteid,
                    Proyectos: proyectos
                };

                comunesServicio.guardarProyectos(prm)
                    .then(function (response) {
                        if (response.data && response.status == 200) {

                            if (response.data.Exito) {
                                parent.postMessage("cerrarModal", window.location.origin);
                                utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                                vm.actualizadetalle++;
                                $sessionStorage.EstadoAsociacionVF = 'Con Asociación';
                                $sessionStorage.EstadoAjusteCreado = true;
                                $sessionStorage.tipoaccion = vm.tipoaccion;
                                vm.textoBuscar = '';
                                vm.datosProyectos = [];
                                guardarCapituloModificado();

                            } else {
                                swal('', response.data.Mensaje, 'warning');
                            }
                        } else {
                            swal('', "Error al realizar la operación", 'error');
                        }
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
                if (erroresRelacionconlapl !== undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    if (erroresJson != undefined) {
                        isValid = (erroresJson == null || erroresJson.length == 0);
                        if (!isValid) {
                            erroresJson[vm.nombreComponente].forEach(p => {

                                if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                            });
                        }
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.limpiarErrores = function (errores) {


            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ProyectoContracredito-error-"+ vm.nombreComponente);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ProyectoCredito-error-" + vm.nombreComponente);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }


            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-EstadoControlPosterior-error-" + vm.nombreComponente);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            //Esto se hace para borrar los iconos en la grilla.  En el html en la clase puse el nombre erroras006 para que solo me traiga los asociados a este error
            let elementos = document.getElementsByClassName("erroras006");
            if (elementos !== undefined) {
                let i;
                for (i = 0; i < elementos.length; i++) {

                    var campoObligatorioJustificacion = elementos[i];
                    if (campoObligatorioJustificacion != undefined) {
                        campoObligatorioJustificacion.innerHTML = "";
                        campoObligatorioJustificacion.classList.add('hidden');
                    }
                }
            }

            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-montoDiligenciados-error-" + vm.nombreComponente);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
     
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ValorCreditoContracredito-error-" + vm.nombreComponente);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
               
           

        }

       

        vm.validarProyectoContracredito = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ProyectoContracredito-error-" + vm.nombreComponente);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarProyectoCredito = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ProyectoCredito-error-" + vm.nombreComponente);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarEstadoControlPosterior = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-EstadoControlPosterior-error-" + vm.nombreComponente);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarEstadoControlPosteriorGrilla = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("AS006-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.ValidaMontosDiligenciados = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-montoDiligenciados-error-" + vm.nombreComponente);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarProyectoContrato = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ProyectoContracredito-error-" + vm.nombreComponente);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarValorCreditoContracredito = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ValorCreditoContracredito-error-" + vm.nombreComponente);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.errores = {
            'AS001': vm.validarProyectoContracredito,
            'AS002': vm.validarProyectoCredito,
            'AS004': vm.validarValorCreditoContracredito,
            'AS006': vm.validarEstadoControlPosterior,
            'AS006-': vm.validarEstadoControlPosteriorGrilla,
            'AS008-': vm.validarEstadoControlPosteriorGrilla,
            'AS008': vm.ValidaMontosDiligenciados,
            'AS001C': vm.validarProyectoContrato,
        }

    }

    angular.module('backbone').component('asociarMuchosUnTipoFormularioa', {
        templateUrl: "src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarMuchosUnTipo/asociarMuchosUnTipoFormularioa.html",
        controller: asociarMuchosUnTipoFormularioa,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            deshabilitarBotonDevolverSeccionProyecto: '&',
            tramiteid: '@',
            tipotramiteid: '@',
            nombrecomponentepaso: '@',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            actualizacomponentes: '=',
            deshabilitar: '@',
            rolanalista: '@',
            tipoaccion: '@',
            tipoconsulta: '@',
            tipoproyecto: '@'
        }
    });

})();