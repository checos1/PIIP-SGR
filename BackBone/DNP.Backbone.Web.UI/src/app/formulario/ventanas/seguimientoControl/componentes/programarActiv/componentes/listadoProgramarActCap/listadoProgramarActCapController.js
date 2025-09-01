(function () {
    'use strict';

    listadoProgramarActCapController.$inject = [
        '$scope',
        '$sessionStorage',
        '$uibModal',
        'utilidades',
        'desagregarEdtServicio',
        'listadoProgramarActCapServicio',
        'utilsValidacionSeccionCapitulosServicio',
        'programarActCapServicio'
    ];

    function listadoProgramarActCapController(
        $scope,
        $sessionStorage,
        $uibModal,
        utilidades,
        desagregarEdtServicio,
        listadoProgramarActCapServicio,
        utilsValidacionSeccionCapitulosServicio,
        programarActCapServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "programaractivprogramaractcap";
        vm.soloLectura = $sessionStorage.soloLectura;
        vm.listadoObjProdNiveles = [];
        vm.listadoActividadesNiveles = [];
        vm.listadoActividadesProducto = [];
        vm.listadoUnidadesMedida = []
        vm.notificacionValidacionPadre = null;
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
            var templateUrl = "src/app/formulario/ventanas/seguimientoControl/componentes/programarActiv/componentes/editarActividadModal/editarActividadModal.html";

            if (vm.soloLectura)
                templateUrl = "src/app/formulario/ventanas/seguimientoControl/componentes/programarActiv/componentes/editarActividadModal/editarActividadModalLectura.html";

            var controller = "editarActividadModalController";
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
            actividad.TotalVigencia = 0;
            for (var i = 0; i < actividad.Vigencias.length; i++) {
                var sumarTotalVigencia = 0;
                for (var j = 0; j < actividad.Vigencias[i].ProgramacionSeguimientoPeriodosValores.length; j++) {
                    sumarTotalVigencia = sumarTotalVigencia + parseFloat(actividad.Vigencias[i].ProgramacionSeguimientoPeriodosValores[j].Valor);
                }
                actividad.Vigencias[i].TotalVigencia = sumarTotalVigencia;
                actividad.TotalVigencia = actividad.TotalVigencia + sumarTotalVigencia;
            }
        }

        function recorrerObjetivosNumber(event, actividad, vigenciaInicial) {

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

            recorrerObjetivos(actividad, vigenciaInicial);
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
            if (tipo == 1) {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    actividad.HabilitaEditar = false;
                    actividad.TotalVigencia = actividad.TotalVigenciaAnterior;
                    for (var i = 0; i < actividad.Vigencias.length; i++) {
                        actividad.Vigencias[i].TotalVigencia = actividad.Vigencias[i].TotalVigenciaAnterior;
                        for (var j = 0; j < actividad.Vigencias[i].ProgramacionSeguimientoPeriodosValores.length; j++) {
                            actividad.Vigencias[i].ProgramacionSeguimientoPeriodosValores[j].Valor = actividad.Vigencias[i].ProgramacionSeguimientoPeriodosValores[j].ValorAnterior;
                        }
                    }
                    OkCancelar();

                }, function funcionCancelar(reason) {
                    console.log("reason", reason);
                }, null, null, "Los posibles datos que haya diligenciado en la tabla 'Detalle beneficiarios totales' se perderán.");
            }
            else {
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

            listadoProgramarActCapServicio.Guardar(actividad.Vigencias)
                .then(function (response) {
                    let exito = response.data;
                    if (exito.Status) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

                        vm.ObtenerResumenObjetivosProductosActividades(actividad);

                        //guardarCapituloModificado();
                        return;
                    }
                    else {
                        utilidades.mensajeError("Error al realizar la operación " + exito.Message, false);
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
            programarActCapServicio.obtenerListadoObjProdNiveles({ BPIN: $sessionStorage.bpinProductos })
                .then(resultado => {
                    if (resultado.data != null) {
                        var data = resultado.data["Objetivos"];
                        for (var i = 0; i < data.length; i++) {
                            for (var j = 0; j < data[i].Productos.length; j++) {
                                for (var k = 0; k < data[i].Productos[j].Actividades.length; k++) {
                                    if (data[i].Productos[j].Actividades[k].ActividadId == actividad.ActividadId &&
                                        data[i].Productos[j].Actividades[k].ActividadProgramacionSeguimientoId == actividad.ActividadProgramacionSeguimientoId) {
                                        actividad = data[i].Productos[j].Actividades[k];
                                        for (var z = 0; z<vm.listadoActividadesNiveles.length; z++) {
                                            if (data[i].Productos[j].Actividades[k].ActividadId == vm.listadoActividadesNiveles[z].ActividadId &&
                                                data[i].Productos[j].Actividades[k].ActividadProgramacionSeguimientoId == vm.listadoActividadesNiveles[z].ActividadProgramacionSeguimientoId) {
                                                vm.listadoActividadesNiveles[z] = data[i].Productos[j].Actividades[k];
                                                if (vm.listadoActividadesNiveles[z].DuracionOptimista > 0 &&
                                                    vm.listadoActividadesNiveles[z].DuracionPesimista > 0 &&
                                                    vm.listadoActividadesNiveles[z].DuracionProbable > 0) vm.listadoActividadesNiveles[z].Promedio = Math.ceil((vm.listadoActividadesNiveles[z].DuracionOptimista + vm.listadoActividadesNiveles[z].DuracionPesimista + vm.listadoActividadesNiveles[z].DuracionProbable) / 3);
                                                else vm.listadoActividadesNiveles[z].Promedio = 0
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
        }

    }

    angular.module('backbone').component('listadoProgramarActCap', {
        templateUrl: "src/app/formulario/ventanas/seguimientoControl/componentes/programarActiv/componentes/listadoProgramarActCap/listadoProgramarActCap.html",
        controller: listadoProgramarActCapController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
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