(function () {
    'use strict';

    reporteActCapController.$inject = [
        '$scope',
        '$sessionStorage',
        '$uibModal',
        '$window',
        'utilidades',
        'desagregarEdtServicio',
        'reporteActCapServicio',
        'utilsValidacionSeccionCapitulosServicio'
    ];

    function reporteActCapController(
        $scope,
        $sessionStorage,
        $uibModal,
        $window,
        utilidades,
        desagregarEdtServicio,
        reporteActCapServicio,
        utilsValidacionSeccionCapitulosServicio

    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "financieroavancepresupuestal";
        vm.componentesRefresh = [
            "desagregaredtdesagregarcap"
        ];
        vm.anteriorIdElemento = '';
        vm.anteriorIdElemento2 = '';
        vm.anteriorIdElemento3 = '';
        vm.ProductoSeleccionado = 0;
        vm.Nivel2Seleccionado = 0;
        vm.Nivel3Seleccionado = 0;
        vm.listadoObjProdNiveles = [];
        vm.unidadesMedida = [];
        vm.periodos = [];

        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.refreshComponente();
            vm.obtenerPeriodos();
        };

        vm.abrirGantt = function () {
            $window.open(backboneURL + "/reporteGantt?BPIN=" + vm.bpin, '_blank');
        }

        vm.refreshComponente = function () {
            vm.obtenerListadoObjProdNiveles();
            vm.obtenerPeriodos();
        }

        vm.reloadEdicion = function (dataSeleccionada) {
            //vm.abrirArbol(dataSeleccionada.DataAgregarModal);
            vm.obtenerPeriodos();
            vm.guardadoevent({ nombreComponenteHijo: this.nombreComponente })
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            console.log("vm.notificacionCambiosCapitulos Programar Actividades", nombreCapituloHijo)
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.refreshComponente();
            }
        }

        vm.obtenerListadoObjProdNiveles = function (arbolCollapse) {
            console.log(arbolCollapse);
            $sessionStorage.bpinProductos = vm.bpin;
            reporteActCapServicio.obtenerListadoObjProdNiveles({ bpin: $sessionStorage.idObjetoNegocio })
                .then(resultado => {
                    console.log("desagregarEdtServicio de programarActCapController", resultado);
                    if (resultado.data != null) {
                        var data = resultado.data["Objetivos"];

                        vm.listadoObjProdNiveles = vm.agregaValidaciones(data);

                        /*---- Abre componente collapse hasta la ruta modificada ----*/

                        if (arbolCollapse != undefined && arbolCollapse != null) {
                            setTimeout(function () {
                                $("#progReporAct-" + arbolCollapse.productoId).collapse("toggle");
                                $("#progReporAct-" + arbolCollapse.entregablesNvl1).collapse("toggle");
                                if (arbolCollapse.entregablesNvl2 != undefined) $("#progReporAct-" + arbolCollapse.entregablesNvl2).collapse("toggle");
                                if (arbolCollapse.entregablesNvl3 != undefined) $("#progReporAct-" + arbolCollapse.entregablesNvl3).collapse("toggle");

                                vm.abrilNivel(arbolCollapse.productoIdCompuesto);
                                vm.abrilNivel(arbolCollapse.entregablesNvl1);
                                if (arbolCollapse.entregablesNvl2 != undefined) vm.abrilNivel(arbolCollapse.entregablesNvl2);
                                if (arbolCollapse.entregablesNvl3 != undefined) vm.abrilNivel(arbolCollapse.entregablesNvl3);
                            }, 500)
                        }
                    }
                });
        }

        vm.obtenerPeriodos = function () {
            vm.periodos = [];
            reporteActCapServicio.obtenerCalendarioPeriodo({ bpin: vm.bpin })
                .then(resultado => {
                    console.log(resultado);
                    vm.periodos = resultado.data;
                });
        }

        vm.agregaValidaciones = function (data) {
            // Listado de objetivos
            for (var i = 0; i < data.length; i++) {
                // Listado de productos
                var productosLength = data[i]["Productos"].length;
                data[i]["Numeracion"] = (i + 1);

                /* --- Cambia información productos-nivles --- */
                for (var j = 0; j < productosLength; j++) {
                    var listadoActividadesNiveles = [];
                    var listadoActividades = data[i]["Productos"][j]["Actividades"] == null ? [] : data[i]["Productos"][j]["Actividades"];
                    var listadoEntregables = data[i].Productos[j]["EntregablesNivel1"];
                    data[i]["Productos"][j]["Numeracion"] = (i + 1) + '.' + (j + 1);

                    if (listadoActividades != null) {
                        for (var k = 0; k < listadoActividades.length; k++) {
                            data[i].Productos[j].Actividades[k]["DataAgregarModal"] = {
                                Nivel: "Actividad",
                                NivelPadre: "Nivel 1",
                                Producto: data[i]["Productos"][j],
                                PadreId: null,
                                Hijos: [],
                                Data: listadoActividades[k],
                                ActividadId: listadoActividades[k]["ActividadId"],
                                CatalogoEntregables: null,
                                ProductoCollapseId: data[i].Productos[j]["IdCompuesto"],
                                Nivel1CollapseId: listadoActividades[k]["IdCompuesto"]
                            };
                        }
                    }

                    if (listadoEntregables != null) {
                        for (var k = 0; k < listadoEntregables.length; k++) {
                            // Validaciones Nivel 1
                            var catalogoEntregableProducto = data[i].Productos[j].EntregablesNivel1[k]["CatalogoEntregables"];
                            var nivelesRegistrados = data[i].Productos[j].EntregablesNivel1[k]["NivelesRegistrados"];
                            data[i].Productos[j].EntregablesNivel1[k]["Numeracion"] = (i + 1) + '.' + (j + 1) + '.' + (k + 1);
                            data[i].Productos[j].EntregablesNivel1[k]["DataAgregarModal"] = {
                                Nivel: "Nivel 2",
                                NivelPadre: "Nivel 1",
                                Producto: data[i]["Productos"][j],
                                PadreId: null,
                                Hijos: data[i].Productos[j].EntregablesNivel1[k]["NivelesRegistrados"],
                                Data: data[i].Productos[j].EntregablesNivel1[k],
                                ActividadId: data[i].Productos[j].EntregablesNivel1[k]["ActividadId"],
                                CatalogoEntregables: catalogoEntregableProducto,
                                ProductoCollapseId: data[i].Productos[j]["IdCompuesto"],
                                Nivel1CollapseId: data[i].Productos[j].EntregablesNivel1[k]["IdCompuesto"]
                            };

                            for (var l = 0; l < nivelesRegistrados.length; l++) {
                                // Validaciones Nivel 2 
                                if (nivelesRegistrados[l].NivelEntregable == "Actividad") listadoActividadesNiveles.push(nivelesRegistrados[l])
                                var hijoNivel2 = data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["Hijos"];
                                data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["DataAgregarModal"] = {
                                    Nivel: "Nivel 3",
                                    NivelPadre: "Nivel 2",
                                    Producto: data[i]["Productos"][j],
                                    PadreId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["SeguimientoEntregableId"],
                                    Hijos: hijoNivel2,
                                    Data: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l],
                                    ActividadId: data[i].Productos[j].EntregablesNivel1[k]["ActividadId"],
                                    CatalogoEntregables: catalogoEntregableProducto,
                                    ProductoCollapseId: data[i].Productos[j]["IdCompuesto"],
                                    Nivel1CollapseId: data[i].Productos[j].EntregablesNivel1[k]["IdCompuesto"],
                                    Nivel2CollapseId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["IdCompuesto"]
                                };
                                for (var m = 0; m < hijoNivel2.length; m++) {
                                    // Validaciones Nivel 3
                                    if (hijoNivel2[m].NivelEntregable == "Actividad") listadoActividadesNiveles.push(hijoNivel2[m])
                                    var hijoNivel3 = hijoNivel2[m]["Hijos"];
                                    data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m]["DataAgregarModal"] = {
                                        Nivel: "Actividad",
                                        NivelPadre: "Nivel 3",
                                        Producto: data[i]["Productos"][j],
                                        PadreId: hijoNivel2[m]["SeguimientoEntregableId"],
                                        Hijos: hijoNivel3,
                                        Data: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos,
                                        ActividadId: data[i].Productos[j].EntregablesNivel1[k]["ActividadId"],
                                        CatalogoEntregables: catalogoEntregableProducto,
                                        ProductoCollapseId: data[i].Productos[j]["IdCompuesto"],
                                        Nivel1CollapseId: data[i].Productos[j].EntregablesNivel1[k]["IdCompuesto"],
                                        Nivel2CollapseId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["IdCompuesto"],
                                        Nivel3CollapseId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m]["IdCompuesto"],
                                    };
                                    for (var n = 0; n < hijoNivel3.length; n++) {
                                        // Validaciones Actividades de Nivel 3
                                        if (hijoNivel3[n].NivelEntregable == "Actividad") listadoActividadesNiveles.push(hijoNivel3[n])
                                        data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m]["Hijos"][n]["DataAgregarModal"] = {
                                            Producto: data[i]["Productos"][j],
                                            Nivel1CollapseId: data[i].Productos[j].EntregablesNivel1[k]["IdCompuesto"],
                                            Nivel2CollapseId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["IdCompuesto"],
                                            Nivel3CollapseId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m]["IdCompuesto"],
                                        };
                                    }
                                }
                            }
                        }
                    }

                    /* --- Agrega Actividades de todos los niveles a productos --- */
                    data[i]["Productos"][j]["ActividadesNiveles"] = [...listadoActividadesNiveles];
                    data[i]["Productos"][j]["ActividadesNiveles"].concat(listadoActividades);
                    console.log("Producto ActividadesNiveles", i, data[i]["Productos"][j].ActividadesNiveles)
                }
            }
            return data;
        }

        /*--------------------- Comportamientos collapse y contenido ---------------------*/

        vm.verNombreCompleto = function (idElement, indexElement) {
            var elValidacion = document.getElementById(idElement + indexElement + '-val');
            var elCorto = document.getElementById(idElement + indexElement + '-min');
            var elCompleto = document.getElementById(idElement + indexElement + '-max');

            if (elCompleto.classList.contains('hidden')) {
                elValidacion.innerHTML = 'VER MENOS';
                elCorto.classList.add('hidden');
                elCompleto.classList.remove('hidden');
            } else {
                elValidacion.innerHTML = 'VER MÁS';
                elCorto.classList.remove('hidden');
                elCompleto.classList.add('hidden');
            }
        }

        vm.abrilNivel = function (idElement, productoId) {
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

            if (vm.anteriorIdElemento2 != '') {
                vm.anteriorIdElemento2 = vm.anteriorIdElemento2.replace("null", "");
                var elMasAnterior2 = document.getElementById(vm.anteriorIdElemento2 + '-mas');
                var elMenosAnterior2 = document.getElementById(vm.anteriorIdElemento2 + '-menos');
                if (elMasAnterior2 != null && elMenosAnterior2 != null) {
                    if (elMasAnterior2.classList.contains('hidden')) {
                        elMenosAnterior2.classList.add('hidden');
                        elMasAnterior2.classList.remove('hidden');
                    }
                }
            }

            vm.anteriorIdElemento = idElement;
            idElement = idElement.replace("null", "");
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.ProductoSeleccionado = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.ProductoSeleccionado = productoId;
                }
            }
        }

        vm.abrilNivel2 = function (idElement, nivel2) {
            if (vm.anteriorIdElemento2 != '' && vm.anteriorIdElemento2 != idElement) {
                vm.anteriorIdElemento2 = vm.anteriorIdElemento2.replace("null", "");
                var elMasAnterior2 = document.getElementById(vm.anteriorIdElemento2 + '-mas');
                var elMenosAnterior2 = document.getElementById(vm.anteriorIdElemento2 + '-menos');
                if (elMasAnterior2 != null && elMenosAnterior2 != null) {
                    if (elMasAnterior2.classList.contains('hidden')) {
                        elMenosAnterior2.classList.add('hidden');
                        elMasAnterior2.classList.remove('hidden');
                    }
                }
            }
            if (vm.anteriorIdElemento3 != '') {
                vm.anteriorIdElemento3 = vm.anteriorIdElemento3.replace("null", "");
                var elMasAnterior3 = document.getElementById(vm.anteriorIdElemento3 + '-mas');
                var elMenosAnterior3 = document.getElementById(vm.anteriorIdElemento3 + '-menos');
                if (elMasAnterior3 != null && elMenosAnterior3 != null) {
                    if (elMasAnterior3.classList.contains('hidden')) {
                        elMenosAnterior3.classList.add('hidden');
                        elMasAnterior3.classList.remove('hidden');
                    }
                }
            }

            vm.anteriorIdElemento2 = idElement;
            idElement = idElement.replace("null", "");
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.Nivel2Seleccionado = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.Nivel2Seleccionado = nivel2;
                }
            }
        }

        vm.abrilNivel3 = function (idElement, nivel3) {
            if (vm.anteriorIdElemento3 != '' && vm.anteriorIdElemento3 != idElement) {
                vm.anteriorIdElemento3 = vm.anteriorIdElemento3.replace("null", "");
                var elMasAnterior3 = document.getElementById(vm.anteriorIdElemento3 + '-mas');
                var elMenosAnterior3 = document.getElementById(vm.anteriorIdElemento3 + '-menos');
                if (elMasAnterior3 != null && elMenosAnterior3 != null) {
                    if (elMasAnterior3.classList.contains('hidden')) {
                        elMenosAnterior3.classList.add('hidden');
                        elMasAnterior3.classList.remove('hidden');
                    }
                }
            }

            vm.anteriorIdElemento3 = idElement;
            idElement = idElement.replace("null", "");
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.Nivel3Seleccionado = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.Nivel3Seleccionado = nivel3;
                }
            }
        }

        vm.abrirMensajeQueEsEsto = function () {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <br /> <span class='tituhori' >Programar Actividades - Desglose de actividades</span>");
        }

        /*--------------------- Agregar - Eliminar niveles / actividades ---------------------*/

        vm.abrirArbol = function (dataModal) {
            var arbolCollapse = {
                productoId: "productos-nivel-1-" + dataModal["Producto"].ProductoId,
                productoIdCompuesto: dataModal["Producto"].IdCompuesto,
                entregablesNvl1: "items-" + dataModal["Nivel1CollapseId"],
                entregablesNvl2: "items-" + dataModal["Nivel2CollapseId"],
                entregablesNvl3: "items-" + dataModal["Nivel3CollapseId"],
            }
            vm.obtenerListadoObjProdNiveles(arbolCollapse);
        }

        /*--------------------- Validaciones ---------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Programar Actividades", errores);
            if (errores != undefined) {
                vm.erroresActivos = [];
                var erroresFiltrados = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
                vm.erroresActivos = erroresFiltrados.erroresActivos;
                console.log("erroresActivos", vm.erroresActivos)
                vm.ejecutarErrores();
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: erroresFiltrados.isValid });
            }
        }

        vm.limpiarErrores = function () {

        }

        vm.ejecutarErrores = function () {
            vm.limpiarErrores();
            vm.erroresActivos.forEach(p => {
                if (vm.errores[p.Error] != undefined) {
                    vm.errores[p.Error]({
                        error: p.Error,
                        descripcion: p.Descripcion,
                        data: p.Data
                    });
                }
            });
        }

        vm.validarAAPFC001 = function (errores) {
            var valores = errores.descripcion.split('|');
            
            var seccionpag = document.getElementById("errorval-" + valores[0] + "-" + valores[1]);
            var validacionpag = document.getElementById("errorfuentemsnval-" + valores[0] + "-" + valores[1]);

            if (seccionpag != undefined) {
                if (validacionpag != undefined) {
                    validacionpag.innerHTML = '<span>' + valores[2] + "</span>";
                    seccionpag.classList.remove('hidden');
                }
            }            
        }

        vm.errores = {
            'AAPFC001': vm.validarAAPFC001,
        }

    }

    angular.module('backbone').component('reporteActCap', {
        templateUrl: "src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avancePresupuestal/reporteActCap/reporteActCap.html",
        controller: reporteActCapController,
        controllerAs: "vm",
        bindings: {
            bpin: '@',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacioncambios: '&',
            notificacionestado: '&'
        }
    });

})();