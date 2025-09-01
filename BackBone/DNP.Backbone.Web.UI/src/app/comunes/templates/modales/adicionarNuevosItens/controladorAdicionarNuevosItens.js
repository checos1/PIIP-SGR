(function () {
    'use strict';

    controladorAdicionarNuevosItens.$inject = ['$scope', '$uibModalInstance', '$http', 'items', '$localStorage', 'constantesTipoFiltro', 'CacheServicios'];

    function controladorAdicionarNuevosItens($scope, $uibModalInstance, $http, items, $localStorage, constantesTipoFiltro, CacheServicios) {
        var vm = this;
        // vm.columnasSelecionadas = [];
        vm.agregarItem = items.agregarItem;
        vm.items = items;
        vm.datoAGuardar = {};
        vm.cancelar = cancelar;
        vm.adicionarDato = adicionarDato;
        vm.modelo = { items: vm.items.campos }
        vm.listaRCache = null;
        vm.getTiposCatalogo = getTiposCatalogo;
        vm.esEdicion = false;

        // vm.columnaSeleccionada = null;
        // vm.ladoSeleccionado = null;
        //vm.seleccionarColumna = function (columna, lado) {
        //    vm.columnaSeleccionada = columna;
        //    vm.ladoSeleccionado = lado;
        //}

        this.$onInit = function () {
            if (vm.items) {
                if (vm.items.datos)
                    vm.esEdicion = true;
                vm.items.campos.forEach(function (item) {
                    vm.datoAGuardar[item.field] = vm.items.datos ? vm.items.datos[item.field] : ''
                })
            }
            obterListaRCache();
        }

        vm.log = function (item) {
        }


        function obterListaRCache() {
            var catalogos = [];
            vm.items.campos.forEach(function (item) {
                if (item.rCache && !(catalogos.filter(p => Object.keys(p) == item.rCache).length > 0)) {
                    CacheServicios.obtenerCatalogo(item.rCache).then(function (datos) {
                        if (datos) {
                            catalogos.push({ [item.rCache]: datos });
                            vm.listaRCache = catalogos;
                        }

                    });
                }
            });
        }

        function getTiposCatalogo(id, item, model) {
            var dato = {};
            if (!vm.listaRCache)
                return dato;
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

                if (existeDepartamentoCache.length && model[existeDepartamentoCache[0].field] != undefined) {
                    let idDepartamento = parseInt(model[existeDepartamentoCache[0].field])
                    dato.Municipios = dato.Municipios.filter(x => x.DepartmentId === idDepartamento)
                }
            }

            if (item.rCache === 'Departamentos') {
                let existeRegionCache = itensConCache.filter(x => x.rCache === 'Regiones')

                if (existeRegionCache.length && model[existeRegionCache[0].field] != undefined) {
                    let idRegion = parseInt(model[existeRegionCache[0].field])
                    dato.Departamentos = dato.Departamentos.filter(x => x.RegionId === idRegion)
                }
            }

            if (item.rCache === 'Entidades') {
                let existeTipoEntidadCache = itensConCache.filter(x => x.rCache === 'TiposEntidades')

                if (existeTipoEntidadCache.length && model[existeTipoEntidadCache[0].field] != undefined) {
                    let idTipoEntidad = parseInt(model[existeTipoEntidadCache[0].field])
                    dato.Entidades = dato.Entidades.filter(x => x.EntityTypeId === idTipoEntidad)
                }
            }

            if (item.rCache === 'TiposEntidades') {
                let existeGruposRecursosCache = itensConCache.filter(x => x.rCache === 'GruposRecursos')

                if (existeGruposRecursosCache.length && model[existeGruposRecursosCache[0].field] != undefined) {
                    let idGrupoRecurso = parseInt(model[existeGruposRecursosCache[0].field])
                    dato.TiposEntidades = dato.TiposEntidades.filter(x => x.ResourceGroupId === idGrupoRecurso)
                }
            }

            if (item.rCache === 'IndicadoresPoliticas') {
                let existePoliticaNivel1Cache = itensConCache.filter(x => x.rCache === 'PoliticasNivel1')

                if (existePoliticaNivel1Cache.length && model[existePoliticaNivel1Cache[0].field] != undefined) {
                    let idPoliticaN1 = parseInt(model[existePoliticaNivel1Cache[0].field])
                    dato.IndicadoresPoliticas = dato.IndicadoresPoliticas.filter(x => x.PolicyTargetingId === idPoliticaN1)
                }
            }

            if (item.rCache === 'Entregables') {
                let existeProductosCache = itensConCache.filter(x => x.rCache === 'Productos')

                if (existeProductosCache.length && model[existeProductosCache[0].field] != undefined) {
                    let idProducto = parseInt(model[existeProductosCache[0].field])
                    dato.Entregables = dato.Entregables.filter(x => x.ProductCId === idProducto)
                }
            }

            if (item.rCache === 'Productos') {
                let existeProgramasCache = itensConCache.filter(x => x.rCache === 'Programas')

                if (existeProgramasCache.length && model[existeProgramasCache[0].field] != undefined) {
                    let idPrograma = parseInt(model[existeProgramasCache[0].field])
                    dato.Productos = dato.Productos.filter(x => x.ProgramId === idPrograma)
                }
            }


            if (item.rCache === 'PoliticasNivel1') {
                let existePoliticaCache = itensConCache.filter(x => x.rCache === 'Politicas')

                if (existePoliticaCache.length && model[existePoliticaCache[0].field] != undefined) {
                    let idPolitica = parseInt(model[existePoliticaCache[0].field])
                    dato.PoliticasNivel1 = dato.PoliticasNivel1.filter(x => x.ParentId === idPolitica)
                }
            }

            if (item.rCache === 'PoliticasNivel2') {
                let existePoliticaNivel1Cache = itensConCache.filter(x => x.rCache === 'PoliticasNivel1')

                if (existePoliticaNivel1Cache.length && model[existePoliticaNivel1Cache[0].field] != undefined) {
                    let idPoliticaNivel1 = parseInt(model[existePoliticaNivel1Cache[0].field])
                    dato.PoliticasNivel2 = dato.PoliticasNivel2.filter(x => x.ParentId === idPoliticaNivel1)
                }
            }

            if (item.rCache === 'PoliticasNivel3') {
                let existePoliticaNivel2Cache = itensConCache.filter(x => x.rCache === 'PoliticasNivel2')

                if (existePoliticaNivel2Cache.length && model[existePoliticaNivel2Cache[0].field] != undefined) {
                    let idPoliticaNivel2 = parseInt(model[existePoliticaNivel2Cache[0].field])
                    dato.PoliticasNivel3 = dato.PoliticasNivel2.filter(x => x.ParentId === idPoliticaNivel2)
                }
            }

            if (item.rCache === 'TiposRecursos') {
                let existeTipoEntidadCache = itensConCache.filter(x => x.rCache === 'TiposEntidades')
                let existeGrupoRecursoCache = itensConCache.filter(x => x.rCache === 'GruposRecursos')
                if (existeTipoEntidadCache.length > 0) {
                    if (model[existeTipoEntidadCache[0].field] != undefined) {
                        let idTipoEntidad = parseInt(model[existeTipoEntidadCache[0].field])
                        if (existeGrupoRecursoCache.length > 0) {
                            if (model[existeGrupoRecursoCache[0].field] != undefined) {
                                let idGrupoRecurso = parseInt(model[existeGrupoRecursoCache[0].field])
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
                if (existeMunicipiosCache.length > 0) {
                    if (model[existeMunicipiosCache[0].propiedad] != undefined) {
                        let idMunicipio = parseInt(model[existeMunicipiosCache[0].field])
                        if (existeTiposAgrupacionesCache.length > 0) {
                            if (model[existeTiposAgrupacionesCache[0].propiedad] != undefined) {
                                let idTipoAgrupacion = parseInt(model[existeTiposAgrupacionesCache[0].field])
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

        function adicionarDato() {
            Object.keys(vm.datoAGuardar).forEach(function (item) {
                vm.items.campos.forEach(function (col) {
                    if (col.rCache) {
                        var catalogos = getTiposCatalogo(null, col, vm.modelo)
                        try {
                            var tempID = (catalogos.find(p => p.Id.toString() == vm.datoAGuardar[col.field]))
                            var nombreVariable = col.field;
                            var codigoVariable = col.field;
                            nombreVariable = nombreVariable.replace('Id', 'Nombre');
                            codigoVariable = codigoVariable.replace('Id', 'Codigo');
                            vm.datoAGuardar[col.field] = tempID.Name;
                            vm.datoAGuardar[nombreVariable] = tempID.Name;
                            vm.datoAGuardar[codigoVariable] = tempID.Id.toString();
                            vm.datoAGuardar[col.field + "RCacheOriginalValue"] = tempID.Id;
                        } catch (ex) {
                            console.log(ex)
                        }
                    }
                })
            })
            $uibModalInstance.close({ nuevosItem: vm.datoAGuardar, datos: vm.items.datos });
        };

        function cancelar() {
            $uibModalInstance.dismiss('cancel');
        };

    }

    angular.module('backbone').controller('controladorAdicionarNuevosItens', controladorAdicionarNuevosItens);
})();