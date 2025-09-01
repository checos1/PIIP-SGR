(function() {
    'use strict';

    agregarCdpProyectoModalController.$inject = [
        '$scope',
        'utilidades',
        'tramiteSGPServicio',
        '$uibModalInstance',
        'proyectoId',
        'tramiteId',
        'rolId',
        'cdpList',
        'editarCdpId'
    ];

    function agregarCdpProyectoModalController(
        $scope,
        utilidades,
        tramiteSGPServicio,
        $uibModalInstance,
        proyectoId,
        tramiteId,
        rolId,
        cdpList,
        editarCdpId
    ) {
        var vm = this;
        vm.proyectoId = proyectoId;
        vm.tramiteId = tramiteId;
        vm.rolId = rolId;
        vm.cdpList = cdpList;
        vm.editarCdpId = editarCdpId;

        vm.errors = {
            valores: false,
            model: false
        }

        vm.model = {
            idNumero: '',
            idFecha: '',
            idValorCDPTramite: '',
            idValorTotal: ''
        }
            
        vm.init = function () {
            vm.setModelEvents();
        }

        vm.setModelEvents = function () {
            $scope.$watch('vm.editarCdpId', function () {
                if (vm.editarCdpId !== '' && vm.editarCdpId !== 0) {
                    vm.setEditModelToCurrentForm();
                }
            });
        }

        vm.setEditModelToCurrentForm = function () {
            const cdpSelected = vm.cdpList.find(w => w.Id == vm.editarCdpId);
            if (cdpSelected === undefined) {
                return;
            }

            vm.model = {
                idNumero: cdpSelected.NumeroCDP,
                idFecha: cdpSelected.FechaCDP,
                idValorCDPTramite: cdpSelected.ValorCDP,
                idValorTotal: cdpSelected.ValorTotalCDP
            }
        }

        vm.validarValoresCdp = function () {
            if (vm.model.idValorCDPTramite !== '' && vm.model.idValorTotal !== '') {
                const valor = Number(vm.model.idValorCDPTramite);
                const valorTotal = Number(vm.model.idValorTotal);

                vm.errors.valores = valor > valorTotal;
            } else {
                vm.errors.valores = false;
            }
        }

        vm.validarModelo = function () {
            for (var input in vm.model) {
                if (vm.model[input].length === 0) {
                    vm.errors.model = true;
                    return;
                }
            }

            vm.errors.model = false;
        }

        vm.guardar = function () {
            vm.validarModelo();

            if (vm.errors.model || vm.errors.valores) {
                return;
            }

            var tramiterequisito = {
                Descripcion: 'CDP para trámite',
                FechaCDP: vm.model.idFecha,
                IdPresupuestoValoresCDP: 0,
                IdPresupuestoValoresAportaCDP: 0,
                IdProyectoRequisitoTramite: 0,
                IdProyectoTramite: 0,
                IdTipoRequisito: 1,
                NumeroCDP: limpiaNumero(vm.model.idNumero),
                NumeroContratoCDP: 0,
                Tipo: 1,
                UnidadEjecutora: '',
                ValorTotalCDP: vm.model.idValorTotal,
                ValorCDP: vm.model.idValorCDPTramite,
                IdValorTotalCDP: 0,
                IdValorAportaCDP: 0,
                IdProyecto: vm.proyectoId,
                IdTramite: vm.tramiteId,
                IdTipoRol: 0,
                IdRol: vm.rolId
            };

            if (vm.cdpList === undefined) {
                vm.cdpList = [];
            }

            const currentCdpList = vm.editarCdpId === 0 ? vm.cdpList : vm.cdpList.filter(w => w.Id != vm.editarCdpId);

            const cdpListRequest = [
                ...currentCdpList,
                tramiterequisito
            ]

            tramiteSGPServicio.actualizarTramitesRequisitos(cdpListRequest)
            .then(function (response) {
                let exito = response.data;
                if (exito) {
                    if ($uibModalInstance.esAjuste) {
                        utilidades.mensajeSuccess("los cambios serán reflejados en la línea de información correspondiente, dentro de la tabla ''Agregar CDP''", false, false, false, "Los datos fueron actualizados con éxito.");
                    }
                    else {
                        utilidades.mensajeSuccess("Se han adicionado lineas de información en la parte inferior de la tabla ''Agregar CDP''", false, false, false, "Los datos fueron cargados y guardados con éxito.");
                    }
                    vm.cerrar('ok');
                }
                else {
                    utilidades.mensajeError("Error al realizar la operación", false);
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

        function limpiaNumero(valor) {
            return valor.toString().replaceAll(".", "");
        }


        vm.cerrar = function cerrar(result) {
            $uibModalInstance.close(result);
        }
    }

    angular.module('backbone')
        .controller('agregarCdpProyectoModalController', agregarCdpProyectoModalController);

}) ();