(function () {
    'use strict';

    angular.module('backbone')
        .controller('modalAgregarInvolucradosSgpController', modalAgregarInvolucradosSgpController);

    modalAgregarInvolucradosSgpController.$inject = [
        '$scope',
        '$uibModalInstance',
        '$sessionStorage',
        'utilidades',
        'usuariosInvolucradosSgpServicio',
        'involucrado',
        'transversalSgpServicio'
    ];

    function modalAgregarInvolucradosSgpController(
        $scope,
        $uibModalInstance,
        $sessionStorage,
        utilidades,
        usuariosInvolucradosSgpServicio,
        involucrado,
        transversalSgpServicio
    ) {
        var vm = this;

        //#region Variables
        vm.options = [];
        vm.mostrarError = false;
        vm.mostrarErrorLongitud = false;
        vm.btnGuardarDisable = true;
        vm.nombrePolitica = "";
        vm.involucrado = involucrado;
        vm.entidadDestinoFlujo = null;
        vm.disabledArea = false;

        vm.model = {
            UsuarioSeleccionado: "",
            Area: ""
        }

        //#endregion

        //#region Metodos

        function guardar() {

            var parametros = {
                ProyectoId: vm.involucrado.ProyectoId,
                TipoConceptoViabilidadId: vm.involucrado.TipoConceptoViabilidadId,
                TipoRolConceptoId: vm.involucrado.TipoRolConceptoId,
                Nombre: vm.model.UsuarioSeleccionado,
                Area: vm.model.Area
            };

            if (!validar()) { return; }

            return usuariosInvolucradosSgpServicio.guardarProyectoViabilidadInvolucrados(parametros)
                .then(function (response) {
                    if (response.data && response.status === 200) {
                        vm.options.push(parametros);
                        utilidades.mensajeSuccess("Involucrado guardado satisfactoriamente", false, false, false);
                        //$sessionStorage.close(vm.options);
                        $uibModalInstance.close(vm.options);
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

        function validar() {
            var valida = true;
            var NombreObligatorio = document.getElementById('NombreObligatorio');
            var AreaObligatoria = document.getElementById('AreaObligatoria');

            if (vm.model.UsuarioSeleccionado == null || vm.model.UsuarioSeleccionado == '') {
                if (NombreObligatorio != undefined) {
                    NombreObligatorio.classList.remove('hidden');
                    valida = false;
                }
            }
            else {
                if (NombreObligatorio != undefined) {
                    NombreObligatorio.classList.add('hidden');
                }
            }

            if (!vm.model.Area) {
                if (AreaObligatoria != undefined) {
                    AreaObligatoria.classList.remove('hidden');
                    valida = false;
                }
            }
            else {
                if (AreaObligatoria != undefined) {
                    AreaObligatoria.classList.add('hidden');
                }
            }

            return valida;
        }

        function cerrar() {
            vm.options = [];
            $uibModalInstance.close();
        }

        function toggleGuardar() {
            if ((vm.options && vm.options.length > 0) || vm.agregarOtro) {
                vm.btnGuardarDisable = false;
            }
            else {
                vm.btnGuardarDisable = true;
            }
        };

        function init() {

            if (vm.involucrado.TipoRolConceptoId === 2) {
                var viabilidadSgp = $sessionStorage.listadoAccionesTramite.find(f => f.Ventana == 'viabilidadSgp');
                if (viabilidadSgp != null) {
                    vm.IdRol = viabilidadSgp.RolId;
                    vm.IdEntidad = viabilidadSgp.IdEntidad;
                    vm.NombreAreaEntidad = viabilidadSgp.Entidad;
                }

            } else {
                var requisitosSgp = $sessionStorage.listadoAccionesTramite.find(f => f.Ventana == 'requisitosViabilidadSgp');
                if (requisitosSgp != null) {
                    vm.IdRol = requisitosSgp.RolId;
                    vm.IdEntidad = requisitosSgp.IdEntidad;
                    vm.NombreAreaEntidad = requisitosSgp.Entidad;
                }
            }

            var parametros = {
                EntidadId: vm.IdEntidad,
                RolId: vm.IdRol,
                InstanciaId: $sessionStorage.idInstancia
            }

            usuariosInvolucradosSgpServicio.ObtenerUsuariosInvolucrados(parametros).then(
                function (involucrados) {
                    vm.usuariosInvolucrados = involucrados.data.filter(x => vm.involucrado.InvolucradosActuales.findIndex(i => i.Nombre == x.NombreUsuario) < 0);
                }, function (error) {
                    utilidades.mensajeError(error);
                });
        }

        function setAreaDependencia() {
            if (!vm.model.UsuarioSeleccionado) {
                vm.model.Area = undefined;
                vm.disabledArea = false;
                return;
            }
            vm.model.Area = vm.NombreAreaEntidad;
            vm.disabledArea = true;
            validar();
        }

        //métodos
        vm.init = init;
        vm.guardar = guardar;
        vm.cerrar = cerrar;
        vm.usuariosInvolucradosSgpServicio = usuariosInvolucradosSgpServicio;
        vm.setAreaDependencia = setAreaDependencia;
    }
})();