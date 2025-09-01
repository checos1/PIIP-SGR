(function () {
    'use strict';

    angular.module('backbone')
        .controller('actualizarFechasModalController', actualizarFechasModalController);

    actualizarFechasModalController.$inject = [
        '$scope',
        '$uibModalInstance',
        'servicioActualizarFechasModal',
        'Programacion',
        'utilidades',
        '$filter'
    ];

    function actualizarFechasModalController(
        $scope,
        $uibModalInstance,
        servicioActualizarFechasModal,
        Programacion,
        utilidades,
        $filter
    ) {
        const vm = this;

        //#region Variables
        vm.cerrado = Programacion.cerrado;
        vm.tipoEntidad = Programacion.TipoEntidad;
        vm.fechaHoy = formatearFecha(obtenerFechaSinHoras(new Date()));

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
            const valido = _validar(vm.data);
            if (!valido)
                return;

            servicioActualizarFechasModal.Guardar({
                IdProgramacion: vm.data.IdProgramacion,
                FlujoId: vm.data.FlujoId,
                TipoEntidad: vm.data.TipoEntidad,
                fechaDesde: moment.utc(vm.data.fechaDesde).toDate(),
                fechaHasta: moment.utc(vm.data.fechaHasta).toDate(),
                creado: vm.data.creado,
                cerrado: vm.data.cerrado,
                iniciarProceso: vm.data.iniciarProceso,
                idNotificacion: vm.data.idNotificacion
            }).then(result => {
                utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                vm.cerrar();
            });
        }

        function obtenerFechaSinHoras(fecha) {
            return new Date(Date.UTC(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), fecha.getHours(), fecha.getMinutes(), fecha.getSeconds()));
        }

        function formatearFecha(fecha) {
            let fechaString = fecha.toISOString();
            return fechaString.substring(0, 19);
        }

        function cerrar() {
            $uibModalInstance.close(vm.data);
        }

        async function init() {
        }

        vm.init = init;
        vm.guardar = guardar;
        vm.cerrar = cerrar;

        //#endregion
    }
})();