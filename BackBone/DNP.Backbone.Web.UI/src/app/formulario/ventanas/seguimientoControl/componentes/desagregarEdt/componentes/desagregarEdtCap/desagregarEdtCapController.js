(function () {
    'use strict';

    desagregarEdtCapController.$inject = [
        '$scope',
        '$sessionStorage',
        '$uibModal',
        'utilidades',
        'desagregarEdtServicio',
        'utilsValidacionSeccionCapitulosServicio',
        '$window'

    ];

    function desagregarEdtCapController(
        $scope,
        $sessionStorage,
        $uibModal,
        utilidades,
        desagregarEdtServicio,
        utilsValidacionSeccionCapitulosServicio,
        $window
        
    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "desagregaredtdesagregarcap";
        vm.componentesRefresh = [];
        vm.listadoObjProdNiveles = [];
        vm.unidadesMedida = [];
        vm.soloLectura = $sessionStorage.soloLectura;
        vm.configuracion = {
            maximoNivelesSinCatalogo: 3,
            cantMaxEntregablesCatalogo: 2,
            cantMinimaNombreEntregable: 20,
            cantMaximaNombreEntregable: 200
        }
        
        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.refreshComponente();
        };

        vm.refreshComponente = function () {
            vm.obtenerListadoObjProdNiveles();
            vm.obtenerParametricas();
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            console.log("vm.notificacionCambiosCapitulos DesagregarEdt", nombreCapituloHijo)
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.refreshComponente();
            }
        }

        vm.obtenerListadoObjProdNiveles = function (arbolCollapse) {
            $sessionStorage.bpinProductos = vm.bpin;
            desagregarEdtServicio.obtenerListadoObjProdNiveles({ bpin: vm.bpin })
                .then(resultado => {
                    console.log("desagregarEdtServicio", resultado);
                    if (resultado.data != null) {
                        var data = resultado.data["Objetivos"];
                        vm.listadoObjProdNiveles = vm.agregaValidaciones(data);

                        /*---- Abre componente collapse hasta la ruta modificada ----*/

                        if (arbolCollapse != undefined && arbolCollapse != null) {
                            setTimeout(function () {
                                $("#" + arbolCollapse.productoId).collapse("toggle");                               
                                $("#" + arbolCollapse.entregablesNvl1).collapse("toggle");                                
                                if (arbolCollapse.entregablesNvl2 != undefined) $("#" + arbolCollapse.entregablesNvl2).collapse("toggle");
                                if (arbolCollapse.entregablesNvl3 != undefined) $("#" + arbolCollapse.entregablesNvl3).collapse("toggle");

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
                    console.log("UM", resultado);
                    if (resultado != null) vm.unidadesMedida = resultado.data;
                })
        }

        vm.abrirDiagramaActividades = function () {
            $window.open(backboneURL + "/reporteEstructuraActividades?BPIN=" + vm.bpin, '_blank');
        }

        vm.agregaValidaciones = function (data) {
            // Listado de objetivos
            for (var i = 0; i < data.length; i++) {
                data[i]["Numeracion"] = (i + 1);

                // Listado de productos
                for (var j = 0; j < data[i]["Productos"].length; j++) {
                    data[i]["Productos"][j]["Numeracion"] = (i + 1) + '.' + (j + 1);

                    for (var k = 0; k < data[i].Productos[j]["EntregablesNivel1"].length; k++) {
                        // Validaciones Nivel 1
                        var cantidaNSinCatalogo = data[i].Productos[j].EntregablesNivel1[k]["NivelesRegistrados"].filter(p => p.EntregableCatalogId == null && p.NivelEntregable == 'Nivel 2');
                        var catalogoEntregableProducto = data[i].Productos[j].EntregablesNivel1[k]["CatalogoEntregables"];

                        data[i].Productos[j].EntregablesNivel1[k]["Numeracion"] = (i + 1) + '.' + (j + 1) + '.' + (k + 1);
                        data[i].Productos[j].EntregablesNivel1[k]["ExcedeEntregablesSinCatalogo"] = (cantidaNSinCatalogo != undefined && cantidaNSinCatalogo.length >= vm.configuracion.maximoNivelesSinCatalogo);
                        data[i].Productos[j].EntregablesNivel1[k]["DataAgregarModal"] = {
                            Nivel: "Nivel 2",
                            NivelPadre: "Nivel 1",
                            Producto: data[i]["Productos"][j],
                            PadreId: null,
                            Hijos: data[i].Productos[j].EntregablesNivel1[k]["NivelesRegistrados"],
                            Data: data[i].Productos[j].EntregablesNivel1[k],
                            ActividadId: data[i].Productos[j].EntregablesNivel1[k]["ActividadId"],
                            DeliverableCatalogPadreId: data[i].Productos[j].EntregablesNivel1[k]["DeliverableCatalogPadreId"],
                            DeliverableCatalogId: data[i].Productos[j].EntregablesNivel1[k]["DeliverableCatalogId"],
                            CatalogoEntregables: catalogoEntregableProducto,
                            ProductoCollapseId: data[i].Productos[j]["IdCompuesto"],
                            Nivel1CollapseId: data[i].Productos[j].EntregablesNivel1[k]["IdCompuesto"]
                        };

                        for (var l = 0; l < data[i].Productos[j].EntregablesNivel1[k]["NivelesRegistrados"].length; l++) {
                            // Validaciones Nivel 2 
                            var hijoNivel2 = data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["Hijos"];
                            var cantidaNSinCatalogo = hijoNivel2 != undefined && hijoNivel2 != null ? hijoNivel2.filter(p => p.EntregableCatalogId == null && p.NivelEntregable == 'Nivel 3') : 0;
                            data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["ExcedeEntregablesSinCatalogo"] = (cantidaNSinCatalogo != undefined && cantidaNSinCatalogo.length >= vm.configuracion.maximoNivelesSinCatalogo);
                            data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["DataAgregarModal"] = {
                                Nivel: "Nivel 3",
                                NivelPadre: "Nivel 2",
                                Producto: data[i]["Productos"][j],
                                PadreId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["SeguimientoEntregableId"],
                                Hijos: hijoNivel2,
                                Data: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l],
                                ActividadId: data[i].Productos[j].EntregablesNivel1[k]["ActividadId"],
                                DeliverableCatalogPadreId: data[i].Productos[j].EntregablesNivel1[k]["DeliverableCatalogId"],
                                DeliverableCatalogId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["EntregableCatalogId"],
                                CatalogoEntregables: catalogoEntregableProducto,
                                ProductoCollapseId: data[i].Productos[j]["IdCompuesto"],
                                Nivel1CollapseId: data[i].Productos[j].EntregablesNivel1[k]["IdCompuesto"],
                                Nivel2CollapseId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["IdCompuesto"]
                            };
                            for (var m = 0; m < hijoNivel2.length; m++) {
                                // Validaciones Nivel 3
                                var hijoNivel3 = hijoNivel2[m]["Hijos"];
                                data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m]["DataAgregarModal"] = {
                                    Nivel: "Actividad",
                                    NivelPadre: "Nivel 3",
                                    Producto: data[i]["Productos"][j],
                                    PadreId: hijoNivel2[m]["SeguimientoEntregableId"],
                                    Hijos: hijoNivel3,
                                    Data: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos,
                                    ActividadId: data[i].Productos[j].EntregablesNivel1[k]["ActividadId"],
                                    DeliverableCatalogPadreId: data[i].Productos[j].EntregablesNivel1[k]["DeliverableCatalogPadreId"],
                                    DeliverableCatalogId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m]["EntregableCatalogId"],
                                    CatalogoEntregables: catalogoEntregableProducto,
                                    ProductoCollapseId: data[i].Productos[j]["IdCompuesto"],
                                    Nivel1CollapseId: data[i].Productos[j].EntregablesNivel1[k]["IdCompuesto"],
                                    Nivel2CollapseId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["IdCompuesto"],
                                    Nivel3CollapseId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m]["IdCompuesto"],
                                };
                                for (var n = 0; n < hijoNivel3.length; n++) {
                                    // Validaciones Actividades de Nivel 3
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
            utilidades.mensajeInformacionN("Desagregar EDT", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <br /> <span class='tituhori' >Desagregar EDT - Desglose de trabajo</span>","La Estructura de Desglose de Trabajo (EDT) hace referencia a la desagregación de los productos del proyecto en entregables y actividades. En esta sección, deberá asociar los entregables del producto para cada uno de los niveles (1, 2 ,3 y actividades) según corresponda. Debe completar la asociación por cada nivel para que el sistema le habilite la agregación de los niveles subsiguientes. Tenga en cuenta que por cada nivel de desagregación debe agregar mínimo 2 elementos, ya sean entregables o actividades.");
        }

        /*--------------------- Agregar - Eliminar niveles / actividades ---------------------*/

        vm.abrirModalAgregar = function (tipoModal, dataAgregarModal) {
            console.log(dataAgregarModal)

            dataAgregarModal["UnidadesMedida"] = vm.unidadesMedida;

            if (tipoModal == 'Actividad') {
                dataAgregarModal["NivelCatalogo"] = 'Actividad';
                vm.agregarHijos(dataAgregarModal, 'agregarActividadModal')
            } else {
                dataAgregarModal["NivelCatalogo"] = dataAgregarModal["Nivel"];
                vm.agregarHijos(dataAgregarModal, 'agregarNivelModal')
            }
        }

        vm.agregarHijos = function (dataModal, modalName) {
            var templateUrl = "src/app/formulario/ventanas/seguimientoControl/componentes/desagregarEdt/componentes/" + modalName + "/" + modalName + ".html"
            var controller = modalName + "Controller";
            try {
                let modalInstance = $uibModal.open({
                    templateUrl: templateUrl,
                    controller: controller,
                    controllerAs: "vm",
                    openedClass: "modal-contentDNP",
                    resolve: {
                        Nivel: function () {
                            return dataModal;
                        },
                        Configuracion: function () {
                            return {
                                cantMaxEntregablesCatalogo: vm.configuracion.cantMaxEntregablesCatalogo,
                                cantMinimaNombreEntregable: vm.configuracion.cantMinimaNombreEntregable,
                                cantMaximaNombreEntregable: vm.configuracion.cantMaximaNombreEntregable
                            }
                        }
                    },
                });
                modalInstance.result.then(data => {
                    if (data != null) {
                        if (data == 'ok') {
                            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                            vm.abrirArbol(dataModal);
                        }
                    }
                });
            } catch (error) {
            }
        }

        vm.eliminar = function (tipo, data) {
            var nivel = document.getElementById(data.IdCompuesto + "-label-nivel");
            var nombreNivel = tipo == 'Actividad' ? "La " : "El ";
            var nombreEliminado = tipo == 'Actividad' ? "a" : "o";
            nombreNivel += nivel != undefined ? nivel.innerHTML.toLowerCase() : '';
            utilidades.mensajeWarning(nombreNivel + ", será eliminad" + nombreEliminado + ". ¿Está seguro de continuar?", function () {
                var dataDelete = {
                    Tipo: tipo,
                    NivelesNuevos: data["Descendencia"]
                }
                desagregarEdtServicio.eliminarNivel(dataDelete).then(function (resultado) {
                    if (resultado.data.Status) {
                        utilidades.mensajeSuccess('', false, false, false, "Los datos fueron eliminados con éxito.");
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                        vm.abrirArbol(data.DataAgregarModal);
                    } else {
                        swal("Error al realizar la operación", resultado.data.Message, 'error');
                    }
                }
                , function (error) { });
            });
        }

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
            console.log("Validación  - Desagregar EDT", errores);
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
            var listadoErrores = document.getElementsByClassName("edtMessagealerttableDNP");
            var listadoErroresContainer = document.getElementsByClassName("errores-contenedor-EDT");
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

        vm.getErrorDES003EDT = function ({ error, descripcion, data }) {
            var dataObj = JSON.parse(descripcion)
            console.log("getErrorDES003EDT", dataObj);
            var imgError = '<span class="img iconadverdnp"><img src="Img/u4630.svg"> </span>';
            var entregabledNivel1Control = 0;
            dataObj.forEach((itemError) => {

                var objetivoEl = document.getElementById("edt-error-button-Objetivo-" + itemError.ObjetivoId)
                var productoEl = document.getElementById("edt-error-button-Producto-" + itemError.ProductoId)
                var entregableNivel1 = document.getElementById("edt-error-button-nivel-1-" + itemError.EntregableNivel1)
                var errorEntregableNivel1 = document.getElementById("edt-error-button-nivel-1-" + itemError.EntregableNivel1 + "-text-error03")
                var entregableNivel2 = document.getElementById("edt-error-button-nivel-2-" + itemError.EntregableNivel1 + itemError.EntregableNivel2)
                var errorEntregableNivel2 = document.getElementById("edt-error-button-nivel-2-" + itemError.EntregableNivel1 + itemError.EntregableNivel2 + "-text-error03");
                var entregableNivel3 = document.getElementById("edt-error-button-nivel-3-" + itemError.EntregableNivel1 + itemError.EntregableNivel2 + itemError.EntregableNivel3)
                var errorEntregableNivel3 = document.getElementById("edt-error-button-nivel-3-" + itemError.EntregableNivel1 + itemError.EntregableNivel2 + itemError.EntregableNivel3 + "-text-error03");

                if (itemError.EntregableNivel3 == undefined)
                    itemError.EntregableNivel3 = null;

                if (itemError.EntregableNivel2 == undefined)
                    itemError.EntregableNivel2 = null;

                if (itemError.EntregableNivel1 == undefined)
                    itemError.EntregableNivel1 = null;

                if (itemError.MensajeError == undefined)
                    itemError.MensajeError = '';

                if (itemError.MensajeErrorActividad == undefined)
                    itemError.MensajeErrorActividad = '';

                if (objetivoEl != undefined && objetivoEl != null &&
                    productoEl != undefined && productoEl != null) {
                    objetivoEl.classList.remove("d-none");
                    productoEl.classList.remove("d-none");

                    if (itemError.EntregableNivel1 != null && /*itemError.EntregableNivel2 == null && itemError.EntregableNivel3 == null &&*/
                        entregableNivel1 != undefined && entregableNivel1 != null &&
                        errorEntregableNivel1 != undefined && errorEntregableNivel1 != null ) {
                        entregableNivel1.classList.remove("d-none");
                        errorEntregableNivel1.classList.remove("d-none");
                        
                        if (itemError.EntregableNivel2 == null)
                            errorEntregableNivel1.innerHTML = imgError + itemError.MensajeError + itemError.MensajeErrorActividad;
                        else if (entregabledNivel1Control != itemError.EntregableNivel1)
                            errorEntregableNivel1.innerHTML = imgError;

                        entregabledNivel1Control = itemError.EntregableNivel1;
                        
                    }
                  
                    if (itemError.EntregableNivel2 != null && /*itemError.EntregableNivel3 == null &&*/
                        entregableNivel2 != undefined && entregableNivel2 != null &&
                        errorEntregableNivel2 != undefined && errorEntregableNivel2 != null) {

                        entregableNivel2.classList.remove("d-none");
                        errorEntregableNivel2.classList.remove("d-none");

                        if (itemError.EntregableNivel3 == null)
                            errorEntregableNivel2.innerHTML = imgError + itemError.MensajeError + itemError.MensajeErrorActividad;
                        else
                            errorEntregableNivel2.innerHTML = imgError;
                        
                    }

                    if (itemError.EntregableNivel3 != null &&
                        entregableNivel3 != undefined && entregableNivel3 != null &&
                        errorEntregableNivel3 != undefined && errorEntregableNivel3 != null) {

                        entregableNivel3.classList.remove("d-none");
                        errorEntregableNivel3.classList.remove("d-none");
                        errorEntregableNivel3.innerHTML = imgError + itemError.MensajeError + itemError.MensajeErrorActividad;
                        
                    }
                }
            })
        }

        vm.errores = {
            'DES003': vm.getErrorDES003EDT
        }

    }

    angular.module('backbone').component('desagregarEdtCap', {
        templateUrl: "src/app/formulario/ventanas/seguimientoControl/componentes/desagregarEdt/componentes/desagregarEdtCap/desagregarEdtCap.html",
        controller: desagregarEdtCapController,
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