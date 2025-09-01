(function () {
    'use strict';

    DatosgeneralesrelacionconlaplController.$inject = [
        '$scope',        
        '$sessionStorage',
        'relacionPlanificacionServicio',
        'utilidades',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio'
    ];

    function DatosgeneralesrelacionconlaplController($scope,
        $sessionStorage,
        relacionPlanificacionServicio,
        utilidades,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio) {

        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = 'datosgeneralesrelacionconlapl';
        vm.titulo = "Modificación Relación con la planificación";
        vm.ClasesbtnGuardar = "btn btnguardarHorizonte";

        /*--- Configuración grid ----*/
        vm.gridDataConpes = [];
        vm.justificacionConpes;
        vm.erroresJustificacion = ""
        vm.erroresActivos = [];

        /*--- Validación Conpes ---*/
        vm.cambiosNuevos;
        vm.cambiosEliminados;

        /*--- Banderas ---*/
        vm.isEdicion;

        /*--- Variables sitio ---*/
        vm.idProyecto;
        vm.idInstancia;
        vm.guiMacroproceso;

        vm.init = function () {
            vm.reloadRelacionPlanificacion();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacion, nombreComponente: vm.nombreComponente, esValido: true });
        };

        vm.reloadRelacionPlanificacion = function () {
            vm.idProyecto = $sessionStorage.proyectoId;
            vm.idInstancia = $sessionStorage.idInstancia;
            vm.configuracion();
            vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
            vm.editar();
            vm.obtenerDocumentosConpes(vm.idProyecto, vm.idInstancia, vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe);
        }

        vm.configuracion = function () {
            vm.isEdicion = false;
        }

        vm.editar = function (estado) {
            vm.isEdicion = null;
            vm.isEdicion = estado == 'editar';
            if (vm.isEdicion) {
                document.getElementById("justificacion-conpes").disabled = false;
                document.getElementById("justificacion-conpes").classList.remove('disabled');
                document.getElementById("btn-guardar-edicion-conpes").classList.remove('disabled');
                vm.ClasesbtnGuardar = "btn btn-default btn-mdHorizonte";
            } else {
                document.getElementById("justificacion-conpes").disabled = true;
                document.getElementById("justificacion-conpes").classList.add('disabled');
                /*document.getElementById("btn-guardar-edicion-conpes").classList.add('disabled');*/
                vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
            }
        }

        vm.guardar = function () {
            if (vm.justificacionConpes == '') {
                utilidades.mensajeError('Debe ingresar una justificación.');
                return false;
            }
            var seccionCapitulo = document.getElementById("seccion-capitulo-" + vm.nombreComponente);
            var data = {
                ProyectoId: vm.idProyecto,
                Justificacion: vm.justificacionConpes,
                SeccionCapituloId: seccionCapitulo.value,
                InstanciaId: vm.idInstancia,
                AplicaJustificacion: 1
            }

            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess(response.data.Mensaje);
                        document.getElementById("justificacionConpes").value = vm.justificacionConpes == undefined ? '' : vm.justificacionConpes;
                        vm.editar('');
                        vm.ejecutarErrores();
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje);
                    }
                });
        }

        vm.obtenerDocumentosConpes = function (idProyecto, idInstancia, guiMacroproceso,NivelId,FlujoId) {
            relacionPlanificacionServicio.obtenerCambiosFirme(idProyecto).then(function (responseCambios) {
                relacionPlanificacionServicio.obtenerDocumentosConpes(idProyecto, idInstancia, guiMacroproceso, NivelId, FlujoId).then(function (response) {
                    var datosConpes = response.data.Conpes;
                    vm.cambiosNuevos = responseCambios.data.Nuevos;
                    vm.cambiosEliminados = responseCambios.data.Eliminados;
                    vm.justificacionConpes = response.data.Justificacion;
                    if (vm.cambiosNuevos != null) {
                        for (var i = 0; i < vm.cambiosNuevos.length; i++) {
                            for (var j = 0; j < datosConpes.length; j++) {
                                if (datosConpes[j].id == vm.cambiosNuevos[i].Id) {
                                    datosConpes[j]["background"] = "editReferenciaDNP";
                                    break;
                                }
                            }
                        }
                    }
                   
                    vm.gridDataConpes = datosConpes;
                    vm.ejecutarErrores();                        
                });
            });
        }

        /* ------------------------- Gestión de errores -------------------------------- */

        vm.limpiarErrores = function () {
            vm.erroresJustificacion = "";
        }

        vm.ejecutarErrores = function () {
            vm.limpiarErrores();
            vm.erroresActivos.forEach(p => {
                if (vm.errores[p.Error] != undefined) {
                    vm.errores[p.Error](p.Error, p.Descripcion);
                }
            });
        }

        vm.validarErroresActivos = function (codError) {
            if (vm.erroresActivos != null) {
                vm.erroresActivos = vm.erroresActivos.filter(p => p.Error != codError);
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: (vm.erroresActivos.length <= 0) });
            }
        }

        /**
         * Función validación Error = JUST001
         * @param {any} codError
         * @param {any} descErrores
         */
        vm.validarJustificacion = function (codError, descErrores) {
            if (vm.justificacionConpes == null || vm.justificacionConpes.length <= 0) {
                vm.erroresJustificacion = descErrores;
            } else {
                vm.limpiarErrores();
                vm.validarErroresActivos(codError);
            }
        }

        /* --------------------------------- Notificación de Validaciones ---------------------------*/

        /**
         * Función que recibe listado de errores de su componente padre por medio del binding notificacionvalidacion
         * @param {any} errores
         */
        vm.notificacionValidacion = function (errores) {
            console.log("Validación  - Justificación Conpes");
            if (errores != undefined) {
                var erroresFiltrados = utilsValidacionSeccionCapitulosServicio.getErroresValidadosJustificacion(vm.nombreComponente, errores);
                vm.erroresActivos = erroresFiltrados.erroresActivos;
                vm.ejecutarErrores();
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: erroresFiltrados.isValid });
            }
        };

        vm.errores = {
            'JUST001': vm.validarJustificacion
        }
    }

    angular.module('backbone').component('datosgeneralesrelacionconlapl', {
        templateUrl: "src/app/formulario/ventanas/transversal/justificacionCambios/componentes/datosgenerales/relacionPlanificacion/relacionPlanificacion.html",
        controller: DatosgeneralesrelacionconlaplController,
        controllerAs: "vm",
        bindings: {
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&'
        }
    });
})();