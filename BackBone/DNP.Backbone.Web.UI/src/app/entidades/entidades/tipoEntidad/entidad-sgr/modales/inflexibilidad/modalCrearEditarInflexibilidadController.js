(function () {
    'use strict';

    modalCrearEditarInflexibilidadController.$inject = [
        'inflexibilidad',
        '$uibModalInstance',
        'inflexibilidadServicio',
        'utilidades'
    ];

    function modalCrearEditarInflexibilidadController(
        inflexibilidad,
        $uibModalInstance,
        inflexibilidadServicio,
        utilidades
    ) {
        var vm = this;

        vm.inflexibilidad = inflexibilidad;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.guardarInflexibilidad = guardarInflexibilidad;
        vm.inflexibilidad.listaTipoInflexibilidad = [{ IdTipo: 1, Nombre: 'Económica' }];

        const _validadores = [
            (inflexibilidad) => ([{ invalido: !inflexibilidad.nombreInflexibilidad, mensaje: "Ingrese un nombre para la Inflexibilidad" }]),
            (inflexibilidad) => ([{ invalido: !inflexibilidad.tipoInflexibilidad, mensaje: "Seleccione un tipo para la Inflexibilidad" }]),
            (inflexibilidad) => ([{ invalido: inflexibilidad.valorTotal <= 0, mensaje: "Ingrese un valor para la Inflexibilidad" }]),
            (inflexibilidad) => ([{ invalido: !inflexibilidad.fechaInicio, mensaje: "Ingrese una fecha de inicio" }]),
            (inflexibilidad) => ([{ invalido: !inflexibilidad.fechaFin, mensaje: "Ingrese una fecha de finalización" }]),
        ];

        function _validarEntidad(model) {
            try {
                const mensajes = [];
                _validadores.forEach(validator => {
                    validator(model).forEach(resultado => {
                        if (resultado.invalido)
                            mensajes.push(resultado.mensaje);
                    });
                })

                if (mensajes.length)
                    _mostarToast(mensajes);

                return !mensajes.length;
            }
            catch (e) {
                console.log(e);
                toastr.error("Error inesperado al validar la Entidad");
                return false;
            }
        }

        function _mostarToast(toasMessages = []) {
            toasMessages.forEach(message => {
                if (!message)
                    return;
                toastr.warning(message);
            })
        }

        /// Comienzo
        vm.$onInit = function () { }

        function guardarInflexibilidad() {

            if (!_validarEntidad(vm.inflexibilidad))
                return;

            inflexibilidadServicio.guardarInflexibilidad(vm.inflexibilidad)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.cerrar()
                    } else
                        utilidades.mensajeError(response.data.Mensaje, false);

                  //  console.log(response.data);
                });
        }
    }

    angular.module('backbone.entidades').controller('modalCrearEditarInflexibilidadController', modalCrearEditarInflexibilidadController);
})();