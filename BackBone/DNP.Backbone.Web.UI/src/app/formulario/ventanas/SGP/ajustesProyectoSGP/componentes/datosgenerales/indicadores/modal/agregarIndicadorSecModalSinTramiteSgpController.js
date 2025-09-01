(function () {
    'use strict';

    angular.module('backbone')
        .controller('agregarIndicadorSecModalSinTramiteSgpController', agregarIndicadorSecModalSinTramiteSgpController);

    agregarIndicadorSecModalSinTramiteSgpController.$inject = [
        '$scope',
        '$uibModalInstance',
        'servicioActualizarFechasModal',
        'Programacion',
        'utilidades',
        '$filter',
        'ProductoId',
        'IndicadoresSec',
        'ObjetivoId'
    ];

    function agregarIndicadorSecModalSinTramiteSgpController(
        $scope,
        $uibModalInstance,
        servicioActualizarFechasModal,
        Programacion,
        utilidades,
        $filter,
        ProductoId,
        IndicadoresSec,
        ObjetivoId
    ) {
        const vm = this;

        //#region Variables
        vm.cerrado = Programacion.cerrado;
        vm.tipoEntidad = Programacion.TipoEntidad;
        vm.fechaHoy = formatearFecha(obtenerFechaSinHoras(new Date()));
        vm.habiltaAgregar = true;

        vm.data = {
            capitulo: Programacion.capitulo,
            fechaDesde: Programacion.fechaDesde,
            //  fechaDesdeFormat: $filter('date')(Programacion.fechaDesde, 'dd/MM/yyyy'),
            fechaHasta: Programacion.fechaHasta,
            IdProgramacion: Programacion.IdProgramacion,
            FlujoId: Programacion.flujoId,
            TipoEntidad: Programacion.TipoEntidad,
            creado: Programacion.creado,
            cerrado: Programacion.cerrado,
            iniciarProceso: Programacion.iniciarProceso,
            idNotificacion: Programacion.IdNotificacion
        }
        vm.ProductoId = ProductoId;
        vm.IndicadoresSec = IndicadoresSec;
        vm.ObjetivoId = ObjetivoId;

        vm.options = [];

        //#endregion

        //#region Metodos

        function _mostarToast(toasMessages = []) {
            toasMessages.forEach(message => {
                if (!message)
                    return;

                toastr.warning(message);
            })
        }

        function _validar(model) {
            try {
                const mensajes = [];
                if (!model.fechaDesde)
                    mensajes.push("Seleccione la fecha desde");
                if (!model.fechaHasta)
                    mensajes.push("Seleccione la fecha hasta");
                if (model.fechaDesde && model.fechaDesde < obtenerFechaSinHoras(new Date()))
                    mensajes.push("La fecha desde debe ser posterior a la fecha y hora actual");
                if (model.fechaHasta && model.fechaHasta < obtenerFechaSinHoras(new Date()))
                    mensajes.push("La fecha hasta debe ser posterior a la fecha y hora actual");
                if (model.fechaDesde && model.fechaHasta) {
                    if (model.fechaDesde > model.fechaHasta) {
                        mensajes.push("La fecha desde debe ser anterior a la fecha hasta");
                    }
                }

                if (mensajes.length)
                    _mostarToast(mensajes);

                return !mensajes.length;
            }
            catch (e) {
                console.log(e);
                toastr.error("Error inesperado al validar");
                return false;
            }
        }

        function guardar() {
            $uibModalInstance.close(vm.options);
        }

        function obtenerFechaSinHoras(fecha) {
            return new Date(Date.UTC(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), fecha.getHours(), fecha.getMinutes(), fecha.getSeconds()));
        }

        function formatearFecha(fecha) {
            let fechaString = fecha.toISOString();
            return fechaString.substring(0, 19);
        }

        function cerrar() {
            vm.options = [];
            $uibModalInstance.close();
        }

        async function init() {
            var CantidadSinMarcar = IndicadoresSec.find(x => x.Marcado == 0);

            if (CantidadSinMarcar == null || CantidadSinMarcar == undefined) {
                vm.habiltaAgregar = false;
            } else {
                vm.habiltaAgregar = true;
            }
        }

        function toggleCurrency(indicador) {
            if (indicador.isChecked) {
                vm.options.push(indicador.CodigoIndicador);
            } else {
                var toDel = vm.options.indexOf(indicador.CodigoIndicador);
                vm.options.splice(toDel, 1);
            }
        };

        vm.init = init;
        vm.guardar = guardar;
        vm.cerrar = cerrar;
        vm.toggleCurrency = toggleCurrency;

        //#endregion
    }
})();