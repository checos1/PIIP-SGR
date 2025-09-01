(function () {
    'use strict';

    entidadesController.$inject = ['$scope',
        'servicioEntidades',
        'constantesAutorizacion',
        '$uibModal',
        'FileSaver',
        'utilidades',
        'servicioUsuarios',
        'autorizacionServicios',
        '$localStorage',
        'backboneServicios'
    ];

    function entidadesController($scope, servicioEntidades, constantesAutorizacion, $uibModal, FileSaver, utilidades, servicioUsuarios, autorizacionServicios, $localStorage, backboneServicios) {
        var vm = this;
        vm.tipoEntidad;
        vm.sigla;
        vm.codigo;

        /// Scopo Nacional
        $scope.vmChildren = [];
        vm.objEntidad = {};

        /// filtro
        vm.columnasDisponiblesPorAgregar = [];
        vm.columnas = ['Entidades','Agrupador','Cabeza Sector'];
        vm.mostrarFiltro = false;
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.entidadeFiltro = "";
        vm.listaEntidadesFiltro = [];
        vm.filtrarEntidades = filtrarEntidades;
        vm.limpiarFiltro = limpiarFiltro;
        vm.filtrarAgrupador = filtrarAgrupador;
        vm.obtenerSectores = obtenerSectores;
        vm.obtenerDepartamentos = obtenerDepartamentos;
        vm.obtenerRoles = obtenerRoles;
        vm.obtenerCRType = obtenerCRType;
        vm.obtenerFase = obtenerFase;

        vm.agrupador = false;
        vm.listaSector = [];
        vm.listaRoles = [];
        vm.listaDepartamentos = [];
        vm.listaCRType = [];
        vm.listaFase = [];


        vm.downloadPdf = downloadPdf;
        vm.downloadExcel = downloadExcel;
        vm.accionCrearEditarEntidad = accionCrearEditarEntidad;
        vm.accionEliminarEntidad = accionEliminarEntidad;
        vm.accionCrearSubEntidad = accionCrearSubEntidad;
        vm.accionEditarSubEntidad = accionEditarSubEntidad;
        vm.accionFlujosViabilidad = accionFlujosViabilidad;
        vm.abrirModalAdicionarColumnas = abrirModalAdicionarColumnas;
        vm.tieneColumna = tieneColumna;

        // Funciones
        vm.tiposEntidad = [];
        vm.tabContent = 'src/app/entidades/entidades/tipoEntidad/nacional.html';
        vm.cambiarTipoEntidadFiltroTab = cambiarTipoEntidadFiltroTab;
        vm.actualizarListaEntidadesFiltro = actualizarListaEntidadesFiltro;
        vm.obtenerSubEntidades = obtenerSubEntidades;

        /// Comienzo
        vm.init = function () {
            vm.tiposEntidad = autorizacionServicios.obtenerTiposEntidad();
            vm.cambiarFiltroEntidad1 = function (tipoEntidad) {
                if (vm.tipoEntidadSeleccionada === tipoEntidad)
                    return;
                vm.tipoEntidadSeleccionada = tipoEntidad;
                return vm.cambiarTipoEntidadFiltroTab(tipoEntidad);
            }
            vm.cambiarFiltroEntidad1(vm.tiposEntidad[0].clave);

            vm.obtenerSectores();
            vm.obtenerDepartamentos();
            vm.obtenerRoles();
            vm.obtenerCRType();
            vm.obtenerFase();
        }

        function tienePermiso(entidad, opcion) {
            
            let tiene = backboneServicios.obtenerAcesso(entidad, opcion);

            return tiene;
        }

        /// Actions
        function downloadExcel() {

            let objExport = $scope.vmChildren[0].listaEntidadesOrigem
                .reduce((acc, entidad) => {
                    acc.push({
                        IdAgrupador: entidad.IdAgrupador,
                        AgrupadorEntidad: entidad.AgrupadorEntidad,
                        IdEntidad: entidad.IdEntidad,
                        Entidad: entidad.Entidad,
                        NombreCompleto: entidad.NombreCompleto,
                        TipoEntidad: entidad.TipoEntidad,
                        CabezaSector: entidad.CabezaSector,
                        TieneHijo: entidad.TieneHijo
                    })

                    if (entidad.TieneHijo)
                        acc.push(...(entidad.SubEntidades || []).map(subEntidad => {
                            return {
                                IdAgrupador: subEntidad.IdAgrupador,
                                AgrupadorEntidad: subEntidad.AgrupadorEntidad,
                                IdEntidad: subEntidad.IdEntidad,
                                Entidad: subEntidad.Entidad,
                                NombreCompleto: `${subEntidad.AgrupadorEntidad} - ${subEntidad.Entidad}`,
                                TipoEntidad: subEntidad.TipoEntidad,
                                CabezaSector: subEntidad.CabezaSector,
                                TieneHijo: subEntidad.TieneHijo
                            }
                        }))

                    return acc;
                }, []);

            servicioEntidades.obtenerExcelEntidades(objExport).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                FileSaver.saveAs(blob, "Entidades.xls");
            }, function (error) {
                console.log(error.data);
            });
        }

        function downloadPdf() {
            servicioEntidades.obtenerPdfEntidades($scope.vmChildren[0].listaEntidadesOrigem).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                FileSaver.saveAs(blob, nombreDelArchivo(retorno));
            });
        };

        function abrirModalAdicionarColumnas() {

            const modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/configurarColumnas/plantillaConfigurarColumnas.html',
                controller: 'controladorConfigurarColumnas',
                controllerAs: "vm",
                size: 'lg',
                resolve: {
                    items: function () {
                        return {
                            columnasActivas: vm.columnas,
                            columnasDisponibles: vm.columnasDisponiblesPorAgregar,
                        };
                    }
                }
            });

            modalInstance.result.then(function (selectedItem) {
                if (!$localStorage.tipoFiltro) {
                    $localStorage.tipoFiltro = {
                        "entidades": {
                            'columnasActivas': selectedItem.columnasActivas,
                            'columnasDisponibles': selectedItem.columnasDisponibles
                        }
                    };
                } else {
                    $localStorage.tipoFiltro["entidades"] = {
                        'columnasActivas': selectedItem.columnasActivas,
                        'columnasDisponibles': selectedItem.columnasDisponibles
                    }
                }

                buscarColumnasLocalStorage();
            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
            });
        };

        function tieneColumna(columna) {
            return vm.columnas.find(x => x == columna) != null;
        }

        function buscarColumnasLocalStorage() {
            if (!$localStorage.tipoFiltro)
                return;

            if ($localStorage.tipoFiltro["entidades"]) {
                vm.columnas = $localStorage.tipoFiltro["entidades"].columnasActivas;
                vm.columnasDisponiblesPorAgregar = $localStorage.tipoFiltro["entidades"].columnasDisponibles;
            }
        }

        function nombreDelArchivo(response) {
            var filename = "";
            var disposition = response.headers("content-disposition");
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) {
                    filename = matches[1].replace(/['"]/g, '');
                }
            }
            return filename;
        }

        function obtenerSectores() {
            servicioEntidades.obtenerSectores()
                .then(function (response) {
                    vm.listaSector = response.data;
                });
        }

        function obtenerDepartamentos() {
            servicioEntidades.obtenerDepartamentos()
                .then(function (response) {
                    vm.listaDepartamentos = response.data;
                });
        }

        function obtenerCRType() {
            servicioEntidades.obtenerCRType()
                .then(function (response) {
                    vm.listaCRType = response.data;
                });
        }

        function obtenerFase() {
            servicioEntidades.obtenerFase()
                .then(function (response) {
                    vm.listaFase = response.data;
                });
        }

        function obtenerRoles() {
            servicioUsuarios.obtenerRoles("")
                .then(function (response) {
                    vm.listaRoles = response.data;
                });
        }

        function cambiarTipoEntidadFiltroTab(tipoEntidad) {
            vm.tipoEntidad = tipoEntidad;
            limpiarFiltro();
        }

        function conmutadorFiltro() {
            vm.mostrarFiltro = !vm.mostrarFiltro;
            if (vm.mostrarFiltro)
                actualizarListaEntidadesFiltro();
        }

        function actualizarListaEntidadesFiltro() {
            vm.listaEntidadesFiltro = $scope.vmChildren[0].listaEntidadesOrigem.map(function (a) {
                return a.NombreCompleto;
            }).filter((v, i, a) => a.indexOf(v) === i);
        }

        function filtrarEntidades() {
            $scope.vmChildren[0].listarEntidades(vm.entidadFiltro, vm.cabezaSector, false, vm.sigla, vm.codigo);
        }

        function limpiarFiltro() {
            vm.entidadFiltro = null;
            vm.agrupador = false;
            vm.cabezaSector = false;
            vm.sigla = null;
            vm.codigo = null;

            if ($scope.vmChildren.length && $scope.vmChildren[0].limparFiltro)
                $scope.vmChildren[0].limparFiltro();
        }

        function filtrarAgrupador() {
            vm.entidadFiltro = null;
            vm.cabezaSector = false;
            vm.sigla = null;
            vm.codigo = null;
            $scope.vmChildren[0].limparFiltro();
            if (vm.agrupador)
                vm.listaEntidadesFiltro = $scope.vmChildren[0].listaEntidadesOrigem.map(function (a) {
                    return a.AgrupadorEntidad;
                }).filter((v, i, a) => a.indexOf(v) === i);
            else
                vm.listaEntidadesFiltro = $scope.vmChildren[0].listaEntidadesOrigem.map(function (a) {
                    return a.NombreCompleto;
                }).filter((v, i, a) => a.indexOf(v) === i);
        }

        function accionCrearEditarEntidad(row) {
            vm.objEntidad = {};
            //   console.log(row);
            let templateUrl = 'src/app/entidades/entidades/modales/modalCrearEditar.html';
            vm.objEntidad.titulo = 'CREAR ENTIDAD';
            vm.objEntidad.tipoEntidad = vm.tipoEntidad;
            vm.objEntidad.listaSector = vm.listaSector;
            vm.objEntidad.listaDepartamentos = vm.listaDepartamentos;
            vm.objEntidad.listaEntityType = vm.tipoEntidad === 'Territorial' ? [{
                Id: 3,
                Nombre: 'Departamentos'
            }, {
                Id: 10,
                Nombre: 'Minoria Étnica'
            }, {
                Id: 4,
                Nombre: 'Municipio'
            }] : [{
                Id: 12,
                Nombre: 'OCAD'
            }, {
                Id: 9,
                Nombre: 'Regional'
            }];

            vm.objEntidad.listaEntidades = $scope.vmChildren[0].listaEntidadesOrigem;

            if (row) {
                servicioEntidades.obtenerEntidadPorEntidadId(row.IdEntidad)
                    .then(function (response) {
                        if (response.data) {
                            // console.log('response.data', response.data);
                            let ent = response.data;
                            vm.objEntidad.titulo = 'EDITAR ENTIDAD';
                            vm.objEntidad.tipoEntidad = ent.TipoEntidad;
                            vm.objEntidad.nombre = ent.Entidad;
                            vm.objEntidad.cabezaSector = ent.CabezaSector;
                            vm.objEntidad.sigla = ent.Sigla;
                            vm.objEntidad.idSector = ent.IdSector
                            vm.objEntidad.codigo = ent.Codigo;
                            vm.objEntidad.id = ent.IdEntidad;
                            vm.objEntidad.entityType = ent.EntityType.toString();
                            vm.objEntidad.parentGuid = ent.ParentGuid;

                            //vm.objEntidad.TieneCabezaSector = ent.TieneCabezaSector;


                            modalCrearEditar(vm.objEntidad, templateUrl);
                        } else {
                            utilidades.mensajeError('Error al recuperar la entidad', false);
                        }
                    });

            } else {
                modalCrearEditar(vm.objEntidad, templateUrl);
            }
        };

        function accionEditarSubEntidad(row) {
            vm.objEntidad = {};
            //   console.log('sub', row);

            let templateUrl = 'src/app/entidades/entidades/modales/modalCrearEditarSubEntidad.html';

            vm.objEntidad.titulo = 'EDITAR SUB-ENTIDAD';
            vm.objEntidad.tipoEntidad = vm.tipoEntidad;
            vm.objEntidad.listaSector = vm.listaSector;
            //vm.objEntidad.listaEntityType = [{ Id: 3, Nombre: 'Departamentos' }, { Id: 10, Nombre: 'Minoria Étnica' }, { Id: 4, Nombre: 'Municipio' }];
            vm.objEntidad.subEntidad = true;

            servicioEntidades.obtenerEntidadPorEntidadId(row.IdEntidad)
                .then(function (response) {
                    if (response.data) {
                        // console.log('response.data', response.data);
                        let ent = response.data;
                        vm.objEntidad.titulo = 'EDITAR SUB-ENTIDAD';

                        vm.objEntidad.nombre = ent.Entidad;
                        vm.objEntidad.sigla = ent.Sigla;
                        vm.objEntidad.idSector = ent.IdSector
                        vm.objEntidad.codigo = ent.Codigo;
                        vm.objEntidad.id = ent.IdEntidad;
                        vm.objEntidad.entityType = ent.EntityType;
                        vm.objEntidad.parentGuid = ent.ParentGuid;

                        let padre = $scope.vmChildren[0].listaEntidadesOrigem.find(x => x.IdEntidad.indexOf(ent.ParentGuid) != -1);
                        vm.objEntidad.nombrePai = padre.NombreCompleto;

                        modalCrearEditar(vm.objEntidad, templateUrl);
                    } else {
                        utilidades.mensajeError('Error al recuperar la entidad', false);
                    }
                });
        };

        function accionCrearSubEntidad(row) {
            vm.objEntidad = {};
            //   console.log('sub', row);
            let templateUrl = 'src/app/entidades/entidades/modales/modalCrearEditarSubEntidad.html';

            vm.objEntidad.titulo = 'CREAR SUB-ENTIDAD';
            vm.objEntidad.subEntidad = true;
            vm.objEntidad.tipoEntidad = vm.tipoEntidad;
            vm.objEntidad.listaSector = vm.listaSector;
            vm.objEntidad.listaDepartamentos = vm.listaDepartamentos;
            vm.objEntidad.parentGuid = row.IdEntidad;
            vm.objEntidad.nombrePai = row.NombreCompleto;

            modalCrearEditar(vm.objEntidad, templateUrl);
        };

        function accionEliminarEntidad(row) {
            utilidades.mensajeWarning("Confirma la exclusión del registro?", function funcionContinuar() {
                servicioEntidades.eliminarEntidad(row.IdEntidad)
                    .then(function (response) {
                        if (response.data.Exito) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            $scope.vmChildren[0].listarEntidades(null, null, true);
                        } else {
                            utilidades.mensajeError("Error al realizar la operación", false);
                        }
                        //console.log(response.data);
                    }, function (response) {
                        if (response.status == 409) {
                            utilidades.mensajeError("No es posible eliminar esta entidad.", false);
                        } else {
                            utilidades.mensajeError("Error al realizar la operación", false);
                        }
                    })
            }, function funcionCancelar() { })
        }

        function modalCrearEditar(objEntidad, templateUrl) {
            $uibModal.open({
                templateUrl: templateUrl,
                controller: 'modalCrearEditarController',
                resolve: {
                    objEntidad: objEntidad
                }
            }).result.then(function (result) {
                $scope.vmChildren[0].listarEntidades(null, null, true);
            }, function (reason) {
                $scope.vmChildren[0].listarEntidades(null, null, true);
            });
        }

        function accionFlujosViabilidad($row) {
            //vm.popOverOptions.toggle();
            console.log('$row', $row);
            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/entidades/entidades/modales/flujosViabilidadModal.html',
                controller: 'flujosViabilidadModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-flujos",
                resolve: {
                    entidad: {
                        idEntidad: $row.IdEntidad,
                        entityTypeCatalogOptionId: $row.EntityTypeCatalogOptionId,
                        nombreCompleto: $row.NombreCompleto,
                        tipoEntidad: $row.TipoEntidad,
                        listaSector: vm.listaSector,
                        listaRoles: vm.listaRoles,
                        listaCRType: vm.listaCRType,
                        listaFase: vm.listaFase,
                        listaEntidades: $scope.vmChildren[0].listaEntidadesOrigem,
                    }
                },
            }).result.then(() => { }, () => { });
        }

        async function obtenerSubEntidades() {
            const promises = ($scope.vmChildren[0].listaEntidadesOrigem || [])
                .filter(x => x.TieneHijo)
                .reduce((promises, entidad) => {
                    promises.push(new Promise((resolve, reject) => {
                        servicioEntidades.obtenerSubEntidadesPorIdEntidad(entidad.IdEntidad)
                            .then(function (response) {
                                entidad.SubEntidades = response.data || [];
                                resolve(true)
                            })
                            .catch(error => reject(error));
                    }));
                    return promises;
                }, []);

            await Promise.all(promises);
        }
    };
    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone.entidades').controller('entidadesController', entidadesController);
})();