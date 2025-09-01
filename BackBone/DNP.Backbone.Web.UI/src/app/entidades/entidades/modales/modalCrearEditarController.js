(function () {
    'use strict';

    modalCrearEditarController.$inject = [
        'objEntidad',
        '$uibModalInstance',
        'servicioEntidades',
        'utilidades',
        'backboneServicios',
        'sesionServicios',
    ];

    function modalCrearEditarController(
        objEntidad,
        $uibModalInstance,
        servicioEntidades,
        utilidades,
        backboneServicios,
        sesionServicios
    ) {
        var vm = this;


        vm.objEntidad = objEntidad;
        //vm.objEntidadBackup = angular.copy(vm.objEntidad);

        vm.cerrar = $uibModalInstance.dismiss;
        vm.guardarEntidad = guardarEntidad;


       // vm.objEntidad = {};
        const _validadores = [
            (objEntidad) => ([{ invalido: !objEntidad.nombre, mensaje: "Ingrese un nombre para la Entidad" }]),
            (objEntidad) => ([{ invalido: !objEntidad.subEntidad && (objEntidad.tipoEntidad == 'Territorial' || objEntidad.tipoEntidad == 'SGR') && !objEntidad.entityType, mensaje: "Seleccione un tipo para la Entidad" }]),
            (objEntidad) => ([{ invalido: objEntidad.entityType == 4 && !objEntidad.parentGuid, mensaje: "Seleccione un departamento para la Entidad" }]),
            (objEntidad) => ([{ invalido: objEntidad.tipoEntidad == 'Nacional' && !objEntidad.idSector, mensaje: "Seleccione un sector para la Entidad" }]),
            (objEntidad) => ([{ invalido: !objEntidad.sigla, mensaje: "Ingrese una sigla para la Entidad" }]),
            (objEntidad) => ([{ invalido: !objEntidad.codigo, mensaje: "Ingrese un código para la Entidad" }]),
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

        function guardarEntidad() {

            if (!_validarEntidad(vm.objEntidad))
                return;

            if (vm.objEntidad.cabezaSector) {
                var sector = vm.objEntidad.listaSector.find(x => x.Id.indexOf(vm.objEntidad.idSector) != -1);

                var tieneCabeza = vm.objEntidad.listaEntidades.find(x => x.AgrupadorEntidad.indexOf(sector.Nombre) != -1 && x.CabezaSector && x.IdEntidad != vm.objEntidad.id);

                if (tieneCabeza) {
                    utilidades.mensajeWarning("Cuando proceda, anulará la selección de la entidad que representa la cabeza del sector. Desea continuar?", function funcionContinuar() {
                        guardar()
                    }, function funcionCancelar() {
                        return;
                    })
                } else
                    guardar();
            } else
                guardar();

        }

        function guardar() {
            servicioEntidades.guardarEntidad(vm.objEntidad)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.cerrar();
                        backboneServicios.obtenerPermisosPorEntidad().then(result => {
                            sesionServicios.setearPermisos(result);
                        });
                    } else
                        utilidades.mensajeError(response.data.Mensaje, false);

                   // console.log(response.data);
                });
        }


    }

    angular.module('backbone.usuarios').controller('modalCrearEditarController', modalCrearEditarController);
})();