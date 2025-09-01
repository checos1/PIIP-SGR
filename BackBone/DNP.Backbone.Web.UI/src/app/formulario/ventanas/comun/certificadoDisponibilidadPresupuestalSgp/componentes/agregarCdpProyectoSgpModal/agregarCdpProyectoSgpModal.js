(function () {
    'use strict';

    agregarCdpProyectoSgpModal.$inject = [
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

    function agregarCdpProyectoSgpModal(
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
        vm.validateFormat = validateFormat;

        vm.errors = {
            valores: false,
            model: false
        }

        vm.model = {
            idNumero: '',
            idObjeto:'',
            idFecha: '',
            idValorCDPTramite: '',
            idValorTotal: ''
        }

        vm.fechaInicioVigencia = new Date(new Date().getFullYear(), 0, 1)
            vm.fechaFinalVigencia = new Date()

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
                idObjeto: cdpSelected.NumeroContratoCDP,
                idFecha: cdpSelected.FechaCDP,
                idValorCDPTramite: cdpSelected.ValorCDP.toString().replace(".", ","),
                idValorTotal: cdpSelected.ValorTotalCDP.toString().replace(".", ",")
            }
        }

        vm.validarValoresCdp = function () {
            if (vm.model.idValorCDPTramite !== undefined && vm.model.idValorCDPTramite !== '' && vm.model.idValorTotal !== undefined && vm.model.idValorTotal !== '') {
                const valor = Number(vm.model.idValorCDPTramite.replace(",", "."));
                const valorTotal = Number(vm.model.idValorTotal.replace(",", "."));

                vm.errors.valores = valor > valorTotal;
            } /*else {
                vm.errors.valores = false;
            }*/
        }

        vm.validarModelo = function () {
            for (var input in vm.model) {
               
                if (vm.model[input] === undefined || vm.model[input].length === 0) {
                    vm.errors.model = true;
                    return;
                } else if (input == "idValorCDPTramite" && vm.model[input] == "0") {
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
                Descripcion: 'acto administrativo para trámite',
                FechaCDP: vm.model.idFecha,
                IdPresupuestoValoresCDP: 0,
                IdPresupuestoValoresAportaCDP: 0,
                IdProyectoRequisitoTramite: 0,
                IdProyectoTramite: 0,
                IdTipoRequisito: 3,
                NumeroCDP: vm.model.idNumero,
                NumeroContratoCDP: vm.model.idObjeto,
                Tipo: 3,
                UnidadEjecutora: '',
                ValorTotalCDP: limpiaNumero(vm.model.idValorTotal),
                ValorCDP: limpiaNumero(vm.model.idValorCDPTramite),
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
                            utilidades.mensajeSuccess("los cambios serán reflejados en la línea de información correspondiente, dentro de la tabla ''Agregar acto administrativo''", false, false, false, "Los datos fueron actualizados con éxito.");
                        }
                        else {
                            utilidades.mensajeSuccess("Se han adicionado lineas de información en la parte inferior de la tabla ''Agregar acto administrativo''", false, false, false, "Los datos fueron cargados y guardados con éxito.");
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
            return valor.toString().replaceAll(".", "").replace(",", ".");
        }

        vm.cerrar = function cerrar(result) {
            $uibModalInstance.close(result);
        }
        vm.focus = function (event) {
            event.target.value = event.target.value.replaceAll(".", "");
        }

        vm.blur = function (event) {
            event.target.value = formatoNumero(event);
        }
    }

    function validateFormat(event) {

        if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
            event.preventDefault();
        }

        let value = event.target.value.replaceAll(".", "");

        if (value.toString().includes(",")) {
            if (event.keyCode == 44) {
                event.preventDefault();
            }
            else {
                let valDecimal = String(value).split(",");

                if (event.target.selectionStart - 1 >= valDecimal[0].length) {
                    if (valDecimal[1].length >= 2) {
                        event.preventDefault();
                    }
                }
                else {
                    if (valDecimal[0].length >= 14) {
                        event.preventDefault();
                    }
                }
            }
        }
        else {
            if (event.keyCode !== 44) {
                let tamanioNumber = value.length;
                if (tamanioNumber >= 14) {
                    event.preventDefault();
                }
            }
        }
    }

  

    function formatoNumero(event) {
        if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
            event.preventDefault();
        }

        let valConvenioSal;
        let valDecimal;
        let valConvenio = String(event.target.value).split(",");
        if (valConvenio) {

            valConvenioSal = valConvenio[0]
                .replace(/[^\d,]/g, "")
                .replace(/^(\d*\,)(.*)\.(.*)$/, '$1$2$3')
                .replace(/\,(\d{2})\d+/, '.$1')
                .replace(/\B(?=(\d{3})+(?!\d))/g, ".");


            if (valConvenio.length > 1) {
                valDecimal = valConvenio[1].replace(/[^\d,]/g, "");                
                valConvenioSal = valDecimal.length > 1 ? valConvenioSal + "," + valDecimal : valConvenioSal;
            }
        }

        return valConvenioSal;
    }

    angular.module('backbone')
        .controller('agregarCdpProyectoSgpModal', agregarCdpProyectoSgpModal);

})();