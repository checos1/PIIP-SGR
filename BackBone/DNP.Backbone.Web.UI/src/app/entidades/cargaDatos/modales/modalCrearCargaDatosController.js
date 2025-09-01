(function () {
    'use strict';

    modalCrearCargaDatosController.$inject = [
        'objCarga',
        '$uibModalInstance',
        'servicioCargaDatos',
        'utilidades',
        'backboneServicios',
    ];

    function modalCrearCargaDatosController(
        objCarga,
        $uibModalInstance,
        servicioCargaDatos,
        utilidades,
        backboneServicios,
    ) {
        var vm = this;
        vm.carga = objCarga;
        vm.guardar = guardar;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.obtenerEntidadesPorTipo = obtenerEntidadesPorTipo;

        const OPCION_CARGADEDATOS = "EntidadesCargarDatos";

        const _validadores = [
            (carga) => ([{ invalido: !carga.tipoCargaDatosId, mensaje: "Ingrese un tipo para la Carga de Datos" }]),
            (carga) => ([{ invalido: !carga.fecha, mensaje: "Ingrese una fecha para la Carga de Datos" }]),
            (carga) => ([{ invalido: !carga.valorCredito, mensaje: "Ingrese un valor de crédito para la Carga de Datos" }]),
            (carga) => ([{ invalido: !carga.valorContraCredito, mensaje: "Ingrese un valor de contra crédito para la Carga de Datos" }]),
            (carga) => ([{ invalido: (vm.carga.tipoEntidad === 'Territorial' || vm.carga.tipoEntidad === 'SGR') && !carga.idEntidad, mensaje: "Ingrese una Entidad para la Carga de Datos" }]),
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

        function obtenerEntidadesPorTipo(tipo) {
            servicioCargaDatos.obtenerEntidadesPorTipo(tipo)
                .then(function (response) {
                    vm.carga.listaEntidades = response.data;
                    //Dejar solo las entidades para las que se tiene permisos
                    let entidadesFiltradas = vm.carga.listaEntidades.filter(ent => backboneServicios.consultarPermiso(ent.IdEntidad, OPCION_CARGADEDATOS));
                    vm.carga.listaEntidades = entidadesFiltradas;
                });
        }

        /// Comienzo
        vm.$onInit = function () {
            if (vm.carga.tipoEntidad == 'Territorial' || vm.carga.tipoEntidad == 'SGR') {
               vm.obtenerEntidadesPorTipo(vm.carga.tipoEntidad);
            }
        }

        function guardar() {

            if (!($('#upload-file-info').text().endsWith("xlsx") || $('#upload-file-info').text().endsWith("xls"))) {
                $('#upload-file-info').html("")
                toastr.warning("Cargar archivos con la extensión \".xls\" o \".xlsx\" solamente");
                return;
            }

            if (!_validarEntidad(vm.carga))
                return;

            var fd = new FormData();
            var dato = document.getElementById("file-archivo").files[0];

            if (!(document.getElementById("file-archivo").files.length > 0)) {
                toastr.warning("Ingrese un archivo para la Carga de Datos");
                return;
            }

            fd.append('GuardarDatos', dato);


            var json = {
                fecha: vm.carga.fecha,
                tipoCargaDatosId: vm.carga.tipoCargaDatosId,
                tipoEntidad: vm.carga.tipoEntidad,
                valorContraCredito: vm.carga.valorContraCredito,
                valorCredito: vm.carga.valorCredito,
                idEntidad: vm.carga.idEntidad
            }

            fd.append('carga', JSON.stringify(json));

            servicioCargaDatos.guardarDatos(fd)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.cerrar()
                    } else
                        utilidades.mensajeError(response.data.Mensaje, false);
                });
        }
    }

    angular.module('backbone.entidades').controller('modalCrearCargaDatosController', modalCrearCargaDatosController);
})();