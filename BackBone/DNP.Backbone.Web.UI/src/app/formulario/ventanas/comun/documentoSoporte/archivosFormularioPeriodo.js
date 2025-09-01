(function () {
    'use strict';

    archivosFormularioPeriodo.$inject = [
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
        'trasladosServicio',
        'comunesServicio'
    ];

    function archivosFormularioPeriodo(
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
        trasladosServicio,
        comunesServicio
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
        vm.tipotramiteid = $sessionStorage.tipoTramiteId;
        vm.tramiteid = $sessionStorage.tipoTramiteId;
        vm.tituloconregistros = 'Archivos cargados';
        vm.titulogrilla = vm.titulosinregistros = 'Aún no se han agregado archivos al paso actual';
        vm.totalRegistros = 0;
        vm.listadocumentosObligatorios = [];
        vm.descripcion = '';
        vm.archivoObigatorioscargados = false;
        vm.TipoDocumentosRolFase = [];
        vm.listaVigencias = [];
        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;
        vm.ConsultarTamanioMaxDocs = null;

        //variables locales
        vm.listaTipoArchivos = [];

        vm.idTipoArchivoSeleccionado = '0';
        vm.idVigenciaSeleccionado = '0';

        vm.modelo = {
            coleccion: "tramites", ext: ".pdf", codigoProceso: $sessionStorage.numeroTramite, descripcionTramite: $sessionStorage.descripcionTramite, idInstancia: $sessionStorage.idInstancia,
            idAccion: $sessionStorage.idAccion, section: vm.section, idTipoTramite: vm.tipotramiteid
        };
        vm.archivo = undefined;
        //region declarar metodos
        vm.initCargarArchivos = initCargarArchivos;
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.onClickCancelar = onClickCancelar;
        vm.onClickCargar = onClickCargar;
        vm.abrirTooltip = abrirTooltip;
        vm.activarexaminar = false;
        vm.desactivarGuardar = true;
        vm.onChangeTipoArchivo = onChangeTipoArchivo;
        vm.nombreArchivo = undefined;
        vm.listaArchivos = [];
        vm.abrirpanel = abrirpanel;

        ////#region Metodos

        $scope.$watch('vm.tramiteid', function () {
            if ((vm.tramiteid !== '' && vm.tramiteid !== undefined) && vm.tramiteid !== null && vm.tramiteid !== '0') {
                consultarTipoDocumento();
                //Este codigo lo adiciono para poder controlar los archivos que se adjuntas para justificacion
                if (vm.nivelarchivo != undefined) vm.nivel = vm.nivelarchivo;
                if (vm.tipoarchivo != undefined) vm.modelo.coleccion = vm.tipoarchivo;
            }
        });

        $scope.$watch('vm.rol', function () {
            if ((vm.rol !== '' && vm.rol !== undefined) && vm.rol !== null) {
                consultarTipoDocumento();
            }
        });

        $scope.$watch('vm.objetonegocioid', function () {
            if ((vm.objetonegocioid !== '' && vm.objetonegocioid !== undefined) && vm.objetonegocioid !== null && vm.objetonegocioid !== '0') {
                consultarTipoDocumento();
            }
        });

        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
                //ObtenerSeccionCapitulo();
            }
        });

        function consultarTipoDocumento() {
            var idTramite = $sessionStorage.tipoTramiteId;
            var tipoTramiteId = $sessionStorage.tipoTramiteId;



            if ((vm.rol === undefined || vm.rol === "") || vm.rol == '00000000-0000-0000-0000-000000000000') {
                vm.rol = '00000000-0000-0000-0000-000000000000';

                var roles = sesionServicios.obtenerUsuarioIdsRoles();

                archivoServicios.obtenerTipoDocumentoTramitePorRol(tipoTramiteId, vm.rol, idTramite, 55).then(function (resultado) {
                    resultado.data.map(function (item) {
                        roles.map(function (item2) {
                            if (item.RolId === item2 && item.faseId === 55) {
                                vm.TipoDocumentosRolFase.push(item);
                            }
                        });
                    });

                    vm.listaTipoArchivos = vm.TipoDocumentosRolFase;
                    cargaTipoArchivosObligatorios(vm.listaTipoArchivos);
                    consultarArchivosTramite();

                });
            } else {
                if (vm.idtipotramitepresupuestal !== undefined && vm.idtipotramitepresupuestal !== "") vm.tipotramiteid = vm.idtipotramitepresupuestal;
                archivoServicios.obtenerTipoDocumentoTramitePorRol(vm.tipotramiteid, vm.rol, vm.tramiteid, null).then(function (resultado) {

                    vm.listaTipoArchivos = resultado.data;
                    cargaTipoArchivosObligatorios(vm.listaTipoArchivos);
                    consultarArchivosTramite();

                });
            }
        }

        function initCargarArchivos() {
            vm.archivo = undefined;
            $sessionStorage.sessionDocumentos = 0;
            comunesServicio.obtenerCalendarioPeriodo(undefined).then(
                function (rta) {
                    if (rta.data != null) {
                        rta.data.map(function (item, index) {

                            var mm = item.Mes.substring(0, 3);
                            if (index === 0) {
                                var yy = item.FechaDesde.substring(0, 4);
                                var vigencia = {
                                    Id: index + 1,
                                    Descripcion: yy + ' - ' + mm
                                }
                                vm.listaVigencias.push(vigencia);
                            }
                            else {
                                var yy = item.FechaHasta.substring(0, 4);
                                var vigencia = {
                                    Id: index + 1,
                                    Descripcion: yy + ' - ' + mm
                                }
                                vm.listaVigencias.push(vigencia);
                            }


                        });
                    }

                }
            );
            consultarArchivosTramite();
            ConsultarTamanioMaxDocs('TamanioMaxDocs', '|');
        }

        function cargaTipoArchivosObligatorios(lista) {
            vm.listadocumentosObligatorios = [];
            lista.map(function (item) {
                if (item.Obligatorio)
                    vm.listadocumentosObligatorios.push(item);
                vm.archivoObigatorioscargados = true;
            });

            if (vm.nombrecomponentepaso == 'selecionarvigenciafuturaautorizacionminhacienda') {
                vm.listaTipoArchivos = vm.listaTipoArchivos.filter((item) => item.TipoDocumento != 'Oficio de valores utilizados por la entidad');
            }

            if (vm.nombrecomponentepaso == 'valoresutilizados') {
                vm.listaTipoArchivos = vm.listaTipoArchivos.filter((item) => item.TipoDocumento == 'Oficio de valores utilizados por la entidad');
            }
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
                vm.desactivarGuardar = false;
            }
        }

        function consultarArchivosTramite() {
            let param
            if (vm.nivel === vm.nivelarchivo) // Cuando ingresa por esta opción es porque va a mostrar los arvhivos del paso anterior
                param = {
                    idInstancia: vm.modelo.idInstancia,
                    idNivel: vm.nivel,
                };
            else {
                param = {
                    idInstancia: vm.modelo.idInstancia,
                    section: vm.modelo.section,
                    idAccion: vm.modelo.idAccion,
                    idNivel: vm.nivel,
                    // idRol: vm.rol,
                };
            }
            if (vm.rol === '')
                param = {
                    idInstancia: vm.modelo.idInstancia
                };

            if (vm.section != null && vm.section != undefined && vm.section != '') {
                param.section = vm.section;
            }

            vm.listaArchivos = [];
            $sessionStorage.sessionDocumentos = 0;

            archivoServicios.obtenerListadoArchivos(param, vm.modelo.coleccion).then(function (response) {
                vm.totalRegistrosObligatorio = [];
                vm.totalRegistros = 0;
                if (response === undefined || typeof response === 'string') {
                    vm.tieneArchivosAdjuntos = false;
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    response.forEach(archivo => {
                        if (archivo.status !== 'Eliminado') {
                            if (archivo.nombre.length > 60) {
                                var descripcionTmp = archivo.nombre.split(" ");
                                archivo.nombre = '';
                                descripcionTmp.map(function (item) {
                                    var ctaTmp = 0;
                                    if (item.length > 60) {
                                        ctaTmp = Math.floor(item.length / 60);
                                        for (var i = 0; i < ctaTmp; i++) {
                                            archivo.nombre += item.substring(60 * i, 60) + " ";
                                        }

                                    }
                                    if ((item.length % 50) > 0) {
                                        archivo.nombre += item.substring(60 * ctaTmp, (60 * ctaTmp) + 60) + " ";
                                    }
                                })
                                descripcionTmp = archivo.metadatos.descripcion.split(" ");
                                archivo.metadatos.descripcion = '';
                                descripcionTmp.map(function (item) {
                                    var ctaTmp = 0;
                                    if (item.length > 60) {
                                        ctaTmp = Math.floor(item.length / 60);
                                        for (var i = 0; i < ctaTmp; i++) {
                                            archivo.metadatos.descripcion += item.substring(60 * i, 60) + " ";
                                        }

                                    }
                                    if ((item.length % 50) > 0) {
                                        archivo.metadatos.descripcion += item.substring(60 * ctaTmp, (60 * ctaTmp) + 60) + " ";
                                    }
                                })
                            };
                            var mostrarEliminarVigencia = false;
                            var indexVigencia = vm.listaVigencias.findIndex(x => x.Descripcion === archivo.metadatos.descripcionperiodo);
                            if (indexVigencia >= 0) {
                                mostrarEliminarVigencia = true;
                            }
                            vm.listaArchivos.push({
                                codigoProceso: archivo.metadatos.codigoproceso,
                                descripcion: archivo.metadatos.descripcion,
                                fecha: moment(archivo.fecha).format("DD/MM/YYYY"),
                                nombreArchivo: archivo.nombre,
                                tipoDocumento: archivo.metadatos.tipodocumento === undefined ? '' : archivo.metadatos.tipodocumento,
                                tipoDocumentoId: archivo.metadatos.tipodocumentoid === undefined ? '' : archivo.metadatos.tipodocumentoid,
                                datosDocumento: archivo.metadatos.datosdocumento,
                                NombreDocumento: archivo.nombre,
                                idArchivoBlob: archivo.metadatos.idarchivoblob,
                                ContenType: archivo.metadatos.contenttype,
                                idMongo: archivo.id,
                                nombre: archivo.nombre,
                                descripcionperiodo: archivo.metadatos.descripcionperiodo,
                                mostrarEliminar: mostrarEliminarVigencia
                            });
                            if (vm.listaArchivos.length > 0) {
                                vm.titulogrilla = vm.tituloconregistros;
                                vm.totalRegistros++;
                            }
                            else {
                                vm.titulogrilla = vm.titulosinregistros;
                                vm.totalRegistros = 0;
                            }
                            var td = vm.listadocumentosObligatorios.filter(x => x.TipoDocumentoId === archivo.metadatos.tipodocumentoid);
                            if (td.length > 0 && td[0].Obligatorio) {
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
                    if (response.filter(p => p.status !== 'Eliminado').length === 0 && vm.guardar === 1) {
                        eliminarCapitulosModificados();/// Aqui borrar
                    }

                    if ($sessionStorage.sessionDocumentos == 0) {
                        $sessionStorage.vigenciaHorizonte = 0;
                        $sessionStorage.DescripcionAccionNivel = 0;
                    }

                    if ($sessionStorage.sessionDocumentos == 0 && vm.nombrecomponentepaso == 'selecionarvigenciafuturaautorizacionminhacienda') {
                        vm.listaArchivos = vm.listaArchivos.filter((item) => item.tipoDocumento != 'Oficio de valores utilizados por la entidad');
                        if (vm.listaArchivos.length > 0) {
                            //$sessionStorage.sessionDocumentos = 100;
                            $sessionStorage.vigenciaHorizonte = 1;
                        }
                    }

                    if ($sessionStorage.sessionDocumentos == 50 && vm.nombrecomponentepaso == 'selecionarvigenciafuturaautorizacionminhacienda') {
                        vm.listaArchivos = vm.listaArchivos.filter((item) => item.tipoDocumento != 'Oficio de valores utilizados por la entidad');
                        if (vm.listaArchivos.length > 0) {
                            //$sessionStorage.sessionDocumentos = 100;
                            $sessionStorage.vigenciaHorizonte = 1;
                        }
                    }

                    if ($sessionStorage.sessionDocumentos == 0 && vm.nombrecomponentepaso == 'valoresutilizados') {
                        vm.listaArchivos = vm.listaArchivos.filter((item) => item.tipoDocumento == 'Oficio de valores utilizados por la entidad');
                        if (vm.listaArchivos.length > 0) {
                            //$sessionStorage.sessionDocumentos = 100;
                            $sessionStorage.DescripcionAccionNivel = 1;
                        }
                    }

                    if ($sessionStorage.sessionDocumentos == 50 && vm.nombrecomponentepaso == 'valoresutilizados') {
                        vm.listaArchivos = vm.listaArchivos.filter((item) => item.tipoDocumento == 'Oficio de valores utilizados por la entidad');
                        if (vm.listaArchivos.length > 0) {
                            $sessionStorage.sessionDocumentos = 100;
                            $sessionStorage.DescripcionAccionNivel = 1;
                        }
                    }
                }
            });




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
            var idSpanArrow = 'file-' + vm.section;
            document.getElementById(idSpanArrow).value = "";
            document.getElementById(idSpanArrow).click();
        }

        $scope.fileNameChanged = function (input) {
            ocultarMensajeError();
            vm.archivo = undefined;
            //if (vm.idTipoArchivoSeleccionado == "0" || vm.idTipoArchivoSeleccionado == "" || vm.idTipoArchivoSeleccionado == null || vm.idTipoArchivoSeleccionado == undefined) {
            //    utilidades.mensajeError("Debe seleccionar un tipo de archivo.");
            //    return;
            //}
            const tamaniomax = vm.ConsultarTamanioMaxDocs * 1024 * 1024;
            if (input.files.length == 1)
                vm.nombreArchivo = input.files[0].name;
            else
                vm.nombreArchivo = input.files.length + " archivos"
            vm.filename = vm.nombreArchivo;
            vm.archivo = input;
            if (vm.archivo != undefined)
                vm.desactivarGuardar = false;
            for (var i = 0; i < input.files.length; i++) {
                vm.extension = obtenerExtension(input.files[i].name);
                if (!validarExtension(vm.extension)) {
                    mostrarMensajeError("El archivo debe tener extensión zip, rar, jpg, jpeg, mp4 o pdf", "archivo", 1);
                    onClickCancelar();
                    return;
                }
                else if (input.files[i].size > tamaniomax) {
                    mostrarMensajeError("El archivo supera el peso de " + vm.ConsultarTamanioMaxDocs + " megas", "archivo", 1);
                    onClickCancelar();
                    return;
                }

            }
        }
        $scope.ChangeArchivoSet = function () {
            if (vm.nombreArchivo == "") {
                //vm.desactivarGuardar = true;
            }
        };

        function validarExtension(extension) {
            switch (extension.toLowerCase()) {
                case 'pdf':
                case 'jpg': case 'jpeg': case 'mp4': case 'zip': case 'rar':
                    return true;
                default: return false;
            }
        }

        function obtenerExtension(nombreArchivo) {
            let partes = nombreArchivo.split('.');
            return partes[partes.length - 1];
        }

        function onClickCargar() {
            ocultarMensajeError();
            var errorp = false;
            if (vm.idTipoArchivoSeleccionado == null || vm.idTipoArchivoSeleccionado == undefined || vm.idTipoArchivoSeleccionado == "" || vm.idTipoArchivoSeleccionado == "0") {
                errorp = true;
                mostrarMensajeError("Campo obligatorio", "tipoArchivo", 1);
            }
            if (vm.archivo === undefined) {
                errorp = true;
                mostrarMensajeError("Campo obligatorio", "archivo", 1);
            }
            if (vm.descripcion == "" || vm.descripcion == null || vm.descripcion == undefined) {
                errorp = true;
                mostrarMensajeError("Campo obligatorio", "descripcion", 1);
            }
            if (vm.idVigenciaSeleccionado == null || vm.idVigenciaSeleccionado == undefined || vm.idVigenciaSeleccionado == "" || vm.idVigenciaSeleccionado == "0") {
                errorp = true;
                mostrarMensajeError("Campo obligatorio", "vigencia", 1);
            }
            //if (errorp)
            //    swal("Los datos presentan incosistencias.","Verifique los campos señalados en la tabla e intente nuevamente.",  "error");
            if (!errorp) {
                //ocultarMensajeError();
                var input = vm.archivo;

                vm.listaArchivos.push({
                    codigoProceso: '',
                    descripcionTramite: '',
                    fecha: '',
                    nombreArchivo: '',
                    tipoDocumento: '',
                    tipoDocumentoId: '',
                    idArchivoBlob: '',
                    ContenType: '',
                    idMongo: '',
                    descripcionperiodo: ''
                });
                let tipoArchivo = vm.listaTipoArchivos.find(x => x.Id == vm.idTipoArchivoSeleccionado);
                let vigenciaPeiodo = vm.listaVigencias.find(x => x.Id == vm.idVigenciaSeleccionado);

                trasladosServicio.obtenerDatosUsuario($sessionStorage.usuario.permisos.IdUsuarioDNP, $sessionStorage.idEntidad,
                    $sessionStorage.idAccion, $sessionStorage.idInstancia).
                    then(function (respuesta) {
                        var rol = $sessionStorage.usuario.roles.find(x => x.IdRol === vm.rol);
                        var usuariotemporal = {};
                        if (respuesta.data !== null)
                            var usuariotemporal = respuesta.data.find(x => x.RolId === vm.rol);

                        var Cuenta = '';
                        var Entidad = '';
                        var NombreUsuario = '';
                        if (usuariotemporal) {
                            Cuenta = usuariotemporal.Cuenta === null ? '' : usuariotemporal.Cuenta;
                            Entidad = usuariotemporal.Entidad === null ? '' : usuariotemporal.Entidad;
                            NombreUsuario = usuariotemporal.NombreUsuario === null ? '' : usuariotemporal.NombreUsuario;
                        }
                        var dato = obtenerFechaSinHoras(new Date()) + '. ' + rol.Nombre + ' ' +
                            NombreUsuario + ' ' + Cuenta + ' ' + Entidad;

                        if (vm.modelo.section === null || vm.modelo.section === undefined) {
                            vm.modelo.section = vm.section;
                        }

                        for (var i = 0; i < input.files.length; i++) {
                            vm.extension = obtenerExtension(input.files[i].name);
                            let archivo = {
                                FormFile: input.files[i],
                                Nombre: input.files[i].name.trimEnd(),
                                Metadatos: {
                                    extension: vm.extension,
                                    idInstancia: vm.modelo.idInstancia,
                                    idAccion: vm.modelo.idAccion,
                                    section: vm.modelo.section,
                                    codigoProceso: vm.modelo.codigoProceso,
                                    descripcionTramite: vm.modelo.descripcionTramite,
                                    tipoDocumento: tipoArchivo.TipoDocumento,
                                    tipoDocumentoId: tipoArchivo.TipoDocumentoId,
                                    idNivel: vm.nivel,
                                    idRol: vm.rol,
                                    idAccion: $sessionStorage.idAccion,
                                    descripcion: vm.descripcion,
                                    datosdocumento: dato,
                                    tipoDocumentoSoporte: tipoArchivo.TipoDocumento,
                                    descripcionperiodo: vigenciaPeiodo.Descripcion
                                }
                            };

                            archivoServicios.cargarArchivo(archivo, vm.modelo.coleccion).then(function (response) {
                                if (response === undefined || typeof response === 'string') {
                                    vm.mensajeError = response;
                                    utilidades.mensajeError(response);
                                } else {
                                    vm.desactivarGuardar = false;
                                    utilidades.mensajeSuccess(
                                        '',
                                        false,
                                        function funcionContinuar() {
                                            $timeout(function () {
                                                vm.archivo = undefined;
                                                vm.nombreArchivo = undefined;
                                                vm.descripcion = "";
                                                vm.idTipoArchivoSeleccionado = "";
                                                vm.idVigenciaSeleccionado = "";
                                                consultarArchivosTramite();
                                            }).then(function () { renderPanel(); });
                                        },
                                        false,
                                        "El documento se ha agregado con éxito a la tabla.");
                                    guardarCapituloModificado();/// Aqui Guarda
                                    vm.guardar = 1;
                                }
                            }, error => {
                                console.log(error);
                            });
                        }

                    });





            }

        };

        function onClickCancelar() {
            vm.archivo = undefined;
            vm.nombreArchivo = undefined;
            vm.desactivarGuardar = true;
        }

        function eliminarArchivoBlob(entity) {
            utilidades.mensajeWarning(
                "¿Está seguro de continuar?",
                function funcionContinuar() {

                    archivoServicios.cambiarEstadoDataArchivo(entity.idMongo, vm.modelo.coleccion).then(function () {
                        consultarArchivosTramite();
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });

                    })
                        .then(function () {
                            $timeout(function () {
                                swal({
                                    title: "<span style='color:#069169'>El documento ha sido eliminado con éxito.<span>",
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
                "El documento será eliminado.");


        }

        function ocultarMensajeError() {
            var idSpanAlertComponent = document.getElementById("soporte-error-" + vm.nombreComponente);
            var idSpanAlertComponent1 = document.getElementById("soporte1-error-" + vm.nombreComponente);
            var idSpanAlertComponent2 = document.getElementById("tipoarchivo-error-" + vm.nombreComponente);
            var idSpanAlertComponent3 = document.getElementById("descripcion-error-" + vm.nombreComponente);
            var idSpanAlertComponent4 = document.getElementById("vigencia-error-" + vm.nombreComponente);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.remove("ico-advertencia");
                idSpanAlertComponent.innerHTML = "<span>Campo obligatorio</span>";
                idSpanAlertComponent.classList.add('hidden');
            }
            if (idSpanAlertComponent1 != undefined) {
                idSpanAlertComponent1.classList.add('hidden');
            }

            if (idSpanAlertComponent2 != undefined) {
                idSpanAlertComponent2.classList.remove("ico-advertencia");
                idSpanAlertComponent2.innerHTML = "<span>Campo obligatorio</span>";
                idSpanAlertComponent2.classList.add('hidden');
            }
            if (idSpanAlertComponent3 != undefined) {
                idSpanAlertComponent3.classList.remove("ico-advertencia");
                idSpanAlertComponent3.innerHTML = "<span>Campo obligatorio</span>";
                idSpanAlertComponent3.classList.add('hidden');
            }
            if (idSpanAlertComponent4 != undefined) {
                idSpanAlertComponent4.classList.remove("ico-advertencia");
                idSpanAlertComponent4.innerHTML = "<span>Campo obligatorio</span>";
                idSpanAlertComponent4.classList.add('hidden');
            }
        }

        function mostrarMensajeError(mensajeError, elemento, numeroCamposError) {
            if (elemento === 'archivo' || (elemento === undefined && numeroCamposError > 1)) {
                var idSpanAlertComponent = document.getElementById("soporte-error-" + vm.nombreComponente);
                var idSpanAlertComponent1 = document.getElementById("soporte1-error-" + vm.nombreComponente);
                if (idSpanAlertComponent != undefined) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                    idSpanAlertComponent.classList.remove('hidden');
                    idSpanAlertComponent.innerHTML = mensajeError;
                }
                if (idSpanAlertComponent1 != undefined) {
                    idSpanAlertComponent1.classList.remove('hidden');
                }
            }
            if (elemento === 'tipoArchivo' || (elemento === undefined && numeroCamposError > 1)) {
                var idSpanAlertComponent2 = document.getElementById("tipoarchivo-error-" + vm.nombreComponente);
                if (idSpanAlertComponent2 != undefined) {
                    idSpanAlertComponent2.classList.add("ico-advertencia");
                    idSpanAlertComponent2.classList.remove('hidden');
                    idSpanAlertComponent2.innerHTML = mensajeError;
                }
            }
            if (elemento === 'descripcion' || (elemento === undefined && numeroCamposError > 1)) {
                var idSpanAlertComponent3 = document.getElementById("descripcion-error-" + vm.nombreComponente);
                if (idSpanAlertComponent3 != undefined) {
                    idSpanAlertComponent3.classList.add("ico-advertencia");
                    idSpanAlertComponent3.classList.remove('hidden');
                    idSpanAlertComponent3.innerHTML = mensajeError;
                }
            }
            if (elemento === 'vigencia' || (elemento === undefined && numeroCamposError > 1)) {
                var idSpanAlertComponent4 = document.getElementById("vigencia-error-" + vm.nombreComponente);
                if (idSpanAlertComponent4 != undefined) {
                    idSpanAlertComponent4.classList.add("ico-advertencia");
                    idSpanAlertComponent4.classList.remove('hidden');
                    idSpanAlertComponent4.innerHTML = mensajeError;
                }//vigencia-error
            }
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

            //ObtenerSeccionCapitulo();
        }


        function descargarArchivoBlob(entity) {
            archivoServicios.obtenerArchivoBytes(entity.idArchivoBlob, vm.modelo.coleccion).then(function (retorno) {

                var binary = atob(retorno.replace(/\s/g, ''));
                var len = binary.length;
                var buffer = new ArrayBuffer(len);
                var view = new Uint8Array(buffer);
                for (var i = 0; i < len; i++) {
                    view[i] = binary.charCodeAt(i);
                }

                var blob = new Blob([view], {
                    type: entity.ContenType
                });
                var downloadUrl = URL.createObjectURL(blob);
                var a = document.createElement("a");
                a.href = downloadUrl;
                a.download = entity.nombreArchivo.trimEnd();
                document.body.appendChild(a);
                a.click();
            }, function (error) {
                utilidades.mensajeError("Error inesperado al descargar");
            });
        }



        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        function formatearFecha(fecha) {
            let fechaString = fecha.toISOString();
            return fechaString.substring(0, 19);
        }

        function obtenerFechaSinHoras(fecha) {
            var s = fecha.toLocaleString();
            return s;
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
            const span = document.getElementById('id-capitulo-' + vm.nombrecomponentepaso);
            vm.seccionCapitulo = span.textContent;


        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.limpiarErrores = function (errores) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.remove("ico-advertencia");
            }

            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-archivo-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
        }

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores(errores);
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var isValid = true;
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == null ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                        var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
                        if (idSpanAlertComponent != undefined) {
                            idSpanAlertComponent.classList.add("ico-advertencia");
                        }
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


        function ConsultarTamanioMaxDocs(VariableKey, Separador) {
            archivoServicios.ConsultarSystemConfiguracion(VariableKey, Separador).then(function (response) {
                vm.ConsultarTamanioMaxDocs = response.data.Valores[0].Valor;
            }, function (error) {
                utilidades.mensajeError("Error inesperado al descargar");
            });
        }


        vm.errores = {
            'VFO006': vm.validarValoresVigenciaInformacionArchivo
        }

        /* ------------------------ FIN Validaciones ---------------------------------*/


    }

    angular.module('backbone').component('archivosFormularioPeriodo', {
        restrict: 'E',
        transclude: true,
        scope: {
            modelo: '='
        },
        templateUrl: "src/app/formulario/ventanas/comun/documentoSoporte/archivosFormularioPeriodo.html",
        controller: archivosFormularioPeriodo,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            nivelarchivo: '@',
            tipoarchivo: '@',
            objetonegocioid: '@',
            nombrecomponentepaso: '@',
            idtipotramitepresupuestal: '@',

        }
    });

})();
