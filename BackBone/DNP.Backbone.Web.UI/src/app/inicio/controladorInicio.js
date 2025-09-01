/// <reference path="../panelprincial/componentes/inbox/controladorinbox.js" />
/// <reference path="../panelprincial/componentes/inbox/controladorconsolaprocesosdefault.js" />
(function () {
    'use strict';


    controladorInicio.$inject = ['$scope', 'serviciosComponenteNotificaciones', '$routeParams', 'backboneServicios', '$localStorage', '$interval', 'servicioPanelPrincipal', '$timeout', 'constantesBackbone','utilidades'];

    function controladorInicio($scope, serviciosComponenteNotificaciones, $routeParams, backboneServicios, $localStorage, $interval, servicioPanelPrincipal, $timeout, constantesBackbone, utilidades) {
        var vm = this;
        vm.misProcesos = {};
        vm.macroProcesos = [];
        vm.macroProcesosCantidad = [];
        
        $timeout(function () {
            servicioPanelPrincipal.obtenerProcesos().then(exitoProceso, errorProceso);
            function exitoProceso(respuesta) {
                $localStorage.procesosList = respuesta.data;
            }
            function errorProceso(respuesta) {
            }
           
            servicioPanelPrincipal.obtenerMacroprocesosCantidad().then(exitoCantidad, errorCantidad);
            function exitoCantidad(respuesta) {
                if (respuesta.data != null) {
                    for (var i = 0; i < respuesta.data.length; i++) {
                        vm.macroProcesosCantidad.push(respuesta.data[i]);
                    }
                

                    $localStorage.cantidadMacroprocesosList = vm.macroProcesosCantidad;
                    var data = vm.macroProcesosCantidad.filter(x => (String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaPlaneacion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaViabilidadRegistro ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaAjustes ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaPriorizacion) && x.TipoObjeto == 'Proyecto');
                    var data = vm.macroProcesosCantidad.filter(x => (String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaPlaneacion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaViabilidadRegistro ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaAjustes ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaPriorizacion) && x.TipoObjeto == 'Proyecto');
                    var cantidadPlaneacionProyecto = 0;
                    if (data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            cantidadPlaneacionProyecto = cantidadPlaneacionProyecto + data[i].Cantidad;
                        }
                    }
                    if (vm.etapa == 'pl')
                        vm.listaProcesos = $localStorage.procesosList.filter(x => String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaPlaneacion);
                    var dataGR = vm.macroProcesosCantidad.filter(x => (String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaGestionRecursos ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaSolicitudRecursos ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaRevisionRequisitos ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaAprobacion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaAjustesGR) && x.TipoObjeto == 'Proyecto');
                    var cantidadRecursosProyecto = 0;
                    if (dataGR.length > 0) {
                        for (var i = 0; i < dataGR.length; i++) {
                            cantidadRecursosProyecto = cantidadRecursosProyecto + dataGR[i].Cantidad;
                        }
                    }
                    if (vm.etapa == 'gr')
                        vm.listaProcesos = $localStorage.procesosList.filter(x => String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaGestionRecursos);
                    var dataE = vm.macroProcesosCantidad.filter(x => (String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaNuevaEjecucion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaProgramacionEjecucion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaAjustesEjecucion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaTramitesEjecucion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaSeguimientoControl) && x.TipoObjeto == 'Proyecto');
                    var cantidadEjecucionProyecto = 0;
                    if (dataE.length > 0) {
                        for (var i = 0; i < dataE.length; i++) {
                            cantidadEjecucionProyecto = cantidadEjecucionProyecto + dataE[i].Cantidad;
                        }
                    }
                    if (vm.etapa == 'ej')
                        vm.listaProcesos = $localStorage.procesosList.filter(x => String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaNuevaEjecucion);
                    var dataEV = vm.macroProcesosCantidad.filter(x => (String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaEvaluacion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaCortoPlazo ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaMedianoPlazo ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaLargoPlazo) && x.TipoObjeto == 'Proyecto');
                    var cantidadEvaluacionProyecto = 0;
                    if (dataEV.length > 0) {
                        for (var i = 0; i < dataEV.length; i++) {
                            cantidadEvaluacionProyecto = cantidadEvaluacionProyecto + dataEV[i].Cantidad;
                        }
                    }
                    if (vm.etapa == 'ev')
                        vm.listaProcesos = $localStorage.procesosList.filter(x => String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaEvaluacion);

                    var cantidadTramiteEjecucion = 0;
                    var txtTramite = 'Tramite'
                    var dataTramite = vm.macroProcesosCantidad.filter(x => angular.equals(String(x.NivelPadreId).toUpperCase(), constantesBackbone.idEtapaNuevaEjecucion));
                    if (dataTramite.length > 0) {
                        for (var i = 0; i < dataTramite.length; i++) {
                            if (angular.equals(utilidades.toNormalForm(dataTramite[i].TipoObjeto), txtTramite)) {
                                cantidadTramiteEjecucion = cantidadTramiteEjecucion + dataTramite[i].Cantidad;
                            }
                        }
                    }

                    var cantidadProgramacionEjecucion = 0;
                    var txtProgramacion = 'Programacion'
                    var dataPE = vm.macroProcesosCantidad.filter(x => angular.equals(String(x.NivelPadreId).toUpperCase(), constantesBackbone.idEtapaNuevaEjecucion));
                    if (dataPE.length > 0) {
                        for (var i = 0; i < dataTramite.length; i++) {
                            if (angular.equals(utilidades.toNormalForm(dataTramite[i].TipoObjeto), txtProgramacion)) {
                                cantidadProgramacionEjecucion = cantidadProgramacionEjecucion + dataPE[i].Cantidad;
                            }
                        }
                    }

                    vm.cantidades = [];
                    vm.cantidades.push({ 'PProyecto': cantidadPlaneacionProyecto, 'GRProyecto': cantidadRecursosProyecto, 'EJProyecto': cantidadEjecucionProyecto, 'EVProyecto': cantidadEvaluacionProyecto, 'EJTramite': cantidadTramiteEjecucion, 'EJProgramacion': cantidadProgramacionEjecucion, 'EJTotal': cantidadTramiteEjecucion + cantidadEjecucionProyecto + cantidadProgramacionEjecucion });
                    $localStorage.cantidadesMisproyectos = vm.cantidades;
                }

                servicioPanelPrincipal.obtenerMacroprocesos().then(exito, error);
                function exito(respuesta) {
                    $localStorage.macroProcesosList = respuesta.data;
                    
                    var tipo = 'pl'
                    for (var i = 0; i < respuesta.data.length; i++) {
                        var cantidadTramite = 0;
                        var cantidadProyecto = 0;
                        if (angular.equals(respuesta.data[i].NivelId.toUpperCase(), constantesBackbone.idEtapaPlaneacion)) {
                            cantidadProyecto = (vm.cantidades != null) ? vm.cantidades[0].PProyecto: 0;
                            tipo = 'pl';
                        }
                        if (angular.equals(respuesta.data[i].NivelId.toUpperCase(), constantesBackbone.idEtapaGestionRecursos)) {
                            cantidadProyecto = (vm.cantidades != null) ? vm.cantidades[0].GRProyecto: 0;
                            tipo = 'gr';
                        }
                        if (angular.equals(respuesta.data[i].NivelId.toUpperCase(), constantesBackbone.idEtapaNuevaEjecucion)) {
                            cantidadProyecto = (vm.cantidades != null) ? vm.cantidades[0].EJProyecto : 0;
                            cantidadTramite = (vm.cantidades != null) ? vm.cantidades[0].EJTramite : 0;
                            tipo = 'ej';
                        }
                        if (angular.equals(respuesta.data[i].NivelId.toUpperCase(), constantesBackbone.idEtapaEvaluacion)) {
                            cantidadProyecto = (vm.cantidades != null) ? vm.cantidades[0].EVProyecto : 0;
                            tipo = 'ev';
                        }

                        vm.macroProcesos.push({
                            NivelId: respuesta.data[i].NivelId, Nombre: respuesta.data[i].Nombre, Archivo: respuesta.data[i].Archivo, Descripcion: respuesta.data[i].Descripcion, CantidadProyecto: cantidadProyecto,
                            CantidadTramite: cantidadTramite, CantidadTotal: cantidadProyecto + cantidadTramite, Contador: i, Tipo: tipo
                        });

                    }
                }
                function error(respuesta) {
                }
            }
            function errorCantidad(respuesta) {
            }

            angular.element(document).ready(function () {
                angular.element("#wrapper").removeClass("active");
            });
            vm.link = function (url) {
                window.location.href = url;
            }
        },2400);

    }

    angular.module('backbone').controller('controladorInicio', controladorInicio);
})();