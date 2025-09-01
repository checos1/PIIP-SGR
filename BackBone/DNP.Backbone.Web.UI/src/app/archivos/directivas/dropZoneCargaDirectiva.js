(function () {
    "use strict";

    angular.module('backbone.archivo.directivas')
        .directive('dropzone', ['$timeout', '$filter', 'utilidades', function ($timeout, $filter, utilidades) {

            //Variables Globales.
            var cargaCompletaByte = 0;
            var totalTamañoByte = 0;
            var mensaje = '';

            return {
                restrict: 'C',
                // ReSharper disable once UnusedParameter
                link: function (scope, element, attrs) {
                    var config = {
                        uploadMultiple: false,
                        createImageThumbnails: false,
                        url: 'fake',
                        maxFilesize: mbConverterToByte(scope.vm.cargaCompleta),
                        paramName: "uploadfile",
                        maxThumbnailFilesize: 10,
                        parallelUploads: 1,
                        //maxFiles: scope.vm.limiteArchivosACargar,
                        autoProcessQueue: false,
                        addRemoveLinks: true,
                        acceptedFiles: scope.vm.listaExtensiones.toString(),
                        dictRemoveFile: '<span class="icon-delete" ng-disabled="scope.vm.deshabilitarCancelarCarga" data-toggle="tooltip" data-placement="bottom" title="Eliminar"></span>',
                        //init: function () {
                        //    this.on("maxfilesexceeded", function () {
                        //        if (this.files[scope.vm.limiteArchivosACargar] !== null) {
                        //            mensaje = $filter('language')('CantitadArchivosInvalidos');
                        //            mensaje = mensaje.replace('[0]', scope.vm.maxConfiguradosCargar);
                        //            alertaArchivos(mensaje);
                        //            this.removeFile(this.files[scope.vm.limiteArchivosACargar]); 
                        //        }
                        //    });
                        //}
                    };

                    var eventHandlers = {
                        'addedfile': function (file) {
                            
                            if (validarExtensionPermitida(file, this)) {
                                if (this.files.length <= scope.vm.limiteArchivosACargar) {
                                    scope.vm.desHabilitarCarga = false;
                                    if (validarNombreEnDopzone(file, this)) {
                                        if (validarTamanoArchivoIndividual(file, this)) {
                                            if (validarTamanoArchivosTotal(file, this)) {
                                                totalTamañoByte = totalTamañoByte + file.size;
                                                scope.vm.archivos.push(file);
                                                scope.vm.nombresEnDropzone.push(file.name);
                                                scope.vm.recargarTabla();
                                            } else {
                                                return;
                                            }
                                        } else {
                                            return;
                                        }
                                    } else {
                                        return;
                                    }

                                    scope.$apply(function () {
                                        scope.fileAdded = true;
                                    });
                                } else {
                                    mensaje = $filter('language')('CantitadArchivosInvalidos');
                                    mensaje = mensaje.replace('[0]', scope.vm.maxConfiguradosCargar);
                                    this.removeFile(file);
                                    alertaArchivos(mensaje);
                                }
                            } else {
                                //this.removeFile(file);
                                return;
                            }
                        },
                        'success': function (file, response) {
                        },
                        'removedfile': function (file) {
                            if (scope.vm.archivoGuardado) {
                                return;
                            }
                            if (this.getAddedFiles().length === 0) {

                                if (this.files.length === 0) {
                                    scope.vm.archivos = [];
                                    scope.vm.nombresEnDropzone = [];
                                    scope.vm.archivosParaCargar = [];

                                    if (totalTamañoByte !== 0) {
                                        totalTamañoByte = totalTamañoByte - file.size;
                                    }

                                    scope.vm.desHabilitarCarga = true;
                                } else {

                                    if (file.status !== 'error') {
                                        if (validarExtensionPermitida(file)) {
                                            if (this.files.length < scope.vm.nombresEnDropzone.length) {
                                                var posicion = scope.vm.nombresEnDropzone.indexOf(file.name);
                                                scope.vm.nombresEnDropzone.splice(posicion, 1);
                                                scope.vm.archivos.splice(posicion, 1);
                                            }

                                            totalTamañoByte = totalTamañoByte - file.size;
                                        }
                                    }

                                    scope.vm.desHabilitarCarga = false;
                                }

                                scope.vm.archivosParaCargar = [];
                                scope.vm.recargarTabla();
                                utilidades.refrescarScope(scope, 500);
                            }
                        }
                    };

                    var dropzone = new Dropzone(element[0], config);

                    dropzone.options.maxFiles = scope.vm.limiteArchivosACargar;

                    angular.forEach(eventHandlers, function (handler, event) {
                        dropzone.on(event, handler);
                    });

                    scope.processDropzone = function () {
                        dropzone.processQueue();
                    };

                    scope.vm.resetDropzone = function () {
                        dropzone.removeAllFiles();
                    };

                    scope.vm.disableDropzone = function () {
                        dropzone.options.addRemoveLinks = false;
                        dropzone.options.dictRemoveFile = "";
                        _.forEach(dropzone.files, function (archivo) {
                            dropzone.emit("complete", archivo);
                        });
                        dropzone.removeEventListeners();
                    };

                    function alertaArchivos(mensaje, file) {
                        utilidades.mensajeError(mensaje);
                    }

                    function mbConverterToByte(mb) {
                        return mb * 1024 * 1024;
                    }

                    function validarTamanoArchivoIndividual(file, that) {
                        var cargaIndividualByte = mbConverterToByte(scope.vm.cargaIndividual);
                        if (file.size > cargaIndividualByte) {
                            mensaje = $filter('language')('TamañoMáximoPorArchivoInvalido');
                            mensaje = mensaje.replace('[0]', scope.vm.cargaIndividual + 'MB');
                            that.removeFile(file);
                            totalTamañoByte = totalTamañoByte + file.size;
                            alertaArchivos(mensaje);                           
                            scope.vm.recargarTabla();

                            return false;
                        }

                        return true;
                    }

                    function validarTamanoArchivosTotal(file, that) {
                        cargaCompletaByte = mbConverterToByte(scope.vm.cargaCompleta);
                        totalTamañoByte = totalTamañoByte + file.size;
                        if (totalTamañoByte > cargaCompletaByte) {
                            mensaje = $filter('language')('TamañoMáximoTotalInvalido');
                            mensaje = mensaje.replace('[0]', scope.vm.cargaCompleta + 'MB');
                            that.removeFile(file);
                            alertaArchivos(mensaje);
                            scope.vm.recargarTabla();

                            return false;
                        }
                        totalTamañoByte = totalTamañoByte - file.size;
                        return true;
                    }

                    function validarNombreEnDopzone(file, that) {
                        if (scope.vm.nombresEnDropzone.length > 0) {

                            var nombreExiste = _.contains(scope.vm.nombresEnDropzone, file.name);

                            if (nombreExiste) {
                                mensaje = $filter('language')('YaExisteNombreParaCargar');
                                mensaje = mensaje.replace('[0]', file.name);
                                totalTamañoByte = totalTamañoByte + file.size;
                                that.removeFile(file);
                                alertaArchivos(mensaje);
                                scope.vm.recargarTabla();

                                return false;
                            } else {
                                return true;
                            }
                        } else {
                            return true;
                        }
                    }

                    function validarExtensionPermitida(file, that) {
                        var extensionArchivo = '.' + utilidades.obtenerExtensionArchivo(file.name);
                        var arrayExtensiones = _.filter(scope.vm.listaExtensiones, function (e) { return e.trim() === extensionArchivo; });
                        var existeExtension = _.size(arrayExtensiones) > 0 ? true : false;

                        if (!existeExtension) {
                            mensaje = $filter('language')('MensajeExtensionNoValida');
                            that.removeFile(file);
                            alertaArchivos(mensaje);
                            return false;
                        } else {
                            return true;
                        }

                    }
                }
            };
        }]);
})();