(function () {
    'use strict';

    firmaTramiteController.$inject = [
        'backboneServicios',
        'sesionServicios',
        'configurarEntidadRolSectorServicio',
        '$scope',
        'utilidades',
        'constantesCondicionFiltro',
        '$sessionStorage',
        'trasladosServicio',
        'constantesBackbone',
        '$routeParams',
        'servicioResumenDeProyectos',
        'uiGridConstants',
        '$timeout',
        '$location',
        'cartaServicio',
        '$q',
        'servicioFichasProyectos', 'archivoServicios', 'FileSaver'
    ];



    function firmaTramiteController(
        backboneServicios,
        sesionServicios,
        configurarEntidadRolSectorServicio,
        $scope,
        utilidades,
        constantesCondicionFiltro,
        $sessionStorage,
        trasladosServicio,
        constantesBackbone,
        $routeParams,
        servicioResumenDeProyectos,
        uiGridConstants,
        $timeout,
        $location,
        cartaServicio,
        $q,
        servicioFichasProyectos, archivoServicios, FileSaver
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.EntityTypeCatalogOptionId = 0;
        vm.nombreSccion = 'Firma Tràmite';
        vm.tipoTramite = 1;
        vm.tipoTramite = $sessionStorage.TipoTramiteId;
        vm.numeroTramite = $sessionStorage.numeroTramite;
        vm.entidad = $sessionStorage.usuario.permisos.Entidades[0].NombreEntidad;
        vm.rolUsuario = $sessionStorage.usuario.roles.find((item) => item.Nombre.includes('Director'));
        /* metodos*/
        vm.archivoSeleccionado = archivoSeleccionado;
        vm.limpiarArchivo = limpiarArchivo;
        /*variables objetos*/
        vm.data = '';
        vm.firmar = firmar;

       
        
       


        function validarSiExisteFirmaUsuario() {
            var x = trasladosServicio.validarSiExisteFirmaUsuario().then(function (response) {
                if (response.data.Exito && (response.statusText === "OK" || response.status === 200)) {
                    $timeout(function () {
                        $scope.firmaCargada = true;
                    }, 100);
                   
                } else {
                    $timeout(function () {
                        $scope.firmaCargada = false;
                    }, 100);
                   
                }
            });
        }

       

              

        vm.initFirmaTramite = function () {
            validarSiExisteFirmaUsuario();
            
        }

        function archivoSeleccionado() {
            
            CargarFirma();
        }


        function limpiarArchivo() {
            $scope.files = [];
            document.getElementById("control").value = "";
            $timeout(function () {
                $scope.data.file = undefined;
            }, 100);
        }


        $scope.SelectFile = function (event) {

            $scope.files = [];
            var files = [];
            const fileByteArray = [];
            var reader = new FileReader();

            if (event != null) {
                event.preventDefault();


                files.push(event.currentTarget.files[0]);

                if (event.currentTarget.files[0].size > 2048000) {
                    swal('', "Tamaño del archivo mayor a 2 megas", 'error');
                }
                else {

                    if ($scope.validaNombreArchivo(files[0].name)) {
                        $scope.files.push({ nombreArchivo: files[0].name, size: files[0].size, arhivo: files[0] });

                        reader.onload = function () {
                            vm.data = reader.result.replace("data:", "")
                                .replace(/^.+,/, "");
                        }
                        reader.readAsDataURL(files[0]);

                    }
                }
            }

        };

       

        function getBase64(file) {
            return new Promise((resolve, reject) => {
                const reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = () => resolve(reader.result);
                reader.onerror = error => reject(error);
            });
        }

        function CargarFirma() {
            var x = trasladosServicio.cargarFirma(vm.data, vm.rolUsuario.IdRol).then(function (response) {
                if (response.data && (response.statusText === "OK" || response.status === 200)) {
                    parent.postMessage("cerrarModal", window.location.origin);
                    utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                    $timeout(function () {
                        $scope.firmaCargada = true;
                    }, 100);
                } else {
                    swal('', "Error al realizar la operación", 'error');
                }
            });
            
        }

        vm.radicadosalida = '';
        function firmar() {
           
                    trasladosServicio.consultarCarta($sessionStorage.TramiteId).then(function (resultado) {
                        if (resultado != undefined && resultado.data != null) {
                            var cartaTmp = resultado.data;
                            if (cartaTmp.RadicadoEntrada != null && cartaTmp.RadicadoEntrada != '') 
                            {
                                var cs = cartaTmp.RadicadoSalida === "null" || cartaTmp.RadicadoSalida === null  ? '0' : cartaTmp.RadicadoSalida;
                                var firma = trasladosServicio.generarRadicadoSalida($sessionStorage.numeroTramite,cs ).then(function (respuesta) {
                                    if (respuesta != undefined && respuesta.data != null && respuesta.data.Estado) {

                                        vm.radicadosalida = respuesta.data.Data.RadicadoSalida;
                                        return respuesta.data.Data.RadicadoSalida;

                                    }
                                    else {
                                        swal('', respuesta.data.Mensaje, 'error');
                                        return undefined;
                                    }

                                }).then(function (respuestaRadicadoSalida) {
                                    if (respuestaRadicadoSalida != undefined) {
                                        var x = trasladosServicio.firmar($sessionStorage.TramiteId, vm.radicadosalida).then(function (resultadoFirmaConcepto) {
                                            if (resultadoFirmaConcepto.data.Exito) {
                                                return true;
                                            }
                                            else {
                                                swal('', resultadoFirmaConcepto.data.Mensaje, 'error');
                                                return false;
                                            }
                                        }).then(function (respuestaFirmaConcepto) {
                                            if (respuestaFirmaConcepto) {

                                                vm.esArchivoOrfeo = true;
                                                var ficha = {
                                                    Nombre: constantesBackbone.apiBackBoneNombrePDFCartaFirma,
                                                };

                                                vm.Ficha = ficha;

                                                var fichaPlantilla = {
                                                    NombreReporte: ficha.Nombre,
                                                    PARAM_BORRADOR: false,
                                                    PARAM_BPIN: $sessionStorage.idObjetoNegocio,
                                                    TramiteId: $sessionStorage.TramiteId
                                                };

                                                crearDocumento(fichaPlantilla).then(function (resultadoarchivo) {
                                                    if (vm.archivoPDF != undefined) {
                                                        var reader = new FileReader();
                                                        reader.onload = function () {
                                                            $timeout(function () {
                                                                vm.data = reader.result.replace("data:", "")
                                                                    .replace(/^.+,/, "");
                                                            }, 0).then(function () {

                                                                var parametros = {
                                                                    NumeroTramite: vm.numeroTramite,
                                                                    TramiteId: $sessionStorage.TramiteId,
                                                                    datosDocumentoElectronicoDto: {
                                                                        fileBase64Bin: vm.data,
                                                                        extension: 'pdf',
                                                                        nombre: 'TramiteCartaConcepto'
                                                                    },
                                                                    usuarioRadica: {
                                                                        Documento: '',
                                                                        Login: ''
                                                                    },
                                                                    datosRadicadoDto: {
                                                                        esPrincipal: true,
                                                                        observacion: "Documento  de firma trámite traslado con número " + vm.numeroTramite,
                                                                        NoRadicado: vm.radicadosalida != undefined || vm.radicadosalida != null ? vm.radicadosalida : ''
                                                                    }
                                                                }
                                                                var x = trasladosServicio.cerrar_CargarDocumentoElectronicoOrfeo(parametros).then(function (resultadoOrfeo) {
                                                                    if (resultadoOrfeo.data && (resultadoOrfeo.statusText === "OK" || resultadoOrfeo.status === 200) && resultadoOrfeo.data.Exito) {
                                                                        trasladosServicio.firmar($sessionStorage.TramiteId).then(function (response) {
                                                                            if (response.data && (response.statusText === "OK" || response.status === 200)) {
                                                                                $scope.firmado = true;
                                                                                vm.visibleValidar = false;
                                                                                trasladosServicio.firmarTramite($sessionStorage.TramiteId);
                                                                                vm.callback({ arg: false, aprueba: true, titulo: '', ocultarDevolver: true });
                                                                                parent.postMessage("cerrarModal", window.location.origin);
                                                                                utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);



                                                                            } else {
                                                                                swal('', "Error al realizar la operación", 'error');
                                                                                vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: false });
                                                                            }
                                                                        });
                                                                    }
                                                                    else {
                                                                        swal('', resultadoOrfeo.data.Mensaje, 'error');
                                                                        vm.callback({ arg: true, aprueba: true, titulo: '', ocultarDevolver: false });
                                                                    }
                                                                });
                                                            });

                                                        }
                                                        reader.readAsDataURL(vm.archivoPDF.FormFile);

                                                    }
                                                })
                                            }
                                        });;
                                    }

                                })

                            }
                            else
                            {
                                swal('', "No hay radicado de entrada para este tramite", 'error');
                                vm.radicadosalida  = undefined;
                                return undefined;
                            }

                        }
                        else
                        {
                            swal('', "No hay carta concepto para este tramite", 'error');
                            return undefined;
                        }
                        
                    })
             //}
            //else
            //    swal('', "No hay radicado de entrada o radicado de salida", 'error');






        }

        function crearDocumento(fichaPlantilla) {
            var extension = '.pdf';
            var nombreArchivo = vm.Ficha.Nombre.replace(/ /gi, "_") + '_' + $sessionStorage.idObjetoNegocio + '_' + moment().format("YYYYMMDDD_HHMMSS") + extension;

            return $q(function (resolve, reject) {
                servicioFichasProyectos.ObtenerIdFicha(vm.Ficha.Nombre).then(function (respuestaFicha) {

                    servicioFichasProyectos.GenerarFicha($.param(fichaPlantilla)).then(function (respuesta) {
                        /*var blob = new Blob([respuesta], { type: 'application/pdf' });*/
                        const blob = utilidades.base64toBlob(respuesta, { type: 'application/pdf' });
                        var fileOfBlob = new File([blob], nombreArchivo, { type: 'application/pdf' });
                        var archivo = {};

                        var metadatos = {
                            NombreAccion: $sessionStorage.nombreAccion,
                            IdAplicacion: 1,//$sessionStorage.IdAplicacion,
                            IdNivel: $sessionStorage.idNivel,
                            IdInstancia: $sessionStorage.idInstancia,
                            IdAccion: $sessionStorage.idAccion,
                            IdInstanciaFlujoPrincipal: $sessionStorage.idInstanciaFlujoPrincipal,
                            IdObjetoNegocio: $sessionStorage.idObjetoNegocio,
                            Size: blob.size,
                            ContenType: 'application/pdf',
                            Extension: extension,
                            FechaCreacion: new Date(),
                            Tipo: 'Ficha',
                            NombreFicha: respuestaFicha.Nombre,
                            TipoFicha: respuestaFicha.Descripcion
                        }

                        archivo = {
                            FormFile: fileOfBlob,
                            Nombre: nombreArchivo,
                            Metadatos: metadatos
                        };

                        if (vm.esArchivoOrfeo) {
                            vm.esArchivoOrfeo = false;
                            vm.archivoPDF = archivo;
                            resolve('Success!');
                        }
                        else if (fichaPlantilla.PARAM_BORRADOR) {
                            resolve(fileOfBlob);
                        } else {
                            archivoServicios.cargarArchivo(archivo, vm.idAplicacion).then(function (response) {
                                if (response === undefined || typeof response === 'string') {
                                    reject(response);
                                } else {
                                    resolve(fileOfBlob);
                                }
                            }, function (error) {
                                reject(error);
                            });
                        }
                    }, function (error) {
                        reject(error);
                    });

                }, function (error) {
                    reject(error);
                });
            });
        }


        $scope.validaNombreArchivo = function (nombre) {
            var regex = /^([\ \(a-zA-Z0-9\s_\\.\-:\ \)])+(.png)$/;
            if (!regex.test(nombre.toLowerCase())) {
                utilidades.mensajeError("El archivo debe tener algún formato de tipo  .png!");
                $scope.files = [];
                $scope.nombreArchivo = '';
                return false;
            }
            else {
                return true;
            }
        }

    }

    angular.module('backbone').component('firmaTramite', {

        templateUrl: "src/app/formulario/ventanas/tramites/componentes/firma/firmaTramite.html",
        controller: firmaTramiteController,
        controllerAs: "vm",
        bindings: {
            disabled: '=',
            callback: '&'
        }
    }).directive("fileDropzoneimage", function () {
        return {
            restrict: 'A',
            scope: {
                filesToUpload: '=',
                validaNombreArchivo: '&',
                data: '='
            },
            link: function (scope, element, attrs) {
                var processDragOverOrEnter;
                processDragOverOrEnter = function (event) {
                    if (event != null) {
                        event.preventDefault();
                    }
                    return false;
                };

                var regex = /^([\ \(a-zA-Z0-9\s_\\.\-:\ \)])+(.png)$/;
                scope.data = '';
                scope.filesToUpload = [];
                element.bind('dragover', processDragOverOrEnter);
                element.bind('dragenter', processDragOverOrEnter);

                return element.bind('drop', function (event) {
                    // try {
                    scope.filesToUpload = (event);
                    var files = [];
                    scope.filesToUpload = [];

                    if (event != null) {
                        event.preventDefault();
                    }

                    var fileCount = 0;
                    angular.forEach(event.originalEvent.dataTransfer.files, function (item) {
                        if (fileCount < 10) { //Can add a variety of file validations                              
                            files.push(item);
                        }
                        fileCount++;
                    });
                    if (fileCount > 10) alert("You can only select up to 10 files. Please note only the first 10 will be processed.");


                    files.forEach(function (item) {



                        var reader = new FileReader();

                        reader.readAsDataURL(item);
                        scope.data = item.name;
                        scope.filesToUpload.push({ nombreArchivo: item.name, size: item.size, arhivo: item });
                        scope.validaNombreArchivo()(scope.data);

                    });
                });


            },
            controller: function ($scope, $element) {
            }

        }
    });;


})();