(function () {
    'use strict';

    cargaArchivosvfController.$inject = [
        '$scope',
        'tramiteVigenciaFuturaServicio',
        '$sessionStorage',
        'utilidades',
        'constantesBackbone',
        'archivoServicios',
        'servicioAcciones',
        'backboneServicios',
        'sesionServicios',
        '$timeout',
        '$q',
        'justificacionCambiosServicio',

    ];

    function cargaArchivosvfController(
        $scope,
        tramiteVigenciaFuturaServicio,
        $sessionStorage,
        utilidades,
        constantesBackbone,
        archivoServicios,
        servicioAcciones,
        backboneServicios,
        sesionServicios,
        $timeout,
        $q,
        justificacionCambiosServicio,

    ) {
        var vm = this;
        vm.lang = "es";
        vm.extension = "";
        vm.filename = "";
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.descargarArchivoBlob = descargarArchivoBlob;
        vm.eliminarArchivoBlob = eliminarArchivoBlob;
        vm.consultarArchivosTramite = consultarArchivosTramite;
        vm.gridOptions;
        vm.archivosLoad = [];
        vm.IdNivel = $sessionStorage.idNivel;
        vm.tituloconregistros = 'Archivos cargados';
        vm.titulogrilla = vm.titulosinregistros = 'Aún no se han asociados documentos Conpes';
        vm.totalRegistros = 0;
        vm.listadocumentosObligatorios = [];
       

        vm.nombreComponente = "documentossoportealojararchivo";

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        //variables locales
        vm.listaTipoArchivos = [];
        
        vm.idTipoArchivoSeleccionado = '0';
        //vm.tipoTramiteId = $sessionStorage.tipoTramiteId;
        vm.modelo = {
            coleccion: "tramites", ext: ".pdf", codigoProceso: $sessionStorage.numeroTramite, descripcionTramite: $sessionStorage.descripcionTramite, idInstancia: $sessionStorage.idInstancia,
            idAccion: vm.idAccionParam, section: "requerimientosTramite", idTipoTramite: vm.tipoTramiteId, allArchivos: $sessionStorage.allArchivosTramite,
            soloLectura: $sessionStorage.soloLectura
        }
        vm.archivo = {};
        //region declarar metodos
        vm.initCargarArchivos = initCargarArchivos;
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.onClickCancelar = onClickCancelar;
        vm.onClickCargar = onClickCargar;
        vm.abrirpanel = abrirpanel;
        vm.abrirTooltip = abrirTooltip;
        vm.activarexaminar = false;
        vm.activarguardar = false;
        vm.onChangeTipoArchivo = onChangeTipoArchivo;


        ////#region Metodos

        $scope.$watch('vm.tramiteid', function () {
            var idrolpresupuesto = constantesBackbone.idRPresupuesto;
            var cumpleRol = $sessionStorage.usuario.roles.filter(x => x.Nombre.includes("Presupuesto") == true);
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {

                //if (cumpleRol.length > 0) {
                archivoServicios.obtenerTipoDocumentoTramitePorRol(vm.tipotramiteid, idrolpresupuesto, vm.tramiteid, null).then(function (resultado) {
                        vm.listaTipoArchivos = resultado.data;
                        cargaTipoArchivosObligatorios(vm.listaTipoArchivos);
                    }).then(function () {
                        consultarArchivosTramite();
                    });
                //}
                //else {
                //    archivoServicios.obtenerTipoDocumentoTramite(vm.tipotramiteid).then(function (resultado) {
                //        vm.listaTipoArchivos = resultado.data;
                //        cargaTipoArchivosObligatorios(vm.listaTipoArchivos);
                //    }).then(function () {
                //        consultarArchivosTramite();
                //    });
                //}

            }
        });



        function initCargarArchivos() {
            
           
            
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
        }

        function cargaTipoArchivosObligatorios(lista) {
            lista.map(function (item) {
                if (item.Obligatorio)
                    vm.listadocumentosObligatorios.push(item);
            });
        }
        vm.rotated = false;
        function abrirpanel() {

            var acc = document.getElementById('divcargaarchivo');
            var i;
            var rotated = false;


            acc.classList.toggle("active");
            var panel = acc.nextElementSibling;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + "px";
            }
            var div = document.getElementById('u4_imgcargaarchivo'),
                deg = vm.rotated ? 180 : 0;
            div.style.webkitTransform = 'rotate(' + deg + 'deg)';
            div.style.mozTransform = 'rotate(' + deg + 'deg)';
            div.style.msTransform = 'rotate(' + deg + 'deg)';
            div.style.oTransform = 'rotate(' + deg + 'deg)';
            div.style.transform = 'rotate(' + deg + 'deg)';
            vm.rotated = !vm.rotated;

            ObtenerSeccionCapitulo();
        }

        function renderPanel() {
            var acc = document.getElementById('divcargaarchivo');
            var panel = acc.nextElementSibling;
            panel.style.maxHeight = panel.scrollHeight + "px";

        }

        function abrirTooltip() {
            utilidades.mensajeInformacionV('Esta es la explicación de la carga de archivos... un hecho establecido hace '
                + 'demasiado tiempo que un lector se distraerá con el contenido del texto de '
                + 'un sitio mientras que mira su diseño.El punto de usar Lorem Ipsum es que '
                + 'tiene una distribución más o menos normal de las letras, al contrario de '
                + 'usar textos como por ejemplo "Contenido aquí, contenido aquí".', false, "Carga de archivos")
        }

        //#region Metodos

        function onChangeTipoArchivo(value) {
            if (vm.idTipoArchivoSeleccionado != '0')
                vm.activarexaminar = true;
            else {
                vm.activarexaminar = false;
                vm.activarguardar = false;
            }
        }

        function consultarArchivosTramite() {
            vm.infoArchivo = {
                coleccion: "tramites", ext: ".pdf", codigoProceso: $sessionStorage.numeroTramite, descripcionTramite: $sessionStorage.descripcionTramite, idInstancia: $sessionStorage.idInstancia,
                idAccion: vm.idAccionParam, section: "requerimientosTramite", idTipoTramite: vm.tipotramiteid
            };

            let param = {
                idInstancia: vm.infoArchivo.idInstancia,
                section: vm.infoArchivo.section,
                idAccion: vm.infoArchivo.idAccion
            };
            vm.listaArchivos = [];

            archivoServicios.obtenerListadoArchivos(param, vm.infoArchivo.coleccion).then(function (response) {
                vm.totalRegistrosObligatorio = [];
                vm.totalRegistros = 0;
                $sessionStorage.TieneCDP = false;
                if (response === undefined || typeof response === 'string') {
                    vm.tieneArchivosAdjuntos = false;
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    response.forEach(archivo => {
                        if (archivo.status !== 'Eliminado') {
                            vm.listaArchivos.push({
                                codigoProceso: archivo.metadatos.codigoproceso,
                                descripcionTramite: archivo.metadatos.descripciontramite,
                                fecha: moment(archivo.fecha).format("DD/MM/YYYY"),
                                nombreArchivo: archivo.nombre,
                                tipoDocumento: archivo.metadatos.tipodocumento === undefined ? '' : archivo.metadatos.tipodocumento,
                                tipoDocumentoId: archivo.metadatos.tipodocumentoid === undefined ? '' : archivo.metadatos.tipodocumentoid,
                                idArchivoBlob: archivo.metadatos.idarchivoblob,
                                ContenType: archivo.metadatos.contenttype,
                                idMongo: archivo.id
                            });
                            //Esta valiable se necesita para las validaciones de los documentos
                            if (archivo.metadatos.tipodocumento === "CDP")
                                $sessionStorage.TieneCDP = true;
                            if (vm.listaArchivos.length > 0) {
                                vm.titulogrilla = vm.tituloconregistros;
                                vm.totalRegistros++;
                            }
                            else {
                                vm.titulogrilla = vm.titulosinregistros;
                                vm.totalRegistros = 0;
                            }
                            var td = vm.listadocumentosObligatorios.filter(x => x.TipoDocumentoId === archivo.metadatos.tipodocumentoid);
                            if (td.length > 0 &&  td[0].Obligatorio) {
                                var archivoobligatorio = vm.totalRegistrosObligatorio.filter(x => x.TipoDocumentoId === archivo.metadatos.tipodocumentoid);
                                if (archivoobligatorio.length === 0) {
                                    vm.totalRegistrosObligatorio.push(td[0]);
                                }
                                if (vm.totalRegistrosObligatorio.length === vm.listadocumentosObligatorios.length) {
                                    $sessionStorage.sessionDocumentos = 100;
                                }
                                else {
                                    $sessionStorage.sessionDocumentos = (vm.totalRegistrosObligatorio.length * 100) / vm.listadocumentosObligatorios.length;
                                    if (!$sessionStorage.sessionDocumentos === 0) guardarCapituloModificado(0);/// Aqui Guarda

                                }
                            }

                        }

                    });
                    if (response.filter(p => p.status !== 'Eliminado').length === 0 ) {
                        eliminarCapitulosModificados();/// Aqui borrar
                    }
                }
            });


            // vm.contObligatoriosNoIngresados = 0;
            // $sessionStorage.contObligatoriosNoIngresados = vm.contObligatoriosNoIngresados;
            //obtenerTiposDocumentos()
            //    .then(function (response) {
            //        if (response.data !== null && response.data.length > 0) {
            //            response.data.forEach(tipoArchivo => {
            //                //vm.listTipoArchivo.push({
            //                //    Id: tipoArchivo.Id,
            //                //    Name: tipoArchivo.TipoDocumento
            //                //});                                    
            //                if (tipoArchivo.Obligatorio === true) {
            //                    vm.tieneArchivosAdjuntos = false;
            //                    vm.archivosLoad.forEach(archivo => {
            //                        if (archivo.tipoDocumentoSoporte === tipoArchivo.TipoDocumento) {
            //                            vm.tieneArchivosAdjuntos = true;
            //                        }
            //                    });

            //                    if (vm.tieneArchivosAdjuntos === false) {
            //                        vm.contObligatoriosNoIngresados = vm.contObligatoriosNoIngresados + 1;
            //                    }
            //                }
            //            });
            //        }
            //        $sessionStorage.contObligatoriosNoIngresados = vm.contObligatoriosNoIngresados;
            //        localStorage.setItem('contObligatoriosNoIngresados', $sessionStorage.contObligatoriosNoIngresados);
            //    });
            //    }
            //}, error => {
            //    console.log(error);
            //});

        }

        vm.obtener = function () {
            var idTramite = $sessionStorage.tramiteId;
            var proyectoId = $sessionStorage.ProyectoId;
            var tipoTramiteId = vm.tipotramiteid;
            var tipoRolId = $sessionStorage.TipoRolId;
            var TipoProyecto = $sessionStorage.TipoProyecto;

            consultarArchivosTramite();

        };


        /*region archivos     */
        function adjuntarArchivo() {
            document.getElementById('file').value = "";
            document.getElementById('file').click();
        }

        $scope.fileNameChanged = function (input) {
            vm.archivo = undefined;
            if (vm.idTipoArchivoSeleccionado == "" || vm.idTipoArchivoSeleccionado == null || vm.idTipoArchivoSeleccionado == undefined) {
                utilidades.mensajeError("Debe seleccionar un tipo de archivo.");
                return;
            }

            if (input.files.length == 1)
                vm.filename = input.files[0].name;
            else
                vm.filename = input.files.length + " archivos"
            vm.archivo = input;
            if (vm.archivo != undefined)
                vm.activarguardar = true;
        }

        function validarExtension(extension) {
            switch (extension.toLowerCase()) {
                case 'pdf': case 'zip': case 'rar': return true;
                default: return false;
            }
        }

        function obtenerExtension(nombreArchivo) {
            let partes = nombreArchivo.split('.');
            return partes[partes.length - 1];
        }

        function onClickCargar() {
            if (vm.idTipoArchivoSeleccionado == null || vm.idTipoArchivoSeleccionado == undefined || vm.idTipoArchivoSeleccionado == "" || vm.idTipoArchivoSeleccionado == "0") {
                utilidades.mensajeError("Debe seleccionar un tipo de archivo.");
            }
            else if (vm.archivo === undefined) {
                swal('', "No ha seleccionado un archivo para cargar.", 'error');
            }
            else {
                var input = vm.archivo;
                for (var i = 0; i < input.files.length; i++) {
                    vm.extension = obtenerExtension(input.files[i].name);
                    if (!validarExtension(vm.extension)) {
                        swal('', "Extensión no permitida.", 'error');
                        return;
                    }
                }
                vm.listaArchivos.push({
                    codigoProceso: '',
                    descripcionTramite: '',
                    fecha: '',
                    nombreArchivo: '',
                    tipoDocumento: '',
                    tipoDocumentoId: '',
                    idArchivoBlob: '',
                    ContenType: '',
                    idMongo: ''
                });
                let tipoArchivo = vm.listaTipoArchivos.find(x => x.Id == vm.idTipoArchivoSeleccionado);
                for (var i = 0; i < input.files.length; i++) {
                    vm.extension = obtenerExtension(input.files[i].name);
                    let archivo = {
                        FormFile: input.files[i],
                        Nombre: input.files[i].name,
                        Metadatos: {
                            extension: vm.extension,
                            idInstancia: vm.modelo.idInstancia,
                            idAccion: vm.modelo.idAccion,
                            section: vm.modelo.section,
                            codigoProceso: vm.modelo.codigoProceso,
                            descripcionTramite: vm.modelo.descripcionTramite,
                            tipoDocumento: tipoArchivo.TipoDocumento,
                            tipoDocumentoId: tipoArchivo.TipoDocumentoId
                        }
                    };

                    archivoServicios.cargarArchivo(archivo, vm.modelo.coleccion).then(function (response) {
                        if (response === undefined || typeof response === 'string') {
                            vm.mensajeError = response;
                            utilidades.mensajeError(response);
                        } else {
                            vm.archivo = undefined;
                            vm.filename = undefined;
                            vm.activarguardar = false;
                            utilidades.mensajeSuccess(
                                "Se han adicionado líneas de información en la parte inferior de la tabla 'Carga de Archivos'",
                                false,
                                function funcionContinuar() {
                                    $timeout(function () {
                                        consultarArchivosTramite();
                                    }).then(function () { renderPanel(); });
                                },
                                false,
                                "El documento ha sido cargado y guardado con éxito.");
                            guardarCapituloModificado();/// Aqui Guarda
                            vm.guardar = 1;
                        }
                    }, error => {
                        console.log(error);
                    });
                }
            }

        };

        function onClickCancelar() {
            vm.archivo = undefined;
            vm.filename = undefined;
            vm.activarguardar = false;
        }

        function eliminarArchivoBlob(entity) {
            utilidades.mensajeWarning(
                "¿Está seguro de continuar?",
                function funcionContinuar() {

                    archivoServicios.cambiarEstadoDataArchivo(entity.idMongo, vm.modelo.coleccion).then(function () {
                        consultarArchivosTramite();
                    })
                        .then(function () {
                            $timeout(function () {
                                swal({
                                    title: "<span style='color:#069169'>Los datos fueron eliminados correctamente.<span>",
                                    text: "<div style='padding-bottom:2rem !important;'></div><button type='button' class='swal2-close' aria-label='Close this dialog' style='display: flex;'>×</button>",
                                    type: "success",
                                    showCancelButton: false,
                                    confirmButtonText: "Aceptar",
                                    cancelButtonText: "Cancelar",
                                    closeOnConfirm: true,
                                    closeOnCancel: true,
                                    customClass: 'sweet-alertTittleSucces',
                                    html: true
                                });

                            }, 50);
                        });

                },
                function funcionCancelar(reason) {
                    console.log("reason", reason);
                },
                "Aceptar",
                "Cancelar",
                "Los datos seleccionado serán eliminados");


        }

        //function cargadespuesEliminar() {
        //    //var deferred = $q.defer();
        //    //try {
        //    //    utilidades.mensajeSuccess(
        //    //        "",
        //    //        false,
        //    //        function funcionContinuar() {
        //                archivoServicios.cambiarEstadoDataArchivo(entity.idMongo, vm.modelo.coleccion).then(function () {
        //                    consultarArchivosTramite();
        //                });
        //    //        },
        //    //        false,
        //    //        "Los datos fueron eliminados correctamente.");
        //    //    deferred.resolve("done");
        //    //} catch (err) {

        //    //    deferred.reject(err);
        //    //}
        //    //return deferred.promise;
        //}

        function descargarArchivoBlob(entity) {
            archivoServicios.obtenerArchivoBytes(entity.idArchivoBlob, vm.modelo.coleccion).then(function (retorno) {
                const blob = utilidades.base64toBlob(retorno, entity.ContenType);
                //var blob = new Blob([retorno], {
                //    type: entity.ContenType
                //});
                var downloadUrl = URL.createObjectURL(blob);
                var a = document.createElement("a");
                a.href = downloadUrl;
                a.download = entity.nombreArchivo;
                document.body.appendChild(a);
                a.click();
            }, function (error) {
                utilidades.mensajeError("Error inesperado al descargar");
            });
        }



        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }
        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,

            }
            justificacionCambiosServicio.eliminarCapitulosModificados(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }
        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-documentossoportealojararchivo');
            vm.seccionCapitulo = span.textContent;


        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.limpiarErrores = function (errores) {

            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-archivo-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-CDP-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            
        }

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores(errores);
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == null ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

               

            }
        }



        vm.validarValoresVigenciaInformacionArchivo = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-archivo-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }



        vm.validarValoresCDP = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-CDP-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.errores = {
            'VFO006': vm.validarValoresVigenciaInformacionArchivo,
            'VFO0066': vm.validarValoresCDP,

        }

        /* ------------------------ FIN Validaciones ---------------------------------*/


    }

    angular.module('backbone').component('cargaArchivosvf', {
        restrict: 'E',
        transclude: true,
        scope: {
            modelo: '='
        },
        templateUrl: "src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/documentoSoporte/cargaArchivo/cargaArchivosvf.html",
        controller: cargaArchivosvfController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
            tramiteid: '@',
            tipotramiteid: '@',
            listaArchivos:'@'
        }
    });

})();