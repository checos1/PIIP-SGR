(function () {
    'use strict';

    angular.module('backbone.archivo').controller('cargaArchivoController', cargaArchivoController)
        .directive('deshabilitarControlDropzone', deshabilitarControlDropzone)
        .filter('bytes', filterBytes);

    function filterBytes() {
        return function (bytes, precision) {
            if (bytes === 0) { return '0 bytes' }
            if (isNaN(parseFloat(bytes)) || !isFinite(bytes)) return '-';
            if (typeof precision === 'undefined') precision = 1;

            var units = ['bytes', 'KB', 'MB', 'GB', 'TB', 'PB'],
                number = Math.floor(Math.log(bytes) / Math.log(1024)),
                val = (bytes / Math.pow(1024, Math.floor(number))).toFixed(precision);

            return (val.match(/\.0*$/) ? val.substr(0, val.indexOf('.')) : val) + ' ' + units[number];
        }
    }

    function deshabilitarControlDropzone() {
        return {
            link: function ($scope, element, attrs) {
                // Trigger when number of children changes,
                // including by directives like ng-repeat
                var watch = $scope.$watch(function () {
                    return $scope.opciones.deshabilitarControles;
                }, function () {
                    // Wait for templates to render
                    $scope.$evalAsync(function () {
                        // Finally, directives are evaluated
                        // and templates are renderer here
                        if ($scope.opciones.deshabilitarControles) {
                            _.each(element, function (dropzoneElement) {
                                dropzoneElement.dropzone.disable();
                            });
                        }
                    });
                });
            },
        };
    }

    cargaArchivoController.$inject = ['$scope', '$location', '$timeout', '$filter', '$routeParams', '$sessionStorage', 'archivoServicios',
        'utilidades', '$uibModal', '$q', '$document', 'uiGridConstants', 'FileSaver', 'Blob', 'constantesArchivos'];

    function cargaArchivoController($scope, $location, $timeout, $filter, $routeParams, $sessionStorage, archivoServicios,
        utilidades, $uibModal, $q, $document, uiGridConstants, FileSaver, Blob, constantesArchivos) {
        var vm = this;

        //#region Métodos
        vm.cancelarCarga = cancelarCarga;
        vm.confirmaCancelar = confirmaCancelar;
        vm.confirmaCargueArchivo = confirmaCargueArchivo;
        vm.confirmaGuardarArchivo = confirmaGuardarArchivo;
        vm.guardarArchivos = guardarArchivos;
        vm.obtenerConfiguracionCargaArchivos = obtenerConfiguracionCargaArchivos;
        vm.recargarTabla = recargarTabla;
        vm.obtenerExtensionArchivo = utilidades.obtenerExtensionArchivo;
        vm.seleccionarTodosLosArchivoParaDescarga = seleccionarTodosLosArchivoParaDescarga;
        vm.seleccionarArchivoParaDescarga = seleccionarArchivoParaDescarga;
        vm.descargarSeleccionados = descargarSeleccionados;
        vm.descargarArchivo = descargarArchivo;
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.ejecutarFiltro = ejecutarFiltro;
        vm.limpiarFiltro = limpiarFiltro;
        vm.editarRegistro = editarRegistro;
        vm.confirmarEliminarArchivo = confirmarEliminarArchivo;
        vm.eliminarArchivo = eliminarArchivo;
        vm.iniciarModalCarga = iniciarModalCarga;
        //#endregion

        //#region Variables Globales
        vm.archivo = '';
        vm.archivos = [];
        vm.archivoGuardado = false;
        vm.archivosParaCargar = [];
        vm.cargaIndividual = 0;
        vm.cargaCompleta = 0;
        vm.cantidadArchivosActuales = 0;
        vm.dataHasLoaded = false;
        vm.desHabilitarCarga = true;
        vm.deshabilitarCancelarCarga = false;
        vm.excluyente = false;
        vm.estaSeleccionandoArchivo = true;
        vm.faltanDatosCategoria = false;
        vm.idAplicacion = $sessionStorage.IdAplicacion;
        vm.idNivel = $sessionStorage.IdNivel;
        vm.idInstancia = $sessionStorage.idInstancia !== null ? $sessionStorage.idInstancia : $sessionStorage.idInstanciaFlujoPrincipal;
        vm.idInstanciaFlujoPrincipal = $sessionStorage.idInstanciaFlujoPrincipal;
        vm.idAccion = $sessionStorage.idAccion;
        vm.idObjetoNegocio = $sessionStorage.idObjetoNegocio;
        vm.nombreAccion = $sessionStorage.nombreAccion;
        vm.listaExtensiones = [];
        vm.listaCategorias = [];
        vm.listaMetadatos = [];
        vm.limiteArchivosACargar = 0;
        vm.maxConfiguradosCargar = 0;
        vm.idConfiguracion = '';
        vm.mensajeError = '';
        vm.multiplesArchivos = false;
        vm.nombresEnDropzone = [];
        vm.resetDropzone;
        vm.disableDropzone;
        vm.configuracionCargaArchivos = _.clone($scope.form.configuracionControlArchivo);
        vm.verArchivosMGA = false;
        vm.verTodosLosArchivos = false;
        vm.verArchivosDescartados = false;
        vm.noVisualizarArchivos = true;
        vm.mostrarModalCarga = false;
        vm.filtroActivado = false;
        vm.filtro = {
            Nombre: null,
            Extension: null,
            FechaInicio: null,
            FechaFin: null
        };
        vm.DatosOrigen = [];
        vm.GridDataOriginal = [];
        vm.grillaMetadatos = {};
        vm.gridApi;
        vm.listaArchivosStorage = [];
        vm.todosArchivosSeleccionados = false;
        vm.esEdicionOrigen = false;
        vm.archivosParaActualizar = [];
        vm.EntidadEliminar = null;
        //#endregion

        //#region templates para el grid
        vm.botonesTemplate =
            '<div class="text-center contenedor-acciones">' +
            '<button type="button" class="btnaccion" ' +
                'ng-hide="row.entity.IdArchivoBlob === null" ' +
                'ng-click="grid.appScope.vm.descargarArchivo(row.entity)" ' +
                'tooltip-placement="bottom" uib-tooltip="{{ \'Descargar\' | language }}" >' +
            '<span aria-hidden="true"><img class="grid-icon-accion" src="img/iconsgrid/iconAccionDownload.png" alt="Descargar"/></span>' +
            '</button>' +
            '<button type="button" class="btnaccion" ' +
                'ng-hide="row.entity.OcultarBotones" ' +
                'ng-click="row.entity.DeshabilitarEdicion = grid.appScope.vm.editarRegistro(row.entity)" ' +
                'tooltip-placement="bottom" uib-tooltip="{{ \'Editar\' | language }}">' +
            '<span aria-hidden="true"><img class="grid-icon-accion" src="img/iconsgrid/iconAccionEditar.png" alt="Editar"/></span>' +
            '</button>' +
            '<button type="button" class="btnaccion" ' +
                'ng-hide="row.entity.OcultarBotones" ng-disabled="row.entity.Id === null" ' +
                'ng-click="grid.appScope.vm.eliminarArchivo(row.entity)" ' +
                'tooltip-placement="bottom" uib-tooltip="{{ \'Eliminar\' | language }}" >' +
            '<span aria-hidden="true"><img class="grid-icon-accion" src="img/iconsgrid/iconAccionEliminar.png" alt="Eliminar"/></span>' +
            '</button>' +
            '</div>';

        vm.selectorRenglonTemplate = '<div class="checkboxes-seleccion text-center">' +
            '<input type="checkbox" class="checkbox-single-row" id="archivo{{row.entity.Nombre}}" ng-model="row.entity.Seleccionado" ' +
            'ng-click="grid.appScope.vm.seleccionarArchivoParaDescarga()" ' +
            'ng-disabled="!row.entity.Descargable" />' +
            '<label for="archivo{{row.entity.Nombre}}" class="sr-only">Selecciona archivo</label>' +
            '</div>';

        vm.selectorCabeceraTemplate = '<div class="checkboxes-seleccion text-center">' +
            '<input type="checkbox" class="checkbox-header" id="inputSeleccionarTodos" ng-model="grid.appScope.vm.todosArchivosSeleccionados" ' +
            'ng-click="grid.appScope.vm.seleccionarTodosLosArchivoParaDescarga()" ' +
            'ng-disabled="grid.appScope.vm.noVisualizarArchivos" />' +
            '<label for="inputSeleccionarTodos" class="sr-only">Selecciona Todos</label>' +
            '</div>';
        //#endregion

        //#region Variables $scope
        $scope.opciones = {
            deshabilitarControles: false
        };
        vm.botonInactivo = true;
        vm.botonDescargarInactivo = true;
        //#endregion

        /** Metodo al cargar el controlador */
        this.$onInit = function () {
            obtenerConfiguracionCargaArchivos();
            $scope.opciones.deshabilitarControles = $scope.form.deshabilitarControles ? $scope.form.deshabilitarControles : false;
        };

        /** Invoca un modal de confirmación para proceder con la eliminación logica del registro */
        function confirmarEliminarArchivo() {
            archivoServicios.eliminarArchivo(vm.EntidadEliminar.Id, 'Eliminado', vm.idAplicacion).then(function (response) {
                if (response === undefined) {
                    utilidades.mensajeError("Hubo un error al eliminar el archivo");
                    recargarTabla();
                } else {
                    vm.limiteArchivosACargar = vm.configuracionCargaArchivos.limiteArchivos != undefined ? vm.configuracionCargaArchivos.limiteArchivos : 1;
                    vm.maxConfiguradosCargar = vm.limiteArchivosACargar;
                    vm.deshabilitarCancelarCarga = false;
                    vm.archivoGuardado = false;

                    vm.resetDropzone();

                    obtenerListadoArchivos();
                    utilidades.refrescarScope($scope, 500);
                    utilidades.mensajeSuccess($filter('language')('EliminadoExitosoMetadato'), null, null);
                }

                vm.EntidadEliminar = null;
            });
        }

        /**
         * Elimina el archivo de forma lógica cambiando la propiedad status = 'Eliminado'
         * @param {any} entity
         */
        function eliminarArchivo(entity) {
            vm.EntidadEliminar = entity;
            utilidades.mensajeWarning($filter('language')('ConfirmarEliminarRegistro'), vm.confirmarEliminarArchivo, null, $filter('language')('Aceptar'));
        }

        /**
         * Activa la edición del registro del archivo
         * @param {object} entity
         */
        function editarRegistro(entity) {
            if (entity.ArchivoDescartado) {
                return true;
            } else {
                return false;
            }
        }

        /** Obtiene el listado de archivos que han sido cargados para la instacia y acción seleccionadas */
        function obtenerListadoArchivos() {
            var params = { };

            if (vm.verTodosLosArchivos) {
                params.IdInstanciaFlujoPrincipal = vm.idInstanciaFlujoPrincipal;
            } else {
                params.IdAccion = vm.idAccion;
                params.IdInstancia = vm.idInstancia;
            }

            archivoServicios.obtenerListadoArchivos(params, vm.idAplicacion).then(function (response) {
                vm.DatosOrigen = _.filter(response, function (r) { return r.status !== 'Eliminado' && r.metadatos.tipo !== 'Ficha'; });

                if (!vm.verArchivosDescartados) {
                    vm.DatosOrigen = _.filter(vm.DatosOrigen, function (d) { return d.status !== 'Descartado'; });
                }

                recargarTabla();
            });
        }

        /** Muestra un mensaje de confirmación para cancelar la carga y cerrar el modal */
        function cancelarCarga() {
            if (_.size(vm.archivos) > 0)
                utilidades.mensajeWarning($filter('language')('CancelarCargaArchivos'), vm.confirmaCancelar, null, $filter('language')('Aceptar'));
            else 
                $('#myModal').modal('hide');
        }

        /** Refresca el scope del controlador despues de mostrar el mensaje de Guardado */
        function confirmaCargueArchivo() {
            vm.archivos = [];
            vm.archivosParaCargar = [];
            vm.archivosParaActualizar = [];
            vm.resetDropzone();
            obtenerListadoArchivos();
            utilidades.refrescarScope($scope, 500);
        }

        /** Procesa los archivos y valida reglas de categorias obligatorias */
        function confirmaGuardarArchivo() {
            vm.archivosParaCargar = [];
            vm.archivosParaActualizar = [];
            vm.faltanDatosCategoria = false;
            var archivosSinCategoria = [];
            var categoriasObligatoriasIncluidas = [];
            var msgArchivosSinCategoria = $filter('language')('ArchivosSinCategoria');

            vm.desHabilitarCarga = true;
            vm.porcentajeAvance = 10;

            _.each(vm.listaCategorias, function (c) {
                if (c.obligatorio) {
                    categoriasObligatoriasIncluidas.push(c);
                }
            });

            _.each(_.filter(vm.GridDataOriginal, function (g) { return g.Id === null }), function (d) {
                crearArchivos(d);

                if (_.size(categoriasObligatoriasIncluidas) > 0) {
                    var categoriaObligatoria = _.findWhere(vm.listaCategorias, { 'Id': d.CategoriaId });

                    if (categoriaObligatoria === undefined || categoriaObligatoria === null) {
                        archivosSinCategoria.push(d.Nombre);
                    }
                }
            });

            _.each(_.filter(vm.GridDataOriginal, function (g) { return g.Id !== null && g.DeshabilitarEdicion === false }), function (d) {
                crearArchivos(d);

                if (_.size(categoriasObligatoriasIncluidas) > 0) {
                    var categoriaObligatoria = _.findWhere(vm.listaCategorias, { 'Id': d.CategoriaId });

                    if (categoriaObligatoria === undefined || categoriaObligatoria === null) {
                        archivosSinCategoria.push(d.Nombre);
                    }
                }
            });

            if (_.size(archivosSinCategoria) > 0) {
                msgArchivosSinCategoria = msgArchivosSinCategoria.replace('[0]', JSON.stringify(archivosSinCategoria));
                utilidades.mensajeError(msgArchivosSinCategoria, null);
                vm.faltanDatosCategoria = true;
            } else {

                if (vm.archivosParaCargar.length > 0 || vm.archivosParaActualizar.length > 0) {
                    cargarArchivos();
                }
            }
        }

        /**
         * Metodo interno que crea un objeto para ser enviado al servicio de guardado
         * @param {any} data
         */
        function crearArchivos(data) {
            var archivo = {};

            var metadatos = {
                Proyecto: $sessionStorage.nombreProyecto,
                NombreAccion: vm.nombreAccion,
                IdAplicacion: vm.idAplicacion,
                IdNivel: vm.idNivel,
                IdInstancia: vm.idInstancia,
                IdAccion: vm.idAccion,
                IdInstanciaFlujoPrincipal: vm.idInstanciaFlujoPrincipal,
                IdObjetoNegocio: vm.idObjetoNegocio,
                Size: data.TamanioReal,
                //ContenType: data.ContenType,
                Extension: data.Extension,
                FechaCreacionArchivo: data.FechaCreacionArchivo != undefined ? data.FechaCreacionArchivo : data.Archivo.lastModifiedDate,
                FechaCreacion: new Date(),
                CategoriaId: data.CategoriaId
            }

            if (data.IdArchivoBlob !== null) {
                metadatos.IdArchivoBlob = data.IdArchivoBlob;
            }

            _.each(vm.listaMetadatos, function (m) {
                if (data[m.Nombre] != undefined) {
                    metadatos[m.Nombre] = data[m.Nombre];
                }
            });

            archivo = {
                FormFile: data.Archivo,
                Nombre: data.Nombre,
                Metadatos: metadatos
            };

            if (data.Id !== null) {
                archivo.Id = data.Id;
            }

            if (data.IdArchivoBlob === null) {
                vm.archivosParaCargar.push(archivo);
            } else {
                vm.archivosParaActualizar.push(archivo);
            }
        }

        /** Metodo interno que recorre la lista de vm.archivosParaCargar y los envia al servicio de archivos */
        function cargarArchivos() {
            vm.botonInactivo = true;
            vm.deshabilitarCancelarCarga = false;
            //vm.cantidadArchivosActuales = vm.archivosParaCargar.length;
            var count = 0;
            var totalRegistrosProcesar = vm.archivosParaCargar.length + vm.archivosParaActualizar.length;

            _.each(vm.archivosParaCargar, function (archivo) {
                archivoServicios.cargarArchivo(archivo, vm.idAplicacion).then(function (response) {
                    if (response === undefined || typeof response === 'string') {
                        vm.archivoGuardado = false;
                        vm.mensajeError = response;
                        utilidades.mensajeError(response);
                    } else {
                        $timeout(function () {
                            if (count++ <= totalRegistrosProcesar) {
                                vm.porcentajeAvance = parseInt((count * 100 / totalRegistrosProcesar).toString().split('.')[0]);
                            }

                            if (vm.porcentajeAvance === 100) {
                                vm.resetDropzone();
                                vm.archivoGuardado = true;
                                vm.deshabilitarCancelarCarga = true;
                                if (vm.archivosParaActualizar.length <= 0)
                                    utilidades.mensajeSuccess($filter('language')('ArchivoGuardado'), null, vm.confirmaCargueArchivo);
                            }
                        }, 300)
                    }
                });
            });

            _.each(vm.archivosParaActualizar, function (archivo) {
                archivoServicios.obtenerArchivoInfo(archivo.Id, vm.idAplicacion).then(function (archivoInfo) {
                    archivoInfo.metadatos = archivo.Metadatos;

                    archivoServicios.actualizarArchivo(archivo.Id, archivoInfo, vm.idAplicacion).then(function (response) {
                        if (response === undefined || typeof response === 'string') {
                            vm.archivoGuardado = false;
                            vm.mensajeError = response;
                            utilidades.mensajeError(response);
                        } else {
                            $timeout(function () {
                                if (count++ <= totalRegistrosProcesar) {
                                    vm.porcentajeAvance = parseInt((count * 100 / totalRegistrosProcesar).toString().split('.')[0]);
                                }

                                if (vm.porcentajeAvance === 100) {
                                    vm.resetDropzone();
                                    vm.archivoGuardado = true;
                                    vm.deshabilitarCancelarCarga = true;
                                    utilidades.mensajeSuccess($filter('language')('ArchivoGuardado'), null, vm.confirmaCargueArchivo);
                                }
                            }, 300)
                        }
                    });
                });
            });
        }

        /** Cancela la carga de archivos y cierra el modal */
        function confirmaCancelar() {
            if (_.size(vm.archivos) > 0) {
                vm.resetDropzone();
            }
            
            vm.archivosParaCargar = [];
            vm.archivos = [];     
            vm.estaSeleccionandoArchivo = true;
            //vm.desHabilitarCarga = true;
            $('#myModal').modal('hide');
        }

        /**Mensaje de confirmación para ejecutar el metodo ConfimarGuardarArchivo */
        function guardarArchivos() {
            utilidades.mensajeWarning($filter('language')('ConfirmaGuardarArchivo'), $timeout(vm.confirmaGuardarArchivo, 1000), null, $filter('language')('Aceptar'));
        }

        /** Metodo posterior al $init que permite obtener la configuración del control de archivos */
        function obtenerConfiguracionCargaArchivos() {
            if (vm.configuracionCargaArchivos !== undefined) {
                vm.listaExtensiones = vm.configuracionCargaArchivos.extensiones != undefined ? _.pluck(vm.configuracionCargaArchivos.extensiones, 'TipoDeExtension') : '';
                vm.listaCategorias = vm.configuracionCargaArchivos.categorias;
                vm.listaMetadatos = vm.configuracionCargaArchivos.metadatos;
                vm.limiteArchivosACargar = vm.configuracionCargaArchivos.limiteArchivos != undefined ? vm.configuracionCargaArchivos.limiteArchivos : 1;
                vm.maxConfiguradosCargar = vm.limiteArchivosACargar;
                vm.cargaCompleta = vm.configuracionCargaArchivos.cargaCompleta;
                vm.cargaIndividual = vm.configuracionCargaArchivos.cargaIndividual;
                vm.excluyente = vm.configuracionCargaArchivos.excluyente === true ? true : false;
                vm.multiplesArchivos = vm.configuracionCargaArchivos.multiplesArchivos === true ? true : false;
                vm.verArchivosMGA = vm.configuracionCargaArchivos.verArchivosMGA === true ? true : false;
                vm.verTodosLosArchivos = vm.configuracionCargaArchivos.verTodosLosArchivos === true ? true : false;
                vm.verArchivosDescartados = vm.configuracionCargaArchivos.verArchivosDescartados === true ? true : false;
                vm.noVisualizarArchivos = vm.configuracionCargaArchivos.noVisualizarArchivos === true ? true : false;
            }

            crearColumnasGrilla();

            vm.dataHasLoaded = true;
        }

        function cellClassFunction(grid, row, col){
            if (row.entity.Status=== "Descartado"){
                return 'ui-row-descartado';
            }
            
            if(row.entity.Status === 'Creado'){
                return 'ui-row-creado';
            }

            return 'ui-row-activo';
        }
        
        //#region Configuración del grid de metadatos
        /** Crea de forma dinamica el grid de los archivos */
        function crearColumnasGrilla() {
            var columns = [
                {
                    name: 'Seleccionado',
                    displayName: '',
                    width: '5%',
                    enableFiltering: false,
                    enableSorting: false,
                    enableCellEdit: false,
                    enableColumnMenu: false,
                    cellTemplate: vm.selectorRenglonTemplate,
                    headerCellTemplate: vm.selectorCabeceraTemplate
                }, //Columna de seleccion de archivos
                {
                    name: 'PasoAccion',
                    displayName: 'Paso',
                    enableCellEdit: false,
                    enableColumnMenu: false,
                    enableSorting: true,
                    cellClass: cellClassFunction
                }, //Columna de Nombre de archivo
                {
                    name: 'Nombre',
                    displayName: 'Nombre',
                    enableCellEdit: false,
                    enableColumnMenu: false,
                    enableSorting: true,
                    cellClass: cellClassFunction
                }, //Columna de Nombre de archivo
                {
                    name: 'Extension',
                    displayName: 'Extensión',
                    enableCellEdit: false,
                    enableColumnMenu: false,
                    enableSorting: true,
                    cellClass: cellClassFunction
                }, //Columna de Extensión de archivo
                {
                    name: 'Tamano',
                    displayName: 'Tamaño',
                    enableCellEdit: false,
                    enableColumnMenu: false,
                    enableSorting: true,
                    cellClass: cellClassFunction
                }, //Columna de Tamaño de archivo
                {
                    name: 'CategoriaId',
                    displayName: 'Categoría',
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
                    editDropdownOptionsArray: vm.listaCategorias,
                    cellClass: cellClassFunction
                }  //Columna de categoria de archivo
            ]; //Columnas del grid

            _.each(vm.listaMetadatos, function (data) {
                columns.push({
                    name: data.Nombre,
                    displayName: data.Obligatorio ? '* ' + data.Nombre : data.Nombre,
                    //enableCellEdit: false,
                    cellEditableCondition: function ($scope, triggerEvent) {
                        //use $scope.row.entity, $scope.col.colDef and triggerEvent to determine if editing is allowed
                        var existe = _.filter(constantesArchivos.metadatosConstantes, function (p) { return p === $scope.col.colDef.name });

                        if ( _.contains(constantesArchivos.metadatosConstantes, $scope.col.colDef.name) )
                            return false;
                        else
                            return true;
                    },
                    enableSorting: false,
                    enableFiltering: true,
                    enableColumnMenu: false,
                    cellClass: cellClassFunction
                }); //Columnas dinamicas de metadatos
            }); //Columnas dinamicas de metadatos

            columns.push({
                name: 'Acciones',
                cellTemplate: vm.botonesTemplate,
                enableCellEdit: false,
                enableSorting: false,
                enableColumnMenu: false,
                enableFiltering: false
            }); //Columna de boton de acciones

            vm.grillaMetadatos = {
                paginationPageSizes: [],
                paginationPageSize: 3,
                enablePaginationControls: true,
                columnDefs: columns,
                rowHeight: 180,
                enableCellSelection: true,
                enableRowSelection: false,
                enableColumnMenu: false,
                enableCellEditOnFocus: true,
                rowEditWaitInterval: -1,
                onRegisterApi: function (gridApi) {
                    vm.gridApi = gridApi;
                }
            }; //Grid de archivos
        }

        /** Obtiene los datos obtenidos del repositorio y los visualiza en el grid */
        function cargaOrigenes() {
            var dataTemp = [];

            if (_.size(vm.DatosOrigen) > 0) {
                _.each(vm.DatosOrigen, function (o) {
                    var archivoTemp = {
                        Id: o.id.toString(),
                        PasoAccion: o.metadatos.nombreaccion,
                        Nombre: o.nombre.slice(0, ((utilidades.obtenerExtensionArchivo(o.nombre).length + 1) * -1)),
                        NombreCompleto: o.nombre,
                        Descargable: true, //puedeDescargarse(o.status.trim()),
                        Status: o.status.trim(),
                        OcultarBotones: o.metadatos.idaccion !== vm.idAccion ? true : o.status.trim() !== "Descartado" ? false : true,
                        ContenType: o.metadatos.contenttype,
                        Tamano: $filter('bytes')(o.metadatos.size),
                        TamanioReal: o.metadatos.size,
                        Extension: o.metadatos.extension,
                        CategoriaId: o.metadatos.categoriaid,
                        IdAplicacion: o.metadatos.idaplicacion,
                        IdNivel: o.metadatos.idnivel,
                        Archivo: undefined,
                        IdArchivoBlob: o.metadatos.idarchivoblob,
                        FechaCreacionArchivo: o.metadatos.fechacreacionarchivo,
                        FechaCreacion: o.metadatos.fechacreacion,
                        ArchivoDescartado: o.status.trim() !== "Descartado" ? false : true,
                        DeshabilitarEdicion: o.status.trim() !== "Descartado" ? (o.metadatos.idarchivoblob != null || o.metadatos.idarchivoblob != undefined) ? true : false : true,
                        ArchivoPasoExterno: o.metadatos.idaccion !== vm.idAccion ? true : false
                    };

                    _.each(vm.listaMetadatos, function (m, key) {
                        if (o.metadatos[m.Nombre.trim().toLowerCase()] !== undefined) {
                            archivoTemp[m.Nombre.trim()] = o.metadatos[m.Nombre.trim().toLowerCase()];
                        }
                    });

                    dataTemp.push(archivoTemp);

                    vm.grillaMetadatos.data = _.sortBy(dataTemp, 'PasoAccion');
                });

                vm.limiteArchivosACargar = vm.maxConfiguradosCargar - _.size(_.filter(vm.DatosOrigen, function (d) { return d.status.trim() !== 'Descartado' && d.metadatos.idaccion === vm.idAccion }));
            }
        }

        /**
         * Retorna el true o false dependiendo de la configuración del control de archivos para descarga de elementos.
         * @param {string} _status
         */
        function puedeDescargarse(_status) {
            if (vm.noVisualizarArchivos)
                return false;
            else {
                if (vm.verTodosLosArchivos)
                    return true;
                else {
                    if (_status.trim().toLowerCase() === 'Descartado') {
                        return vm.verArchivosDescartados;
                    } else {
                        return true;
                    }
                }
            }
        }

        /** Recarga la tabla cuando se carga un archivo */
        function recargarTabla() {
            vm.grillaMetadatos.data = [];
            var dataTemp = [];

            if (_.size(vm.DatosOrigen) > 0) {
                cargaOrigenes();
                dataTemp = vm.grillaMetadatos.data;
            }

            if (_.size(vm.archivos) > 0) {
                _.each(vm.archivos, function (data) {
                    var archivoTemp = {
                        Id: null,
                        PasoAccion: vm.nombreAccion,
                        Nombre: data.name.slice(0, ((utilidades.obtenerExtensionArchivo(data.name).length + 1) * -1)),
                        NombreCompleto: data.name,
                        Descargable: false,
                        Status: 'Creado',
                        OcultarBotones: false,
                        ContenType: data.type,
                        Tamano: $filter('bytes')(data.size),
                        TamanioReal: data.size,
                        Extension: utilidades.obtenerExtensionArchivo(data.name),
                        IdAplicacion: vm.idAplicacion,
                        IdNivel: vm.idNivel,
                        Archivo: data,
                        IdArchivoBlob: null,
                        FechaCreacionArchivo: data.lastModifiedDate,
                        FechaCreacion: new Date(),
                        ArchivoDescartado: false,
                        DeshabilitarEdicion: true,
                        ArchivoPasoExterno: false
                    };

                    _.each(vm.listaMetadatos, function (data, key) {
                        var valueMetadato = '';

                        if (data.Nombre.trim().toUpperCase() === 'BPIN') {
                            valueMetadato = vm.idObjetoNegocio
                        }

                        archivoTemp[data.Nombre] = valueMetadato;
                    });

                    dataTemp.push(archivoTemp);

                    vm.grillaMetadatos.data = _.sortBy(dataTemp, 'PasoAccion');
                });
            }

            vm.GridDataOriginal = vm.grillaMetadatos.data;
            vm.botonInactivo = _.size(vm.grillaMetadatos.data) <= 0;
        }

        /** Selecciona todos los registros de archivos y activa el boton de descarga de archivos */
        function seleccionarTodosLosArchivoParaDescarga() {
            var activas = vm.gridApi.core.getVisibleRows(vm.gridApi.grid);

            _.each(activas, function (value) {
                if (value.entity.Descargable)
                    value.entity.Seleccionado = vm.todosArchivosSeleccionados;
            });

            vm.botonDescargarInactivo = !vm.todosArchivosSeleccionados;
        }

        /** Activar el boton de descarga de archivos */
        function seleccionarArchivoParaDescarga() {
            var activas = vm.gridApi.core.getVisibleRows(vm.gridApi.grid);
            var cuantosActivos = 0;

            _.each(activas, function (a) {
                if (a.entity.Seleccionado) {
                    cuantosActivos++;
                }
            });

            vm.botonDescargarInactivo = cuantosActivos > 0 ? false : true;
        }

        /** Activa el panel de filtros para el grid de archivos */
        function conmutadorFiltro() {
            limpiarFiltro();
            vm.filtroActivado = !vm.filtroActivado;
        }

        /** Ejecuta el filtro */
        function ejecutarFiltro() {
            var between = false;

            if (vm.filtro.FechaInicio != null || vm.filtro.FechaFin != null) {
                between = true;
            }

            var datosFiltrados = _.filter(vm.GridDataOriginal, function (df) {
                var valorEncontrado = df;

                if (vm.filtro.Nombre != null) {
                    if (valorEncontrado.Nombre.toUpperCase().includes(vm.filtro.Nombre.toUpperCase()))
                        valorEncontrado = valorEncontrado;
                    else
                        valorEncontrado = null;
                }

                if (vm.filtro.Extension != null) {
                    if (valorEncontrado != null) {
                        if (valorEncontrado.Extension.toUpperCase().includes(vm.filtro.Extension.toUpperCase()))
                            valorEncontrado = valorEncontrado;
                        else
                            valorEncontrado = null;
                    }                    
                };

                return valorEncontrado;
            });

            if (between) {
                datosFiltrados = _.filter(datosFiltrados, function (w) {
                    var fechaInicio = moment(vm.filtro.FechaInicio != null ? vm.filtro.FechaInicio : moment(), 'DD/MM/YYYY');
                    var fechaFin = moment(vm.filtro.FechaFin != null ? vm.filtro.FechaFin : moment(), 'DD/MM/YYYY');
                    var fctemp = moment(w.FechaCreacion);
                    var fechaCreacionTemp = moment({ year: fctemp.year(), month: fctemp.month(), day: fctemp.date() });
                    var fcatemp = moment(w.FechaCreacionArchivo);
                    var fechaCreacionArchivoTemp = moment({ year: fcatemp.year(), month: fcatemp.month(), day: fcatemp.date() })

                    return (fechaCreacionTemp.isSameOrAfter(fechaInicio) && fechaCreacionTemp.isSameOrBefore(fechaFin))
                        || (fechaCreacionArchivoTemp.isSameOrAfter(fechaInicio) && fechaCreacionArchivoTemp.isSameOrBefore(fechaFin));
                });
            }            

            vm.grillaMetadatos.data = datosFiltrados;
        }

        /** Limpia el grid de filtros */
        function limpiarFiltro() {
            vm.filtro.Nombre = null;
            vm.filtro.Extension = null;
            vm.filtro.FechaInicio = null;
            vm.filtro.FechaFin = null;
            vm.grillaMetadatos.data = vm.GridDataOriginal;
        }
        //#endregion

        /** Descarga todos los archivos seleccionados en el grid */
        function descargarSeleccionados() {
            var activas = vm.gridApi.core.getVisibleRows(vm.gridApi.grid);

            _.each(activas, function (e) {
                if (e.entity.Seleccionado && e.entity.IdArchivoBlob != undefined) {
                    archivoServicios.obtenerArchivoBytes(e.entity.IdArchivoBlob, vm.idAplicacion).then(function (retorno) {
                        const blob = utilidades.base64toBlob(retorno, e.entity.ContenType);
                        //var blob = new Blob([retorno], {
                        //    type: e.entity.ContenType
                        //});
                        FileSaver.saveAs(blob, e.entity.NombreCompleto);
                    }, function (error) {
                        toastr.error("Error inesperado al descargar");
                    });
                }
            });
        }

        /**
         * Permite descargar el archivo seleccionado
         * @param {object} entity
         */
        function descargarArchivo(entity) {
            if (entity.IdArchivoBlob != undefined) {
                archivoServicios.obtenerArchivoBytes(entity.IdArchivoBlob, vm.idAplicacion).then(function (retorno) {
                    const blob = utilidades.base64toBlob(retorno, entity.ContenType);
                    /*const blob = new Blob([retorno], { type: entity.ContenType });*/
                    FileSaver.saveAs(blob, entity.NombreCompleto);
                }, function (error) {
                    toastr.error("Error inesperado al descargar");
                });
            }
        }

        //Inicio de la configuración del control de carga de archivos
        
        function iniciarModalCarga() {
            obtenerListadoArchivos();
            $('#myModal').modal('show');
            vm.mostrarModalCarga = true;
        }
    }
})();
