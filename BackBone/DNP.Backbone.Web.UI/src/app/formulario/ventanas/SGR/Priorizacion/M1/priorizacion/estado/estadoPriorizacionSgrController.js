(function () {
    'use strict';    

    estadoPriorizacionSgrController.$inject = ['$scope','priorizacionSgrServicio', 'justificacionCambiosServicio','utilidades', '$sessionStorage'];

    function estadoPriorizacionSgrController(
        $scope,
        priorizacionSgrServicio,
        justificacionCambiosServicio,
        utilidades,
        $sessionStorage,
    ) {
        var vm = this;
        vm.nombreComponente = "sgrpriorizacionm1priorizacionestadopriorizacion";
        vm.Bpin = null;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;
        vm.ConvertirNumero = ConvertirNumero;
        vm.seccionCapitulo = null;

        //vm.componentesRefresh = [
        //    "sgrviabilidadrequisitosdatosgeneralesdatosadicionalesverificacion",
        //    "sgrviabilidadrequisitosdatosgeneralesagregarsectores"
        //];

        vm.init = function () {
            vm.proyectoId = $sessionStorage.proyectoId;
            vm.nivelId = $sessionStorage.idNivel;
            vm.tieneMensajeValidacion = false;
            vm.mensajeValidacion = "";
            vm.mostrarEstadosPriorizacion();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        };       
       
        vm.mostrarEstadosPriorizacion = function () {
            priorizacionSgrServicio.SGR_Proyectos_MostrarEstadosPriorizacion(vm.proyectoId)
                .then(function (response) {
                    if (response.data != null) {
                        vm.obj = response.data;
                        if (vm.obj[0].Fuentes != null) vm.Fuentes = vm.obj[0].Fuentes;
                        if (vm.obj[0].Metodologias != null) vm.Metodologias = vm.obj[0].Metodologias;
                       

                        //if (vm.obj[0].AcuerdoProyecto != null) {
                        //    if (vm.obj[0].AcuerdoProyecto.length > 0) {
                        //        vm.AcuerdoProyecto = vm.obj[0].AcuerdoProyecto;
                        //        vm.AcuerdoOriginalId = vm.obj[0].AcuerdoProyecto[0].AcuerdoId;
                        //        vm.MostrarGrid = true;
                        //    }
                        //}
                    }
                }, function (error) {
                    utilidades.mensajeError('Ocurrió un problema al mostrar los estados de la priorización del proyecto.');
                });
        };

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: vm.idInstancia,
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
         
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }
    }

    angular.module('backbone').component('estadoPriorizacionSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/Priorizacion/M1/priorizacion/estado/estadoPriorizacionSgr.html",

        controller: estadoPriorizacionSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            guardadocomponent: '&',
            notificacioncambios: '&'
        },
    });

})();