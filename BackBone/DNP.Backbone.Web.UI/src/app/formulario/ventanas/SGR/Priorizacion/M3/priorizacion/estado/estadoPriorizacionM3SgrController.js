(function () {
    'use strict';    

    estadoPriorizacionM3SgrController.$inject = ['$scope', 'priorizacionSgrServicio', 'utilidades', '$sessionStorage'];

    function estadoPriorizacionM3SgrController(
        $scope,
        priorizacionSgrServicio,
        utilidades,
        $sessionStorage,
    ) {
        var vm = this;
        vm.nombreComponente = "sgrpriorizacionm3priorizacionestadopriorizacion";
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
                    }
                }, function (error) {
                    utilidades.mensajeError('Ocurrió un problema al mostrar los estados de la priorización del proyecto.');
                });
        };    

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }
    }

    angular.module('backbone').component('estadoPriorizacionM3Sgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/Priorizacion/M3/priorizacion/estado/estadoPriorizacionM3Sgr.html",

        controller: estadoPriorizacionM3SgrController,
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