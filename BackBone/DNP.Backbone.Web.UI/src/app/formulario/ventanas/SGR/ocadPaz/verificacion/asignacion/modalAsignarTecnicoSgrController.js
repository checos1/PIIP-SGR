(function () {
    'use strict';

    angular.module('backbone')
        .controller('modalAsignarTecnicoSgrController', modalAsignarTecnicoSgrController);

    modalAsignarTecnicoSgrController.$inject = [
        '$uibModalInstance',
        '$sessionStorage',
        'utilidades',
        'requisitosPazSgrServicio',
        'RolFiltroId',
        'EntidadId',
        'ProyectoId',
        'InstanciaId',
        'AccionId',
        'UsuarioEncargadoOcadPazId',
        'UsuarioEncargadoAsignado'
    ];

    function modalAsignarTecnicoSgrController(
        $uibModalInstance,
        $sessionStorage,
        utilidades,
        requisitosPazSgrServicio,
        RolFiltroId,
        EntidadId,
        ProyectoId,
        InstanciaId,
        AccionId,
        UsuarioEncargadoOcadPazId,
        UsuarioEncargadoAsignado
    ) {
        var vm = this;

        //#region Variables
        vm.options = [];
        vm.mostrarError = false;
        vm.mostrarErrorLongitud = false;
        vm.btnGuardarDisable = true;
        vm.nombrePolitica = "";
        vm.rolFiltroId = RolFiltroId;
        vm.entidadId = EntidadId;
        vm.ProyectoId = ProyectoId;
        vm.InstanciaId = InstanciaId;
        vm.AccionId = AccionId;
        vm.UsuarioEncargadoOcadPazId = UsuarioEncargadoOcadPazId;
        vm.UsuarioEncargadoAsignado = UsuarioEncargadoAsignado;
        //#endregion

        //#region Metodos

        function guardar() {

            if (vm.data.IdUsuarioDNP === null || vm.data.IdUsuarioDNP === undefined || vm.data.IdUsuarioDNP === "") {
                utilidades.mensajeError('Debe seleccionar un usuario', false);
                return;
            }

            var data = {
                InstanciaId: vm.InstanciaId,
                idAccion: vm.AccionId,
                AvanceId: 1
            }

            if (vm.UsuarioEncargadoOcadPazId != null) {
                GuardarAsignacionUsuario();
                return;
            }

            return requisitosPazSgrServicio.Flujos_SubPasoEjecutar(data)
                .then(function (response) {
                    if (response.data && response.status === 200) {
                        GuardarAsignacionUsuario();
                    }
                    else {
                        var mensaje = response.data.Mensaje;
                        utilidades.mensajeError(mensaje.substr(mensaje.indexOf(':') + 1), false);
                    }
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación");
                });
        }

        function GuardarAsignacionUsuario() {
            var data = {
                ProyectoId: vm.ProyectoId,
                InstanciaId: vm.InstanciaId,
                AccionId: vm.AccionId,
                UsuarioEncargado: vm.data.IdUsuarioDNP,
                RolUsuarioEncargadoId: vm.rolFiltroId,
                UsuarioEncargadoOcadPazId: UsuarioEncargadoOcadPazId
            }

            return requisitosPazSgrServicio.SGR_OCADPaz_GuardarAsignacionUsuarioEncargado(data).then(function (response) {
                if (response.data) {
                    utilidades.mensajeSuccess("Los datos han sido guardados con éxito y se ha enviado el proyecto con éxito al siguiente paso del flujo", false, false, false);
                    $uibModalInstance.close(response.data);
                } else {
                    utilidades.mensajeError("Error al realizar la operación", false);
                    vm.disabledAgregar = true;
                    vm.habilitaEnvioSubPaso = true;
                    vm.habilitaBotones = true;
                }
            });
        }

        function cerrar() {
            $uibModalInstance.dismiss('cancel');
        };

        function init() {
            var parametros = {
                EntidadId: vm.entidadId,
                RolId: vm.rolFiltroId,
            }

            return requisitosPazSgrServicio.obtenerUsuariosVerificacionOcadPaz(parametros).then(
                function (result) {
                    vm.usuariosRolEntidad = result.data.filter(x => x.IdUsuarioDNP !== vm.UsuarioEncargadoAsignado);
                });
        }

        //métodos
        vm.init = init;
        vm.guardar = guardar;
        vm.cerrar = cerrar;
        vm.requisitosPazSgrServicio = requisitosPazSgrServicio;
    }
})();