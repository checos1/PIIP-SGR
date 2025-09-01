(function() {
    'use strict';

    modalMensajesMttoController.$inject = [
        'listaMensajeMtto',
        '$uibModalInstance',
        '$scope',
    ];

    function modalMensajesMttoController(
        listaMensajeMtto,
        $uibModalInstance,
        $scope
    ) {

        var vm = this;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.mostrarMensaje = mostrarMensaje;

        var slides = $scope.slides = [];

        /// Comienzo
        vm.init = function() {

            $scope.myInterval = 0;
            $scope.noWrapSlides = true;            
                        
            var fechaMsg, horaMsg;

            // recorre la lista de los mensajes y los agrega a la lista que se muestra en el slider
            listaMensajeMtto.forEach(function (msgMtto) {
                
                // temporal ya que la fecha viene en formato: 2020-10-10T23:12:00.000
                fechaMsg = msgMtto.FechaCreacion.split('T')[0];
                horaMsg = msgMtto.FechaCreacion.split('T')[1];
                
                slides.push({
                    id: msgMtto.Id,
                    titulo: msgMtto.NombreMensaje,
                    fecha: fechaMsg,
                    hora: horaMsg,
                    mensaje: msgMtto.MensajeTemplate
                  });                               
            });            

        };

        function mostrarMensaje() {         
            vm.cerrar();
        }
    }

    angular.module('backbone').controller('modalMensajesMttoController', modalMensajesMttoController );

})();