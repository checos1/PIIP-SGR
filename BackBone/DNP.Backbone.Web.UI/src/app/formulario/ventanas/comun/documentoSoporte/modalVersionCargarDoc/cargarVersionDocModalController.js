(function () {
    'use strict';

    cargarVersionDocModalController.$inject = [
        '$uibModalInstance',
        '$scope',
        'servicioFichasProyectos',
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
        'FileSaver',
        'transversalSgrServicio',
        '$filter',
        'documentoSoporteServicios'
    ];

    function cargarVersionDocModalController(
        $uibModalInstance,
        $scope,
        servicioFichasProyectos,
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
        FileSaver,
        transversalSgrServicio,
        $filter,
        documentoSoporteServicios
    ) {
        var vm = this;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.guardar = onClickCargar;
        vm.lang = "es";
        vm.extension = "";
        vm.filename = "";
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.gridOptions;
        vm.archivosLoad = [];
        vm.IdNivel = $sessionStorage.idNivel;
        vm.IdInstancia = $sessionStorage.idInstancia;
        vm.IdAccion = $sessionStorage.idAccion;
        vm.tipotramiteid = $sessionStorage.tipoTramiteId;
        vm.tramiteid = $sessionStorage.TramiteId;
        vm.puedeGenerarverReq = false;
        vm.nombrecomponentepaso = $sessionStorage.nombrecomponentepaso;


        vm.listadocumentosObligatorios = [];
        vm.descripcion = '';
        vm.archivoObigatorioscargados = false;
        vm.TipoDocumentosRolFase = [];
        vm.rol;
        vm.roles;
        vm.seccionCapitulo = null; //Para guardar los capitulos modificados y que se llenen las lunas
        vm.listaTipoArchivos = [];
        vm.idTipoArchivoSeleccionado = '0';
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;

        vm.modelo = {
            coleccion: "tramites",
            ext: ".pdf",
            codigoProceso: $sessionStorage.numeroTramite,
            descripcionTramite: $sessionStorage.descripcionTramite,
            idInstancia: $sessionStorage.idInstancia,
            idAccion: $sessionStorage.idAccion,
            section: vm.section,
            idTipoTramite: vm.tipotramiteid
        };

        vm.archivo = undefined;
        vm.nombre = '';
        vm.initCargarArchivosModalVersion = initCargarArchivosModalVersion;
        vm.onClickCancelar = onClickCancelar;
        vm.activarexaminar = false;
        vm.onChangeTipoArchivo = onChangeTipoArchivo;
        vm.nombreArchivo = undefined;
        vm.nombreArchivoEdit = undefined;
        vm.configuracionReporte = {};
        vm.objetonegocioid = $sessionStorage.idObjetoNegocio;
        vm.disabled = $sessionStorage.soloLectura;
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;
        vm.listaTipoArchivosFiltro = $sessionStorage.listaTipoArchivosFiltro;
        vm.disabledSelect = true;
        vm.entityData = $sessionStorage.entityData;
        vm.pesoArchivo = '';
        vm.macroproceso = documentoSoporteServicios.GetNombreMacroproceso($sessionStorage.etapa);
        vm.nivel = $sessionStorage.idNivel;

        function consultarTipoDocumento() {

            var idTramite = $sessionStorage.tramiteId;
            var tipoTramiteId = $sessionStorage.tipoTramiteId;

            if (vm.idtipotramitepresupuestal !== undefined && vm.idtipotramitepresupuestal !== "")
                tipoTramiteId = vm.idtipotramitepresupuestal;

            if (idTramite === undefined)
                idTramite = 0;

            if ((vm.rol === undefined || vm.rol === "")) {
                var nombreAccion = $sessionStorage.InstanciaSeleccionada.NombreAccion;
                var listadoAcciones = $sessionStorage.listadoAccionesTramite;
                var listaRolesUsuario = $sessionStorage.usuario.roles;
                var rolesComunes;

                for (var i = 0; i < listadoAcciones.length; i++) {
                    if (listadoAcciones[i].Nombre === nombreAccion) {
                        rolesComunes = listadoAcciones[i].Roles.filter(function (rol) {
                            return listaRolesUsuario.some(function (usuarioRol) {
                                return usuarioRol.IdRol === rol.IdRol;
                            });
                        });
                    }
                }
                vm.roles = rolesComunes.map(function (rol) {
                    return rol.IdRol;
                }).join(',');

                vm.rol = rolesComunes[0].IdRol;
            }

            documentoSoporteServicios.ObtenerListaTipoDocumentosSoportePorRolTrv(tipoTramiteId, "A", idTramite, vm.IdNivel, vm.IdInstancia, vm.IdAccion)
                .then(function (resultado) {
                    vm.listaTipoArchivos = JSON.parse(resultado.data);
                    console.log("listaTipoArchivosModalVersiones");
                    console.log(vm.listaTipoArchivos);
                    cargaTipoArchivosObligatorios(vm.listaTipoArchivos);
                    obtenerDocumentoSeleccionado();
                });
        }

        function obtenerDocumentoSeleccionado() {
            let archivoSel = vm.listaTipoArchivos.find(x => x.TipoDocumentoId == vm.entityData.tipoDocumentoId);
            vm.idTipoArchivoSeleccionado = archivoSel.Id;
        }

        function limitarNombreArchivo(nombreArchivo) {
            if (nombreArchivo != undefined) {
                if (nombreArchivo.length <= 30)
                    return nombreArchivo;

                const extension = nombreArchivo.substring(nombreArchivo.lastIndexOf('.'));
                const nombreSinExtension = nombreArchivo.substring(0, nombreArchivo.lastIndexOf('.'));

                return nombreSinExtension.slice(0, 30 - extension.length) + "..." + extension;

            } else {
                return nombreArchivo;
            }
        };

        function initCargarArchivosModalVersion() {
            consultarTipoDocumento();
            vm.archivo = undefined;
            $sessionStorage.sessionDocumentos = 0;
            transversalSgrServicio.SGR_Transversal_LeerParametro("GeneraFichaFlujoIdVerReqSGR")
                .then(function (respuestaParametro) {
                    if (respuestaParametro.data.Valor.includes($sessionStorage.idFlujoIframe)) {
                        vm.puedeGenerarverReq = true;
                    }
                }, function (error) {
                    utilidades.mensajeError(error);
                });
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

        function onChangeTipoArchivo(value) {
            if (vm.idTipoArchivoSeleccionado != '0')
                vm.activarexaminar = true;
            else {
                vm.activarexaminar = false;
            }
        }

        function adjuntarArchivo() {

            if (vm.section == undefined)
                vm.section = '';

            var idSpanArrow = 'file-' + vm.section;
            document.getElementById(idSpanArrow).value = "";
            document.getElementById(idSpanArrow).click();
        }

        $scope.fileNameChanged = function (input) {

            ocultarMensajeError();
            vm.archivo = undefined;

            if (input.files.length == 1)
                vm.nombreArchivo = input.files[0].name;
            else
                vm.nombreArchivo = input.files.length + " archivos";

            vm.filename = vm.nombreArchivo;
            vm.archivo = input;
            vm.nombreArchivoEdit = limitarNombreArchivo(vm.nombreArchivo);

            if (vm.archivo != undefined) {
                for (var i = 0; i < input.files.length; i++) {
                    vm.extension = obtenerExtension(input.files[i].name);
                    if (!validarExtension(vm.extension)) {
                        mostrarMensajeError("El archivo debe tener extension pdf, zip o rar", "archivo", 1);
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

            vm.pesoArchivo = input.files[0].size;
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

            var errorp = false;

            if (vm.idTipoArchivoSeleccionado == null || vm.idTipoArchivoSeleccionado == undefined || vm.idTipoArchivoSeleccionado == "" || vm.idTipoArchivoSeleccionado == "0") {
                errorp = true;
                mostrarMensajeError("Es necesario seleccionar un tipo de documento.", "tipoArchivo", 1);
            }

            if (vm.archivo === undefined) {
                errorp = true;
                mostrarMensajeError("Es necesario adjuntar un archivo.", "archivo", 1);
            }

            if (!errorp) {

                ocultarMensajeError();
                var input = vm.archivo;
                let tipoArchivo = vm.listaTipoArchivos.find(x => x.Id == vm.idTipoArchivoSeleccionado);

                // Definir el valor de la única ventana permitida
                const ventanasPermitidas = new Set(['elaboracionEntidadNacionalSgr', 'ElaboracionctusIntegradoSgr', 'ElaboracionCormagdalenaSgr', 'avalusoViabilidadSgr']);

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

                trasladosServicio.obtenerDatosUsuario($sessionStorage.usuario.permisos.IdUsuarioDNP, identidadVentanaEntidad, $sessionStorage.idAccion, $sessionStorage.idInstancia)
                    .then(function (respuesta) {

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

                        var myDate = new Date();
                        var formattedDate = $filter('date')(myDate, 'dd/MM/yyyy HH:mm');
                        var dato = formattedDate + '. ' + rol.Nombre + ' ' +
                            NombreUsuario + ' ' + Cuenta + ' ' + Entidad;

                        if (vm.modelo.section === null || vm.modelo.section === undefined)
                            vm.modelo.section = vm.section;

                        for (var i = 0; i < input.files.length; i++) {
                            vm.extension = obtenerExtension(input.files[i].name);

                            //Validacion para incrementar la version segun tipo de documento
                            let idTipoDocumento = tipoArchivo.TipoDocumentoId;

                            let documentoEncontrado = vm.listaTipoArchivosFiltro.find(function (item) {
                                return item.tipoDocumentoId === idTipoDocumento;
                            });

                            let versionDocumentoSoporte;

                            if (documentoEncontrado)
                                versionDocumentoSoporte = parseInt(documentoEncontrado.versionDocumentoSoporte, 10) + 1;
                            else
                                versionDocumentoSoporte = 1;

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
                                    idNivel: vm.nivel,
                                    idRol: vm.roles,
                                    idAccion: $sessionStorage.idAccion,
                                    descripcion: vm.descripcion === null || vm.descripcion === undefined || vm.descripcion == "" ? 'Sin descripcion' : vm.descripcion,
                                    datosdocumento: dato,
                                    tipoDocumentoSoporte: tipoArchivo.TipoDocumento,
                                    versionDocumentoSoporte: versionDocumentoSoporte.toString(),
                                    pasoDocumento: $sessionStorage.InstanciaSeleccionada.NombreAccion.toString(),
                                    macroproceso: vm.macroproceso.toString(),
                                    pesoDocumento: vm.pesoArchivo.toString(),
                                    proyectoId: $sessionStorage.proyectoId.toString(),
                                    objetoNegocioId: $sessionStorage.idObjetoNegocio.toString()
                                }
                            };

                            archivoServicios.cargarArchivo(archivo, vm.modelo.coleccion).then(function (response) {
                                if (response === undefined || typeof response === 'string') {
                                    vm.mensajeError = response;
                                    utilidades.mensajeError(response);
                                } else {
                                    utilidades.mensajeSuccess('', false, false, false, "El documento " + tipoArchivo.TipoDocumento + " versi&oacute;n " + versionDocumentoSoporte + " se ha agregado con &eacute;xito.");
                                    cerrarModal();
                                    guardarCapituloModificado(1);/// Aqui Guarda                          
                                }
                            }, error => {
                                console.log(error);
                            });
                        }
                    });
            }
        };

        function cerrarModal() {
            $uibModalInstance.close();
        }

        function onClickCancelar() {
            vm.archivo = undefined;
            vm.nombreArchivo = undefined;
            vm.nombreArchivoEdit = undefined;
        }

        function ocultarMensajeError() {
            var idSpanAlertComponent = document.getElementById("soporte-error");
            var idSpanAlertComponent1 = document.getElementById("soporte1-error");
            var idSpanAlertComponent2 = document.getElementById("tipoarchivo-error");

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
        }

        //Para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado(paramModificado) {

            ObtenerSeccionCapitulo();

            var data = {
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: paramModificado,
                cuenta: 1
            }

            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombrecomponentepaso });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        //Para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombrecomponentepaso);
            vm.seccionCapitulo = span.textContent;
        }

    }

    angular.module('backbone').controller('cargarVersionDocModalController', cargarVersionDocModalController);

})();
