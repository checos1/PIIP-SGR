(function () {
    'use strict';

    cargaDatosController.$inject = ['$scope', 'servicioCargaDatos', 'constantesAutorizacion', 'utilidades', '$uibModal', '$window'];

    function cargaDatosController($scope, servicioCargaDatos, constantesAutorizacion, utilidades, $uibModal, $window) {
        var vm = this;

        vm.datos = [];

        /// Modais
        vm.abrirModalCrear = abrirModalCrear;
        vm.abrirModalVerDatos = abrirModalVerDatos;
        vm.abrirModalEliminar = abrirModalEliminar;
       // vm.obtenerEntidadesPorTipo = obtenerEntidadesPorTipo;

        vm.listaEntidades = [];

        /// Filtros
        vm.mostrarFiltro = false;
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.filtrar = filtrar;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.filtro = {
            TipoCargaDatosId: null,
            fechaInicio: null,
            fechaTermino: null,
        };
        vm.listaTipoCargaDatos = [];

        /// Plantilla de acciones de la tabla
        vm.plantillaAccionesTabla = 'src/app/entidades/cargaDatos/plantillas/plantillaAccionesTabla.html';
        vm.plantillaEstadoTabla = 'src/app/entidades/cargaDatos/plantillas/plantillaEstadoTabla.html';
        /// Definiciones de componente
        vm.opciones = {
            cambiarTipoEntidadFiltro: cambiarTipoEntidadFiltro,
            nivelJerarquico: 1,
            gridOptions: {
                //showHeader: true,
                paginationPageSizes: [5, 10, 15, 25, 50, 100],
                paginationPageSize: 10,
                expandableRowHeight: 220
            }
        };

        /// Definiciones de columna componente
        vm.columnDefs = [{
            field: 'Fecha',
            displayName: 'FECHA DE CARGUE',
            enableHiding: false,
            type: 'date',
            cellFilter: 'date:\'dd/MM/yyyy\'',
            width: '20%',
            cellTooltip: (row, col) => row.entity[col.field]
        }, {
            field: 'ValorCredito',
            displayName: 'VALOR CRÉDITO',
            cellFilter: 'number: 2',
            enableHiding: false,
            width: '20%',
            cellTooltip: (row, col) => row.entity[col.field]
        }, {
            field: 'ValorContraCredito',
            displayName: 'VALOR CONTRA CRÉDITO',
            cellFilter: 'number: 2',
            enableHiding: false,
            width: '20%',
            cellTooltip: (row, col) => row.entity[col.field]
        }, {
            field: 'Entidad',
            displayName: 'Entidad',
            enableHiding: false,
            width: '18%',
            cellTooltip: (row, col) => row.entity[col.field]
        }, {
            field: 'Estado',
            displayName: 'Estado',
            enableFiltering: false,
            enableHiding: false,
            enableSorting: false,
            enableColumnMenu: false,
            cellTemplate: vm.plantillaEstadoTabla,
            width: '10%'
        }, {
            field: 'Accion',
            displayName: 'Acción',
            enableFiltering: false,
            enableHiding: false,
            enableSorting: false,
            enableColumnMenu: false,
            headerCellClass: 'text-center',
            cellTemplate: vm.plantillaAccionesTabla,
            width: '10%'
        }];

        /// Comienzo
        vm.init = function () {
            vm.tipoEntidad = 'Nacional';

        }

        function conmutadorFiltro() {
            if (vm.mostrarFiltro) {
                limpiarCamposFiltro();
            }

            vm.mostrarFiltro = !vm.mostrarFiltro;
        }

        function filtrar() {
            listarCargaDatos(vm.filtro);
        }

        function limpiarCamposFiltro() {
            vm.filtro = {
                TipoCargaDatosId: null,
                fechaInicio: null,
                fechaTermino: null,
            };
            listarCargaDatos(null);
        }

       

        /// Getters
        function listarCargaDatos(filtro) {
            ///vm.usuariosFiltro = [];
            vm.datos = null;
            servicioCargaDatos.obtenerCargaDatos(vm.tipoEntidad)
                .then(function (response) {
                    var datos = response.data;
                  //  console.log(datos);
                    if (datos != null && datos.length > 0) {
                        //var listaCarga = [];
                        if (filtro !== null && filtro.TipoCargaDatosId !== null)
                            datos = datos.filter(x => x.Id.toString().indexOf(filtro.TipoCargaDatosId) != -1)


                        datos.forEach(item => {
                            item.agrupadorEntidad = item.Nombre;
                            item.entidad = "";
                            item.tipoEntidad = vm.tipoEntidad;
                          
                            //if (filtro !== null && filtro.TipoCargaDatosId !== null) {
                            //    if (item.TipoCargaDatosId == filtro.TipoCargaDatosId) {
                            //        listaCarga = item.ListaCargaDatos;

                            //    }
                            //} else {
                            //    listaCarga = item.ListaCargaDatos;
                            //}
                            //item.ListaCargaDatos = item.ListaCargaDatos.filter(x => x.NombreCompleto.indexOf(entidadeFiltro) != -1 && x.CabezaSector == cabezaSector);;
                            //    .forEach(carga => {
                            //    usuario.idEntidad = item.IdEntidad;
                            //    usuario.nombreEntidad = item.agrupadorEntidad + ' - ' + item.entidad;
                            //    usuario.IdEntidad = item.IdEntidad;
                            //});
                            item.subGridOptions = {
                                //columnDefs: vm.columnDef,
                                paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                paginationPageSize: 5,
                                expandableRowHeight: '200px',
                                data: item.ListaCargaDatos,
                            }
                        });
                    }
                    vm.listaTipoCargaDatos = datos.map((x) => { return { Id: x.Id, Nombre: x.Nombre } })
                    vm.datos = datos;
                });
        }

        // Actions
        function cambiarTipoEntidadFiltro(tipoEntidad) {
            vm.tipoEntidad = tipoEntidad;
            
            listarCargaDatos(null);
        }

        function abrirModalCrear() {
            $uibModal.open({
                templateUrl: 'src/app/entidades/cargaDatos/modales/modalCrearCargaDatos.html',
                controller: 'modalCrearCargaDatosController',
                resolve: {
                    objCarga: {
                        tipoEntidad: vm.tipoEntidad,
                        listaTipoCargaDatos: vm.datos.map((x) => { return { Id: x.Id, Nombre: x.Nombre } }),
                        listaEntidades: vm.listaEntidades
                    },
                }
            }).result.then(function (result) {
                listarCargaDatos(null);
            }, function (reason) {
                listarCargaDatos(null);
            });
        }

        function abrirModalVerDatos(obj) {
            $uibModal.open({
                templateUrl: 'src/app/entidades/cargaDatos/modales/modalVerDatos.html',
                controller: 'modalVerCargaDatosController',
                resolve: {
                    objCarga: {
                        IdDatos: obj.Id,
                        tipoEntidad: vm.tipoEntidad,
                        idArchivo: obj.IdArchivo,
                        listaTipoCargaDatos: vm.datos.map((x) => { return { Id: x.Id, Nombre: x.Nombre } })
                    },
                }
            }).result.then(function (result) {
                listarCargaDatos(null);
            }, function (reason) {
                listarCargaDatos(null);
            });
            //$uibModal.open({  
            //    templateUrl: 'src/app/entidades/cargaDatos/modales/modalVerDatos.html',
            //    controller: 'modalVerDatosController',
            //    resolve: {
            //        IdDatos: obj.Id.toString(),
            //    }
            //}).result.then(function (result) {
            //    //listarUsuarios();
            //}, function (reason) {
            //    //listarUsuarios();
            //});//apiBackboneObtenerDatosMongoDb
        }


        function abrirModalEliminar(obj) {
            utilidades.mensajeWarning("Confirma la exclusión del registro?", function funcionContinuar() {
                //utilidades.mensajeError("Error al realizar la operación", false);
                servicioCargaDatos.eliminarCargaDatos(obj.Id)
                    .then(function (response) {
                        if (response.data.Exito) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            listarCargaDatos(null);
                        } else {
                            utilidades.mensajeError("Error al realizar la operación", false);
                        }

                    }, function (response) {
                        utilidades.mensajeError("Error al realizar la operación", false);
                    })
            }, function funcionCancelar() {
            })
        }

    }

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone.entidades').controller('cargaDatosController', cargaDatosController);
})();





