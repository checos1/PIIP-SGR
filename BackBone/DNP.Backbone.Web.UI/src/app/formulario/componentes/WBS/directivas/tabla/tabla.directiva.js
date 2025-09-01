(function () {
    'use strict';

    angular.module('backbone.formulario').directive('tablaWbs', tablaWbs)
        .filter("formatPrice", function () {
            return function (price, item, digits, thoSeperator, decSeperator, bdisplayprice) {
                if (price != null && ((item.formato === undefined || item.formato != 'int32') && (item.tipo != 'integer' && item.tipo != 'string'))) {
                    var i;
                    digits = (typeof digits === "undefined") ? 2 : digits;
                    bdisplayprice = (typeof bdisplayprice === "undefined") ? true : bdisplayprice;
                    thoSeperator = (typeof thoSeperator === "undefined") ? "." : thoSeperator;
                    decSeperator = (typeof decSeperator === "undefined") ? "," : decSeperator;
                    price = price.toString();
                    var _temp = price.split(".");
                    var dig = (typeof _temp[1] === "undefined") ? "00" : _temp[1];
                    if (bdisplayprice && parseInt(dig, 10) === 0) {
                        dig = "00";
                    } else {
                        dig = dig.toString();
                        if (dig.length > digits) {
                            dig = (Math.round(parseFloat("0." + dig) * Math.pow(10, digits))).toString();
                        }
                        for (i = dig.length; i < digits; i++) {
                            dig += "0";
                        }
                    }
                    var num = _temp[0];
                    var s = "",
                        ii = 0;
                    for (i = num.length - 1; i > -1; i--) {
                        s = ((ii++ % 3 === 2) ? ((i > 0) ? thoSeperator : "") : "") + num.substr(i, 1) + s;
                    }
                    return s + decSeperator + dig;
                }
                return price;
            }
        }).directive('format', ['$filter', function ($filter) {
            return {
                require: '?ngModel',
                link: function (scope, elem, attrs, ctrl) {
                    if (!ctrl) return;

                    ctrl.$formatters.unshift(function (a) {
                        return $filter(attrs.format)(ctrl.$modelValue)
                    });
                }
            };
        }]);

    tablaController.$inject = ['$scope', 'wbsServicio', 'CacheServicios', 'templatesServicio', '$timeout', 'utilidades', '$filter', '$uibModal', '$http', 'ejecutorReglasServicios', '$localStorage'];
    function tablaController($scope, wbsServicio, CacheServicios, templatesServicio, $timeout, utilidades, $filter, $uibModal, $http, ejecutorReglasServicios, $localStorage) {
        var vm = this;

        vm.templatesServicio = templatesServicio;
        vm.datoAGuardar = {};
        vm.pieDeTabla = {};
        vm.adicionarFilaTabla = adicionarFilaTabla;
        vm.visibilidadFilaAdicionar = false;
        vm.adicionarDato = adicionarDato;
        vm.eliminarDato = eliminarDato;
        vm.adicionarNodo = adicionarNodo;
        vm.cancelar = cancelar;
        vm.cancelarCambios = cancelarCambios;
        vm.totalizar = totalizar;
        vm.filtrar = filtrar;
        vm.desHabilitarBottonAdiccionar = true;
        vm.tieneCabecera = tieneCabecera;
        vm.getCabecera = getCabecera;
        vm.adicionarDesglose = adicionarDesglose;
        vm.cabeceras = [];
        vm.getCache = getCache;
        vm.getCache1 = getCache1;
        vm.newIsArray = newIsArray;
        vm.nuevosItensModalInstance = {};
        vm.getTiposCatalogo = getTiposCatalogo;
        vm.tieneScroll = tieneScroll;
        vm.tieneAccion = tieneAccion;
        vm.esCabeceraoDetalle = false;
        vm.aplicarReglasEdicion = aplicarReglasEdicion;
        vm.aplicarReglasAgregar = aplicarReglasAgregar;
        vm.exportarEstructura = exportarEstructura;
        vm.onKeypressEvent = onKeypressEvent;
        vm.onKeypressDecimalEvent = onKeypressDecimalEvent;
        vm.removerDecimales = removerDecimales;
        vm.listaRCache = [];
        this.$onInit = function () {
            obterListaRCache();

            return $timeout(function () {
                totalizar();
                cabeceraDetalle();

                if (vm.datos) {
                    if (vm.inserirPrimeiroFilho) {
                        adicionarFilaTabla();
                        vm.inserirPrimeiroFilho = false;
                    }
                }
            }, 1000);

        }

        function removerDecimales(event) {
            $(event.target).val(function (index, value) {
                return value.replaceAll(",","");
            });
        }

        function onKeypressEvent(event) {
            $(event.target).val(function (index, value) {
                value = parseFloat(value).toFixed(0);
                return value.toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",");
            });
        }

        function onKeypressDecimalEvent(event) {
            $(event.target).val(function (index, value) {
                value = parseFloat(value).toFixed(2);
                return  value.toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",");
            });
        }

        function evaluar(expresion1, operador, expresion2) {
            return ejecutorReglasServicios.evaluar(expresion1, operador, expresion2);
        }

        function obtenerValorWBS(propiedad, entidad, formato) {
            var valorPropiedad = obtenerValorEnDatos(vm.datosMaster, entidad, propiedad, {})
            return ejecutorReglasServicios.converterValorWBS(valorPropiedad.value, formato)
        }

        function controlarError(error) {
            console.log(error)
        }

        function obtenerValor(valor, formato) {
            return ejecutorReglasServicios.obtenerValor(vm.model, valor, formato);
        }

        function obtenerValorEnDatos(dato, entidad, campo, propiedad) {
            var prop = propiedad;
            _.each(dato, function (item) {

                if (!prop.found) {
                    if (item != null && typeof (item) === 'object' && !Array.isArray(item)) {
                        if (item.hasOwnProperty(campo) && !Array.isArray(item[campo])) {
                            prop.value = item[campo]
                            if (item === entidad) {
                                prop.found = true;
                                return;
                            }
                            obtenerValorEnDatos(item, entidad, campo, prop)
                        }
                        else if (item === entidad) {
                            prop.found = true;
                            return;
                        }
                        else {
                            obtenerValorEnDatos(item, entidad, campo, prop)
                        }
                    }
                    else if (Array.isArray(item)) {
                        obtenerValorEnDatos(item, entidad, campo, prop)
                    }
                }
            })
            return prop;
        }

        function establecerValorFijo(propiedadRegla, propiedadEditada, oldValue, newValue, valor, entidad) {
            if (propiedadRegla == propiedadEditada) {
                entidad[propiedadEditada] = valor;
                mostrarAlertaRegla("La propiedad " + propiedadRegla + " deve tener solo el valor: " + valor);
            }

        }

        function establecerValorMinimo(propiedadRegla, propiedadEditada, oldValue, newValue, valor, entidad, nuevaEntidad, tipoReglaAplicada) {
            newValue = ejecutorReglasServicios.converterValorWBS(newValue, "integer");

            if (propiedadRegla == propiedadEditada) {
                if (newValue < valor) {
                    if (tipoReglaAplicada === 'agregar') {
                        nuevaEntidad[propiedadEditada] = oldValue;
                    }
                    else {
                        entidad[propiedadEditada] = oldValue;
                    }
                    mostrarAlertaRegla("La propiedad " + propiedadRegla + " deve tener el valor mínimo de: " + valor);
                }
            }

        }

        function establecerValorMaximo(propiedadRegla, propiedadEditada, oldValue, newValue, valor, entidad, nuevaEntidad, tipoReglaAplicada) {
            newValue = ejecutorReglasServicios.converterValorWBS(newValue, "integer");

            if (propiedadRegla == propiedadEditada) {
                if (newValue > valor) {
                    if (tipoReglaAplicada === 'agregar') {
                        nuevaEntidad[propiedadEditada] = oldValue;
                    }
                    else {
                        entidad[propiedadEditada] = oldValue;
                    }

                    mostrarAlertaRegla("La propiedad " + propiedadRegla + "  deve tener el valor máximo de: " + valor);
                }
            }
        }

        function noSePuedeModificar(propiedadRegla, propiedadEditada, oldValue, newValue, valor, entidad, nuevaEntidad, tipoReglaAplicada) {
            if (propiedadRegla == propiedadEditada) {
                if (tipoReglaAplicada === 'agregar') {
                    nuevaEntidad[propiedadEditada] = oldValue;
                }
                else {
                    entidad[propiedadEditada] = oldValue;
                }
                mostrarAlertaRegla("La propiedad " + propiedadRegla + " no se puede modificar");
            }
        }



        function aplicarReglasAgregar(oldValue, newValue, entidad, colDef) {
            var nuevaEntidad = entidad;
            var tipoReglaAplicada = 'agregar';
            entidad = vm.datos;
            if (oldValue == newValue) {
                return;
            }

            var propiedadEditada = colDef.propiedad;

            eval(vm.schema.reglasAlAgregar);
            return true;
        }

        function aplicarReglasEdicion(oldValue, newValue, entidad, colDef, reglas) {
            var tipoReglaAplicada = 'actualizar';
            var nuevaEntidad = null;
            if (oldValue == newValue) {
                return;
            }

            var propiedadEditada = colDef.propiedad;

            eval(vm.schema.reglasAlActualizar);

            return true;
        }

        function mostrarAlertaRegla(texto) {
            swal({
                title: "Error de validación de la regla",
                text: texto,
                type: 'warning',
                showCancelButton: false,
                confirmButtonText: "Ok",
                closeOnConfirm: true
            }, function (isConfirm) {
            });
        }

        function tieneScroll() {
            let table = document.getElementsByClassName('over-auto')[0];
            let tieneScroll = false;
            if (table)
                tieneScroll = table.scrollWidth > table.clientWidth;

            if (tieneScroll) {
                table.classList.add('tiene-margin');
                return true;
            }

            return false;
        }

        function tieneAccion() {
            return (vm.modelo.desglose &&
                vm.modelo.desglose > 0 &&
                vm.modelo.desglose > vm.nivelDesglose) || vm.modelo.eliminar;
        }

        function getCache(id, item) {
            var idCache = item.cacheValue;
            try {
                if (!item[idCache]) {
                    item[idCache] = {};
                    CacheServicios.obtenerCatalogo(item.rCache + '/' + id).then(function (data) { item[idCache].cacheValue = data.Name; })
                }
                return item[idCache].cacheValue || idCache;
            } catch (e) {

            }
        }

        function getCache1(id, item) {
            var idCache = id;
            try {
                if (!item[idCache]) {
                    item[idCache] = {};
                    CacheServicios.obtenerCatalogo(item.rCache + '/' + id).then(function (data) { item[idCache].cacheValue = data.Name; })
                }
                return item[idCache].cacheValue || idCache;
            } catch (e) {

            }
        }

        function obterListaRCache() {
            var catalogos = [];
            vm.modelo.items.forEach(function (item) {

                if (item.rCache && !(catalogos.filter(p => Object.keys(p) == item.rCache).length > 0)) {

                    CacheServicios.obtenerCatalogo(item.rCache).then(function (datos) {

                        if (datos) {
                            catalogos.push({ [item.rCache]: datos });
                        }

                    });
                }
            });

            vm.listaRCache = catalogos;
            console.log(vm.listaRCache);
        }

        function getTiposCatalogo(id, item, model) {
            var dato = {};
            let cache = vm.listaRCache.find(p => Object.keys(p) == item.rCache)[item.rCache]
            dato[item.rCache] = [...cache];

            //tratamiento para departamentos y municipios
            dato = tratarListaRCache(dato, item, model);

            return dato ? dato[item.rCache] : [];
        }

        function tratarListaRCache(dato, item, model) {
            let itensConCache = vm.modelo.items.filter(x => x.rCache);

            if (item.rCache === 'Municipios') {
                let existeDepartamentoCache = itensConCache.filter(x => x.rCache === 'Departamentos')

                if (existeDepartamentoCache.length && model[existeDepartamentoCache[0].propiedad] != undefined) {
                    let idDepartamento = parseInt(model[existeDepartamentoCache[0].propiedad])
                    dato.Municipios = dato.Municipios.filter(x => x.DepartmentId === idDepartamento)
                }
            }


            if (item.rCache === 'Departamentos') {
                let existeRegionCache = itensConCache.filter(x => x.rCache === 'Regiones')

                if (existeRegionCache.length && model[existeRegionCache[0].propiedad] != undefined) {
                    let idRegion = parseInt(model[existeRegionCache[0].propiedad])
                    dato.Departamentos = dato.Departamentos.filter(x => x.RegionId === idRegion)
                }
            }

            if (item.rCache === 'Entidades') {
                let existeTipoEntidadCache = itensConCache.filter(x => x.rCache === 'TiposEntidades')

                if (existeTipoEntidadCache.length && model[existeTipoEntidadCache[0].propiedad] != undefined) {
                    let idTipoEntidad = parseInt(model[existeTipoEntidadCache[0].propiedad])
                    dato.Entidades = dato.Entidades.filter(x => x.EntityTypeId === idTipoEntidad)
                }
            }

            if (item.rCache === 'TiposEntidades') {
                let existeGruposRecursosCache = itensConCache.filter(x => x.rCache === 'GruposRecursos')

                if (existeGruposRecursosCache.length && model[existeGruposRecursosCache[0].propiedad] != undefined) {
                    let idGrupoRecurso = parseInt(model[existeGruposRecursosCache[0].propiedad])
                    dato.TiposEntidades = dato.TiposEntidades.filter(x => x.ResourceGroupId === idGrupoRecurso)
                }
            }

            if (item.rCache === 'IndicadoresPoliticas') {
                let existePoliticaNivel1Cache = itensConCache.filter(x => x.rCache === 'PoliticasNivel1')

                if (existePoliticaNivel1Cache.length && model[existePoliticaNivel1Cache[0].propiedad] != undefined) {
                    let idPoliticaN1 = parseInt(model[existePoliticaNivel1Cache[0].propiedad])
                    dato.IndicadoresPoliticas = dato.IndicadoresPoliticas.filter(x => x.PolicyTargetingId === idPoliticaN1)
                }
            }

            if (item.rCache === 'Entregables') {
                let existeProductosCache = itensConCache.filter(x => x.rCache === 'Productos')

                if (existeProductosCache.length && model[existeProductosCache[0].propiedad] != undefined) {
                    let idProducto = parseInt(model[existeProductosCache[0].propiedad])
                    dato.Entregables = dato.Entregables.filter(x => x.ProductCId === idProducto)
                }
            }

            if (item.rCache === 'Productos') {
                let existeProgramasCache = itensConCache.filter(x => x.rCache === 'Programas')

                if (existeProgramasCache.length && model[existeProgramasCache[0].propiedad] != undefined) {
                    let idPrograma = parseInt(model[existeProgramasCache[0].propiedad])
                    dato.Productos = dato.Productos.filter(x => x.ProgramId === idPrograma)
                }
            }

            if (item.rCache === 'PoliticasNivel1') {
                let existePoliticaCache = itensConCache.filter(x => x.rCache === 'Politicas')

                if (existePoliticaCache.length && model[existePoliticaCache[0].propiedad] != undefined) {
                    let idPolitica = parseInt(model[existePoliticaCache[0].propiedad])
                    dato.PoliticasNivel1 = dato.PoliticasNivel1.filter(x => x.ParentId === idPolitica)
                }
            }

            if (item.rCache === 'PoliticasNivel2') {
                let existePoliticaNivel1Cache = itensConCache.filter(x => x.rCache === 'PoliticasNivel1')

                if (existePoliticaNivel1Cache.length && model[existePoliticaNivel1Cache[0].propiedad] != undefined) {
                    let idPoliticaNivel1 = parseInt(model[existePoliticaNivel1Cache[0].propiedad])
                    dato.PoliticasNivel2 = dato.PoliticasNivel2.filter(x => x.ParentId === idPoliticaNivel1)
                }
            }

            if (item.rCache === 'PoliticasNivel3') {
                let existePoliticaNivel2Cache = itensConCache.filter(x => x.rCache === 'PoliticasNivel2')

                if (existePoliticaNivel2Cache.length && model[existePoliticaNivel2Cache[0].propiedad] != undefined) {
                    let idPoliticaNivel2 = parseInt(model[existePoliticaNivel2Cache[0].propiedad])
                    dato.PoliticasNivel3 = dato.PoliticasNivel2.filter(x => x.ParentId === idPoliticaNivel2)
                }
            }

            if (item.rCache === 'TiposRecursos') {
                let existeTipoEntidadCache = itensConCache.filter(x => x.rCache === 'TiposEntidades')
                let existeGrupoRecursoCache = itensConCache.filter(x => x.rCache === 'GruposRecursos')
                console.log(existeTipoEntidadCache);
                console.log(existeGrupoRecursoCache);
                if (existeTipoEntidadCache.length > 0) {
                    if (model[existeTipoEntidadCache[0].propiedad] != undefined) {
                        let idTipoEntidad = parseInt(model[existeTipoEntidadCache[0].propiedad])
                        if (existeGrupoRecursoCache.length > 0) {
                            if (model[existeGrupoRecursoCache[0].propiedad] != undefined) {
                                let idGrupoRecurso = parseInt(model[existeGrupoRecursoCache[0].propiedad])
                                dato.TiposRecursos = dato.TiposRecursos.filter(x => x.EntityTypeId === idTipoEntidad && x.ResourceGroupId == idGrupoRecurso)
                            }
                        }
                        else {
                            dato.TiposRecursos = dato.TiposRecursos.filter(x => x.EntityTypeId === idTipoEntidad)
                        }

                    }

                }
            }
            if (item.rCache === 'GruposRecursos') {
                if ($localStorage.GruposRecursosNoId != null) {
                    dato.GruposRecursos = dato.GruposRecursos.filter(element => element.Id != $localStorage.GruposRecursosNoId);
                }
                else if ($localStorage.GruposRecursosId != null) {
                    dato.GruposRecursos = dato.GruposRecursos.filter(element => element.Id == $localStorage.GruposRecursosId);
                }
            }

            if (item.rCache === 'Agrupaciones') {
                let existeMunicipiosCache = itensConCache.filter(x => x.rCache === 'Municipios')
                let existeTiposAgrupacionesCache = itensConCache.filter(x => x.rCache === 'TiposAgrupaciones')
                console.log(existeMunicipiosCache);
                console.log(existeTiposAgrupacionesCache);
                if (existeMunicipiosCache.length > 0) {
                    if (model[existeMunicipiosCache[0].propiedad] != undefined) {
                        let idMunicipio = parseInt(model[existeMunicipiosCache[0].propiedad])
                        if (existeTiposAgrupacionesCache.length > 0) {
                            if (model[existeTiposAgrupacionesCache[0].propiedad] != undefined) {
                                let idTipoAgrupacion = parseInt(model[existeTiposAgrupacionesCache[0].propiedad])
                                dato.Agrupaciones = dato.Agrupaciones.filter(x => x.MunicipalityId === idMunicipio && x.TipoAgrupacionId == idTipoAgrupacion)
                            }
                        }
                        else {
                            dato.Agrupaciones = dato.Agrupaciones.filter(x => x.MunicipalityId === idMunicipio)
                        }
                    }
                }
            }

            return dato;
        }

        function filtrar(nodo) {
            if (!nodo.hasOwnProperty('Filtrar')) {
                return true;
            }
            else {
                return nodo.Filtrar;
            }
        }

        function cabeceraDetalle() {
            if (vm.datos)
                vm.cabeceras = vm.getCabecera(vm.datos[vm.modelo.propiedad], vm.modelo.items);
        }

        function getCabecera(datos, cols) {
            if (!datos || !tieneCabecera(cols)) return datos;
            var isCabecera = false;
            var hashs = [];
            var result = [];
            var cabeceras = [];
            var detalles = [];
            datos.forEach(function (item) {
                var hash = "";

                cols.forEach(function (col) {
                    if (col.cabecera) {
                        isCabecera = true;
                        vm.esCabeceraoDetalle = true;
                        hash += item[col.propiedad]
                    }
                })

                var detalle = Object.assign({}, item);
                detalle.cabeceraHash = hash;
                detalles.push(detalle)

                if (!hashs.includes(hash)) {
                    hashs.push(hash)
                    item.cabeceraHash = hash;
                    item.isCabecera = true;
                    vm.esCabeceraoDetalle = true;
                    cabeceras.push(item);
                }
            })

            if (!isCabecera)
                return datos;

            cabeceras.forEach(function (item) {
                item.items = detalles.filter(function (detalle) {
                    return detalle.cabeceraHash === item.cabeceraHash;
                })
                result.push(item);
            })

            return result;
        }

        function tieneCabecera(cols) {
            if (!cols) return false;
            var tiene = false;
            cols.forEach(function (col) {
                if (col.cabecera)
                    tiene = true;
            })
            return tiene;
        }

        function adicionarFilaTabla(modelo) {
            if (!vm.visibilidadFilaAdicionar) {
                vm.visibilidadFilaAdicionar = true;
                vm.desHabilitarBottonAdiccionar = false;
                openModaleNuevosItens();
            }
            else {
                vm.visibilidadFilaAdicionar = false;
                vm.nuevosItensModalInstance.close("cancel");
            }
        }

        function openModaleNuevosItens() {
            vm.nuevosItensModalInstance = $uibModal.open({
                animation: true,
                templateUrl: '/src/app/comunes/templates/modales/adicionarNuevosItens/adicionarNuevosItensWbs.html',
                scope: $scope,
                controllerAs: "vm",
                backdrop: false,
                keyboard: false,
                size: 'lg'
            });
        }

        function eliminarDato(dato, propiedad) {
            utilidades.mensajeWarning($filter('language')('ConfirmarEliminarRegistro'),
                function () {
                    if (vm.datos &&
                        vm.datos[propiedad] &&
                        vm.datos[propiedad].length > 0) {

                        var indice = vm.datos[propiedad].indexOf(dato);
                        if (indice != -1) {
                            vm.datos[propiedad].splice(indice, 1);
                        }
                        if (vm.esCabeceraoDetalle) {
                            cabeceraDetalle();
                        }
                        totalizar();
                        $scope.$apply();

                    }
                });
        }

        function adicionarNodo(dato, propiedad) {
            if (vm.datos &&
                vm.datos[propiedad] &&
                vm.datos[propiedad].length > 0) {

                var indice = vm.datos[propiedad].indexOf(dato);
                if (indice != -1) {
                    vm.inserirPrimeiroFilho = true
                    vm.datos[propiedad][indice][propiedad] = []
                    sessionStorage.setItem("tempDesglose", JSON.stringify([propiedad, indice, propiedad]));
                }

                totalizar();
            }
        }

        function adicionarDesglose(dato) {
            if (dato) {
                return !dato.isCabecera && (vm.modelo.desglose && vm.modelo.desglose > 0 && vm.modelo.desglose > vm.nivelDesglose);
            }
            return false;
        }

        function newIsArray(dato, propiedad) {
            if (dato) {
                return dato[propiedad] && Array.isArray(dato[propiedad])
            }
            return false
        }

        function cancelar() {
            utilidades.mensajeWarning($filter('language')('ConfirmaCancelar'), vm.cancelarCambios);
        }

        function cancelarCambios() {
            var tempDesglose = JSON.parse(sessionStorage.getItem("tempDesglose"));
            if (tempDesglose)
                vm.datos[tempDesglose[0]] = null;

            vm.datoAGuardar = {};
            vm.visibilidadFilaAdicionar = false;
            vm.desHabilitarBottonAdiccionar = true;
            $scope.$apply();

            vm.nuevosItensModalInstance.close("adicionarDato");
        }

        function ajustaTipoDato() {
            vm.modelo.items.filter(x => x.visible && x.rCache)
                .forEach(x => vm.datoAGuardar[x.propiedad] = parseInt(vm.datoAGuardar[x.propiedad]));
        }

        function exportarEstructura() {
            var Results = [];
            for (var i = 0; i < vm.modelo.items.length; i++) {
                if (vm.modelo.items[i].visible === true)
                    Results.push(vm.modelo.items[i].propiedad);
            }
            var CsvString = "";
            Results.forEach(function (ColItem, RowIndex) {
                if (RowIndex == 0)
                    CsvString += ColItem;
                else
                    CsvString += ',' + ColItem;
            });

            for (var i = 0; i < vm.datos[vm.modelo.propiedad].length; i++) {
                CsvString += "\r\n";
                Results.forEach(function (ColItem, RowIndex) {
                    var item = vm.modelo.items.filter(function (item) {
                        return item.propiedad == ColItem;
                    });
                    if (RowIndex == 0) {
                        if (item[0].rCache != undefined)
                            CacheServicios.obtenerCatalogo(item[0].rCache + '/' + vm.datos[vm.modelo.propiedad][i][ColItem]).then(function (data) { CsvString += data.Name; });
                        else
                            CsvString += vm.datos[vm.modelo.propiedad][i][ColItem];
                    }
                    else {
                        if (item[0].rCache != undefined) {
                            if (vm.datos[vm.modelo.propiedad][i][ColItem] != null) {
                                var prueba = vm.getTiposCatalogo(vm.datos[vm.modelo.propiedad][i][ColItem], item[0], vm.datoAGuardar);
                                var nombreEs = prueba.filter(function (item) {
                                    return item.Id == vm.datos[vm.modelo.propiedad][i][ColItem];
                                });
                                CsvString += ',' + nombreEs[0].Name;
                            }
                            else
                                CsvString += ',' + 'null';


                        }
                        else
                            CsvString += ',' + vm.datos[vm.modelo.propiedad][i][ColItem];
                    }

                });
            }
            CsvString = "data:application/csv," + encodeURIComponent(CsvString);
            var x = document.createElement("A");
            x.setAttribute("href", CsvString);
            x.setAttribute("download", "somedata.csv");
            document.body.appendChild(x);
            x.click();
        }

        function adicionarDato() {
            utilidades.mensajeWarning($filter('language')('ConfirmarGuardar'),
                function () {

                    vm.visibilidadFilaAdicionar = false;

                    if (!vm.datos.hasOwnProperty(vm.modelo.propiedad)) {
                        vm.datos[vm.modelo.propiedad] = [];
                    }
                    ajustaTipoDato();

                    vm.datos[vm.modelo.propiedad].push(vm.datoAGuardar);

                    
                    totalizar();
                    vm.datoAGuardar = {};
                    vm.desHabilitarBottonAdiccionar = true;

                    if (vm.esCabeceraoDetalle) {
                        cabeceraDetalle();
                    }

                    $scope.$apply();
                    vm.nuevosItensModalInstance.close("adicionarDato");
                });
        }

        $scope.SelectFile = function (evt) {
            let file = evt.target.files[0];
            let reader = new FileReader();
            reader.onload = (e) => {
                // Cuando el archivo se terminó de cargar
                let lines = parseCSV(e.target.result);
                let output = reverseMatrix(lines);

                for (var i = 0; i < vm.datos[vm.modelo.propiedad].length; i++) {
                    var lista = Object.keys(vm.datos[vm.modelo.propiedad][i]);
                    for (var j = 0; j < lista.length; j++) {
                        var estructura = vm.modelo.items.filter(function (item) {
                            return item.propiedad == lista[j];
                        });
                        if (estructura.length > 0) {
                            if (estructura[0].visible == true && estructura[0].editar == true && estructura[0].rCache == undefined) {
                                for (var k = 0; k < output.length; k++) {
                                    if (output[k][0] == lista[j]) {
                                        vm.datos[vm.modelo.propiedad][i][lista[j]] = output[k][i+1];
                                    }
                                }

                            }
                        }
                        
                    }
                    
                }

                if (vm.esCabeceraoDetalle) {
                    cabeceraDetalle();
                }

                $scope.$apply();
            };
            // Leemos el contenido del archivo seleccionado
            reader.readAsBinaryString(file);
        };

        function reverseMatrix(matrix) {
            let output = [];
            // Por cada fila
            matrix.forEach((values, row) => {
                // Vemos los valores y su posicion
                values.forEach((value, col) => {
                    // Si la posición aún no fue creada
                    if (output[col] === undefined) output[col] = [];
                    output[col][row] = value;
                });
            });
            return output;
        }

        function parseCSV(text) {
            // Obtenemos las lineas del texto
            let lines = text.replace(/\r/g, '').split('\n');
            return lines.map(line => {
                // Por cada linea obtenemos los valores
                let values = line.split(',');
                return values;
            });
        }

        function totalizar() {
            vm.modelo.items.forEach(function (item) {
                if (item.visible) {
                    vm.pieDeTabla[item.propiedad] = {
                        total: 0,
                        totalizar: (item.totalizar && (['decimal', 'int', 'integer', 'number', 'double'].indexOf(item.tipo) > -1)),
                        tipo: item.tipo
                    };
                }
            });

            angular.forEach(vm.datos, function (value) {
                var tipoArray = tipoDeAtributo(value);
                if (tipoArray === "array") {
                    value.forEach(function (valor) {
                        angular.forEach(valor, function (valor, propiedad) {
                            if (vm.pieDeTabla.hasOwnProperty(propiedad) && vm.pieDeTabla[propiedad].totalizar) {
                                if (isNaN(valor)) {
                                    valor = 0;
                                } else if (!valor) {
                                    valor = 0;
                                }

                                switch (vm.pieDeTabla[propiedad].tipo) {
                                    case 'number':
                                        valor = Number(valor); break;
                                    case 'integer':
                                        valor = parseInt(valor, 10); break;
                                }

                                vm.pieDeTabla[propiedad].total += valor;
                            }
                        });
                    });
                }
            });
        }

        function tipoDeAtributo(obj) {
            return ({}).toString.call(obj).match(/\s([a-z|A-Z]+)/)[1].toLowerCase();
        }

    }

    function tablaWbs() {
        return {
            restrict: 'E',
            transclude: true,
            scope: {
                datos: '=',
                modelo: '=',
                opciones: '<',
                inserirPrimeiroFilho: '=',
                nivelDesglose: '=',
                datosMaster: '=',
                schema: '='
            },
            templateUrl: "/src/app/formulario/componentes/WBS/directivas/tabla/tabla.template.html",
            controller: tablaController,
            controllerAs: 'vm',
            bindToController: true
        };
    }

})();