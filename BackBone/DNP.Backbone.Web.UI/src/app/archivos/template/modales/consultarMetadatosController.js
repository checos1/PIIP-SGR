(function () {
    'use strict';

    angular.module('backbone.archivo').controller('ConsultarMetadatosController', consultarMetadatosController).filter('griddropdown', function () {
        return function (input, map, idField, valueField, initial) {
            if (initial === "undefined") {
                initial = "";
            }
            if (typeof map !== "undefined") {
                for (var i = 0; i < map.length; i++) {
                    if (map[i][idField] === input) {
                        return map[i][valueField];
                    }
                }
            } else if (initial) {
                return initial;
            }
            return input;
        };
    });

    consultarMetadatosController.$inject = [
        '$scope', '$timeout', '$filter', '$q', 'archivoServicios', 'utilidades', '$uibModalInstance', 'idAplicacion', 'idNivel', 'idsArchivos', 'idInstancia', 'idAccion', 'idObjetoNegocio'
    ];

    function consultarMetadatosController($scope, $timeout, $filter, $q, archivoServicios, utilidades, $uibModalInstance, idAplicacion, idNivel, idsArchivos, idInstancia, idAccion, idObjetoNegocio) {
        var vm = this;

        //Metodos
        vm.aceptarGuardadoMetadatos = aceptarGuardadoMetadatos;
        vm.borrarRegistroGrid = borrarRegistroGrid;
        vm.cancelarGuardarMetadatos = cancelarGuardarMetadatos;
        vm.confirmaCancelacion = confirmaCancelacion;
        vm.guardarMetadatos = guardarMetadatos;
        vm.prepararGridMetadatos = prepararGridMetadatos;
        vm.seleccionarMetadatoObligatorio = seleccionarMetadatoObligatorio;
        vm.construirObjetoArchivoMetadato = construirObjetoArchivoMetadato;
        vm.confirmarEliminarMetadato = confirmarEliminarMetadato;
        //Variables
        vm.faltanDatosCategoria = false;
        vm.faltanDatosMetadatos = false;
        vm.archivos = [];
        vm.archivosPorAplicacion = [];
        vm.archivosMetadatosCategoriasPorAplicacionNivel = [];
        vm.columnaInicialMetadatos = 4;
        vm.columnaMetadatoObligatorio = 4;
        vm.categoria;
        vm.categoriaObligatoria = undefined;
        vm.botonesTemplate = '<div class="text-center"><button id="btnBorrarRegistro" type="button" ng-class="\'btn btn-default btn-sm text-center\'" ng-click="grid.appScope.vm.confirmarEliminarMetadato(row.entity.Id)" tooltip-placement="bottom" uib-tooltip="Eliminar"><span class="icon-delete"></span></button></div>';
        vm.gridApi;
        vm.grillaMetadatos = {};
        vm.idAplicacion = idAplicacion;
        vm.idNivel = idNivel;
        vm.idInstancia = idInstancia,
            vm.idAccion = idAccion,
            vm.idObjetoNegocio = idObjetoNegocio,
            vm.idsArchivos = idsArchivos;
        vm.listaAcciones = ["Borrar"];
        vm.metadatos = [];
        vm.metadatosObligatorios = [];

        //Configuración grid
        vm.grillaMetadatos.onRegisterApi = function (gridApi) {
            vm.gridApi = gridApi;
            gridApi.rowEdit.on.saveRow($scope, function (rowEntity) {
                var promise = $q.defer();
                gridApi.rowEdit.setSavePromise(rowEntity, promise.promise);
                promise.resolve();
            });
            gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {
                construirObjetoArchivoMetadato(rowEntity, colDef);
            });
        };

        function aceptarGuardadoMetadatos() {
            $scope.$apply();
            $uibModalInstance.close(vm.archivosMetadatosCategoriasPorAplicacionNivel);
        }

        function cancelarGuardarMetadatos() {
            utilidades.mensajeWarning($filter('language')('CancelarAsociaciónMetadatos'), vm.confirmaCancelacion, null, $filter('language')('Aceptar'));
        }

        function confirmaCancelacion() {
            $scope.$apply();
            $uibModalInstance.close(null);
        }

        function confirmarEliminarMetadato(entity) {
            vm.metadatoAEliminar = entity;
            utilidades.mensajeWarning($filter('language')('EliminarMetadato'), vm.borrarRegistroGrid);
        }

        function borrarRegistroGrid() {
            vm.grillaMetadatos.data.splice(vm.grillaMetadatos.data.indexOf(vm.metadatoAEliminar), 1);
            vm.archivos.splice(vm.archivos.indexOf(vm.metadatoAEliminar), 1);
            $timeout(function () {
                utilidades.mensajeSuccess($filter('language')('EliminadoExitosoMetadato'), false);
            }, 500);
        }

        function crearColumnasGrilla(data) {
            var columns = [
                {
                    name: 'Nombre', displayName: 'Nombre', enableCellEdit: false, enableColumnMenu: false,
                    enableSorting: false
                },
                {
                    name: 'Extension', displayName: 'Extensión', enableCellEdit: false, enableColumnMenu: false,
                    enableSorting: false
                },
                {
                    name: 'Tamano', displayName: 'Tamaño', enableCellEdit: false, enableColumnMenu: false,
                    enableSorting: false
                },
                {
                    name: 'Descripcion',
                    displayName: '* Categoría',
                    enableSorting: false,
                    width: '30%',
                    editableCellTemplate: 'src/app/archivos/template/dropDownCategorias.html',
                    cellTemplate: 'src/app/archivos/template/dropDownCategorias.html',
                    enableCellEdit: true,
                    enableCellEditOnFocus: true,
                    enableFocusedCellEdit: true,
                    enableColumnMenu: false,
                    cellFilter: "griddropdown:editDropdownOptionsArray:editDropdownIdLabel:editDropdownValueLabel:row.entity.Descripcion",
                    editDropdownIdLabel: 'Id',
                    editDropdownValueLabel: 'Descripcion',
                    editDropdownOptionsArray: data.Categorias
                }
            ];
            _.each(data.Metadatos, function (data) {
                columns.push({
                    name: data.Nombre,
                    displayName: data.Obligatorio ? '* ' + data.Nombre : data.Nombre,
                    enableCellEdit: true,
                    enableCellEditOnFocus: true,
                    enableSorting: false,
                    enableColumnMenu: false,
                    type: 'text',
                    enableFocusedCellEdit: true,
                    editableCellTemplate: data.Obligatorio ? 'src/app/archivos/template/editMetadatosObligatorio.html' : 'src/app/archivos/template/editMetadatosNoObligatorio.html',
                    cellTemplate: data.Obligatorio ? 'src/app/archivos/template/editMetadatosObligatorio.html' : 'src/app/archivos/template/editMetadatosNoObligatorio.html'
                });
            });
            columns.push({ name: 'Acciones', cellTemplate: vm.botonesTemplate, enableCellEdit: false, enableSorting: false, enableColumnMenu: false });
            vm.grillaMetadatos = {
                columnDefs: columns,
                rowHeight: 180,
                enableCellSelection: true,
                enableRowSelection: false,
                enableColumnMenu: false,
                enableCellEditOnFocus: true,
                rowEditWaitInterval: -1
            };
            _.each(data.Archivos, function (archivo) {
                vm.archivosPorAplicacion.push(archivo);
            });
            vm.grillaMetadatos.data = vm.archivosPorAplicacion;
        }

        function construirObjetoArchivoMetadato(rowEntity) {
            var archivoMetadato = {
                IdAplicacion: vm.idAplicacion,
                IdNivel: vm.idNivel,
                IdArchivo: rowEntity.Id,
                IdCategoria: rowEntity.Descripcion === undefined ? 'undefined' : rowEntity.Descripcion === null ? "undefined" : rowEntity.Descripcion,
                IdInstancia: vm.idInstancia,
                IdAccion: vm.idAccion,
                IdObjetoNegocio: vm.idObjetoNegocio
            };
            var estaEnLista = listaContieneObjetoMetadato(archivoMetadato, vm.archivosMetadatosCategoriasPorAplicacionNivel);
            if (!estaEnLista) {
                vm.archivosMetadatosCategoriasPorAplicacionNivel.push(archivoMetadato);
            }
            construirObjetoMetadatoArchivo(rowEntity);
        }

        function construirObjetoMetadatoArchivo(rowEntity) {
            for (var key in rowEntity) {
                if (rowEntity.hasOwnProperty(key)) {
                    _.each(vm.metadatos, function (metadato) {
                        if (metadato.Nombre === key) {
                            var metadatoArchivo = {};
                            metadatoArchivo.IdMetadato = metadato.Id;
                            metadatoArchivo.Nombre = metadato.Nombre;
                            metadatoArchivo.Descripcion = metadato.Descripcion;
                            metadatoArchivo.Valor = rowEntity[key];
                            metadatoArchivo.IdArchivo = rowEntity.Id;
                            crearListaMetadatosArchivo(metadatoArchivo);
                        }
                    });
                }
            }
        }

        function crearListaMetadatosArchivo(metadatoArchivo) {
            _.each(vm.archivosMetadatosCategoriasPorAplicacionNivel, function (archivo) {
                if (archivo.IdArchivo === metadatoArchivo.IdArchivo) {
                    if (archivo.Metadatos === undefined) {
                        archivo.Metadatos = [];
                        archivo.Metadatos.push(metadatoArchivo);
                    } else {
                        if (archivo.IdArchivo === metadatoArchivo.IdArchivo) {
                            var estaEnLista = listaContieneObjeto(metadatoArchivo, archivo.Metadatos);
                            if (estaEnLista) {
                                _.each(archivo.Metadatos, function (metadato) {
                                    if (metadato.Nombre === metadatoArchivo.Nombre) {
                                        metadato.Valor = metadatoArchivo.Valor;
                                    }
                                });
                            } else {
                                archivo.Metadatos.push(metadatoArchivo);
                            }
                        }
                    }
                }
            });
        }

        function ObtenerDatosGrilla() {
            if (vm.gridApi.grid && vm.gridApi.grid.rows.length > 0) {
                for (var i = 0; i < vm.gridApi.grid.rows.length; i++) {
                    construirObjetoArchivoMetadato(vm.gridApi.grid.rows[i].entity, null);
                }
            }
        }

        function guardarMetadatos() {
            ObtenerDatosGrilla();
            vm.faltanDatosMetadatos = true;
            validarCategorias();
            if (vm.categoriaObligatoria === undefined) {
                validarCeldasObligatorias();
                if (vm.metadatosObligatorios.length === 0) {
                    vm.faltanDatosCategoria = false;
                    archivoServicios.asociarMetadatosAplicacionArchivos(vm.archivosMetadatosCategoriasPorAplicacionNivel).then(function (data) {
                        if (data === 201) {
                            utilidades.mensajeSuccess($filter('language')('MetadatosGuardados'), null, vm.aceptarGuardadoMetadatos);
                        }
                        else {
                            utilidades.mensajeError(data);
                        }
                    });
                } else {
                    utilidades.mensajeError($filter('language')('MetadatosObligatorios'), vm.seleccionarMetadatoObligatorio);
                }
            }
            else {
                vm.faltanDatosCategoria = true;
                validarCeldasObligatorias();
                if (vm.metadatosObligatorios.length === 0)
                    utilidades.mensajeError($filter('language')('CategoriasObligatorias'), seleccionarCategoriaObligatoria());
                else
                    utilidades.mensajeError($filter('language')('CategoriasMetadatosObligatorias'), seleccionarCategoriaObligatoria());
            }
        }

        function listaContieneObjetoMetadato(obj, list) {
            var i;
            for (i = 0; i < list.length; i++) {
                if (list[i].IdArchivo === obj.IdArchivo) {
                    list[i].IdCategoria = obj.IdCategoria;
                    return true;
                }
            }
            return false;
        }

        function listaContieneObjeto(obj, list) {
            var i;
            for (i = 0; i < list.length; i++) {
                if (list[i].Nombre === obj.Nombre && list[i].IdArchivo === obj.IdArchivo) {
                    return true;
                }
            }
            return false;
        }

        function prepararGridMetadatos() {
            var parametros = {
                IdAplicacion: vm.idAplicacion,
                IdNivel: vm.idNivel,
                Archivos: vm.idsArchivos
            };
            archivoServicios.obtenerArchivosMetadatosCategoriasPorAplicacionNivel(parametros).then(function (data) {
                if (data) {
                    vm.metadatos = data.Metadatos;
                    vm.archivos = data.Archivos;
                    crearColumnasGrilla(data);
                }
            });
        }

        function seleccionarMetadatoObligatorio() {
            vm.gridApi.cellNav.scrollToFocus(vm.grillaMetadatos.data[vm.metadatosObligatorios[0].filaObligatoria], vm.grillaMetadatos.columnDefs[vm.metadatosObligatorios[0].columnaObligatoria]);
            vm.metadatosObligatorios = [];
        }

        function seleccionarCategoriaObligatoria() {
            vm.gridApi.cellNav.scrollToFocus(vm.grillaMetadatos.data[vm.categoriaObligatoria === undefined ? 0 : vm.categoriaObligatoria], vm.grillaMetadatos.columnDefs[3]);
            vm.categoriaObligatoria = undefined;
        }

        function validarCategorias() {
            _.each(vm.archivos, function (archivo) {
                var metadatosArchivoExistente = $filter('filter')(vm.archivosMetadatosCategoriasPorAplicacionNivel, { 'IdArchivo': archivo.Id });
                if (metadatosArchivoExistente.length === 1) {
                    _.each(vm.archivosMetadatosCategoriasPorAplicacionNivel, function (metadatosArchivo) {
                        if ((metadatosArchivo.IdCategoria === "undefined" || metadatosArchivo.IdCategoria === null) && metadatosArchivo.IdArchivo === archivo.Id) {
                            vm.categoriaObligatoria = vm.archivos.indexOf(archivo);
                        }
                    });
                }
                else {
                    vm.categoriaObligatoria = vm.archivos.indexOf(archivo);
                }
            });
        }

        function validarCeldasObligatorias() {
            vm.metadatosObligatorios = [];
            _.each(vm.archivos, function (archivo) {
                var metadatosArchivoExistente = $filter('filter')(vm.archivosMetadatosCategoriasPorAplicacionNivel, { 'IdArchivo': archivo.Id });
                if (metadatosArchivoExistente.length === 1) {
                    _.each(vm.archivosMetadatosCategoriasPorAplicacionNivel, function (metadatosArchivo) {
                        vm.columnaMetadatoObligatorio = vm.columnaInicialMetadatos;
                        _.each(vm.metadatos, function (metadatoOriginal) {
                            if (metadatoOriginal.Obligatorio) {
                                var metadatoObligatorio = {
                                    nombreArchivo: archivo.Nombre,
                                    metadato: metadatoOriginal.Nombre,
                                    columnaObligatoria: vm.columnaMetadatoObligatorio,
                                    filaObligatoria: vm.archivos.indexOf(archivo)
                                };

                                if (metadatosArchivo.IdArchivo === archivo.Id) {
                                    var keys = [];
                                    for (var key in archivo) {
                                        if (archivo.hasOwnProperty(key))
                                            keys.push(key);
                                    }
                                    var tieneLaPropiedad = false;
                                    for (var i = 0; i < keys.length; i++) {
                                        var valor = archivo[keys[i]];
                                        if (keys[i] === metadatoOriginal.Nombre && valor !== null && valor !== undefined) {
                                            tieneLaPropiedad = true;
                                            break;
                                        }
                                    }
                                    if (!tieneLaPropiedad && metadatoOriginal.Obligatorio) {
                                        vm.metadatosObligatorios.push(metadatoObligatorio);
                                    }
                                }
                            }
                            vm.columnaMetadatoObligatorio++;
                        });
                    });
                }
                else {
                    vm.columnaMetadatoObligatorio = vm.columnaInicialMetadatos;
                    var keys = [];
                    for (var key in archivo) {
                        if (archivo.hasOwnProperty(key))
                            keys.push(key);
                    }
                    _.each(vm.metadatos, function (metadatoOriginal) {
                        if (metadatoOriginal.Obligatorio) {
                            var ExisteCampo = false;
                            for (var i = 0; i < keys.length; i++) {
                                var valor = archivo[keys[i]];
                                if (keys[i] === metadatoOriginal.Nombre && metadatoOriginal.Obligatorio) {
                                    ExisteCampo = true;
                                    if (valor === null || valor === undefined) {
                                        var metadatoObligatorio = {
                                            nombreArchivo: archivo.Nombre,
                                            metadato: metadatoOriginal.Nombre,
                                            columnaObligatoria: vm.columnaMetadatoObligatorio,
                                            filaObligatoria: vm.archivos.indexOf(archivo)
                                        };
                                        vm.metadatosObligatorios.push(metadatoObligatorio);
                                        break;
                                    }
                                }
                            }
                            if (!ExisteCampo) {
                                var metadatoObligatorio = {
                                    nombreArchivo: archivo.Nombre,
                                    metadato: metadatoOriginal.Nombre,
                                    columnaObligatoria: vm.columnaMetadatoObligatorio,
                                    filaObligatoria: vm.archivos.indexOf(archivo)
                                };
                                vm.metadatosObligatorios.push(metadatoObligatorio);
                            }
                        }
                        vm.columnaMetadatoObligatorio++;
                    });
                }
            });
        }

        vm.$onInit = prepararGridMetadatos();
    }
})();