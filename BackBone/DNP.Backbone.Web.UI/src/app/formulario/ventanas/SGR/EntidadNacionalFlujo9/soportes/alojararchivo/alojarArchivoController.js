(function () {
    'use strict';

    alojarArchivoController.$inject = [
        '$scope',
        'servicioFichasProyectos',
        '$sessionStorage',
        'utilidades',
        'archivoServicios',
        '$timeout',
        'justificacionCambiosServicio',
        'trasladosServicio',
        'FileSaver',
        'transversalSgrServicio'
    ];

    function alojarArchivoController(
        $scope,
        servicioFichasProyectos,
        $sessionStorage,
        utilidades,
        archivoServicios,
        $timeout,
        justificacionCambiosServicio,
        trasladosServicio,
        FileSaver,
        transversalSgrServicio
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
        vm.IdInstancia = $sessionStorage.idInstancia;
        vm.IdAccion = $sessionStorage.idAccion;
        vm.tipotramiteid = $sessionStorage.tipoTramiteId;
        vm.tramiteid = $sessionStorage.TramiteId;

        vm.tituloconregistros = 'Archivos cargados';
        vm.titulogrilla = vm.titulosinregistros = 'Aún no se han agregado archivos al paso actual';
        vm.totalRegistros = 0;
        vm.listadocumentosObligatorios = [];
        vm.descripcion = '';
        vm.archivoObigatorioscargados = false;
        vm.TipoDocumentosRolFase = [];

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        //variables locales
        vm.listaTipoArchivos = [];
        vm.idTipoArchivoSeleccionado = '0';
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;

        vm.modelo = {
            coleccion: "tramites", ext: ".pdf", codigoProceso: $sessionStorage.numeroTramite, descripcionTramite: $sessionStorage.descripcionTramite, idInstancia: $sessionStorage.idInstancia,
            idAccion: $sessionStorage.idAccion, section: vm.section, idTipoTramite: vm.tipotramiteid
        };
        vm.archivo = undefined;

        vm.nombre = '';

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
        vm.configuracionReporte = {};

        vm.disabled = $sessionStorage.soloLectura;
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;

        ////#region Metodos

        $scope.$watch('vm.tramiteid', function () {
            if ((vm.tramiteid !== '' && vm.tramiteid !== undefined) && vm.tramiteid !== null) {
                consultarTipoDocumento();
                //Este codigo lo adiciono para poder controlar los archivos que se adjuntas para justificacion
                if (vm.nivelarchivo != undefined) vm.nivel = vm.nivelarchivo;
                if (vm.tipoarchivo != undefined) vm.modelo.coleccion = vm.tipoarchivo;
            }
        });

        $scope.$watch('vm.objetonegocioid', function () {
            if ((vm.objetonegocioid !== '' && vm.objetonegocioid !== undefined) && vm.objetonegocioid !== null) {
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

        $scope.$on("BloquearPorCTUSPendiente", function (evt, data) {
            vm.disabled = true;
            $("#btnExaminar").attr('disabled', true);
        });

        function consultarTipoDocumento() {

            var idTramite = $sessionStorage.tramiteId;
            var tipoTramiteId = $sessionStorage.tipoTramiteId;

            if (vm.idtipotramitepresupuestal !== undefined && vm.idtipotramitepresupuestal !== "") tipoTramiteId = vm.idtipotramitepresupuestal;

            if (idTramite === undefined) {
                idTramite = 0;
            }

            if ((vm.rol === undefined || vm.rol === "")) {
                vm.rol = '00000000-0000-0000-0000-000000000000';
            }

            archivoServicios.obtenerTipoDocumentoSoportePorRol(tipoTramiteId, "A", idTramite, vm.IdNivel, vm.IdInstancia, vm.IdAccion).then(function (resultado) {
                vm.listaTipoArchivos = resultado.data;
                cargaTipoArchivosObligatorios(vm.listaTipoArchivos);
                consultarArchivosTramite();
            });
        }

        function initCargarArchivos() {
            vm.archivo = undefined;
            $sessionStorage.sessionDocumentos = 0;
        }

        function cargaTipoArchivosObligatorios(lista) {
            lista.map(function (item) {
                if (item.Obligatorio) {
                    vm.listadocumentosObligatorios.push(item);
                    if (item.CampoValidacion != undefined && item.CampoValidacion != null && item.CampoValidacion != "") {
                        if (item.CampoValidacion === "Fecha") {
                            $sessionStorage.fechaProcesoViabilidad = item.FechaValidacion;
                        }
                    }
                }
                vm.archivoObigatorioscargados = true;
            });
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
                    idNivel: vm.IdNivel,
                    idRol: vm.rol,
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
                    //utilidades.mensajeError(response);
                } else {
                    response.forEach(archivo => {
                        if (archivo.status !== 'Eliminado') {
                            vm.listaArchivos.push({
                                codigoProceso: archivo.metadatos.codigoproceso,
                                descripcion: archivo.metadatos.descripcion,
                                fecha: moment(archivo.fecha).format("DD/MM/YYYY"),
                                nombreArchivo: archivo.nombre,
                                tipoDocumento: archivo.metadatos.tipodocumento === undefined ? '' : archivo.metadatos.tipodocumento,
                                tipodocumentoCodigo: archivo.metadatos.tipodocumentoCodigo === undefined ? '' : archivo.metadatos.tipodocumentoCodigo,
                                tipoDocumentoId: archivo.metadatos.tipodocumentoid === undefined ? '' : archivo.metadatos.tipodocumentoid,
                                datosDocumento: archivo.metadatos.datosdocumento,
                                NombreDocumento: archivo.nombre,
                                idArchivoBlob: archivo.metadatos.idarchivoblob,
                                ContenType: archivo.metadatos.contenttype,
                                idMongo: archivo.id,
                                nombre: archivo.nombre
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
                }
            });




        }

        vm.obtener = function () {
            var idTramite = $sessionStorage.tramiteId;
            var proyectoId = $sessionStorage.proyectoId;
            var tipoTramiteId = vm.tipotramiteid;
            var tipoRolId = $sessionStorage.TipoRolId;
            var TipoProyecto = $sessionStorage.TipoProyecto;

            consultarArchivosTramite();

        };

        vm.mostrarBotonPdf = function () {
            var mostrar = false;
            if (vm.nombreComponente == "sgrviabilidadavalusosoportes") { mostrar = true; }
            return mostrar;
            ;

        };

        vm.verPdf = function () {

            transversalSgrServicio.SGR_Transversal_LeerParametro("GenerarFichaViabilidadSGR")
                .then(function (respuestaParametro) {
                    if (respuestaParametro.data.Valor == 'S') {
                        transversalSgrServicio.SGR_Transversal_ObtenerConfiguracionReportes($sessionStorage.idInstancia)
                            .then(function (configuracionReporte) {
                                vm.configuracionReporte = configuracionReporte.data;
                                //const nombreFichaTramite = "sgr_ficha_viabilidad_ET";
                                const nombreFichaTramite = vm.configuracionReporte.NombreRdl;
                                const borrador = true;
                                const projectId = $sessionStorage.proyectoId;

                                servicioFichasProyectos.ObtenerIdFicha(nombreFichaTramite)
                                    .then(function (respuestaFicha) {
                                        var fichaPlantilla = {
                                            NombreReporte: nombreFichaTramite,
                                            IdReporte: respuestaFicha.ID,
                                            PARAM_BORRADOR: true,
                                            PARAM_BPIN: projectId,
                                            InstanciaId: $sessionStorage.idInstancia,
                                            NivelId: $sessionStorage.idNivel,
                                            TramiteId: projectId
                                        };

                                        servicioFichasProyectos.GenerarFichaSGR($.param(fichaPlantilla))
                                            .then(function (respuesta) {
                                                if (borrador) {
                                                    const nombreArchivo = nombreFichaTramite.replace(/ /gi, "_") + '_' + projectId + '_' + moment().format("YYYYMMDDD_HHMMSS") + "pdf";
                                                    const blob = new Blob([respuesta], { type: 'application/pdf' });
                                                    const file = new File([blob], nombreArchivo, { type: 'application/pdf' });
                                                    FileSaver.saveAs(file, nombreArchivo);
                                                }
                                            }, function (error) {
                                                utilidades.mensajeError(error);
                                            });
                                    }, function (error) {
                                        utilidades.mensajeError(error);
                                    });
                            }, function (error) {
                                utilidades.mensajeError(error);
                            });
                    };
                }, function (error) {
                    utilidades.mensajeError(error);
                });

        }





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
                    mostrarMensajeError("El archivo debe tener extensión pdf, zip o rar", "archivo", 1);
                    onClickCancelar();
                    return;
                }
                else if (input.files[i].size > 2097152) {
                    mostrarMensajeError("El archivo supera el peso de 2 megas", "archivo", 1);
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
                case 'pdf': case 'zip': case 'rar': return true;
                default: return false;
            }
        }

        function obtenerExtension(nombreArchivo) {
            let partes = nombreArchivo.split('.');
            return partes[partes.length - 1];
        }

        function onClickCargar() {
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

            if (!errorp) {
                ocultarMensajeError();
                var input = vm.archivo;
                let tipoArchivo = vm.listaTipoArchivos.find(x => x.Id == vm.idTipoArchivoSeleccionado);

                //const ventanasPermitidas = new Set(['elaboracionEntidadNacionalSgr']);

                // Definir el valor de la única ventana permitida
                const ventanasPermitidas = new Set(['avalusoViabilidadSgr']);

                // Buscar un elemento en la lista de acciones del trámite con la ventana igual a ventanaPermitida
                var roles = $sessionStorage.listadoAccionesTramite.find((f) => ventanasPermitidas.has(f.Ventana));

                // Asignar el valor de $sessionStorage.EntidadAdscritaId si la ventana es igual a ventanaPermitida;
                // de lo contrario, asignar el valor de $sessionStorage.idEntidad
                var identidadVentanaEntidad = roles && ventanasPermitidas.has(roles.Ventana) ? $sessionStorage.EntidadAdscritaId : $sessionStorage.idEntidad;

                // Justificación:
                // - La constante ventanaPermitida facilita la gestión del valor de la ventana permitida.
                // - La búsqueda de roles con la ventana deseada se realiza con el método find, almacenando el resultado en la variable roles.
                // - La asignación de identidadVentanaEntidad se realiza mediante una expresión condicional (ternaria),
                //   asegurando la asignación correcta del valor según la condición especificada.
                // - La verificación de la existencia de roles evita errores en caso de que no se encuentre ningún elemento con la ventana deseada.

                trasladosServicio.obtenerDatosUsuario($sessionStorage.usuario.permisos.IdUsuarioDNP, identidadVentanaEntidad,
                    $sessionStorage.idAccion, $sessionStorage.idInstancia).
                    then(function (respuesta) {
                        var rol = $sessionStorage.usuario.roles.find(x => x.IdRol === vm.rol);
                        var usuariotemporal = {};
                        if (respuesta.data !== null) {
                            if ((rol == null || rol == undefined) && (respuesta.data.length > 0)) {
                                rol = $sessionStorage.usuario.roles.find(x => x.IdRol === respuesta.data[0].RolId);
                            }

                            usuariotemporal = rol;
                        }

                        var Cuenta = '';
                        var Entidad = '';
                        var NombreUsuario = '';
                        if (usuariotemporal) {
                            Cuenta = usuariotemporal.Cuenta === null || usuariotemporal.Cuenta === undefined ? '' : usuariotemporal.Cuenta;
                            Entidad = usuariotemporal.Entidad === null || usuariotemporal.Entidad === undefined ? '' : usuariotemporal.Entidad;
                            NombreUsuario = usuariotemporal.NombreUsuario === null || usuariotemporal.NombreUsuario === undefined ? '' : usuariotemporal.NombreUsuario;
                            vm.Rol = usuariotemporal.IdRol;
                        }
                        var dato = formatearFecha(obtenerFechaSinHoras(new Date())) + '. ' + rol.Nombre + ' ' +
                            NombreUsuario + ' ' + Cuenta + ' ' + Entidad;

                        if (vm.modelo.section === null || vm.modelo.section === undefined) {
                            vm.modelo.section = vm.section;
                        }

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
                                    tipoDocumentoId: tipoArchivo.TipoDocumentoId,
                                    tipoDocumentoCodigo: tipoArchivo.TipoDocumentoCodigo,
                                    idNivel: vm.IdNivel,

                                    idRol: vm.rol,
                                    idAccion: $sessionStorage.idAccion,
                                    descripcion: vm.descripcion,
                                    datosdocumento: dato,
                                    tipoDocumentoSoporte: tipoArchivo.TipoDocumento,
                                }
                            };

                            archivoServicios.cargarArchivo(archivo, vm.modelo.coleccion).then(function (response) {
                                if (response === undefined || typeof response === 'string') {
                                    vm.mensajeError = response;
                                    utilidades.mensajeError(response);
                                } else {
                                    vm.listaArchivos.push({
                                        codigoProceso: '',
                                        descripcionTramite: '',
                                        fecha: '',
                                        nombreArchivo: input.files[0].name,
                                        tipoDocumento: tipoArchivo.TipoDocumento,
                                        tipoDocumentoCodigo: tipoArchivo.TipoDocumentoCodigo,
                                        tipoDocumentoId: tipoArchivo.TipoDocumentoId,
                                        descripcion: vm.descripcion,
                                        idArchivoBlob: '',
                                        ContenType: '',
                                        idMongo: ''
                                    });

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
                                                consultarArchivosTramite();
                                            }).then(function () { renderPanel(); });
                                        },
                                        false,
                                        "El documento se ha agregado con éxito a la tabla.");
                                    vm.limpiarErrores('');
                                    guardarCapituloModificado(1);/// Aqui Guarda
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
                        guardarCapituloModificado(0);
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
            var idSpanAlertComponent = document.getElementById("soporte-error");
            var idSpanAlertComponent1 = document.getElementById("soporte1-error");
            var idSpanAlertComponent2 = document.getElementById("tipoarchivo-error");
            var idSpanAlertComponent3 = document.getElementById("descripcion-error");
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
        }

        function mostrarMensajeError(mensajeError, elemento, numeroCamposError) {
            if (elemento === 'archivo' || (elemento === undefined && numeroCamposError > 1)) {
                var idSpanAlertComponent = document.getElementById("soporte-error");
                var idSpanAlertComponent1 = document.getElementById("soporte1-error");
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
                var idSpanAlertComponent2 = document.getElementById("tipoarchivo-error");
                if (idSpanAlertComponent2 != undefined) {
                    idSpanAlertComponent2.classList.add("ico-advertencia");
                    idSpanAlertComponent2.classList.remove('hidden');
                    idSpanAlertComponent2.innerHTML = mensajeError;
                }
            }
            if (elemento === 'descripcion' || (elemento === undefined && numeroCamposError > 1)) {
                var idSpanAlertComponent3 = document.getElementById("descripcion-error");
                if (idSpanAlertComponent3 != undefined) {
                    idSpanAlertComponent3.classList.add("ico-advertencia");
                    idSpanAlertComponent3.classList.remove('hidden');
                    idSpanAlertComponent3.innerHTML = mensajeError;
                }
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
                const blob = utilidades.base64toBlob(retorno, entity.ContenType);
                FileSaver.saveAs(blob, entity.nombre);
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
            return new Date(Date.UTC(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), fecha.getHours(), fecha.getMinutes(), fecha.getSeconds()));
        }


        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado(paramModificado) {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: paramModificado,
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

            var campomensajeerror2 = document.getElementById(vm.nombreComponente + "A" + "-archivo-error");
            if (campomensajeerror2 != undefined) {
                campomensajeerror2.innerHTML = "";
                campomensajeerror2.classList.add('hidden');
            }
            var campomensajeerror3 = document.getElementById(vm.nombreComponente + "B" + "-archivo-error");
            if (campomensajeerror3 != undefined) {
                campomensajeerror3.innerHTML = "";
                campomensajeerror3.classList.add('hidden');
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

        vm.validarErrores2 = function (errores) {
            var campomensajeerror = document.getElementById(vm.nombreComponente + "A-archivo-error");
            if (campomensajeerror != undefined) {
                campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                campomensajeerror.classList.remove('hidden');
            }
        }
        vm.validarErrores3 = function (errores) {
            var campomensajeerror = document.getElementById(vm.nombreComponente + "B-archivo-error");
            if (campomensajeerror != undefined) {
                campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                campomensajeerror.classList.remove('hidden');
            }
        }

        vm.validarValoresVigenciaInformacionArchivo = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-archivo-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }





        vm.errores = {
            'SGRAVAL2': vm.validarValoresVigenciaInformacionArchivo
        }

        /* ------------------------ FIN Validaciones ---------------------------------*/


    }

    angular.module('backbone').component('alojarArchivo', {
        restrict: 'E',
        transclude: true,
        scope: {
            modelo: '='
        },
        templateUrl: "/src/app/formulario/ventanas/SGR/EntidadNacionalFlujo9/soportes/alojararchivo/alojarArchivo.html",
        controller: alojarArchivoController,
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