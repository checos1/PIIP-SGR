(function () {
    'use strict';

    angular.module('backbone')
        .controller('modalAgregarInvolucradosSgrController', modalAgregarInvolucradosSgrController);

    modalAgregarInvolucradosSgrController.$inject = [
        '$scope',
        '$uibModalInstance',
        '$sessionStorage',
        'utilidades',
        'usuariosInvolucradosSgrServicio',
        'involucrado'
    ];

    function modalAgregarInvolucradosSgrController(
        $scope,
        $uibModalInstance,
        $sessionStorage,
        utilidades,
        usuariosInvolucradosSgrServicio,
        involucrado
    ) {
        var vm = this;

        //#region Variables
        vm.options = [];
        vm.mostrarError = false;
        vm.mostrarErrorLongitud = false;
        vm.btnGuardarDisable = true;
        vm.nombrePolitica = "";
        vm.involucrado = involucrado;

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
                Area: vm.model.Area,
            };

            if (!validar()) { return; }

            return usuariosInvolucradosSgrServicio.guardarProyectoViabilidadInvolucrados(parametros)
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

            if (vm.model.Area.length <= 5) {
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
            var parametros = {
                EntidadId: vm.involucrado.EntidadId,
                RolId: vm.involucrado.RolFiltroId,
                InstanciaId: $sessionStorage.idInstancia
            }

            usuariosInvolucradosSgrServicio.ObtenerUsuariosInvolucrados(parametros).then(
                function (involucrados) {
                    vm.usuariosInvolucrados = involucrados.data.filter(x => vm.involucrado.InvolucradosActuales.findIndex(i => i.Nombre == x.NombreUsuario) < 0);
                });
        }

        //métodos
        vm.init = init;
        vm.guardar = guardar;
        vm.cerrar = cerrar;
        vm.usuariosInvolucradosSgrServicio = usuariosInvolucradosSgrServicio;
    }
})();