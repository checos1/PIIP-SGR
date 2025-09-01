(function () {
    'use strict';

    angular.module('backbone.entidades')
        .controller('consultarPagosModalController', consultarPagosModalController);

    consultarPagosModalController.$inject = [
        '$scope',
        '$uibModalInstance',
        '$filter',
        'inflexibilidad',
        'inflexibilidadServicio',
        'utilidades',
        'archivoServicios',
        'FileSaver'
    ];

    function consultarPagosModalController(
        $scope,
        $uibModalInstance,
        $filter,
        inflexibilidad,
        inflexibilidadServicio,
        utilidades,
        archivoServicios,
        FileSaver
    ) {
        const vm = this;

        //#region Variables
        vm.inflexibilidad = inflexibilidad;
        vm.pagamentoSeleccionadaParaSubirArchivo = {};
        vm.idAplicacion = "inflexibilidad";

        vm.pagamentos = [];

        vm.adicionarPagamento = adicionarPagamento;
        vm.removerPagamento = removerPagamento;
        vm.enviarDocumento = enviarDocumento;
        vm.visualizar = visualizar;
        vm.guardar = guardar;
        vm.cerrar = cerrar;

        const _validadores = [
            (pagamentos) => {
                return pagamentos.reduce((acc, pagamento, index) => {
                    const numero = ++index;
                    acc.push(...[
                        { invalido: !pagamento.fechaFin, mensaje: `Ingrese una fecha de finalización en el pago de número ${numero}` },
                        { invalido: !pagamento.fechaInicio, mensaje: `Ingrese una fecha de inicio en el pago de número ${numero}` },
                        { invalido: !pagamento.valor, mensaje: `Ingrese un valor en el pago de número ${numero}` },
                    ])

                    return acc;
                }, [])
            }
        ];

        //#endregion

        //#region Metodos

        function adicionarPagamento() {
            vm.pagamentos.push({
                valor: null,
                fechaInicio: null,
                fechaFin: null,
                IdInflexibilidad: inflexibilidad.id
            });
        }

        function removerPagamento(index) {

            if (index == null || index == undefined)
                return;

            if (vm.pagamentos.length == 1) {
                toastr.warning("Debe tener al menos un pago");
                return;
            }
            //  let rowIndex = vm.pagamentos.indexOf(index);
            vm.pagamentos.splice(index, 1);

            return;
        }

        function visualizar(pagamento) {
            if (pagamento.idArchivoBlob) {
                if (pagamento.contentType) {
                    let parametrosContentType = pagamento.contentType.split(',');
                    descargarArchivo({
                        IdArchivoBlob: pagamento.idArchivoBlob,
                        ContenType: parametrosContentType[1],
                        NombreCompleto: `comprobante.${parametrosContentType[0]}`
                    });
                }
                else {
                    toastr.warning("No hay archivo para descargar");
                }
            }
            else {
                toastr.warning("No hay archivo para descargar");
            }
        }

        function enviarDocumento(pagamento) {
            if (!pagamento.Id) {
                toastr.warning('Primero guarde el registro antes de enviar un archivo');
                return;
            }
            vm.pagamentoSeleccionadaParaSubirArchivo = pagamento;
            document.getElementById("file").click();
        }

        function validarExtension(extension) {
            switch (extension.toLowerCase()) {
                case 'exe': case 'bin': case 'src': case 'vbs': return false;
                default: return true;
            }
        }

        function obtenerExtension(nombreArchivo) {
            let partes = nombreArchivo.split('.');
            return partes[partes.length - 1];
        }

        vm.cargarArchivo = function () {
            let fileControl = document.getElementById("file");
            let archivo = {
                FormFile: fileControl.files[0],
                Nombre: fileControl.files[0].name,
                Metadatos: {}
            };
            vm.extension = obtenerExtension(archivo.Nombre);
            if (!validarExtension(vm.extension)) {
                toastr.warning('Extensión no permitida');
                return;
            }
            vm.contentType = fileControl.files[0].type;
            archivoServicios.cargarArchivo(archivo, vm.idAplicacion).then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    //Actualizar el idArchivoBlob en el pago de inflexibilidad
                    vm.pagamentoSeleccionadaParaSubirArchivo.idArchivoBlob = response[0].idArchivoBlob;
                    vm.pagamentoSeleccionadaParaSubirArchivo.contentType = `${vm.extension},${vm.contentType}`;
                    guardarPagamento(vm.pagamentoSeleccionadaParaSubirArchivo);
                }
            }, error => {
                console.log(error);
            });
        };

        function _validarPagamentos(pagamentos) {
            try {
                const mensajes = [];
                _validadores.forEach(validator => {
                    validator(pagamentos).forEach(resultado => {
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
                toastr.error("Error inesperado al validar");
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

        function guardar() {
            const valido = _validarPagamentos(vm.pagamentos);
            if (!valido)
                return;

            inflexibilidadServicio.guardarInflexibilidadPagos(vm.pagamentos)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.cerrar()
                    } else
                        utilidades.mensajeError(response.data.Mensaje, false);
                });
        }

        function guardarPagamento(pagamento) {
            inflexibilidadServicio.actualizarIdArchivo(pagamento)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess($filter('language')('ArchivoGuardado'), false, false, false);
                    } else
                        utilidades.mensajeError(response.data.Mensaje, false);
                });
        }

        function cerrar() {
            $uibModalInstance.close(false);
        }

        /// Comienzo
        vm.$onInit = function () {
            inflexibilidadServicio.obtenerInflexibilidadPagos(vm.inflexibilidad.id)
                .then(function (response) {
                    if (response.data) {
                        response.data.forEach(item => {
                            vm.pagamentos.push({
                                Id: item.Id,
                                valor: item.Valor,
                                fechaInicio: new Date(item.FechaInicio),
                                fechaFin: new Date(item.FechaFin),
                                IdInflexibilidad: item.IdInflexibilidad,
                                idArchivoBlob: item.IdArchivoBlob,
                                contentType: item.ContentType
                            });
                        });
                        adicionarPagamento();
                    } else
                        adicionarPagamento();
                });
        }

        /**
         * Permite descargar el archivo seleccionado
         * @param {object} entity
         */
        function descargarArchivo(entity) {
            if (entity.IdArchivoBlob != undefined) {
                //Obtener los metadatos
                archivoServicios.obtenerArchivoInfo(entity.IdArchivoBlob, vm.idAplicacion).then(result => {
                    let metadatos = result;
                    archivoServicios.obtenerArchivoBytes(entity.IdArchivoBlob, vm.idAplicacion).then(function (retorno) {
                        const blob = utilidades.base64toBlob(retorno, entity.ContenType);
                        //const blob = new Blob([retorno], { type: entity.ContenType });
                        FileSaver.saveAs(blob, entity.NombreCompleto);
                    }, function (error) {
                        toastr.error("Error inesperado al descargar");
                    });
                })
            }
        }

        //#endregion
    }
})();

