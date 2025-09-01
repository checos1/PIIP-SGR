(function () {
    'use strict';

    listadoPoliticasController.$inject = [
        '$scope',
        '$sessionStorage',
        '$uibModal',
        'utilidades',
        'desagregarEdtServicio',
        'listadoPoliticasServicio',
        'utilsValidacionSeccionCapitulosServicio',
        'indicadorPoliticasServicio'
    ];

    function listadoPoliticasController(
        $scope,
        $sessionStorage,
        $uibModal,
        utilidades,
        desagregarEdtServicio,
        listadoPoliticasServicio,
        utilsValidacionSeccionCapitulosServicio,
        indicadorPoliticasServicio
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
        vm.recorrerObjetivosNumber = recorrerObjetivosNumber;
        vm.recorrerObjetivos = recorrerObjetivos;
        vm.habilitaEditar = habilitaEditar;
        vm.configuracion = {
          
        }
        
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

                    if ((n[1].length == 2 && n[1] > 99) || (n[1].length > 2 && n[1] > 99)) {
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
        }

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
                        event.target.value = n[0] + '.' + n[1].slice(0, 2);
                        return;
                    }

                    if ((n[1].length == 2 && n[1] > 99) || (n[1].length > 2 && n[1] > 99)) {
                        event.preventDefault();
                    }
                }
            }
        }

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

        function recorrerObjetivosNumber(event) {

            if (Number.isNaN(event.target.value)) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 3, }).format(0.00);
            }

            if (event.target.value == null) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 3, }).format(0.00);
            }

            if (event.target.value == "") {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 3, }).format(0.00);
            }

            event.target.value = parseFloat(event.target.value.replace(",", "."));

            //recorrerObjetivos(actividad, vigenciaInicial);
        }

        vm.ejecutarErrores = function () {
           
        }
        vm.errores = {
        }
        vm.abrilNivel = function (idElement) {
            idElement = idElement.replace("null", "");
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
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
            actividad.HabilitaEditar = false;
            var data = {
                AvanceCantidades: actividad.AvanceCantidades,
                CostoPeriodo: actividad.CostoPeriodo,
                Igual: $sessionStorage.FlujoIgualPresupuestal,
                ProyectoId: $sessionStorage.proyectoId
            }

            listadoPoliticasServicio.Guardar(data)
                .then(function (response) {
                    let exito = response.data;
                    if (exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.ObtenerResumenObjetivosProductosActividades(actividad);
                        return;
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

        vm.ObtenerResumenObjetivosProductosActividades = function (actividad) {
            console.log(actividad);
            console.log(vm.listadoActividadesNiveles);
            console.log($sessionStorage.bpinProductos);
            indicadorPoliticasServicio.obtenerListadoObjProdNiveles({ BPIN: $sessionStorage.bpinProductos })
                .then(resultado => {
                    if (resultado.data != null) {
                        var data = resultado.data["Objetivos"];
                        console.log(data);
                        for (var i = 0; i < data.length; i++) {
                            for (var j = 0; j < data[i].Productos.length; j++) {
                                if (data[i].Productos[j].Actividades != null) {
                                    for (var k = 0; k < data[i].Productos[j].Actividades.length; k++) {
                                        if (data[i].Productos[j].Actividades[k].ActividadId == actividad.ActividadId &&
                                            data[i].Productos[j].Actividades[k].ActividadProgramacionSeguimientoId == actividad.ActividadProgramacionSeguimientoId) {
                                            actividad = data[i].Productos[j].Actividades[k];
                                            for (var z = 0; z < vm.listadoActividadesNiveles.length; z++) {
                                                if (data[i].Productos[j].Actividades[k].ActividadId == vm.listadoActividadesNiveles[z].ActividadId &&
                                                    data[i].Productos[j].Actividades[k].ActividadProgramacionSeguimientoId == vm.listadoActividadesNiveles[z].ActividadProgramacionSeguimientoId) {
                                                    vm.listadoActividadesNiveles[z] = data[i].Productos[j].Actividades[k];
                                                    console.log(vm.listadoActividadesNiveles[z]);
                                                    z = vm.listadoActividadesNiveles.length;
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
            var variable = $("#icoCE" + vigencia + "-" + actividad)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmasCE-" + vigencia + "-" + actividad);
            var imgmenos = document.getElementById("imgmenosCE-" + vigencia + "-" + actividad);
            var detail = $("#detailCE-" + vigencia + "-" + actividad);
            if (variable === "+") {
                $("#icoCE" + vigencia + "-" + actividad).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';
                if (detail != undefined) detail[0].classList.remove("hidden");

            } else {
                $("#icoCE" + vigencia + "-" + actividad).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
                if (detail != undefined) detail[0].classList.add("hidden");
            }
        }

        vm.AbrilNivelCP = function (vigencia, actividad) {
            var variable = $("#icoCP" + vigencia + "-" + actividad)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmasCP-" + vigencia + "-" + actividad);
            var imgmenos = document.getElementById("imgmenosCP-" + vigencia + "-" + actividad);
            var detail = $("#detailCP-" + vigencia + "-" + actividad);
            if (variable === "+") {
                $("#icoCP" + vigencia + "-" + actividad).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';
                if (detail != undefined) detail[0].classList.remove("hidden");

            } else {
                $("#icoCP" + vigencia + "-" + actividad).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
                if (detail != undefined) detail[0].classList.add("hidden");
            }
        }

        vm.AbrilNivelCFC = function (vigencia, actividad) {
            var variable = $("#icoCFC" + vigencia + "-" + actividad)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmasCFC-" + vigencia + "-" + actividad);
            var imgmenos = document.getElementById("imgmenosCFC-" + vigencia + "-" + actividad);
            var detail = $("#detailCFC-" + vigencia + "-" + actividad);
            if (variable === "+") {
                $("#icoCFC" + vigencia + "-" + actividad).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';
                if (detail != undefined) detail[0].classList.remove("hidden");

            } else {
                $("#icoCFC" + vigencia + "-" + actividad).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
                if (detail != undefined) detail[0].classList.add("hidden");
            }
        }

    }

    angular.module('backbone').component('listadoPoliticas', {
        templateUrl: "src/app/formulario/ventanas/seguimientoControl/componentes/seguimientoFocalizacion/componentes/segIndicadorPoliticas/listadoPoliticas/listadoPoliticas.html",
        controller: listadoPoliticasController,
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