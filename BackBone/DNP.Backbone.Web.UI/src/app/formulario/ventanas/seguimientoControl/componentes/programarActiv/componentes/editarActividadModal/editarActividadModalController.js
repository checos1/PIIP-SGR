(function () {
    'use strict';
    angular.module('backbone').controller('editarActividadModalController', editarActividadModalController);

    editarActividadModalController.$inject = [
        '$uibModalInstance',
        '$sessionStorage',
        'utilidades',
        'Nivel',
        'Actividad',
        'ListadoActividadesNivel',
        'ListadoActividadesProducto',
        'UnidadesMedida',
        'editarActividadModalServicio'
    ];

    function editarActividadModalController(
        $uibModalInstance,
        $sessionStorage,
        utilidades,
        Nivel,
        Actividad,
        ListadoActividadesNivel,
        ListadoActividadesProducto,
        UnidadesMedida,
        editarActividadModalServicio
    ) {
        var vm = this;
        var regExpNumberComa = /^[1-9]\d*(\,\d{2,})?$/;
        var regExpNumberComa4dec = /^[1-9]\d*(\,\d{4,})?$/;
        var regExpNumber = /^[1-9]\d*$/;

        vm.isNivel1 = false;
        vm.Actividad = Actividad;
        vm.Nivel = Nivel;
        vm.ListadoActividadesNivel = ListadoActividadesNivel;
        vm.ListadoActividadesProducto = ListadoActividadesProducto;
        vm.UnidadesMedida = UnidadesMedida;
        vm.actualizaFila = actualizaFila;

        vm.listadoActividades = []
        vm.listadoUnidadesMedida = [];
        vm.Tipo = [];
        vm.configuracion = {
            maximoAdelanto: 0.5,
            maximoPosposicion: 0.5
        }
        vm.defaultAct = { Nombre: 'Ninguna', Index: '' }
        vm.actividadDependiente = {}
        vm.editarActividad = {}
        vm.errores = {
            ActividadPredecesora: '',
            ActividadTipo: '',
            CantidadTotal: '',
            CostoTotal: '',
            DuracionOptimista: '',
            DuracionPesimista: '',
            DuracionProbable: '',
            UnidadMedida: '',
            Tipo: '',
            PosPosicion: '',
            Adelanto: '',
            Predecesora: '',
            UnidadMedida: ''
        }

        function init() {
            vm.Tipo = [
                { Id: 'CC', Text: 'Comienzo-Comienzo' },
                { Id: 'FC', Text: 'Fin-Comienzo' },
            ];
            vm.isNivel1 = vm.Nivel == 'Nivel 1';
            vm.listadoActividades = getActividadesFiltradas(vm.ListadoActividadesProducto, vm.Actividad);
            vm.listadoUnidadesMedida = vm.isNivel1 ? [...UnidadesMedida] : [];
            vm.listadoUnidadesMedida.unshift({ Id: '', Text: 'Seleccionar...' });
            setActividad(vm.isNivel1, vm.Actividad, vm.ListadoActividadesProducto);
        }

        function guardar() {
            if (isValido()) {
                var predecesoraId = null;
                var predecesoraSeguimientoId = null;

                if (vm.editarActividad.ActividadPredecesora.Index != "") {
                    var actPredecesora = [...vm.ListadoActividadesProducto].find(p => p.Index == vm.editarActividad.ActividadPredecesora.Index);
                    predecesoraId = actPredecesora.ActividadId;
                    predecesoraSeguimientoId = actPredecesora.SeguimientoEntregableId;
                }

                vm.editarActividad["PredecesoraId"] = predecesoraId;
                vm.editarActividad["SeguimientoEntregablePredecesoraId"] = predecesoraSeguimientoId;
                vm.editarActividad["TipoSigla"] = vm.editarActividad.Tipo.Id;
                vm.editarActividad["UnidadMedidaId"] = vm.editarActividad.UnidadMedida.Id;

                editarActividadModalServicio.editarActividad(vm.editarActividad)
                    .then(resultado => {
                        if (resultado.data != undefined) {
                            if (resultado.data.Status) {
                                utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
                                cerrar('ok');
                            } else {
                                swal("Error al realizar la operación", resultado.data.Message, 'error');
                            }
                        } else {
                            swal('', "Error al realizar la operación", 'error');
                        }
                    });
            }
        }

        function setActividad(isNivel1, actividad, listActividades) {
            vm.editarActividad = {
                ActividadId: actividad.ActividadId,
                SeguimientoEntregableId: actividad.SeguimientoEntregableId,
                ActividadProgramacionSeguimientoId: actividad.ActividadProgramacionSeguimientoId,
                ActividadPredecesora: findActividadPredecesora(listActividades, actividad.PredecesoraId, actividad.SeguimientoEntregablePredecesoraId),
                Tipo: findItem(vm.Tipo, actividad.Tipo),
                CantidadTotal: actividad.CantidadTotal,
                CostoTotal: actividad.CostoTotal,
                DuracionOptimista: actividad.DuracionOptimista,
                DuracionPesimista: actividad.DuracionPesimista,
                DuracionProbable: actividad.DuracionProbable,
                UnidadMedida: isNivel1 ? findItem(vm.listadoUnidadesMedida, actividad.UnidadMedidaId) : actividad.UnidadMedida,
                PosPosicion: actividad.PosPosicion,
                Adelanto: actividad.Adelanto,
                Bpin: $sessionStorage.idObjetoNegocio,
                ProyectoId: $sessionStorage.proyectoId
            };
            vm.onChangePredecesora();
        }

        function getActividadesFiltradas(listActividades, actividad) {
            const indxActividadModal = [...listActividades].findIndex(p => p.ActividadId == actividad.ActividadId && p.SeguimientoEntregableId == actividad.SeguimientoEntregableId);
            const listActividadFiltradas = [...vm.ListadoActividadesProducto];
            listActividadFiltradas.splice(indxActividadModal, 1)
            listActividadFiltradas.forEach((p, indx) => {
                var nombreActividad = p.NombreActividad == null ? p.NombreEntregable : p.NombreActividad
                p["Index"] = indx+1;
                p["Nombre"] = p.Consecutivo + " " + nombreActividad
                return p;
            });

            listActividadFiltradas.unshift(vm.defaultAct)
            return [...listActividadFiltradas];
        }

        function findItem(list, idItem) {
            var item = list.find(p => p.Id == idItem);
            return item != undefined ? item : { Id: '', Text: '' }
        }

        function findActividadPredecesora(list, actividadPredecesoraId, seguimientoEntregablePredecesoraId) {
            const actPredecesora = [...list].find(p => p.ActividadId == actividadPredecesoraId && p.SeguimientoEntregableId == seguimientoEntregablePredecesoraId);
            const actFinal = actPredecesora == undefined ? vm.defaultAct : actPredecesora;
            return actFinal;
        }

        function validarCantidadTotal() {
            var match = regExpNumberComa.test(vm.editarActividad.CantidadTotal);
            if (!match) {
                vm.errores.CantidadTotal = 'Este campo debe ser númerico, puede tener dos decimales separados con coma (,)';
            }
            else vm.errores.CantidadTotal = '';
            return match
        }

        function validarCostoTotalActividad() {
            if (vm.editarActividad.SeguimientoEntregableId == null) {
                vm.errores.CostoTotal = '';
                return true;
            }
            var match = regExpNumberComa4dec.test(vm.editarActividad.CostoTotal);
            if (!match) {
                vm.errores.CostoTotal = 'Este campo debe ser númerico, puede tener cuatro decimales separados con coma (,)';
            }
            else vm.errores.CostoTotal = '';
            return match
        }

        function validarDuracionOptimista() {
            var isValid = true;
            vm.errores.DuracionOptimista = validarNumero(vm.editarActividad.DuracionOptimista);
            isValid = (vm.errores.DuracionOptimista.length <= 0);
            if(isValid) {
                var valoresValidar = {
                    mensajeError: 'La duración optimista debe ser menor o igual a la duración pesimista',
                    numero1: vm.editarActividad.DuracionOptimista,
                    numero2: vm.editarActividad.DuracionPesimista
                }
                vm.errores.DuracionOptimista = validarMayor(valoresValidar)  
                isValid = (vm.errores.DuracionOptimista.length <= 0);              
            }
            return isValid
        }

        function validarDuracionPesimista() {
            var isValid = true;
            vm.errores.DuracionPesimista = validarNumero(vm.editarActividad.DuracionPesimista);
            isValid = (vm.errores.DuracionPesimista.length <= 0);
            if (isValid) {
                var valoresValidar = {
                    mensajeError: 'La duración pesimista debe ser mayor o igual a la duración probable',
                    numero1: vm.editarActividad.DuracionProbable,
                    numero2: vm.editarActividad.DuracionPesimista
                }
                vm.errores.DuracionPesimista = validarMayor(valoresValidar)
                isValid = (vm.errores.DuracionPesimista.length <= 0);
            }
            return isValid
        }

        function validarDuracionProbable() {
            var isValid = true;
            vm.errores.DuracionProbable = validarNumero(vm.editarActividad.DuracionProbable);
            isValid = (vm.errores.DuracionProbable.length <= 0);
            if (isValid) {
                var valoresValidar = {
                    mensajeError: 'La duración probable debe ser mayor o igual a la duración optimista',
                    numero1: vm.editarActividad.DuracionOptimista,
                    numero2: vm.editarActividad.DuracionProbable
                }
                vm.errores.DuracionProbable = validarMayor(valoresValidar)
                isValid = (vm.errores.DuracionProbable.length <= 0);
            }
            return isValid
        }

        function validarMayor(valoresValidar) {
            var error = "";
            var error1 = validarNumero(valoresValidar.numero1);
            var error2 = validarNumero(valoresValidar.numero2);
            var cantidadNumero1 = (error1.length <= 0) ? parseInt(valoresValidar.numero1) : 0;
            var cantidadNumero2 = (error2.length <= 0) ? parseInt(valoresValidar.numero2) : 0;

            if (cantidadNumero1 > cantidadNumero2) {
                error = valoresValidar.mensajeError;
            }
            return error;
        }

        function validarNumero(testExp) {
            var match = regExpNumber.test(testExp);
            var error = ''
            if (!match) {
                error = 'Este campo debe ser un número entero y debe ser mayor a 0';
            }
            return error;
        }

        function validarPosposicion() {
            var esValido = true;
            if (vm.editarActividad.ActividadPredecesora.Index != "" &&
                vm.editarActividad.PosPosicion != null &&
                vm.editarActividad.PosPosicion != "") {

                var actPredecesora = [...vm.ListadoActividadesProducto].find(p => p.Index == vm.editarActividad.ActividadPredecesora.Index)
                vm.errores.PosPosicion = validarNumero(vm.editarActividad.PosPosicion);
                esValido = (vm.errores.PosPosicion.length <= 0)
                if (esValido && (vm.editarActividad.Tipo.Id == 'FC' ||
                    vm.editarActividad.Tipo.Id == 'CC')) {
                    var duracionPromedio = (parseInt(actPredecesora.DuracionOptimista == null ? 0 : actPredecesora.DuracionOptimista) +
                        parseInt(actPredecesora.DuracionPesimista == null ? 0 : actPredecesora.DuracionPesimista) +
                        parseInt(actPredecesora.DuracionProbable == null ? 0 : actPredecesora.DuracionProbable)) / 3;

                    esValido = ((vm.editarActividad.PosPosicion) > (duracionPromedio * vm.configuracion.maximoPosposicion));
                    if (esValido) {
                        vm.errores.PosPosicion = 'La posposición en la dependencia FC o CC no podrá ser superior al ' + (vm.configuracion.maximoPosposicion * 100) + '% de la duración promedio en días de la actividad Predecesora';
                        esValido = false;
                    } else {
                        vm.errores.PosPosicion = ''
                        esValido = true;
                    }
                }
            }
            return esValido;
        }

        function validarAdelanto() {
            var esValido = true;
            if (vm.editarActividad.ActividadPredecesora.Index != "" &&
                vm.editarActividad.Adelanto != null &&
                vm.editarActividad.Adelanto != "") {

                var actPredecesora = [...vm.ListadoActividadesProducto].find(p => p.Index == vm.editarActividad.ActividadPredecesora.Index)
                vm.errores.Adelanto = validarNumero(vm.editarActividad.Adelanto);
                esValido = (vm.errores.Adelanto.length <= 0)

                if (esValido && (vm.editarActividad.Tipo.Id  == 'FC')) {
                    var duracionPromedio = (parseInt(actPredecesora.DuracionOptimista == null ? 0 : actPredecesora.DuracionOptimista) +
                        parseInt(actPredecesora.DuracionPesimista == null ? 0 : actPredecesora.DuracionPesimista) +
                        parseInt(actPredecesora.DuracionProbable == null ? 0 : actPredecesora.DuracionProbable)) / 3;

                    esValido = ((vm.editarActividad.Adelanto) > (duracionPromedio * vm.configuracion.maximoAdelanto));
                    if (esValido) {
                        vm.errores.Adelanto = 'El Adelanto en la dependencia FC no podrá ser superior al ' + (vm.configuracion.maximoAdelanto * 100) + '% de la duración promedio en días de la actividad Predecesora';
                        esValido = false;
                    } else {
                        vm.errores.Adelanto = ''
                        esValido = true;
                    }
                }
            }
            return esValido;
        }

        function validaPredecesora() {
            const actPredecesora = vm.editarActividad.ActividadPredecesora;
            let isError = false;
            if (vm.editarActividad.ActividadPredecesora.Index != '') {
                isError = (vm.Actividad.ActividadId == actPredecesora.PredecesoraId && vm.Actividad.SeguimientoEntregableId == actPredecesora.SeguimientoEntregablePredecesoraId);
                if (isError) {
                    vm.errores.Predecesora = 'No se permiten actividades dependientes entre si: la actividad A es dependiente de B, B no puede ser dependiente de A.'
                }
                else vm.errores.Predecesora = ''
            } else vm.errores.Predecesora = ''

            return !isError;
        }

        function validaUnidadMedida() {
            var isValid = true;
            if (vm.isNivel1 && vm.editarActividad.UnidadMedida.Id == "") {
                vm.errores.UnidadMedida = "Este campo es oblgatorio";
                isValid = false;
            } else {
                vm.errores.UnidadMedida = "";
            }
            return isValid;
        }
        function onChangePredecesora() {
            vm.errores.Predecesora = ''

            /* ---- Validaciones actividad Predecesora NINGUNA ---- */
            if (this.editarActividad.ActividadPredecesora.Index == '') {
                var tipo = vm.Tipo.find(p => p.Id == 'FC')
                vm.editarActividad.Tipo = tipo;
                vm.editarActividad.PosPosicion = null;
                vm.editarActividad.Adelanto = null;
            }
        }

        function onChangeTipo() {         
            if (vm.editarActividad.Tipo.Id != 'FC') {
                vm.editarActividad.Adelanto = null;
            }
        }

        function isValido() {
            var isValidoCantidadTotal = validarCantidadTotal();
            var isValidoCantidadTotalActividad = validarCostoTotalActividad();
            var isValidoDuracionOptimista = validarDuracionOptimista();
            var isValidoDuracionPesimista = validarDuracionPesimista();
            var isValidoDuracionProbable = validarDuracionProbable();
            var isValidoPosposicion = validarPosposicion();
            var isValidoAdelanto = validarAdelanto();
            var isValidoActividadPredecesora = validaPredecesora();
            var isValidoUnidadMedida = validaUnidadMedida();

            return (isValidoCantidadTotal &&
                isValidoCantidadTotalActividad &&
                isValidoDuracionOptimista &&
                isValidoDuracionPesimista &&
                isValidoDuracionProbable &&
                isValidoPosposicion &&
                isValidoAdelanto &&
                isValidoActividadPredecesora &&
                isValidoUnidadMedida);
        }

        function cancelar() {
            utilidades.mensajeWarning("Los posibles datos que haya diligenciado se perderán. ¿Está seguro de continuar?", function funcionContinuar() {
                setTimeout(function () {
                    cerrar('cerrar')
                }, 500);
            }, function funcionCancelar(reason) {
                return;
            });
        }
        function cerrar(result) {
            $uibModalInstance.close(result);
        }

        vm.validateFormat = function (event, cantidad) {
            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44 && event.keyCode != 46) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                vm.puntoDigitado = false;
                if (cantidad == 4)
                    tamanioPermitido = 16;
                else
                    tamanioPermitido = 14;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, cantidad);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > cantidad) {
                        tamanioPermitido = n[0].length + cantidad;
                        event.target.value = n[0] + '.' + n[1].slice(0, cantidad);
                        return;
                    }

                    if (cantidad == 2) {
                        if ((n[1].length == 1 && n[1] > 9) || (n[1].length > 1 && n[1] > 99)) {
                            event.preventDefault();
                        }
                    }
                    else {
                        if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                            event.preventDefault();
                        }
                    }

                }
            } else {
                if (tamanio > 12 && event.keyCode != 44 && event.keyCode != 46) {
                    event.preventDefault();
                }
            }

            if ((event.keyCode == 44 || event.keyCode == 46) && tamanio == 12) {
                vm.puntoDigitado = true;
            }
            else if (vm.puntoDigitado && tamanio == 12) {
                vm.puntoDigitado = false;
            }
            else {
                if (cantidad == 4) {
                    if (tamanio > tamanioPermitido || tamanio > 16) {
                        event.preventDefault();
                    }
                }
                else {
                    if (tamanio > tamanioPermitido || tamanio > 14) {
                        event.preventDefault();
                    }
                }

            }
        }

        vm.validarTamanio = function (event, cantidad) {

            if (Number.isNaN(event.target.value)) {
                event.target.value = "0"
                return;
            }

            if (event.target.value == null) {
                event.target.value = "0"
                return;
            }

            if (event.target.value == "") {
                event.target.value = "0"
                return;
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");

            if (permitido) {
                vm.puntoDigitado = false;
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                if (decimales > cantidad) {
                }
                if (cantidad == 4)
                    tamanioPermitido = 16;
                else
                    tamanioPermitido = 14;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, cantidad);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > cantidad) {
                        tamanioPermitido = n[0].length + cantidad;
                        event.target.value = n[0] + '.' + n[1].slice(0, cantidad);
                        return;
                    }

                    if (cantidad == 2) {
                        if ((n[1].length == 1 && n[1] > 9) || (n[1].length > 1 && n[1] > 99)) {
                            event.preventDefault();
                        }
                    }
                    else {
                        if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                            event.preventDefault();
                        }
                    }
                }
            }
        }

        function actualizaFila(event, cantidad) {

            if (Number.isNaN(event.target.value)) {
                if (cantidad == 2)
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                else
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
            }

            if (event.target.value == null) {
                if (cantidad == 2)
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                else
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
            }

            if (event.target.value == "") {
                if (cantidad == 2)
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                else
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
            }

            event.target.value = parseFloat(event.target.value.replace(",", "."));

            const val = event.target.value;
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > cantidad ? event.target.value : parseFloat(val).toFixed(cantidad);
            return new Intl.NumberFormat('es-co', { minimumFractionDigits: cantidad, }).format(total);
        }

        vm.init = init;
        vm.guardar = guardar;
        vm.cerrar = cerrar;
        vm.cancelar = cancelar;
        vm.onChangePredecesora = onChangePredecesora;
        vm.onChangeTipo = onChangeTipo;
    }
})();