(function () {
    'use strict';

    programarActCapController.$inject = [
        '$scope',
        '$sessionStorage',
        '$uibModal',
        '$window',
        'utilidades',
        'desagregarEdtServicio',
        'programarActCapServicio',
        'utilsValidacionSeccionCapitulosServicio'
    ];

    function programarActCapController(
        $scope,
        $sessionStorage,
        $uibModal,
        $window,
        utilidades,
        desagregarEdtServicio,
        programarActCapServicio,
        utilsValidacionSeccionCapitulosServicio
        
    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "programaractivprogramaractcap";
        vm.componentesRefresh = [];
        vm.listadoObjProdNiveles = [];
        vm.unidadesMedida = [];
        
        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.refreshComponente();           
        };

        vm.abrirGantt = function () {
            $window.open(backboneURL + "/reporteGantt?BPIN=" + vm.bpin , '_blank');
        }

        vm.refreshComponente = function () {
            vm.obtenerListadoObjProdNiveles();
            vm.obtenerParametricas();
        }

        vm.reloadEdicion = function (dataSeleccionada) {
            vm.abrirArbol(dataSeleccionada.DataAgregarModal);
            vm.guardadoevent({ nombreComponenteHijo: this.nombreComponente })
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            console.log("vm.notificacionCambiosCapitulos Programar Actividades", nombreCapituloHijo)
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.refreshComponente();
            }
        }

        vm.obtenerListadoObjProdNiveles = function (arbolCollapse) {
            programarActCapServicio.obtenerListadoObjProdNiveles({ bpin: vm.bpin })
                .then(resultado => {
                    console.log("desagregarEdtServicio de programarActCapController", resultado);
                    if (resultado.data != null) {
                        var data = resultado.data["Objetivos"];

                        vm.listadoObjProdNiveles = vm.agregaValidaciones(data);

                        /*---- Abre componente collapse hasta la ruta modificada ----*/

                        if (arbolCollapse != undefined && arbolCollapse != null) {
                            setTimeout(function () {
                                $("#progAct-" + arbolCollapse.productoId).collapse("toggle");
                                $("#progAct-" + arbolCollapse.entregablesNvl1).collapse("toggle");
                                if (arbolCollapse.entregablesNvl2 != undefined) $("#progAct-" + arbolCollapse.entregablesNvl2).collapse("toggle");
                                if (arbolCollapse.entregablesNvl3 != undefined) $("#progAct-" + arbolCollapse.entregablesNvl3).collapse("toggle");

                                vm.abrilNivel(arbolCollapse.productoIdCompuesto);
                                vm.abrilNivel(arbolCollapse.entregablesNvl1);
                                if (arbolCollapse.entregablesNvl2 != undefined) vm.abrilNivel(arbolCollapse.entregablesNvl2);
                                if (arbolCollapse.entregablesNvl3 != undefined) vm.abrilNivel(arbolCollapse.entregablesNvl3);
                            }, 500)
                        }
                    }
                });
        }

        vm.obtenerParametricas = function () {
            desagregarEdtServicio.obtenerUnidadesMedida()
                .then(resultado => {
                    if (resultado != null) vm.unidadesMedida = resultado.data;
                })
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
            var listadoErrores = document.getElementsByClassName("messagealerttableDNP");
            var listadoErroresContainer = document.getElementsByClassName("errores-contenedor");
            for (var i = 0; i < listadoErroresContainer.length; i++) {
                if (!listadoErroresContainer[i].classList.contains("d-none")) {
                    listadoErroresContainer[i].classList.add("d-none");

                }
            }

            for (var i = 0; i < listadoErrores.length; i++) {
                if (!listadoErrores[i].classList.contains("d-none")) {
                    listadoErrores[i].classList.add("d-none")
                    listadoErrores[i].innerHTML = ''
                }
            }
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

        vm.getErrorDES003 = function ({ error, descripcion, data }) {
            var dataObj = JSON.parse(descripcion)
            console.log("getErrorDES003", dataObj);
            var imgError = '<span class="img iconadverdnp mr-2"><img src="Img/u4630.svg"></span>';
            dataObj.forEach((itemError) => {

                var objetivoEl = document.getElementById("progAct-error-button-Objetivo-" + itemError.ObjetivoId)
                var productoEl = document.getElementById("progAct-error-button-Producto-" + itemError.ProductoId)
                var entregableNivel1 = document.getElementById("progAct-error-button-nivel-1-" + itemError.EntregableNivel1)
                var errorEntregableNivel1 = document.getElementById("progAct-error-button-nivel-1-" + itemError.EntregableNivel1 + "-text-error03")
                var entregableNivel2 = document.getElementById("progAct-error-button-nivel-2-" + itemError.EntregableNivel1 + itemError.EntregableNivel2)
                var errorEntregableNivel2 = document.getElementById("progAct-error-button-nivel-2-" + itemError.EntregableNivel1 + itemError.EntregableNivel2 + "-text-error03");
                var entregableNivel3 = document.getElementById("progAct-error-button-nivel-3-" + itemError.EntregableNivel1 + itemError.EntregableNivel2 + itemError.EntregableNivel3)
                var errorEntregableNivel3 = document.getElementById("progAct-error-button-nivel-3-" + itemError.EntregableNivel1 + itemError.EntregableNivel2 + itemError.EntregableNivel3 + "-text-error03");
                var entregableNivel3 = document.getElementById("progAct-error-button-nivel-3-" + itemError.EntregableNivel1 + itemError.EntregableNivel2 + itemError.EntregableNivel3)
                var errorEntregableNivel3 = document.getElementById("progAct-error-button-nivel-3-" + itemError.EntregableNivel1 + itemError.EntregableNivel2 + itemError.EntregableNivel3 + "-text-error03");
                

                var itemErrorSeguimientoId = itemError.SeguimientoId
                if (itemErrorSeguimientoId == undefined)
                    itemErrorSeguimientoId = ''
                    
                var seguimientoSpan = document.getElementById("segui-error-button-" + itemError.EntregableNivel1 + "-" + itemErrorSeguimientoId)
                var seguimientoError = document.getElementById("segui-error-button-text-error01-" + itemError.EntregableNivel1 + "-" + itemErrorSeguimientoId);
                var seguimientoSpanM = document.getElementById("segui-error-buttonM-" + itemError.EntregableNivel1 + "-" + itemErrorSeguimientoId)
                var seguimientoErrorM = document.getElementById("segui-error-button-text-error01M-" + itemError.EntregableNivel1 + "-" + itemErrorSeguimientoId);
                var seguimientoProgramacionSpan = document.getElementById("seguiProg-error-button-" + itemError.EntregableNivel1 + "-" + itemErrorSeguimientoId + "-" + itemError.Acumulado)
                var seguimientoProgramacionError = document.getElementById("seguiProg-error-button-text-error01-" + itemError.EntregableNivel1 + "-" + itemErrorSeguimientoId + "-" + itemError.Acumulado);

                if (itemError.EntregableNivel3 == undefined)
                    itemError.EntregableNivel3 = null;

                if (itemError.EntregableNivel2 == undefined)
                    itemError.EntregableNivel2 = null;

                if (itemError.SeguimientoId == undefined)
                    itemError.SeguimientoId = null;

                if (itemError.Acumulado == undefined)
                    itemError.Acumulado = null;

                if (itemError.MensajeError == undefined)
                    itemError.MensajeError = '';

                if (itemError.MensajeErrorValor == undefined)
                    itemError.MensajeErrorValor = '';

                if (itemError.MensajeErrorProgramar == undefined)
                    itemError.MensajeErrorProgramar = '';

                if (objetivoEl != undefined && objetivoEl != null &&
                    productoEl != undefined && productoEl != null) {
                    objetivoEl.classList.remove("d-none");
                    productoEl.classList.remove("d-none");

                    if (itemError.EntregableNivel1 != null && /*itemError.EntregableNivel2 == null && itemError.EntregableNivel3 == null &&*/
                        seguimientoSpan != undefined && seguimientoSpan != null &&
                        seguimientoError != undefined && seguimientoError != null) {
                        seguimientoSpan.classList.remove("d-none");
                        seguimientoError.classList.remove("d-none");
                        seguimientoError.innerHTML = imgError; // itemError.MensajeError;
                    }

                    if (itemError.EntregableNivel1 != null && /*itemError.EntregableNivel2 == null && itemError.EntregableNivel3 == null &&*/
                        seguimientoProgramacionSpan != undefined && seguimientoProgramacionSpan != null &&
                        seguimientoProgramacionError != undefined && seguimientoProgramacionError != null) {
                        seguimientoProgramacionSpan.classList.remove("d-none");
                        seguimientoProgramacionError.classList.remove("d-none");
                        seguimientoProgramacionError.innerHTML = imgError + itemError.MensajeError;
                    }
                    else if (itemError.EntregableNivel1 != null && /*itemError.EntregableNivel2 == null && itemError.EntregableNivel3 == null &&*/
                        seguimientoSpanM != undefined && seguimientoSpanM != null &&
                        seguimientoErrorM != undefined && seguimientoErrorM != null) {
                        seguimientoSpanM.classList.remove("d-none");
                        seguimientoErrorM.classList.remove("d-none");
                        seguimientoErrorM.innerHTML = '<span class="img iconadverdnp mr-2"><img src="Img/u4630.svg">Debe editar la actividad.</span>'; // itemError.MensajeError;
                    }

                    if (itemError.EntregableNivel1 != null && /*itemError.EntregableNivel2 == null && itemError.EntregableNivel3 == null &&*/
                        entregableNivel1 != undefined && entregableNivel1 != null &&
                        errorEntregableNivel1 != undefined && errorEntregableNivel1 != null) {
                        entregableNivel1.classList.remove("d-none");
                        errorEntregableNivel1.classList.remove("d-none");
                        errorEntregableNivel1.innerHTML = imgError + itemError.MensajeError + itemError.MensajeErrorValor + itemError.MensajeErrorProgramar;
                    }

                    if (itemError.EntregableNivel2 != null && /*itemError.EntregableNivel3 == null &&*/
                        entregableNivel2 != undefined && entregableNivel2 != null &&
                        errorEntregableNivel2 != undefined && errorEntregableNivel2 != null) {

                        entregableNivel2.classList.remove("d-none");
                        errorEntregableNivel2.classList.remove("d-none");
                        errorEntregableNivel2.innerHTML = imgError + itemError.MensajeError + itemError.MensajeErrorValor + itemError.MensajeErrorProgramar;
                    }

                    if (itemError.EntregableNivel3 != null &&
                        entregableNivel3 != undefined && entregableNivel3 != null &&
                        errorEntregableNivel3 != undefined && errorEntregableNivel3 != null) {

                        entregableNivel3.classList.remove("d-none");
                        errorEntregableNivel3.classList.remove("d-none");
                        errorEntregableNivel3.innerHTML = imgError + itemError.MensajeError + itemError.MensajeErrorValor + itemError.MensajeErrorProgramar;
                    }
                }
            })
        }

        vm.errores = {
            'DES003': vm.getErrorDES003
        }

    }

    angular.module('backbone').component('programarActCap', {
        templateUrl: "src/app/formulario/ventanas/seguimientoControl/componentes/programarActiv/componentes/programarActCap/programarActCap.html",
        controller: programarActCapController,
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