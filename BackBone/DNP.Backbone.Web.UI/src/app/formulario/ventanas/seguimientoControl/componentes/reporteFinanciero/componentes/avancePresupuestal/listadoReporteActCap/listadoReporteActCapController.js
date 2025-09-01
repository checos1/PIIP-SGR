(function () {
    'use strict';

    listadoReporteActCapController.$inject = [
        '$scope',
        '$sessionStorage',
        '$uibModal',
        'utilidades',
        'desagregarEdtServicio',
        'listadoReporteActCapServicio',
        'utilsValidacionSeccionCapitulosServicio',
        'reporteActCapServicio'
    ];

    function listadoReporteActCapController(
        $scope,
        $sessionStorage,
        $uibModal,
        utilidades,
        desagregarEdtServicio,
        listadoReporteActCapServicio,
        utilsValidacionSeccionCapitulosServicio,
        reporteActCapServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.soloLectura = $sessionStorage.soloLectura;
        vm.listadoObjProdNiveles = [];
        vm.listadoActividadesNiveles = [];
        vm.listadoActividadesProducto = [];
        vm.listadoUnidadesMedida = []
        vm.notificacionValidacionPadre = null;
        vm.editarIguales = false;
        vm.amount = '1700000023.23';
        vm.cambiarPagina = cambiarPagina;
        vm.pagina = 0;
        vm.recorrerObjetivos = recorrerObjetivos;
        vm.habilitaEditar = habilitaEditar;
        vm.configuracion = {

        }
        vm.anteriorIdElemento = '';
        vm.actividadSeleccionada = 0;
        vm.entregableSeleccionado = 0;
        vm.vigenciaCFCSeleccionada = 0;
        vm.vigenciaCPSeleccionada = 0;
        vm.vigenciaCESeleccionada = 0;
        vm.elementIdCP = "";
        vm.elementMasCP = "";
        vm.elementMenosCP = "";
        vm.elementIdCE = "";
        vm.elementMasCE = "";
        vm.elementMenosCE = "";
        vm.elementIdCFC = "";
        vm.elementMasCFC = "";
        vm.elementMenosCFC = "";
        vm.elementCP = "";
        vm.elementCE = "";
        vm.elementCFC = "";

        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
        };

        $scope.$watch("vm.listadoactividades", function () {
            if (vm.listadoactividades != undefined) {
                vm.listadoActividadesNiveles = [...vm.listadoactividades];
            }
        })

        $scope.$watch("vm.listadoactividadesproducto", function () {
            if (vm.listadoactividadesproducto != undefined) {
                vm.listadoActividadesProducto = [...vm.listadoactividadesproducto];

                vm.setNombrePredecesora([...vm.listadoActividadesProducto])
            }
        })

        $scope.$watch("vm.unidadesmedida", function () {
            if (vm.unidadesmedida != undefined) {
                vm.listadoUnidadesMedida = [...vm.unidadesmedida];
            }
        })


        /*--------------------- Handler ------------------------*/
        vm.reloadActividades = function () {
        }
        /*--------------------- Editar actividades ---------------------*/

        vm.abrirModalEditar = function (actividadSel) {
            var templateUrl = "src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avancePresupuestal/editarActividadModal/editarActividadModal.html"
            var controller = "editarActividadPresupuestalModalController";
            try {
                let modalInstance = $uibModal.open({
                    templateUrl: templateUrl,
                    controller: controller,
                    controllerAs: "vm",
                    openedClass: "modal-contentDNP",
                    size: 'lg',
                    resolve: {
                        Nivel: function () {
                            return vm.nivel
                        },
                        Actividad: function () {
                            return actividadSel;
                        },
                        ListadoActividadesNivel: function () {
                            return vm.listadoActividadesNiveles
                        },
                        ListadoActividadesProducto: function () {
                            return vm.listadoActividadesProducto
                        },
                        UnidadesMedida: function () {
                            return vm.listadoUnidadesMedida
                        }
                    },
                });
                modalInstance.result.then(data => {
                    if (data != null) {
                        if (data == 'ok') {
                            vm.reload({ actividad: actividadSel })
                        } else if (data == 'cerrar') {
                            utilidades.mensajeSuccess('', false, false, false, "Se ha cancelado la edición");
                        }
                    }
                });
            } catch (error) {
            }
        }

        /*--------------------- Editar información actividades  ------------------*/

        vm.setNombrePredecesora = function (listadoActividadesProducto) {
            vm.listadoActividadesNiveles.forEach(act => {
                var actPredecesora = listadoActividadesProducto.find(p => p.ActividadId == act.PredecesoraId);
                if (actPredecesora != undefined) {
                    act["NombrePredecesora"] = actPredecesora.Consecutivo + " " + (actPredecesora.NombreActividad != null ? actPredecesora.NombreActividad : actPredecesora.NombreEntregable);
                }
                if (act.DuracionOptimista > 0 &&
                    act.DuracionPesimista > 0 &&
                    act.DuracionProbable > 0) act["Promedio"] = Math.ceil((act.DuracionOptimista + act.DuracionPesimista + act.DuracionProbable) / 3);
                else act["Promedio"] = 0
            })
        }

        /*--------------------- Programar actividades ---------------------*/


        /*--------------------- Validaciones ---------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Programar Actividades Listado Actividades", errores);
            if (errores != undefined) {

            }
        }

        vm.limpiarErrores = function () {

        }

        vm.actualizaFila = function (event) {
            if (Number.isNaN(event.target.value)) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }
            if (event.target.value == null) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }
            if (event.target.value == "") {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }
            event.target.value = parseFloat(event.target.value.replace(",", "."));


            const val = event.target.value;
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
            return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);


        };

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 2) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + '.' + n[1].slice(0, 2);
                        return;
                    }

                    if ((n[1].length == 1 && n[1] > 999) || (n[1].length > 1 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            } else {
                if (tamanio > 12 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 12) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 16) {
                    event.preventDefault();
                }
            }
        };

        vm.validateFormatNegative = function (event) {

            if ((event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            if (event.key == '.') {
                event.preventDefault();
            }

            //var value = event.target.value;

            //// Verificar si hay más de un signo negativo en el valor
            //var count = (value.match(/-/g) || []).length;
            //if (count > 1) {
            //    event.target.value = value.slice(0, -1); // Eliminar el último signo negativo adicional
            //}


            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 13;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 15;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[1], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1) return;
                    if (spiltArray.length === 2) return;

                    if (n[1].length > 2) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + '.' + n[1].slice(0, 2);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            } else {
                if (tamanio > 15 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 15) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 15) {
                    event.preventDefault();
                }
            }
        };

        vm.validarTamanio = function (event) {

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
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                if (decimales > 2) {
                }
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 2) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + '.' + n[1].slice(0, 4);
                        return;
                    }

                    if ((n[1].length == 1 && n[1] > 999) || (n[1].length > 1 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            }
        };

        vm.validarTamanioNegative = function (event) {

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
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                if (decimales > 2) {
                }
                tamanioPermitido = 15;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '.') return;

                    if (n[1].length > 2) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + '.' + n[1].slice(0, 3);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            }
        };

        function cambiarPagina(index) {
            vm.pagina = index;
        }

        function recorrerObjetivos(actividad, vigenciaInicial) {
            for (var i = 0; i < actividad.Vigencias.length; i++) {
                var sumarTotalVigencia = 0;
                for (var j = 0; j < actividad.Vigencias[i].ProgramacionSeguimientoPeriodosValores.length; j++) {
                    sumarTotalVigencia = sumarTotalVigencia + parseFloat(actividad.Vigencias[i].ProgramacionSeguimientoPeriodosValores[j].Valor);
                }
                actividad.Vigencias[i].TotalVigencia = sumarTotalVigencia;
            }
        }

        vm.ejecutarErrores = function () {

        }
        vm.errores = {
        }

        vm.abrilNivel = function (idElement, actividadId, seguimientoEntregableId) {
            vm.vigenciaCFCSeleccionada = 0;
            vm.vigenciaCPSeleccionada = 0;
            vm.vigenciaCESeleccionada = 0;
            if (vm.anteriorIdElemento != '' && vm.anteriorIdElemento != idElement) {
                vm.anteriorIdElemento = vm.anteriorIdElemento.replace("null", "");
                var elMasAnterior = document.getElementById(vm.anteriorIdElemento + '-mas');
                var elMenosAnterior = document.getElementById(vm.anteriorIdElemento + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

            if (vm.elementCP != "") {
                $(vm.elementIdCP).html('+');
                vm.elementMasCP.style.display = 'block';
                vm.elementMenosCP.style.display = 'none';
            }

            if (vm.elementCE != "") {
                $(vm.elementIdCE).html('+');
                vm.elementMasCE.style.display = 'block';
                vm.elementMenosCE.style.display = 'none';
            }

            if (vm.elementCFC != "") {
                $(vm.elementIdCFC).html('+');
                vm.elementMasCFC.style.display = 'block';
                vm.elementMenosCFC.style.display = 'none';
            }

            vm.anteriorIdElemento = idElement;
            idElement = idElement.replace("null", "");
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.actividadSeleccionada = 0;
                    vm.entregableSeleccionado = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.actividadSeleccionada = actividadId;
                    vm.entregableSeleccionado = seguimientoEntregableId;
                }
            }
        }

        function habilitaEditar(actividad, tipo) {
            if ($sessionStorage.FlujoIgualPresupuestal != 1 && $sessionStorage.FlujoIgualPresupuestal != 2 && $sessionStorage.FlujoIgualPresupuestal != '1' && $sessionStorage.FlujoIgualPresupuestal != '2') {
                utilidades.mensajeError("Debe diligenciar en la sección Avance Financiero, la pregunta ¿El avance del flujo de caja es igual al avance presupuestal?", false);
                return false;
            }
            if (tipo == 1) {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    actividad.HabilitaEditar = false;
                    for (var i = 0; i < actividad.AvanceCantidades.length; i++) {
                        actividad.AvanceCantidades[i].CantidadEjecutadaMes = actividad.AvanceCantidades[i].CantidadEjecutadaMesAnterior;
                        actividad.AvanceCantidades[i].Observaciones = actividad.AvanceCantidades[i].ObservacionesAnterior;
                    }
                    for (var i = 0; i < actividad.CostoPeriodo.length; i++) {
                        actividad.CostoPeriodo[i].CostoEjecutadoMes = actividad.CostoPeriodo[i].CostoEjecutadoMesAnterior;
                        actividad.CostoPeriodo[i].Observaciones = actividad.CostoPeriodo[i].ObservacionesAnterior;
                    }
                    OkCancelar();

                }, function funcionCancelar(reason) {
                    console.log("reason", reason);
                }, null, null, "Los posibles datos que haya diligenciado en la tabla 'Detalle beneficiarios totales' se perderán.");
            }
            else {
                if ($sessionStorage.FlujoIgualPresupuestal == 1 || $sessionStorage.FlujoIgualPresupuestal == '1')
                    vm.editarIguales = true;
                else
                    vm.editarIguales = false;
                actividad.HabilitaEditar = true;
            }
        }

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "Se ha cancelado la edición.");
            }, 500);
        }

        vm.actualizarVigencia = function (actividad) {

            if (actividad.CostoPeriodo[0].TipoCosto === "Presupuestal" && actividad.CostoPeriodo[0].CostoEjecutadoMes.toString().includes(",")) {
                actividad.CostoPeriodo[0].CostoEjecutadoMes = actividad.CostoPeriodo[0].CostoEjecutadoMes.replace(',', '.');
            }

            actividad.CostoPeriodo.forEach(act => {
                if (act.TipoCosto === "Presupuestal" && act.CostoEjecutadoMes.toString().includes(",")) {
                    act.CostoEjecutadoMes = act.CostoEjecutadoMes.replace(',', '.');
                }
            })

            var data = {
                AvanceCantidades: actividad.AvanceCantidades,
                CostoPeriodo: actividad.CostoPeriodo,
                Igual: $sessionStorage.FlujoIgualPresupuestal,
                ProyectoId: $sessionStorage.proyectoId,
                ActividadId: actividad.ActividadId
            }

            listadoReporteActCapServicio.Guardar(data)
                .then(function (response) {
                    let exito = response.data;
                    if (response.data.Status) {
                        actividad.HabilitaEditar = false;
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.reload({ actividad: actividad })
                        vm.ObtenerResumenObjetivosProductosActividades(actividad);
                        return;
                    }
                    else {
                        utilidades.mensajeError("Error al realizar la operación" || response.data.Message, false);
                    }
                })
                .catch(error => {
                    console.log(error);
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación");
                });
        }

        vm.ObtenerResumenObjetivosProductosActividades = function (actividad) {
            console.log(actividad);
            console.log(vm.listadoActividadesNiveles);
            console.log($sessionStorage.idObjetoNegocio);
            reporteActCapServicio.obtenerListadoObjProdNiveles({ BPIN: $sessionStorage.idObjetoNegocio })
                .then(resultado => {
                    if (resultado.data != null) {
                        var data = resultado.data["Objetivos"];
                        console.log(data);
                        for (var i = 0; i < data.length; i++) {
                            for (var j = 0; j < data[i].Productos.length; j++) {
                                if (data[i].Productos[j].Actividades != null) {
                                    for (var k = 0; k < data[i].Productos[j].Actividades.length; k++) {
                                        if (data[i].Productos[j].Actividades[k].ActividadId == actividad.ActividadId) {
                                            actividad = data[i].Productos[j].Actividades[k];
                                            for (var z = 0; z < vm.listadoActividadesNiveles.length; z++) {
                                                if (data[i].Productos[j].Actividades[k].ActividadId == vm.listadoActividadesNiveles[z].ActividadId) {
                                                    vm.listadoActividadesNiveles[z] = data[i].Productos[j].Actividades[k];
                                                    console.log(vm.listadoActividadesNiveles[z]);
                                                    z = vm.listadoActividadesNiveles.length;

                                                    if (actividad.SeguimientoEntregableId == null || actividad.SeguimientoEntregableId == 'null' || actividad.SeguimientoEntregableId == undefined) {
                                                        setTimeout(function () {
                                                            $("#ReporActividadProgramar-" + actividad.ActividadId + '-').collapse("toggle");
                                                            vm.abrilNivel('reporProgramarActividad-' + actividad.ActividadId + '-', actividad.ActividadId, actividad.SeguimientoEntregableId)
                                                        }, 500)

                                                    }
                                                    else {
                                                        $("#ReporActividadProgramar-" + actividad.ActividadId + '-' + actividad.SeguimientoEntregableId).collapse("toggle");
                                                        vm.abrilNivel('reporProgramarActividad-' + actividad.ActividadId + '-' + actividad.SeguimientoEntregableId, actividad.ActividadId, actividad.SeguimientoEntregableId);
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                                else {
                                    for (var k = 0; k < data[i].Productos[j].EntregablesNivel1.length; k++) {
                                        for (var l = 0; l < data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados.length; l++) {
                                            if (data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos.length > 0) {
                                                for (var m = 0; m < data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos.length; m++) {
                                                    if (data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m].Hijos.length > 0) {
                                                        for (var n = 0; n < data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m].Hijos.length; n++) {
                                                            if (data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m].Hijos[n].ActividadId == actividad.ActividadId &&
                                                                data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m].Hijos[n].SeguimientoEntregableId == actividad.SeguimientoEntregableId) {
                                                                actividad = data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m].Hijos[n];
                                                                console.log(actividad);
                                                                for (var z = 0; z < vm.listadoActividadesNiveles.length; z++) {
                                                                    if (data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m].Hijos[n].ActividadId == vm.listadoActividadesNiveles[z].ActividadId &&
                                                                        data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m].Hijos[n].SeguimientoEntregableId == vm.listadoActividadesNiveles[z].SeguimientoEntregableId) {
                                                                        vm.listadoActividadesNiveles[z] = data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m].Hijos[n];
                                                                        console.log(vm.listadoActividadesNiveles[z]);
                                                                        z = vm.listadoActividadesNiveles.length;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else {
                                                        if (data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m].ActividadId == actividad.ActividadId &&
                                                            data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m].SeguimientoEntregableId == actividad.SeguimientoEntregableId) {
                                                            actividad = data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m];
                                                            console.log(actividad);
                                                            for (var z = 0; z < vm.listadoActividadesNiveles.length; z++) {
                                                                if (data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m].ActividadId == vm.listadoActividadesNiveles[z].ActividadId &&
                                                                    data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m].SeguimientoEntregableId == vm.listadoActividadesNiveles[z].SeguimientoEntregableId) {
                                                                    vm.listadoActividadesNiveles[z] = data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m];
                                                                    console.log(vm.listadoActividadesNiveles[z]);
                                                                    z = vm.listadoActividadesNiveles.length;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else {
                                                if (data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].ActividadId == actividad.ActividadId &&
                                                    data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].SeguimientoEntregableId == actividad.SeguimientoEntregableId) {
                                                    actividad = data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l];
                                                    console.log(actividad);
                                                    for (var z = 0; z < vm.listadoActividadesNiveles.length; z++) {
                                                        if (data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].ActividadId == vm.listadoActividadesNiveles[z].ActividadId &&
                                                            data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].SeguimientoEntregableId == vm.listadoActividadesNiveles[z].SeguimientoEntregableId) {
                                                            vm.listadoActividadesNiveles[z] = data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l];
                                                            console.log(vm.listadoActividadesNiveles[z]);
                                                            z = vm.listadoActividadesNiveles.length;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }

                            }
                        }
                    }
                });
        }

        vm.AbrilNivelCE = function (vigencia, actividad) {
            vm.vigenciaCFCSeleccionada = 0;
            vm.vigenciaCPSeleccionada = 0;
            if (vm.elementCE != "") {
                $(vm.elementIdCE).html('+');
                vm.elementMasCE.style.display = 'block';
                vm.elementMenosCE.style.display = 'none';
            }

            if (vm.elementCP != "") {
                $(vm.elementIdCP).html('+');
                vm.elementMasCP.style.display = 'block';
                vm.elementMenosCP.style.display = 'none';
            }

            if (vm.elementCFC != "") {
                $(vm.elementIdCFC).html('+');
                vm.elementMasCFC.style.display = 'block';
                vm.elementMenosCFC.style.display = 'none';
            }

            vm.elementIdCE = "#icoCE" + vigencia + "-" + actividad;
            var variable = $("#icoCE" + vigencia + "-" + actividad)[0].innerText;
            variable = variable.replace(/ /g, "");
            vm.elementCE = variable;
            var imgmas = document.getElementById("imgmasCE-" + vigencia + "-" + actividad);
            vm.elementMasCE = imgmas;
            var imgmenos = document.getElementById("imgmenosCE-" + vigencia + "-" + actividad);
            vm.elementMenosCE = imgmenos;
            if (variable === "+") {
                $("#icoCE" + vigencia + "-" + actividad).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';
                vm.vigenciaCESeleccionada = vigencia;

            } else {
                $("#icoCE" + vigencia + "-" + actividad).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
                vm.vigenciaCESeleccionada = 0;
            }
        }

        vm.AbrilNivelCP = function (vigencia, actividad) {
            vm.vigenciaCESeleccionada = 0;
            vm.vigenciaCFCSeleccionada = 0;
            if (vm.elementCP != "") {
                $(vm.elementIdCP).html('+');
                vm.elementMasCP.style.display = 'block';
                vm.elementMenosCP.style.display = 'none';
            }

            if (vm.elementCFC != "") {
                $(vm.elementIdCFC).html('+');
                vm.elementMasCFC.style.display = 'block';
                vm.elementMenosCFC.style.display = 'none';
            }

            if (vm.elementCE != "") {
                $(vm.elementIdCE).html('+');
                vm.elementMasCE.style.display = 'block';
                vm.elementMenosCE.style.display = 'none';
            }

            vm.elementIdCP = "#icoCP" + vigencia + "-" + actividad;
            var variable = $("#icoCP" + vigencia + "-" + actividad)[0].innerText;
            variable = variable.replace(/ /g, "");
            vm.elementCP = variable;
            var imgmas = document.getElementById("imgmasCP-" + vigencia + "-" + actividad);
            vm.elementMasCP = imgmas;
            var imgmenos = document.getElementById("imgmenosCP-" + vigencia + "-" + actividad);
            vm.elementMenosCP = imgmenos;
            if (variable === "+") {
                $("#icoCP" + vigencia + "-" + actividad).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';
                vm.vigenciaCPSeleccionada = vigencia;

            } else {
                $("#icoCP" + vigencia + "-" + actividad).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
                vm.vigenciaCPSeleccionada = 0;
            }
        }

        vm.AbrilNivelCFC = function (vigencia, actividad) {
            vm.vigenciaCESeleccionada = 0;
            vm.vigenciaCPSeleccionada = 0;
            if (vm.elementCFC != "") {
                $(vm.elementIdCFC).html('+');
                vm.elementMasCFC.style.display = 'block';
                vm.elementMenosCFC.style.display = 'none';
            }

            if (vm.elementCP != "") {
                $(vm.elementIdCP).html('+');
                vm.elementMasCP.style.display = 'block';
                vm.elementMenosCP.style.display = 'none';
            }

            if (vm.elementCE != "") {
                $(vm.elementIdCE).html('+');
                vm.elementMasCE.style.display = 'block';
                vm.elementMenosCE.style.display = 'none';
            }

            vm.elementIdCFC = "#icoCFC" + vigencia + "-" + actividad;
            var variable = $("#icoCFC" + vigencia + "-" + actividad)[0].innerText;
            variable = variable.replace(/ /g, "");
            vm.elementCFC = variable;
            var imgmas = document.getElementById("imgmasCFC-" + vigencia + "-" + actividad);
            vm.elementMasCFC = imgmas;
            var imgmenos = document.getElementById("imgmenosCFC-" + vigencia + "-" + actividad);
            vm.elementMenosCFC = imgmenos;
            if (variable === "+") {
                $("#icoCFC" + vigencia + "-" + actividad).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';
                vm.vigenciaCFCSeleccionada = vigencia;

            } else {
                $("#icoCFC" + vigencia + "-" + actividad).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
                vm.vigenciaCFCSeleccionada = 0;
            }
        }

    }

    angular.module('backbone').component('listadoReporteActCap', {
        templateUrl: "src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avancePresupuestal/listadoReporteActCap/listadoReporteActCap.html",
        controller: listadoReporteActCapController,
        controllerAs: "vm",
        bindings: {
            listadoactividades: '<',
            listadoactividadesproducto: '<',
            nivel: '<',
            unidadesmedida: '<',
            reload: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&'
        }
    });

})();